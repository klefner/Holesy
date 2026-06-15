using UnityEngine;

public class HoleBase : MonoBehaviour
{
    public bool  Alive        { get; private set; } = true;
    public bool  IsPlayer     { get; private set; }
    public float Score        { get; set; }          = 0f;
    public float BonusRadius  { get; set; }          = 0f;
    public float Radius       { get; private set; }
    public float TargetRadius { get; set; }
    public Color HoleColor    { get; private set; }
    public string HoleName    { get; set; } = "Hole";

    private Vector3  _targetPos;
    private float    _moveSpeed = GameManager.PLAYER_SPEED;

    private Transform _disc;
    private Renderer  _rimRenderer;
    private Material  _rimMat;
    private Light     _rimLight;
    private float     _flashTimer;
    private const float FLASH_DUR = 0.15f;

    // Glow accumulated from eating light sources. Drives rim light in evening/night.
    private float _rimGlow;
    public void AddRimGlow(float amount) => _rimGlow = Mathf.Min(_rimGlow + amount, 30f);

    private static Mesh _ringMesh;
    private static Mesh _discMesh;

    // ── Init ──────────────────────────────────────────────────────────────────

    public void Init(Color color, bool isPlayer, float moveSpeed)
    {
        HoleColor    = color;
        IsPlayer     = isPlayer;
        _moveSpeed   = moveSpeed;
        Radius       = GameManager.MIN_RADIUS;
        TargetRadius = Radius;
        _targetPos   = transform.position;

        BuildVisuals();
    }

    void BuildVisuals()
    {
        // ── Hole disc: stencil mask, cuts hole in ground ───────────────────
        var discGO = new GameObject("Disc");
        discGO.transform.SetParent(transform, false);
        discGO.transform.localPosition = new Vector3(0f, -0.03f, 0f);
        discGO.AddComponent<MeshFilter>().sharedMesh = GetDiscMesh();
        var discRend = discGO.AddComponent<MeshRenderer>();
        discRend.material = new Material(Shader.Find("DowntownDevour/HoleMask"));
        _disc = discGO.transform;

        // ── Colored rim ring ───────────────────────────────────────────────
        var rimGO = new GameObject("Rim");
        rimGO.transform.SetParent(transform, false);
        rimGO.transform.localPosition = new Vector3(0f, 0.01f, 0f);
        var mf = rimGO.AddComponent<MeshFilter>();
        _rimRenderer = rimGO.AddComponent<MeshRenderer>();
        mf.sharedMesh = GetRingMesh();

        _rimMat = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        _rimMat.SetColor("_BaseColor", HoleColor * 0.5f);
        _rimMat.SetFloat("_Smoothness", 0.80f);
        _rimMat.SetFloat("_Metallic",   0.40f);
        _rimMat.SetColor("_EmissionColor", HoleColor * 4.0f); // bright glow for Diablo bloom
        _rimMat.EnableKeyword("_EMISSION");
        _rimMat.renderQueue = 2100;
        _rimRenderer.material = _rimMat;

        // Point light makes the hole cast colored light on surrounding city — the
        // Diablo "walking light source" effect. Range scales with hole size in Update.
        var rimLightGO = new GameObject("HoleLight");
        rimLightGO.transform.SetParent(transform, false);
        rimLightGO.transform.localPosition = Vector3.up * 0.5f;
        _rimLight = rimLightGO.AddComponent<Light>();
        _rimLight.type      = LightType.Point;
        _rimLight.color     = HoleColor;
        _rimLight.intensity = 2.5f;
        _rimLight.range     = 8f;
        _rimLight.shadows   = LightShadows.None;

        UpdateVisualScale();
    }

    // ── Per-frame ─────────────────────────────────────────────────────────────

    void Update()
    {
        if (!Alive) return;

        Vector3 pos = transform.position;
        pos.y = 0f;
        transform.position = Vector3.MoveTowards(pos, _targetPos, _moveSpeed * Time.deltaTime);

        Radius += (TargetRadius - Radius) * Mathf.Min(1f, Time.deltaTime * 6f);
        UpdateVisualScale();

        if (_flashTimer > 0f)
        {
            _flashTimer -= Time.deltaTime;
            if (_flashTimer <= 0f) ResetRimColor();
        }
    }

