using System.Collections.Generic;
using UnityEngine;

public class CityGenerator : MonoBehaviour
{
    // Ground-plane surfaces use GroundMasked (stencil hole cut-out).
    // All above-ground objects use URP Lit.
    private static readonly Dictionary<Color, Material> _matCache    = new Dictionary<Color, Material>();
    private static readonly Dictionary<Color, Material> _groundCache = new Dictionary<Color, Material>();

    // ── Colors ────────────────────────────────────────────────────────────────
    static readonly Color COL_GROUND   = new Color(0.28f, 0.28f, 0.28f);
    static readonly Color COL_ROAD     = new Color(0.18f, 0.18f, 0.18f);
    static readonly Color COL_SIDEWALK = new Color(0.52f, 0.51f, 0.49f);
    static readonly Color COL_GRASS    = new Color(0.25f, 0.50f, 0.22f);
    static readonly Color COL_GLASS    = new Color(0.50f, 0.65f, 0.80f);
    static readonly Color COL_BLDG1    = new Color(0.75f, 0.70f, 0.62f);
    static readonly Color COL_BLDG2    = new Color(0.55f, 0.62f, 0.70f);
    static readonly Color COL_BLDG3    = new Color(0.65f, 0.55f, 0.50f);

    static readonly Color[] CAR_COLORS =
    {
        new Color(0.85f, 0.15f, 0.15f),
        new Color(0.20f, 0.40f, 0.85f),
        new Color(0.92f, 0.92f, 0.92f),
        new Color(0.90f, 0.80f, 0.10f),
        new Color(0.38f, 0.38f, 0.38f),
        new Color(0.55f, 0.28f, 0.08f),
    };

    private Transform _cityRoot;

    public void Build()
    {
        _matCache.Clear();
        _groundCache.Clear();
        _cityRoot = new GameObject("City").transform;

        BuildGround();
        BuildRoads();
        BuildBlocks();
        SpawnCars(35);
    }

    // ── Ground + roads ────────────────────────────────────────────────────────

    void BuildGround()
    {
        var go = GameObject.CreatePrimitive(PrimitiveType.Plane);
        go.name = "Ground";
        go.transform.SetParent(_cityRoot, false);
        go.transform.localScale = new Vector3(GameManager.WORLD_SIZE / 10f, 1f,
                                               GameManager.WORLD_SIZE / 10f);
        ApplyGroundMat(go.GetComponent<Renderer>(), COL_GROUND);
        Destroy(go.GetComponent<Collider>());
    }

    void BuildRoads()
    {
        float half  = GameManager.HALF;
        float block = GameManager.BLOCK;
        float rw    = GameManager.ROAD_W;
        float thick = 0.04f;

        for (int i = -3; i <= 3; i++)
        {
            float centre = i * block;

            PlaceGroundBox("RoadV", _cityRoot,
                new Vector3(centre, thick / 2f, 0f),
                new Vector3(rw, thick, GameManager.WORLD_SIZE),
                COL_ROAD);

            PlaceGroundBox("RoadH", _cityRoot,
                new Vector3(0f, thick / 2f, centre),
                new Vector3(GameManager.WORLD_SIZE, thick, rw),
                COL_ROAD);
        }
    }

    // ── City blocks ───────────────────────────────────────────────────────────

    void BuildBlocks()
    {
        float block    = GameManager.BLOCK;
        float rw       = GameManager.ROAD_W;
        float interior = block - rw;

        for (int bx = -3; bx <= 3; bx++)
            for (int bz = -3; bz <= 3; bz++)
                BuildBlock(new Vector3(bx * block, 0f, bz * block), interior);
    }

    void BuildBlock(Vector3 centre, float size)
    {
        float half  = size / 2f - 1f;
        float thick = 0.03f;

        // Sidewalk slab (uses ground-masked shader)
        PlaceGroundBox("Sidewalk", _cityRoot,
            centre + Vector3.up * thick / 2f,
            new Vector3(size - 0.5f, thick, size - 0.5f),
            COL_SIDEWALK);

        float roll = Random.value;
        if (roll < 0.35f)
        {
            float h = Random.Range(14f, 24f);
            PlaceBuilding(centre + Vector3.up * h / 2f,
                new Vector3(Random.Range(6f, 9f), h, Random.Range(6f, 9f)),
                COL_GLASS, h);
        }
        else if (roll < 0.65f)
        {
            float h = Random.Range(7f, 13f);
            PlaceBuilding(centre + Vector3.up * h / 2f,
                new Vector3(Random.Range(7f, 11f), h, Random.Range(7f, 11f)),
                Random.value < 0.5f ? COL_BLDG1 : COL_BLDG2, h);
        }
        else
        {
            int count = Random.Range(2, 5);
            for (int k = 0; k < count; k++)
            {
                if (Random.value < 0.85f)
                {
                    Vector3 offset = new Vector3(Random.Range(-half, half), 0f,
                                                 Random.Range(-half, half));
                    float h = Random.Range(3f, 7f);
                    PlaceBuilding(centre + offset + Vector3.up * h / 2f,
                        new Vector3(Random.Range(3f, 6f), h, Random.Range(3f, 6f)),
                        COL_BLDG3, h);
                }
            }
        }

        PlaceSidewalkProps(centre, size);

        float[] corners = { -half, half };
        foreach (float cx in corners)
            foreach (float cz in corners)
                if (Random.value < 0.7f)
                    PlaceLamp(centre + new Vector3(cx, 0f, cz));
    }

