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

    public enum GameState { Playing, GameOver }

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

        _city     = gameObject.AddComponent<CityGenerator>();
        _military = gameObject.AddComponent<MilitarySystem>();
        UI        = gameObject.AddComponent<UIManager>();
        Audio     = gameObject.AddComponent<AudioManager>();

        SetupLighting();
        SetupCamera();
    }

    void Start()
    {
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
        RenderSettings.ambientLight = new Color(0.55f, 0.58f, 0.68f);

        var sunGO = new GameObject("Sun");
        var sun   = sunGO.AddComponent<Light>();
        sun.type      = LightType.Directional;
        sun.intensity = 1.2f;
        sun.color     = new Color(1f, 0.95f, 0.85f);
        sun.shadows   = LightShadows.Soft;
        sunGO.transform.rotation = Quaternion.Euler(50f, -28f, 0f);
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
    }

    // ── Hole spawning ─────────────────────────────────────────────────────────

    void SpawnHoles()
    {
        AllHoles.Clear();

        Player = HoleFactory.CreatePlayer(new Color(0.30f, 0.60f, 1.00f), Vector3.zero);
        AllHoles.Add(Player.Hole);

        var cfgs = AIConfig.Defaults();
        Vector3[] aiPos = { new Vector3(-40, 0, -40), new Vector3(40, 0, 40), new Vector3(-40, 0, 40) };
        for (int i = 0; i < cfgs.Length; i++)
        {
            var ai = HoleFactory.CreateAI(cfgs[i], aiPos[i]);
            AllHoles.Add(ai.Hole);
        }

        Camera.main.GetComponent<GameCamera>().Target = Player.Hole.transform;
    }

    // ── Main loop ─────────────────────────────────────────────────────────────

    void Update()
    {
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
                float dx = obj.transform.position.x - hx;
                float dz = obj.transform.position.z - hz;
                if (dx * dx + dz * dz < r2)
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

    // ── Public game actions ───────────────────────────────────────────────────

    public void ConsumeObject(HoleBase hole, ConsumableObject obj)
    {
        obj.MarkConsumed(hole);
        hole.Score += obj.Value;
        hole.RecalcTargetRadius();
        Audio.PlayConsume(obj.Category, obj.Size);
    }

    public void EatHole(HoleBase eater, HoleBase eaten)
    {
        if (!eaten.Alive) return;
        eaten.Die();

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
        => MIN_RADIUS + GROWTH_K * Mathf.Log(1f + h.Score / GROWTH_SCALE) + h.BonusRadius;

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
        State = GameState.GameOver;
        _military.Stop();
        UI.ShowEndScreen();
        Audio.StopMusic();
    }

    public void RestartGame()
        => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
}
