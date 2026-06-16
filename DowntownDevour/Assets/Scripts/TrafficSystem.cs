using System.Collections.Generic;
using UnityEngine;

// Spawns and manages moving traffic cars during Evening and Night.
// Each car is a full ConsumableObject (so the hole can eat it) plus a CarDriver
// for movement. Cars wrap at world edges and crash when they meet head-on.
public class TrafficSystem : MonoBehaviour
{
    readonly List<GameObject> _cars = new List<GameObject>();
    bool _running;

    // Road grid: 7 roads in X (vertical), 7 roads in Z (horizontal).
    // Road centres at i * BLOCK for i in [-3, 3].  Lane offset ±2 from centre.
    const float LANE_OFFSET = 2.0f;
    const float START_PAD   = 8f;   // keep cars off the very edge at spawn

    public void StartTraffic()
    {
        if (_running) return;
        _running = true;
        SpawnAllLanes();
    }

    public void StopTraffic()
    {
        _running = false;
        foreach (var go in _cars)
            if (go != null) Destroy(go);
        _cars.Clear();
    }

    void SpawnAllLanes()
    {
        float block = GameManager.BLOCK;
        float half  = GameManager.HALF - START_PAD;

        for (int i = -3; i <= 3; i++)
        {
            float road = i * block;

            // N/S roads (constant X): lane A travels +Z, lane B travels -Z.
            SpawnLane(road - LANE_OFFSET, 0f, new Vector3(0f, 0f, 1f), half);
            SpawnLane(road + LANE_OFFSET, 0f, new Vector3(0f, 0f, -1f), half);

            // E/W roads (constant Z): lane A travels +X, lane B travels -X.
            SpawnLane(0f, road - LANE_OFFSET, new Vector3(1f, 0f, 0f), half);
            SpawnLane(0f, road + LANE_OFFSET, new Vector3(-1f, 0f, 0f), half);
        }
    }

    // xOff / zOff: which of the two perpendicular coords is the lane's fixed coord.
    // dir: unit drive direction.  One coord of dir is 0 (the fixed lane coord).
    void SpawnLane(float xLane, float zLane, Vector3 dir, float half)
    {
        int count = Random.Range(1, 3); // 1-2 cars per lane
        for (int i = 0; i < count; i++)
        {
            float along = Random.Range(-half, half);
            Vector3 pos = dir.x != 0
                ? new Vector3(along, 0f, zLane)
                : new Vector3(xLane, 0f, along);

            var go = BuildTrafficCar(pos, dir);
            _cars.Add(go);
        }
    }

    // ── Car construction ──────────────────────────────────────────────────────
    // Builds a simplified car visual (body + headlights) and registers it as a
    // ConsumableObject so the hole can eat it exactly like parked cars.