    void UpdateVisualScale()
    {
        // The custom disc/ring meshes are built with radius 1.0 (NOT 0.5 like the
        // old Cylinder primitive), so scale by Radius — not diameter.  Scaling by
        // Radius*2 made the visible hole twice the logical consume radius, which
        // is why the drop zone looked like an inner subset of the hole.
        if (_disc != null)
            _disc.localScale = new Vector3(Radius, 0.02f, Radius);

        if (_rimRenderer != null)
            _rimRenderer.transform.localScale = new Vector3(Radius, 1f, Radius);

        if (_rimLight != null)
        {
            bool darkMode = GameManager.Instance != null &&
                (GameManager.Instance.CurrentTimeOfDay == GameManager.TimeOfDay.Evening ||
                 GameManager.Instance.CurrentTimeOfDay == GameManager.TimeOfDay.Night);

            if (darkMode)
            {
                // Rim ring becomes a real light source in evening/night, brightening
                // with each lamp consumed — the hole grows into a moving street lamp.
                float glow = _rimGlow;
                _rimLight.intensity = Mathf.Min(2.5f + glow * 1.8f, 35f);
                _rimLight.range     = Radius * 3.5f + 8f + glow * 2f;
                // Brighten the rim emission (skip during damage flash so red still shows)
                if (_rimMat != null && _flashTimer <= 0f)
                    _rimMat.SetColor("_EmissionColor", HoleColor * (4f + glow * 0.6f));
            }
            else
            {
                _rimLight.intensity = 2.5f;
                _rimLight.range     = Radius * 2.5f + 6f;
            }
        }
    }

    // ── Movement API ──────────────────────────────────────────────────────────

    public void SetTargetPosition(Vector3 worldPos)
    {
        worldPos.y = 0f;
        worldPos.x = Mathf.Clamp(worldPos.x, -GameManager.HALF + 2f, GameManager.HALF - 2f);
        worldPos.z = Mathf.Clamp(worldPos.z, -GameManager.HALF + 2f, GameManager.HALF - 2f);
        _targetPos = worldPos;
    }

    public void SetMoveSpeed(float speed) => _moveSpeed = speed;

    // ── Game mechanics ────────────────────────────────────────────────────────

    public void RecalcTargetRadius()
    {
        TargetRadius = Mathf.Max(GameManager.MIN_HOLE_RADIUS,
                                 GameManager.RadiusFromScore(this) - _damageShrink);
    }

    // Direct radius deficit from gunfire that score can no longer absorb.
    // Lets sustained fire push a hole below the formula's MIN_RADIUS floor
    // and kill it (browser parity: holes can be shot down).
    private float _damageShrink;

    public void TakeDamage(float amount)
    {
        if (!Alive) return;

        // 1. Bonus radius (earned by eating holes) absorbs damage first
        float fromBonus = Mathf.Min(BonusRadius, amount);
        BonusRadius -= fromBonus;
        float rem = amount - fromBonus;

        // 2. Then earned size: walk Score back down the inverse growth curve
        if (rem > 0f)
        {
            float scoreR    = GameManager.ScoreRadius(Score);
            float scorePart = Mathf.Min(scoreR - GameManager.MIN_RADIUS, rem);
            if (scorePart > 0f)
            {
                Score = Mathf.Max(0f, GameManager.ScoreForRadius(scoreR - scorePart));
                rem  -= scorePart;
            }
        }

        // 3. Anything left shrinks the hole directly, below the formula floor
        if (rem > 0f) _damageShrink += rem;

        RecalcTargetRadius();

        _flashTimer = FLASH_DUR;
        if (_rimMat != null)
        {
            _rimMat.SetColor("_BaseColor",     Color.red);
            _rimMat.SetColor("_EmissionColor", Color.red * 4.0f);
        }
        if (_rimLight != null) _rimLight.color = Color.red;

        if (GameManager.RadiusFromScore(this) - _damageShrink < GameManager.MIN_HOLE_RADIUS)
            GameManager.Instance.OnHoleShotDown(this);
    }

    public void Die()
    {
        Alive = false;
        if (_rimLight != null) _rimLight.enabled = false;
        gameObject.SetActive(false);
    }

