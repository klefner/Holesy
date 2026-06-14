using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    // ── World constants ───────────────────────────────────────────────────────
    public const float WORLD_SIZE = 220f;
    public const float HALF       = WORLD_SIZE / 2f;
    public const float BLOCK      = 32f;
    public const float ROAD_W     = 8f;

    // ── Growth constants ──────────────────────────────────────────────────────
    public const float MIN_RADIUS          = 0.9f;
    public const float GROWTH_K            = 0.95f;
    public const float GROWTH_SCALE        = 40f;
    public const float HOLE_EAT_RADIUS_BONUS = 0.6f;
    public const float MIN_HOLE_RADIUS     = 0.5f;
    public const float GAME_DURATION       = 120f;

    // ── Speed constants ───────────────────────────────────────────────────────
    public const float PLAYER_SPEED  = 14f;
    public const float AI_SPEED      = 12f;
    public const float AI_FLEE_SPEED = 13.5f;

    public enum GameState { Playing, Paused, GameOver }

    public GameState              State         { get; private set; }
    public float                  TimeRemaining { get; private set; }
    public PlayerHole             Player        { get; private set; }
    public List<HoleBase>         AllHoles      { get; } = new List<HoleBase>();
    public List<ConsumableObject> AllObjects    { get; } = new List<ConsumableObject>();

    public UIManager    UI    { get; private set; }
    public AudioManager Audio { get; private set; }

    private CityGenerator  _city;
    private MilitarySystem _military;

    private readonly List<(HoleBase hole, ConsumableObject obj)> _consumeQueue
        = new List<(HoleBase, ConsumableObject)>();

    // ── Lifecycle ─────────────────────────────────────────────────────────────

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;

        // Stronger-than-real gravity reads better at city scale, and matches
        // ConsumableObject.FALL_GRAVITY so falling pace doesn't change the
        // instant the hole takes over an object's fall.
        Physics.gravity = new Vector3(0f, -18f, 0f);

        _city     = gameObject.AddComponent<CityGenerator>();
        _military = gameObject.AddComponent<MilitarySystem>();
        UI        = gameObject.AddComponent<UIManager>();
        Audio     = gameObject.AddComponent<AudioManager>();

        SetupLighting();
        SetupCamera();
    }

    void Start()
    {
        // A previous run (or a pause before scene reload) may have left time
        // frozen — always restore normal flow when a fresh game begins.
        Time.timeScale = 1f;

        TimeRemaining = GAME_DURATION;
        State         = GameState.Playing;

        _city.Build();
        SpawnHoles();
        _military.Begin();
        UI.Init();
        Audio.StartMusic();
    }

    void SetupLighting()
    {
        // Dark night city — ambient gives only faint silhouettes; the player's
        // follow-lantern (see SpawnHoles) is what actually makes the area around
        // the hole readable. The dark night building albedos (0.08–0.22) swallow
        // ambient on their own, so the lantern carries playability.
        RenderSettings.ambientMode  = UnityEngine.Rendering.AmbientMode.Flat;
        RenderSettings.ambientLight = new Color(0.26f, 0.26f, 0.34f);

        // Atmospheric fog — purple-black haze obscures the far city edge.
        RenderSettings.fog              = true;
        RenderSettings.fogMode          = FogMode.Linear;
        RenderSettings.fogColor         = new Color(0.05f, 0.04f, 0.10f);
        RenderSettings.fogStartDistance = 60f;
        RenderSettings.fogEndDistance   = 200f;

        // Moon — dim cool-blue directional, enough to silhouette building edges.
        var sunGO = new GameObject("Sun");
        var sun   = sunGO.AddComponent<Light>();
        sun.type      = LightType.Directional;
        sun.intensity = 0.70f;
        sun.color     = new Color(0.62f, 0.70f, 0.90f);
        sun.shadows   = LightShadows.Soft;
        sunGO.transform.rotation = Quaternion.Euler(28f, -30f, 0f);
    }

    void SetupCamera()
    {
        if (Camera.main == null)
        {
            var camGO = new GameObject("Main Camera");
            camGO.tag = "MainCamera";
            camGO.AddComponent<Camera>();
            camGO.AddComponent<AudioListener>();
        }

        var cam = Camera.main;
        cam.clearFlags      = CameraClearFlags.SolidColor;
        cam.backgroundColor = new Color(0.05f, 0.05f, 0.08f); // dark void — shows through the stencil hole
        cam.farClipPlane    = 600f;
        cam.gameObject.AddComponent<GameCamera>();
        cam.gameObject.AddComponent<DiabloPostProcessing>();
    }

    // ── Hole spawning ─────────────────────────────────────────────────────────

    void SpawnHoles()
    {
        AllHoles.Clear();

        Player = HoleFactory.CreatePlayer(new Color(0.30f, 0.60f, 1.00f), Vector3.zero);
        AllHoles.Add(Player.Hole);
        AddPlayerLantern(Player.Hole.transform);

        var cfgs = AIConfig.Defaults();
        Vector3[] aiPos = { new Vector3(-40, 0, -40), new Vector3(40, 0, 40), new Vector3(-40, 0, 40) };
        for (int i = 0; i < cfgs.Length; i++)
        {
            var ai = HoleFactory.CreateAI(cfgs[i], aiPos[i]);
            AllHoles.Add(ai.Hole);
        }

        Camera.main.GetComponent<GameCamera>().Target = Player.Hole.transform;
    }

    // A bright warm light floating above the player's hole. Parented to the hole
    // transform, so it tracks the hole automatically as it moves. This is the
    // primary illumination the player navigates by — the night city is otherwise
    // too dark to read.
    void AddPlayerLantern(Transform holeTransform)
    {
        var go = new GameObject("PlayerLantern");
        go.transform.SetParent(holeTransform, false);
        go.transform.localPosition = new Vector3(0f, 16f, 0f);

        var l = go.AddComponent<Light>();
        l.type       = LightType.Point;
        l.color      = new Color(1.0f, 0.94f, 0.82f); // warm white
        l.intensity  = 32f;
        l.range      = 55f;
        l.shadows    = LightShadows.None;             // performance — many objects in pool
        l.renderMode = LightRenderMode.ForcePixel;    // ensure per-pixel quality for the hero light
    }

    // ── Main loop ─────────────────────────────────────────────────────────────

    void Update()
    {
        // Pause is reachable any time the game is live (not on the end screen).
        // Esc / P toggle it; the on-screen pause button does the same.
        if (State != GameState.GameOver &&
            (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P)))
            TogglePause();

        if (State != GameState.Playing) return;

        TimeRemaining -= Time.deltaTime;
        if (TimeRemaining <= 0f)
        {
            TimeRemaining = 0f;
            EndGame();
            return;
        }

        ProcessConsumptions();
        ProcessHoleVsHole();
        UI.Tick();
    }

    void ProcessConsumptions()
    {
        _consumeQueue.Clear();
        foreach (var hole in AllHoles)
        {
            if (!hole.Alive) continue;
            float r2 = hole.Radius * hole.Radius;
            float hx = hole.transform.position.x;
            float hz = hole.transform.position.z;

            foreach (var obj in AllObjects)
            {
                if (obj.IsConsumed || obj.Size > hole.Radius) continue;
                // Debris in flight keeps real physics; the hole only takes over
                // once the object is near ground level
                if (obj.transform.position.y > 2f) continue;
                float dx = obj.transform.position.x - hx;
                float dz = obj.transform.position.z - hz;
                // FootprintRadius makes the swallow trigger on hole-edge contact
                // with the object's edge, not its center
                float tr = hole.Radius + obj.FootprintRadius;
                if (dx * dx + dz * dz < tr * tr)
                    _consumeQueue.Add((hole, obj));
            }
        }

        foreach (var (h, o) in _consumeQueue)
        {
            if (!o.IsConsumed) ConsumeObject(h, o);
        }
    }

    void ProcessHoleVsHole()
    {
        for (int i = 0; i < AllHoles.Count; i++)
        {
            var a = AllHoles[i];
            if (!a.Alive) continue;
            for (int j = i + 1; j < AllHoles.Count; j++)
            {
                var b = AllHoles[j];
                if (!b.Alive) continue;

                float dx   = a.transform.position.x - b.transform.position.x;
                float dz   = a.transform.position.z - b.transform.position.z;
                float dist = Mathf.Sqrt(dx * dx + dz * dz);

                HoleBase bigger  = a.Radius >= b.Radius ? a : b;
                HoleBase smaller = a.Radius >= b.Radius ? b : a;

                if (dist < bigger.Radius - 0.2f && bigger.Radius > smaller.Radius * 1.02f)
                    EatHole(bigger, smaller);
            }
        }
    }

    // ── Pause ─────────────────────────────────────────────────────────────────

    public bool IsPaused => State == GameState.Paused;

    public void TogglePause()
    {
        if (State == GameState.Playing)      Pause();
        else if (State == GameState.Paused)  Resume();
    }

    public void Pause()
    {
        if (State != GameState.Playing) return;
        State          = GameState.Paused;
        Time.timeScale = 0f;   // freezes movement, AI, physics and the timer
        UI.ShowPauseScreen();
    }

    public void Resume()
    {
        if (State != GameState.Paused) return;
        State          = GameState.Playing;
        Time.timeScale = 1f;
        UI.HidePauseScreen();
    }

    // Leaves the game mid-play. In a build this quits the application; in the
    // editor it stops play mode. Matches the end-screen EXIT behaviour.
    public void QuitGame()
    {
        Time.timeScale = 1f;
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    // ── Public game actions ───────────────────────────────────────────────────

    public void ConsumeObject(HoleBase hole, ConsumableObject obj)
    {
        obj.MarkConsumed(hole);
        hole.Score += obj.Value;
        hole.RecalcTargetRadius();
        Audio.PlayConsume(obj.Category, obj.Size, hole.IsPlayer, hole.transform.position);
    }

    public void EatHole(HoleBase eater, HoleBase eaten)
    {
        if (!eaten.Alive) return;
        eaten.Die();
        // Being eaten is a lose condition for the player (browser parity)
        if (eaten.IsPlayer) EndGame();

        float reward = 250f
            + Mathf.Floor(eaten.Score   * 0.3f)
            + Mathf.Floor(eaten.Radius  * 100f);

        eater.Score       += reward;
        eater.BonusRadius += eaten.Radius * HOLE_EAT_RADIUS_BONUS;
        if (eaten.BonusRadius > 0f)
            eater.BonusRadius += eaten.BonusRadius * 0.5f;
        eater.RecalcTargetRadius();

        Audio.PlayHoleEat();
    }

    // ── Static helpers ────────────────────────────────────────────────────────

    public static float RadiusFromScore(HoleBase h)
        => ScoreRadius(h.Score) + h.BonusRadius;

    // Base radius earned by score alone (no bonus), always >= MIN_RADIUS
    public static float ScoreRadius(float score)
        => MIN_RADIUS + GROWTH_K * Mathf.Log(1f + score / GROWTH_SCALE);

    // Inverse of ScoreRadius: the score that yields the given base radius
    public static float ScoreForRadius(float baseRadius)
        => GROWTH_SCALE * (Mathf.Exp((baseRadius - MIN_RADIUS) / GROWTH_K) - 1f);

    // Called when gunfire shrinks a hole below survivable size
    public void OnHoleShotDown(HoleBase h)
    {
        if (!h.Alive) return;
        h.Die();
        if (h.IsPlayer) EndGame();
    }

    public static string TierLabel(float radius)
    {
        if (radius < 1.3f) return "Pothole";
        if (radius < 1.8f) return "Crack";
        if (radius < 2.3f) return "Gap";
        if (radius < 2.8f) return "Dip";
        if (radius < 3.3f) return "Trench";
        if (radius < 3.8f) return "Pit";
        if (radius < 4.3f) return "Manhole";
        if (radius < 4.8f) return "Burrow";
        if (radius < 5.3f) return "Cavity";
        if (radius < 5.8f) return "Hollow";
        if (radius < 6.3f) return "Crater";
        if (radius < 6.8f) return "Chasm";
        if (radius < 7.3f) return "Sinkhole";
        if (radius < 7.8f) return "Void";
        return "Abyss";
    }

    void EndGame()
    {
        State          = GameState.GameOver;
        Time.timeScale = 1f;   // ensure the end screen is interactive even if paused
        _military.Stop();
        UI.ShowEndScreen();
        Audio.StopMusic();
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;   // timeScale persists across scene loads — reset it
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