    GameObject BuildTrafficCar(Vector3 pos, Vector3 dir)
    {
        Color[] palette = {
            new Color(0.28f, 0.06f, 0.06f),
            new Color(0.08f, 0.12f, 0.30f),
            new Color(0.18f, 0.18f, 0.20f),
            new Color(0.26f, 0.22f, 0.04f),
            new Color(0.10f, 0.10f, 0.11f),
            new Color(0.20f, 0.10f, 0.04f),
        };

        Color col   = palette[Random.Range(0, palette.Length)];
        Color dark  = Sc(col, 0.52f);
        Color glass = new Color(0.14f, 0.22f, 0.32f);

        var root = new GameObject("TrafficCar");
        root.transform.position   = pos;
        root.transform.localScale = Vector3.one * 0.60f;

        // Rotate so local +Z aligns with drive direction
        if      (Mathf.Abs(dir.x) > 0.5f)
            root.transform.rotation = Quaternion.Euler(0f, dir.x > 0 ? 90f : -90f, 0f);
        // dir.z: +Z → no rotation, -Z → 180°
        else if (dir.z < 0f)
            root.transform.rotation = Quaternion.Euler(0f, 180f, 0f);

        // Body
        AddBox(root, new Vector3(0f, 0.52f, 0f), new Vector3(1.70f, 0.62f, 3.50f), col);
        AddBox(root, new Vector3(0f, 1.18f, 0f), new Vector3(1.52f, 0.54f, 2.20f), col);
        AddBox(root, new Vector3(0f, 1.18f,  1.08f), new Vector3(1.35f, 0.42f, 0.08f), glass);
        AddBox(root, new Vector3(0f, 1.18f, -1.08f), new Vector3(1.35f, 0.42f, 0.08f), glass);
        AddBox(root, new Vector3(0f, 0.35f,  1.78f), new Vector3(1.55f, 0.30f, 0.20f), dark);
        AddBox(root, new Vector3(0f, 0.35f, -1.78f), new Vector3(1.55f, 0.30f, 0.20f), dark);

        // Emissive headlights + taillights
        Color headBase = new Color(0.95f, 0.95f, 0.88f);
        Color headEmit = new Color(2.0f,  1.95f, 1.60f);
        Color tailBase = new Color(0.80f, 0.05f, 0.05f);
        Color tailEmit = new Color(1.80f, 0.08f, 0.08f);
        AddEmissiveBox(root, new Vector3( 0.60f, 0.55f,  1.80f), new Vector3(0.34f, 0.22f, 0.06f), headBase, headEmit);
        AddEmissiveBox(root, new Vector3(-0.60f, 0.55f,  1.80f), new Vector3(0.34f, 0.22f, 0.06f), headBase, headEmit);
        AddEmissiveBox(root, new Vector3( 0.60f, 0.55f, -1.80f), new Vector3(0.34f, 0.22f, 0.06f), tailBase, tailEmit);
        AddEmissiveBox(root, new Vector3(-0.60f, 0.55f, -1.80f), new Vector3(0.34f, 0.22f, 0.06f), tailBase, tailEmit);

        // Headlight spotlight — cone down the road ahead
        var hlGO = new GameObject("HeadLight");
        hlGO.transform.SetParent(root.transform, false);
        hlGO.transform.localPosition = new Vector3(0f, 0.55f, 1.85f);
        var hl = hlGO.AddComponent<Light>();
        hl.type           = LightType.Spot;
        hl.spotAngle      = 32f;
        hl.innerSpotAngle = 10f;
        hl.color          = new Color(1.0f, 0.95f, 0.85f);
        hl.intensity      = 26f;
        hl.range          = 30f;
        hl.shadows        = LightShadows.None;

        // Red taillight glow
        var tlGO = new GameObject("TailLight");
        tlGO.transform.SetParent(root.transform, false);
        tlGO.transform.localPosition = new Vector3(0f, 0.55f, -1.85f);
        var tl = tlGO.AddComponent<Light>();
        tl.type      = LightType.Point;
        tl.color     = new Color(1.0f, 0.05f, 0.05f);
        tl.intensity = 3f;
        tl.range     = 8f;
        tl.shadows   = LightShadows.None;

        // Wheels (minimal — just the tires)
        Color tire = new Color(0.08f, 0.08f, 0.09f);
        foreach (var wp in new Vector3[] {
            new Vector3( 0.85f, 0.28f,  1.15f), new Vector3(-0.85f, 0.28f,  1.15f),
            new Vector3( 0.85f, 0.28f, -1.15f), new Vector3(-0.85f, 0.28f, -1.15f) })
        {
            AddCylinder(root, wp, Quaternion.Euler(0f, 0f, 90f),
                new Vector3(0.52f, 0.22f, 0.52f), tire);
        }

        // Bounding collider + Rigidbody so the hole can swallow this car
        var bc = root.AddComponent<BoxCollider>();
        bc.center = new Vector3(0f, 0.85f, 0f);
        bc.size   = new Vector3(1.80f, 1.70f, 3.60f);

        var rb            = root.AddComponent<Rigidbody>();
        rb.mass           = 1.2f;
        rb.linearDamping  = 0.05f;
        rb.angularDamping = 0.8f;
        rb.isKinematic    = true;   // CarDriver moves it manually; hole releases it

        var co = root.AddComponent<ConsumableObject>();
        co.Init(1.1f, 3, 50f, ObjectCategory.Car, 1.0f);
        GameManager.Instance.AllObjects.Add(co);

        var driver = root.AddComponent<CarDriver>();
        driver.DriveDir   = dir;
        driver.Speed      = Random.Range(9f, 15f);
        driver.Consumable = co;

        return root;
    }

    // ── Primitive helpers (no external deps) ─────────────────────────────────

    static void AddBox(GameObject parent, Vector3 lp, Vector3 ls, Color col)
    {
        var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
        go.transform.SetParent(parent.transform, false);
        go.transform.localPosition = lp;
        go.transform.localScale    = ls;
        go.GetComponent<Renderer>().sharedMaterial = LitMat(col);
        Object.Destroy(go.GetComponent<Collider>());
    }

    static void AddEmissiveBox(GameObject parent, Vector3 lp, Vector3 ls, Color baseCol, Color emit)
    {
        var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
        go.transform.SetParent(parent.transform, false);
        go.transform.localPosition = lp;
        go.transform.localScale    = ls;
        go.GetComponent<Renderer>().material = EmissiveMat(baseCol, emit);
        Object.Destroy(go.GetComponent<Collider>());
    }

    static void AddCylinder(GameObject parent, Vector3 lp, Quaternion lr, Vector3 ls, Color col)
    {
        var go = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        go.transform.SetParent(parent.transform, false);
        go.transform.localPosition = lp;
        go.transform.localRotation = lr;
        go.transform.localScale    = ls;
        go.GetComponent<Renderer>().sharedMaterial = LitMat(col);
        Object.Destroy(go.GetComponent<Collider>());
    }

    static Material LitMat(Color c)
    {
        var mat = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        mat.SetColor("_BaseColor", c);
        mat.SetFloat("_Smoothness", 0.45f);
        return mat;
    }

    static Material EmissiveMat(Color baseCol, Color emit)
    {
        var mat = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        mat.SetColor("_BaseColor",     baseCol);
        mat.SetFloat("_Smoothness",    0.60f);
        mat.EnableKeyword("_EMISSION");
        mat.SetColor("_EmissionColor", emit);
        return mat;
    }

    static Color Sc(Color c, float f)
        => new Color(Mathf.Clamp01(c.r * f), Mathf.Clamp01(c.g * f), Mathf.Clamp01(c.b * f));
}