    // ── Props ─────────────────────────────────────────────────────────────────

    void PlaceSidewalkProps(Vector3 blockCentre, float blockSize)
    {
        int   count = Random.Range(3, 7);
        float edge  = blockSize / 2f - 0.5f;

        for (int i = 0; i < count; i++)
        {
            int   side = Random.Range(0, 4);
            float pos1 = Random.Range(-edge, edge);
            Vector3 p  = blockCentre;
            switch (side)
            {
                case 0: p += new Vector3(pos1, 0f,  edge); break;
                case 1: p += new Vector3(pos1, 0f, -edge); break;
                case 2: p += new Vector3( edge, 0f, pos1); break;
                case 3: p += new Vector3(-edge, 0f, pos1); break;
            }
            PlaceRandomProp(p);
        }
    }

    void PlaceRandomProp(Vector3 pos)
    {
        float r = Random.value;
        if      (r < 0.25f) PlacePerson(pos);
        else if (r < 0.40f) PlaceCone(pos);
        else if (r < 0.55f) PlaceHydrant(pos);
        else if (r < 0.68f) PlaceTrashCan(pos);
        else if (r < 0.78f) PlaceMailbox(pos);
        else if (r < 0.86f) PlaceTree(pos);
        else                PlaceBench(pos);
    }

    void PlacePerson(Vector3 pos)
    {
        pos.y = 0f;
        var go = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        go.name = "Person";
        go.transform.SetParent(_cityRoot, false);
        go.transform.position   = pos + Vector3.up * 0.5f;
        go.transform.localScale = Vector3.one * 0.5f;
        ApplyLitMat(go.GetComponent<Renderer>(), new Color(0.85f, 0.70f, 0.58f));
        Destroy(go.GetComponent<Collider>());
        RegisterConsumable(go, 0.5f, 1, 10f, ObjectCategory.Person);
    }

    void PlaceCone(Vector3 pos)
    {
        pos.y = 0f;
        var go = CreateProp("Cone", pos + Vector3.up * 0.3f,
                            new Vector3(0.35f, 0.6f, 0.35f), new Color(0.95f, 0.45f, 0.05f));
        RegisterConsumable(go, 0.4f, 1, 8f, ObjectCategory.Prop);
    }

    void PlaceHydrant(Vector3 pos)
    {
        pos.y = 0f;
        var go = CreateProp("Hydrant", pos + Vector3.up * 0.35f,
                            new Vector3(0.45f, 0.7f, 0.45f), new Color(0.8f, 0.1f, 0.1f));
        RegisterConsumable(go, 0.5f, 1, 15f, ObjectCategory.Prop);
    }

    void PlaceTrashCan(Vector3 pos)
    {
        pos.y = 0f;
        var go = CreateProp("Trash", pos + Vector3.up * 0.35f,
                            new Vector3(0.5f, 0.7f, 0.5f), new Color(0.3f, 0.3f, 0.3f));
        RegisterConsumable(go, 0.55f, 1, 12f, ObjectCategory.Prop);
    }

    void PlaceMailbox(Vector3 pos)
    {
        pos.y = 0f;
        var go = CreateProp("Mailbox", pos + Vector3.up * 0.4f,
                            new Vector3(0.45f, 0.8f, 0.45f), new Color(0.1f, 0.3f, 0.8f));
        RegisterConsumable(go, 0.45f, 1, 10f, ObjectCategory.Prop);
    }

    void PlaceTree(Vector3 pos)
    {
        pos.y = 0f;
        var trunk = CreateProp("Trunk", pos + Vector3.up * 0.6f,
                               new Vector3(0.3f, 1.2f, 0.3f), new Color(0.35f, 0.22f, 0.12f));

        var canopy = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        canopy.name = "Canopy";
        canopy.transform.SetParent(_cityRoot, false);
        canopy.transform.position   = pos + Vector3.up * 1.8f;
        canopy.transform.localScale = new Vector3(1.4f, 1.4f, 1.4f);
        ApplyLitMat(canopy.GetComponent<Renderer>(), new Color(0.20f, 0.60f, 0.20f));
        Destroy(canopy.GetComponent<Collider>());

        RegisterConsumable(trunk, 1.1f, 2, 25f, ObjectCategory.Tree);
    }