    void ResetRimColor()
    {
        if (_rimMat != null)
        {
            _rimMat.SetColor("_BaseColor",     HoleColor * 0.5f);
            _rimMat.SetColor("_EmissionColor", HoleColor * 4.0f);
        }
        if (_rimLight != null) _rimLight.color = HoleColor;
    }

    // ── Disc mesh (shared, built once) ───────────────────────────────────────

    static Mesh GetDiscMesh()
    {
        if (_discMesh != null) return _discMesh;

        const int SEG   = 128;
        var verts = new Vector3[SEG + 1];
        var tris  = new int[SEG * 3];

        verts[0] = Vector3.zero;
        for (int i = 0; i < SEG; i++)
        {
            float a = i / (float)SEG * Mathf.PI * 2f;
            verts[i + 1] = new Vector3(Mathf.Cos(a), 0f, Mathf.Sin(a));
        }

        for (int i = 0; i < SEG; i++)
        {
            int t = i * 3;
            tris[t]     = 0;
            tris[t + 1] = i + 1;
            tris[t + 2] = (i + 1) % SEG + 1;
        }

        _discMesh = new Mesh { name = "HoleDisc" };
        _discMesh.vertices  = verts;
        _discMesh.triangles = tris;
        _discMesh.RecalculateNormals();
        _discMesh.RecalculateBounds();
        return _discMesh;
    }

    // ── Ring mesh (shared, built once) ────────────────────────────────────────

    static Mesh GetRingMesh()
    {
        if (_ringMesh != null) return _ringMesh;

        const int   SEG   = 128;
        const float OUTER = 1.0f;
        const float INNER = 0.82f;

        var verts = new Vector3[SEG * 2];
        var uvs   = new Vector2[SEG * 2];
        var tris  = new int   [SEG * 6];

        for (int i = 0; i < SEG; i++)
        {
            float a = i / (float)SEG * Mathf.PI * 2f;
            float c = Mathf.Cos(a), s = Mathf.Sin(a);
            verts[i * 2]     = new Vector3(c * OUTER, 0f, s * OUTER);
            verts[i * 2 + 1] = new Vector3(c * INNER, 0f, s * INNER);
            uvs[i * 2]       = new Vector2(c * 0.5f + 0.5f,   s * 0.5f + 0.5f);
            uvs[i * 2 + 1]   = new Vector2(c * 0.41f + 0.5f,  s * 0.41f + 0.5f);
        }

        for (int i = 0; i < SEG; i++)
        {
            int next = (i + 1) % SEG;
            int t    = i * 6;
            int o = i * 2, n = next * 2;
            tris[t]     = o;     tris[t + 1] = n;     tris[t + 2] = o + 1;
            tris[t + 3] = n;     tris[t + 4] = n + 1; tris[t + 5] = o + 1;
        }

        _ringMesh = new Mesh { name = "HoleRing" };
        _ringMesh.vertices  = verts;
        _ringMesh.uv        = uvs;
        _ringMesh.triangles = tris;
        _ringMesh.RecalculateNormals();
        _ringMesh.RecalculateBounds();
        return _ringMesh;
    }
}

// ── Factory ───────────────────────────────────────────────────────────────────

public static class HoleFactory
{
    public static PlayerHole CreatePlayer(Color color, Vector3 pos)
    {
        var go = new GameObject("Player");
        go.transform.position = pos;
        var hole = go.AddComponent<HoleBase>();
        hole.HoleName = "Player";
        hole.Init(color, isPlayer: true, moveSpeed: GameManager.PLAYER_SPEED);
        var ph = go.AddComponent<PlayerHole>();
        ph.Hole = hole;
        return ph;
    }

    public static AIHole CreateAI(AIConfig cfg, Vector3 pos)
    {
        var go = new GameObject(cfg.Name);
        go.transform.position = pos;
        var hole = go.AddComponent<HoleBase>();
        hole.HoleName = cfg.Name;
        hole.Init(cfg.HoleColor, isPlayer: false, moveSpeed: GameManager.AI_SPEED);
        var ai = go.AddComponent<AIHole>();
        ai.Hole   = hole;
        ai.Config = cfg;
        return ai;
    }
}
