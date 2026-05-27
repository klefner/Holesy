using UnityEngine;

// Shared data + visuals for every hole (player and AI).
// Movement target is set each frame by PlayerHole or AIHole.
public class HoleBase : MonoBehaviour
{
    public bool  Alive       { get; private set; } = true;
    public bool  IsPlayer    { get; private set; }
    public float Score       { get; set; }         = 0f;
    public float BonusRadius { get; set; }         = 0f;
    public float Radius      { get; private set; }
    public float TargetRadius { get; set; }

    public Color HoleColor { get; private set; }

    // Name shown in UI (set from factory)
    public string HoleName { get; set; } = "Hole";

    private Vector3 _targetPos;
    private float   _moveSpeed = GameManager.PLAYER_SPEED;

    // Visuals
    private Transform _disc;
    private Renderer  _rimRenderer;
    private float     _flashTimer;

    private static Mesh _ringMesh;   // shared across all hole instances
    private const float FLASH_DUR = 0.15f;

    // ── Init ─────────────────────────────────────────────────────────────────

    public void Init(Color color, bool isPlayer, float moveSpeed)
    {
        HoleColor  = color;
        IsPlayer   = isPlayer;
        _moveSpeed = moveSpeed;
        Radius     = GameManager.MIN_RADIUS;
        TargetRadius = Radius;
        _targetPos = transform.position;

        BuildVisuals();
    }

    void BuildVisuals()
    {
        // Flat dark disc (the hole)
        var discGO = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        discGO.name = "Disc";
        discGO.transform.SetParent(transform, false);
        discGO.transform.localPosition = new Vector3(0f, -0.03f, 0f);
        Destroy(discGO.GetComponent<Collider>());
        SetMaterialColor(discGO.GetComponent<Renderer>(), Color.black);
        _disc = discGO.transform;

        // Colored rim ring
        var rimGO = new GameObject("Rim");
        rimGO.transform.SetParent(transform, false);
        rimGO.transform.localPosition = new Vector3(0f, 0.01f, 0f);
        var mf = rimGO.AddComponent<MeshFilter>();
        _rimRenderer = rimGO.AddComponent<MeshRenderer>();
        mf.sharedMesh = GetRingMesh();

        var mat = new Material(Shader.Find("Standard"));
        mat.color = HoleColor;
        mat.SetFloat("_Glossiness", 0.6f);
        mat.SetFloat("_Metallic", 0.2f);
        mat.SetColor("_EmissionColor", HoleColor * 0.4f);
        mat.EnableKeyword("_EMISSION");
        _rimRenderer.material = mat;

        UpdateVisualScale();
    }

    static void SetMaterialColor(Renderer r, Color c)
    {
        var mat = new Material(Shader.Find("Standard"));
        mat.color = c;
        mat.SetFloat("_Glossiness", 0.05f);
        r.material = mat;
    }

    // ── Per-frame ─────────────────────────────────────────────────────────────

    void Update()
    {
        if (!Alive) return;

        // Move toward target
        Vector3 pos = transform.position;
        pos.y = 0f;
        transform.position = Vector3.MoveTowards(pos, _targetPos, _moveSpeed * Time.deltaTime);

        // Smooth radius growth
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
        if (_disc != null)
            _disc.localScale = new Vector3(Radius * 2f, 0.02f, Radius * 2f);

        if (_rimRenderer != null)
            _rimRenderer.transform.localScale = new Vector3(Radius * 2f, 1f, Radius * 2f);
    }

    // ── Movement API (called by PlayerHole / AIHole) ──────────────────────────

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
        TargetRadius = Mathf.Max(GameManager.MIN_HOLE_RADIUS, GameManager.RadiusFromScore(this));
    }

    public void TakeDamage(float amount)
    {
        BonusRadius = Mathf.Max(0f, BonusRadius - amount);
        RecalcTargetRadius();
        _flashTimer = FLASH_DUR;
        if (_rimRenderer != null)
            _rimRenderer.material.color = Color.red;
    }

    public void Die()
    {
        Alive = false;
        gameObject.SetActive(false);
    }

    void ResetRimColor()
    {
        if (_rimRenderer != null)
            _rimRenderer.material.color = HoleColor;
    }

    // ── Ring mesh (shared, built once) ─────────────────────────────────────────

    static Mesh GetRingMesh()
    {
        if (_ringMesh != null) return _ringMesh;

        const int SEG = 64;
        const float OUTER = 1f;
        const float INNER = 0.82f;

        var verts = new Vector3[SEG * 2];
        var uvs   = new Vector2[SEG * 2];
        var tris  = new int[SEG * 6];

        for (int i = 0; i < SEG; i++)
        {
            float a = i / (float)SEG * Mathf.PI * 2f;
            float c = Mathf.Cos(a), s = Mathf.Sin(a);
            verts[i * 2]     = new Vector3(c * OUTER, 0f, s * OUTER);
            verts[i * 2 + 1] = new Vector3(c * INNER, 0f, s * INNER);
            uvs[i * 2]       = new Vector2(c * 0.5f + 0.5f,     s * 0.5f + 0.5f);
            uvs[i * 2 + 1]   = new Vector2(c * 0.41f + 0.5f,    s * 0.41f + 0.5f);
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