    void PlaceBench(Vector3 pos)
    {
        pos.y = 0f;
        var go = CreateProp("Bench", pos + Vector3.up * 0.3f,
                            new Vector3(1.0f, 0.4f, 0.4f), new Color(0.50f, 0.35f, 0.18f));
        RegisterConsumable(go, 1.0f, 2, 20f, ObjectCategory.Prop);
    }

    void PlaceLamp(Vector3 pos)
    {
        pos.y = 0f;
        CreateProp("LampPole", pos + Vector3.up * 2.0f,
                   new Vector3(0.12f, 4.0f, 0.12f), new Color(0.6f, 0.6f, 0.6f));

        var go = CreateProp("Lamp", pos + Vector3.up * 0.5f,
                            new Vector3(0.4f, 1.0f, 0.4f), new Color(0.7f, 0.7f, 0.7f));
        RegisterConsumable(go, 0.9f, 2, 22f, ObjectCategory.Prop);
    }

    void PlaceBuilding(Vector3 pos, Vector3 size, Color color, float height)
    {
        var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
        go.name = "Building";
        go.transform.SetParent(_cityRoot, false);
        go.transform.position   = pos;
        go.transform.localScale = size;
        ApplyLitMat(go.GetComponent<Renderer>(), color);
        Destroy(go.GetComponent<Collider>());

        float buildSize  = Mathf.Max(size.x, size.z);
        float buildValue = height < 8f ? 120f : height < 15f ? 300f : 600f;
        RegisterConsumable(go, buildSize, height < 8f ? 4 : 5, buildValue, ObjectCategory.Building);
    }

    // ── Cars ──────────────────────────────────────────────────────────────────

    void SpawnCars(int count)
    {
        for (int i = 0; i < count; i++)
        {
            bool  horizontal = Random.value < 0.5f;
            float block      = GameManager.BLOCK;
            float roadCentre = Mathf.Round(Random.Range(-3, 4)) * block;
            float along      = Random.Range(-GameManager.HALF + 5f, GameManager.HALF - 5f);

            Vector3 pos = horizontal
                ? new Vector3(along, 0f, roadCentre)
                : new Vector3(roadCentre, 0f, along);

            SpawnCar(pos, horizontal);
        }
    }

    void SpawnCar(Vector3 pos, bool horizontal)
    {
        Color col = CAR_COLORS[Random.Range(0, CAR_COLORS.Length)];
        var go    = CreateProp("Car", pos + Vector3.up * 0.55f,
                               new Vector3(1.6f, 1.1f, 3.2f), col);
        if (!horizontal) go.transform.rotation = Quaternion.Euler(0f, 90f, 0f);
        RegisterConsumable(go, 1.8f, 3, 50f, ObjectCategory.Car);
    }

    // ── Helpers ───────────────────────────────────────────────────────────────

    GameObject CreateProp(string name, Vector3 pos, Vector3 size, Color color)
    {
        var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
        go.name = name;
        go.transform.SetParent(_cityRoot, false);
        go.transform.position   = pos;
        go.transform.localScale = size;
        ApplyLitMat(go.GetComponent<Renderer>(), color);
        Destroy(go.GetComponent<Collider>());
        return go;
    }

    // Ground-level boxes use GroundMasked so the hole cuts through them.
    void PlaceGroundBox(string name, Transform parent, Vector3 pos, Vector3 size, Color color)
    {
        var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
        go.name = name;
        go.transform.SetParent(parent, false);
        go.transform.position   = pos;
        go.transform.localScale = size;
        ApplyGroundMat(go.GetComponent<Renderer>(), color);
        Destroy(go.GetComponent<Collider>());
    }

    void RegisterConsumable(GameObject go, float size, int tier, float value, ObjectCategory cat)
    {
        var co = go.AddComponent<ConsumableObject>();
        co.Init(size, tier, value, cat);
        GameManager.Instance.AllObjects.Add(co);
    }

    // URP Lit material for above-ground objects (buildings, cars, props).
    static void ApplyLitMat(Renderer r, Color c)
    {
        if (!_matCache.TryGetValue(c, out var mat))
        {
            mat = new Material(Shader.Find("Universal Render Pipeline/Lit"));
            mat.SetColor("_BaseColor", c);
            mat.SetFloat("_Smoothness", 0.15f);
            mat.SetFloat("_Metallic",   0.0f);
            _matCache[c] = mat;
        }
        r.sharedMaterial = mat;
    }

    // GroundMasked material for roads, sidewalks, ground plane.
    // These disappear inside the hole (stencil NotEqual 1).
    static void ApplyGroundMat(Renderer r, Color c)
    {
        if (!_groundCache.TryGetValue(c, out var mat))
        {
            mat = new Material(Shader.Find("DowntownDevour/GroundMasked"));
            mat.SetColor("_BaseColor", c);
            _groundCache[c] = mat;
        }
        r.sharedMaterial = mat;
    }
}
