using System.Collections.Generic;
using UnityEngine;

public class CityGenerator : MonoBehaviour
{
    static readonly Dictionary<string, Material> _litCache      = new Dictionary<string, Material>();
    static readonly Dictionary<string, Material> _groundCache   = new Dictionary<string, Material>();
    static readonly Dictionary<string, Material> _emissiveCache = new Dictionary<string, Material>();

    // ── Palette ───────────────────────────────────────────────────────────
    static readonly Color COL_GROUND    = new Color(0.22f, 0.22f, 0.22f);
    static readonly Color COL_ROAD      = new Color(0.16f, 0.16f, 0.16f);
    static readonly Color COL_SIDEWALK  = new Color(0.14f, 0.14f, 0.13f);  // dark wet concrete
    static readonly Color COL_LANE_MRK  = new Color(0.90f, 0.80f, 0.15f);
    static readonly Color COL_CROSSWALK = new Color(0.92f, 0.92f, 0.90f);

    // Night city palette — dark concrete, smoked glass, deep stone
    static readonly Color COL_GLASS = new Color(0.08f, 0.12f, 0.18f);  // near-black tinted glass
    static readonly Color COL_BLDG1 = new Color(0.22f, 0.20f, 0.17f);  // dark concrete/stone
    static readonly Color COL_BLDG2 = new Color(0.15f, 0.18f, 0.24f);  // dark blue-grey brutalist
    static readonly Color COL_BLDG3 = new Color(0.20f, 0.15f, 0.12f);  // dark terracotta brick

    static readonly Color[] CAR_COLORS = {
        new Color(0.85f, 0.15f, 0.15f),
        new Color(0.20f, 0.40f, 0.85f),
        new Color(0.92f, 0.92f, 0.92f),
        new Color(0.90f, 0.80f, 0.10f),
        new Color(0.38f, 0.38f, 0.38f),
        new Color(0.55f, 0.28f, 0.08f),
    };

    private Transform _cityRoot;

    // ── Entry ─────────────────────────────────────────────────────────────
    public void Build()
    {
        _litCache.Clear();
        _groundCache.Clear();
        _emissiveCache.Clear();
        _cityRoot = new GameObject("City").transform;
        BuildGround();
        BuildBoundaryWalls();
        BuildRoads();
        BuildBlocks();
        SpawnCars(50);
    }

    // ── Boundary walls ────────────────────────────────────────────────────
    // Invisible colliders around the playfield so physics debris can never
    // be launched off the map — beyond the edge there is no ground to land
    // on or hide it, and it would tumble through the void in plain sight.
    void BuildBoundaryWalls()
    {
        float half = GameManager.HALF;
        const float H = 80f, T = 2f;
        float len = GameManager.WORLD_SIZE + T * 2f;

        Vector3[] centers = {
            new Vector3( half + T / 2f, H / 2f, 0f),
            new Vector3(-half - T / 2f, H / 2f, 0f),
            new Vector3(0f, H / 2f,  half + T / 2f),
            new Vector3(0f, H / 2f, -half - T / 2f),
        };
        Vector3[] sizes = {
            new Vector3(T, H, len),
            new Vector3(T, H, len),
            new Vector3(len, H, T),
            new Vector3(len, H, T),
        };

        for (int i = 0; i < 4; i++)
        {
            var wall = new GameObject("BoundaryWall");
            wall.transform.SetParent(_cityRoot, false);
            wall.transform.position = centers[i];
            wall.AddComponent<BoxCollider>().size = sizes[i];
        }
    }

    // ── Ground ────────────────────────────────────────────────────────────
    void BuildGround()
    {
        var go = GameObject.CreatePrimitive(PrimitiveType.Plane);
        go.name = "Ground";
        go.transform.SetParent(_cityRoot, false);
        go.transform.localScale = Vector3.one * (GameManager.WORLD_SIZE / 10f);
        go.GetComponent<Renderer>().sharedMaterial = MkGroundMat(COL_GROUND);
        // Keep MeshCollider so physics debris lands on the ground
    }

    // ── Roads ─────────────────────────────────────────────────────────────
    void BuildRoads()
    {
        float block = GameManager.BLOCK, rw = GameManager.ROAD_W;
        float half  = GameManager.HALF;
        float thick = 0.04f, dashH = 0.055f;
        float dashLen = 2.0f, dashGap = 1.8f, dashW = 0.18f;

        for (int i = -3; i <= 3; i++)
        {
            float c = i * block;
            GroundBox("RoadV", new Vector3(c, thick/2f, 0f),
                new Vector3(rw, thick, GameManager.WORLD_SIZE), COL_ROAD);
            GroundBox("RoadH", new Vector3(0f, thick/2f, c),
                new Vector3(GameManager.WORLD_SIZE, thick, rw), COL_ROAD);

            for (float p = -half + dashLen/2f; p < half; p += dashLen + dashGap)
            {
                GroundBox("DV", new Vector3(c, dashH/2f, p),
                    new Vector3(dashW, dashH, dashLen), COL_LANE_MRK);
                GroundBox("DH", new Vector3(p, dashH/2f, c),
                    new Vector3(dashLen, dashH, dashW), COL_LANE_MRK);
            }
        }

        // Crosswalk stripes at every intersection
        float cwW = 0.55f, cwH = 0.06f, cwDepth = 1.5f;
        for (int ix = -3; ix <= 3; ix++)
        for (int iz = -3; iz <= 3; iz++)
        {
            float cx = ix * block, cz = iz * block;
            float ofs = rw/2f + 0.4f;
            for (int s = -1; s <= 1; s += 2)
            {
                float o = s * 1.1f;
                GroundBox("CwN", new Vector3(cx + o, cwH/2f, cz + ofs),
                    new Vector3(cwW, cwH, cwDepth), COL_CROSSWALK);
                GroundBox("CwS", new Vector3(cx + o, cwH/2f, cz - ofs),
                    new Vector3(cwW, cwH, cwDepth), COL_CROSSWALK);
                GroundBox("CwE", new Vector3(cx + ofs, cwH/2f, cz + o),
                    new Vector3(cwDepth, cwH, cwW), COL_CROSSWALK);
                GroundBox("CwW", new Vector3(cx - ofs, cwH/2f, cz + o),
                    new Vector3(cwDepth, cwH, cwW), COL_CROSSWALK);
            }
        }
    }

    // ── Blocks ────────────────────────────────────────────────────────────
    void BuildBlocks()
    {
        float block = GameManager.BLOCK, rw = GameManager.ROAD_W;
        float interior = block - rw;
        for (int bx = -3; bx <= 3; bx++)
        for (int bz = -3; bz <= 3; bz++)
            BuildBlock(new Vector3(bx * block, 0f, bz * block), interior);
    }

    void BuildBlock(Vector3 centre, float size)
    {
        float half = size / 2f - 1f;
        GroundBox("Sidewalk", centre + Vector3.up * 0.015f,
            new Vector3(size - 0.5f, 0.03f, size - 0.5f), COL_SIDEWALK);

        float roll = Random.value;
        if (roll < 0.35f)
        {
            float h = Random.Range(16f, 26f);
            PlaceBuilding(centre, Random.Range(6f, 9f), Random.Range(6f, 9f), COL_GLASS, h);
        }
        else if (roll < 0.65f)
        {
            float h  = Random.Range(7f, 14f);
            float bw = Random.Range(7f, 11f), bd = Random.Range(7f, 11f);
            PlaceBuilding(centre, bw, bd, Random.value < 0.5f ? COL_BLDG1 : COL_BLDG2, h);
        }
        else
        {
            int count = Random.Range(2, 5);
            for (int k = 0; k < count; k++)
                if (Random.value < 0.85f)
                {
                    var ofs = new Vector3(Random.Range(-half + 2f, half - 2f), 0f,
                                         Random.Range(-half + 2f, half - 2f));
                    float h  = Random.Range(3f, 7f);
                    PlaceBuilding(centre + ofs, Random.Range(3f, 6f), Random.Range(3f, 6f), COL_BLDG3, h);
                }
        }

        PlaceSidewalkProps(centre, size);

        float[] sides = { -half, half };
        foreach (float cx in sides)
        foreach (float cz in sides)
            if (Random.value < 0.7f)
                PlaceLamp(centre + new Vector3(cx, 0f, cz));
    }

    // ── Building construction ─────────────────────────────────────────────
    void PlaceBuilding(Vector3 base0, float bw, float bd, Color col, float height)
    {
        bool isGlass = col == COL_GLASS;
        bool isTall  = height >= 14f;
        bool isMed   = height >= 7f;

        var root = Root("Building", base0);
        float sm = isGlass ? 0.75f : 0.12f;
        float mt = isGlass ? 0.05f : 0f;

        // Podium
        float podiumH = 0f;
        if (isTall) podiumH = Mathf.Max(2.5f, height * 0.10f);
        else if (isMed) podiumH = 1.5f;

        if (podiumH > 0f)
        {
            float extra = isTall ? 1.4f : 0.8f;
            Box(root, "Podium", Y(podiumH/2f),
                new Vector3(bw + extra, podiumH, bd + extra), Sc(col, 0.78f), 0.18f);
        }

        // Main shaft
        float shaftH = isTall ? height * 0.65f : (isMed ? height * 0.80f : height * 0.88f);
        float shaftMid = podiumH + shaftH / 2f;
        Box(root, "Shaft", Y(shaftMid), new Vector3(bw, shaftH, bd), col, sm, mt);

        // Horizontal floor bands
        Color band = Sc(col, isGlass ? 1.38f : 1.22f);
        for (float fy = podiumH + 2.5f; fy < podiumH + shaftH - 0.5f; fy += 2.5f)
            Box(root, "Band", Y(fy), new Vector3(bw + 0.08f, 0.10f, bd + 0.08f), band, 0.55f);

        // Interior floor slabs — hidden behind exterior skin, revealed when building breaks
        // Concrete color contrasts with glass/brick exterior so player can see the building had mass
        Color interior = new Color(0.68f, 0.62f, 0.50f);
        float slabStep = height < 8f ? 3.2f : height < 15f ? 2.8f : 2.5f;
        for (float fy = podiumH + slabStep * 0.5f; fy < podiumH + shaftH - 0.3f; fy += slabStep)
            Box(root, "Slab", new Vector3(0f, fy, 0f),
                new Vector3(bw * 0.78f, 0.20f, bd * 0.78f), interior, 0.12f);

        // Window bays on all 4 faces — emissive for night glow
        if (height > 5f)
        {
            // Base (dark) window color
            Color winBase = new Color(0.06f, 0.07f, 0.10f);
            // Emissive: warm amber for most buildings, cool blue-white for glass towers
            Color winEmit = isGlass
                ? new Color(0.55f, 0.72f, 1.40f)   // cool neon-blue HDR
                : new Color(1.20f, 0.85f, 0.30f);   // warm amber HDR (>1 triggers bloom)

            float bayH = shaftH * 0.76f;
            float bayY = podiumH + shaftH * 0.50f;
            float ep   = 0.04f;
            int   nW   = Mathf.Max(2, Mathf.RoundToInt(bw / 2.8f));
            int   nD   = Mathf.Max(2, Mathf.RoundToInt(bd / 2.8f));
            float wW   = bw / (nW + 1f) * 0.58f;
            float wD   = bd / (nD + 1f) * 0.58f;

            for (int wi = 1; wi <= nW; wi++)
            {
                float x = -bw/2f + bw / (nW + 1f) * wi;
                EmissiveBox(root, "WF", new Vector3(x, bayY,  bd/2f + ep), new Vector3(wW, bayH, 0.07f), winBase, winEmit);
                EmissiveBox(root, "WB", new Vector3(x, bayY, -bd/2f - ep), new Vector3(wW, bayH, 0.07f), winBase, winEmit);
            }
            for (int wi = 1; wi <= nD; wi++)
            {
                float z = -bd/2f + bd / (nD + 1f) * wi;
                EmissiveBox(root, "WR", new Vector3( bw/2f + ep, bayY, z), new Vector3(0.07f, bayH, wD), winBase, winEmit);
                EmissiveBox(root, "WL", new Vector3(-bw/2f - ep, bayY, z), new Vector3(0.07f, bayH, wD), winBase, winEmit);
            }
        }

        // Upper sections
        float topOfShaft = podiumH + shaftH;

        if (isTall)
        {
            float midH = height * 0.18f;
            float midW = bw * 0.75f, midD = bd * 0.75f;
            Box(root, "Mid", Y(topOfShaft + midH/2f),
                new Vector3(midW, midH, midD), col, isGlass ? 0.82f : 0.15f, mt);

            for (float fy = topOfShaft + 2.5f; fy < topOfShaft + midH - 0.5f; fy += 2.5f)
                Box(root, "Band2", Y(fy), new Vector3(midW + 0.06f, 0.10f, midD + 0.06f), band, 0.45f);

            float topStart = topOfShaft + midH;
            if (isGlass)
            {
                float topH = height * 0.10f;
                Box(root, "Top", Y(topStart + topH/2f),
                    new Vector3(midW * 0.60f, topH, midD * 0.60f), Sc(col, 1.22f), 0.90f, 0.12f);
                Box(root, "Spire", Y(topStart + topH + 2.2f),
                    new Vector3(0.22f, 4.0f, 0.22f), new Color(0.78f, 0.80f, 0.86f), 0.85f, 0.65f);
            }
            else
            {
                float crownH = height * 0.08f;
                Box(root, "Crown", Y(topStart + crownH/2f),
                    new Vector3(midW * 0.82f, crownH, midD * 0.82f), Sc(col, 0.88f), 0.18f);
                if (Random.value < 0.30f) WaterTower(root, topStart + crownH);
            }
        }
        else if (isMed)
        {
            Box(root, "Parapet", Y(topOfShaft + 0.35f),
                new Vector3(bw + 0.4f, 0.70f, bd + 0.4f), Sc(col, 0.82f), 0.15f);
            if (Random.value < 0.55f)
                Box(root, "HVAC",
                    new Vector3(Random.Range(-bw/4f, bw/4f), topOfShaft + 0.5f, Random.Range(-bd/4f, bd/4f)),
                    new Vector3(Random.Range(0.9f, 1.6f), 0.75f, Random.Range(1.1f, 2.0f)),
                    new Color(0.72f, 0.73f, 0.74f), 0.18f);
            if (Random.value < 0.20f) WaterTower(root, topOfShaft + 0.7f);
        }
        else
        {
            Box(root, "Parapet", Y(topOfShaft + 0.25f),
                new Vector3(bw + 0.25f, 0.45f, bd + 0.25f), Sc(col, 0.78f), 0.10f);
            if (Random.value < 0.40f)
                Box(root, "HVAC",
                    new Vector3(Random.Range(-bw/4f, bw/4f), topOfShaft + 0.35f, Random.Range(-bd/4f, bd/4f)),
                    new Vector3(0.9f, 0.65f, 1.4f), new Color(0.72f, 0.73f, 0.74f), 0.15f);
        }

        // No single-trigger consumable on the building root — BuildingCollapse
        // handles each piece individually as the hole sweeps under the building.
        // footprintRad is used for a cheap bounding-circle pre-check each frame.
        float footprintRad = Mathf.Sqrt(bw * bw + bd * bd) * 0.5f + 0.5f;

        // A hole has to be a meaningful fraction of the building's size to
        // bring it down; smaller holes only rattle it.  Bigger and taller
        // buildings demand a bigger hole.
        float minHole = Mathf.Clamp(0.30f * Mathf.Min(bw, bd) + 0.05f * height, 0.8f, 5f);

        var collapse = root.AddComponent<BuildingCollapse>();
        collapse.Init(footprintRad, minHole);
        foreach (Transform child in root.transform)
            collapse.RegisterPart(child);
    }

    void WaterTower(GameObject parent, float baseY)
    {
        float ox = Random.Range(-1.0f, 1.0f);
        float oz = Random.Range(-1.0f, 1.0f);
        Color wood = new Color(0.52f, 0.38f, 0.22f);
        Color dark = new Color(0.38f, 0.26f, 0.14f);

        Prim(PrimitiveType.Cylinder, parent, "WTBarrel",
            new Vector3(ox, baseY + 1.5f, oz), Quaternion.identity,
            new Vector3(1.2f, 1.5f, 1.2f), wood, 0.05f);
        Prim(PrimitiveType.Sphere, parent, "WTRoof",
            new Vector3(ox, baseY + 3.3f, oz), Quaternion.identity,
            new Vector3(1.4f, 0.5f, 1.4f), dark, 0.05f);
        for (int i = 0; i < 4; i++)
        {
            float a = i * 90f * Mathf.Deg2Rad;
            Prim(PrimitiveType.Cylinder, parent, "WTLeg",
                new Vector3(ox + Mathf.Sin(a)*0.55f, baseY + 0.5f, oz + Mathf.Cos(a)*0.55f),
                Quaternion.identity, new Vector3(0.07f, 0.55f, 0.07f), dark, 0.05f);
        }
    }

    // ── Sidewalk props ────────────────────────────────────────────────────
    void PlaceSidewalkProps(Vector3 blockCentre, float blockSize)
    {
        int   count = Random.Range(4, 9);
        float edge  = blockSize / 2f - 0.5f;
        for (int i = 0; i < count; i++)
        {
            int   side = Random.Range(0, 4);
            float p1   = Random.Range(-edge + 1f, edge - 1f);
            var   p    = blockCentre;
            switch (side)
            {
                case 0: p += new Vector3(p1, 0f,  edge); break;
                case 1: p += new Vector3(p1, 0f, -edge); break;
                case 2: p += new Vector3( edge, 0f, p1); break;
                case 3: p += new Vector3(-edge, 0f, p1); break;
            }
            PlaceRandomProp(p);
        }
    }

    void PlaceRandomProp(Vector3 pos)
    {
        float r = Random.value;
        if      (r < 0.22f) PlacePerson(pos);
        else if (r < 0.38f) PlaceTree(pos);
        else if (r < 0.50f) PlaceHydrant(pos);
        else if (r < 0.62f) PlaceTrashCan(pos);
        else if (r < 0.72f) PlaceMailbox(pos);
        else if (r < 0.80f) PlaceCone(pos);
        else                PlaceBench(pos);
    }

    void PlacePerson(Vector3 pos)
    {
        pos.y = 0f;
        var root = Root("Person", pos);
        root.transform.rotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);

        Color[] clothColors = {
            new Color(0.80f, 0.18f, 0.18f), new Color(0.18f, 0.35f, 0.80f),
            new Color(0.18f, 0.58f, 0.28f), new Color(0.72f, 0.58f, 0.10f),
            new Color(0.58f, 0.18f, 0.58f), new Color(0.10f, 0.48f, 0.60f),
            new Color(0.80f, 0.48f, 0.08f), new Color(0.28f, 0.28f, 0.30f),
        };
        Color cloth = clothColors[Random.Range(0, clothColors.Length)];
        Color skin  = new Color(Random.Range(0.60f, 0.92f), Random.Range(0.42f, 0.72f),
                                Random.Range(0.32f, 0.55f));

        Prim(PrimitiveType.Capsule, root, "Body", Y(0.58f), Quaternion.identity,
            new Vector3(0.34f, 0.58f, 0.34f), cloth, 0.08f);
        Prim(PrimitiveType.Sphere,  root, "Head", Y(1.30f), Quaternion.identity,
            Vector3.one * 0.28f, skin, 0.10f);

        Consumable(root, 0.5f, 1, 10f, ObjectCategory.Person, 0.2f, mass: 0.15f);
    }

    void PlaceTree(Vector3 pos)
    {
        pos.y = 0f;
        var root = Root("Tree", pos);

        Prim(PrimitiveType.Cylinder, root, "Trunk", Y(0.65f), Quaternion.identity,
            new Vector3(0.22f, 0.65f, 0.22f), new Color(0.38f, 0.24f, 0.13f), 0.05f);

        float[] yo = { 1.5f, 2.2f, 2.8f };
        float[] ro = { 1.2f, 1.0f, 0.65f };
        Color[] gr = {
            new Color(0.17f, 0.55f, 0.14f),
            new Color(0.22f, 0.63f, 0.18f),
            new Color(0.28f, 0.70f, 0.23f),
        };
        for (int i = 0; i < 3; i++)
            Prim(PrimitiveType.Sphere, root, "Canopy",
                new Vector3(Random.Range(-0.12f, 0.12f), yo[i], Random.Range(-0.12f, 0.12f)),
                Quaternion.identity, Vector3.one * ro[i] * 2f, gr[i], 0.05f);

        Consumable(root, 1.1f, 2, 25f, ObjectCategory.Tree, 0.9f, mass: 1.5f);
    }

    void PlaceLamp(Vector3 pos)
    {
        pos.y = 0f;
        var root = Root("Lamp", pos);
        Color metal = new Color(0.28f, 0.30f, 0.34f);  // dark metal at night

        Prim(PrimitiveType.Cylinder, root, "Pole", Y(2.5f), Quaternion.identity,
            new Vector3(0.09f, 2.5f, 0.09f), metal, 0.4f, 0.4f);
        Prim(PrimitiveType.Cylinder, root, "Arm", new Vector3(0.5f, 4.9f, 0f),
            Quaternion.Euler(0f, 0f, 90f), new Vector3(0.07f, 0.5f, 0.07f), metal, 0.4f, 0.4f);

        // Emissive globe — warm amber HDR so bloom halos around it
        Color globeBase = new Color(0.98f, 0.90f, 0.60f);
        Color globeEmit = new Color(2.0f,  1.60f, 0.60f);  // HDR amber
        EmissivePrim(PrimitiveType.Sphere, root, "Globe", new Vector3(0.9f, 4.72f, 0f),
            Quaternion.identity, new Vector3(0.38f, 0.28f, 0.38f), globeBase, globeEmit, 0.75f);

        // Point light — casts amber pool on the street below
        var lightGO = new GameObject("LampLight");
        lightGO.transform.SetParent(root.transform, false);
        lightGO.transform.localPosition = new Vector3(0.9f, 4.5f, 0f);
        var pt = lightGO.AddComponent<Light>();
        pt.type      = LightType.Point;
        pt.color     = new Color(1.0f, 0.78f, 0.35f);
        pt.intensity = 3.5f;
        pt.range     = 12f;
        pt.shadows   = LightShadows.None;  // performance — many lamps in scene

        Consumable(root, 0.9f, 2, 22f, ObjectCategory.Prop, 0.4f, mass: 0.8f);
    }

    void PlaceHydrant(Vector3 pos)
    {
        pos.y = 0f;
        var root = Root("Hydrant", pos);
        Color red = new Color(0.80f, 0.12f, 0.12f);

        Prim(PrimitiveType.Cylinder, root, "Body", Y(0.36f), Quaternion.identity,
            new Vector3(0.36f, 0.36f, 0.36f), red, 0.5f, 0.1f);
        Prim(PrimitiveType.Sphere, root, "Cap", Y(0.80f), Quaternion.identity,
            new Vector3(0.30f, 0.22f, 0.30f), Sc(red, 0.80f), 0.5f, 0.1f);

        Consumable(root, 0.5f, 1, 15f, ObjectCategory.Prop, 0.2f, mass: 1.0f);
    }

    void PlaceTrashCan(Vector3 pos)
    {
        pos.y = 0f;
        var root = Root("Trash", pos);
        Color dark = new Color(0.22f, 0.22f, 0.24f);

        Prim(PrimitiveType.Cylinder, root, "Body", Y(0.40f), Quaternion.identity,
            new Vector3(0.38f, 0.40f, 0.38f), dark, 0.10f);
        Prim(PrimitiveType.Cylinder, root, "Lid", Y(0.82f), Quaternion.identity,
            new Vector3(0.42f, 0.06f, 0.42f), Sc(dark, 1.2f), 0.15f);

        Consumable(root, 0.55f, 1, 12f, ObjectCategory.Prop, 0.22f, mass: 0.2f);
    }

    void PlaceMailbox(Vector3 pos)
    {
        pos.y = 0f;
        var root = Root("Mailbox", pos);
        root.transform.rotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
        Color blue = new Color(0.10f, 0.28f, 0.80f);

        Box(root, "Post", new Vector3(0f, 0.60f, 0f),
            new Vector3(0.12f, 1.2f, 0.12f), new Color(0.30f, 0.30f, 0.32f), 0.30f, 0.30f);
        Box(root, "Box", new Vector3(0f, 1.30f, 0f),
            new Vector3(0.52f, 0.42f, 0.72f), blue, 0.35f, 0.10f);
        Prim(PrimitiveType.Sphere, root, "Dome", new Vector3(0f, 1.52f, 0f),
            Quaternion.identity, new Vector3(0.52f, 0.22f, 0.72f), Sc(blue, 0.82f), 0.40f);

        Consumable(root, 0.45f, 1, 10f, ObjectCategory.Prop, 0.35f, mass: 0.4f);
    }

    void PlaceCone(Vector3 pos)
    {
        pos.y = 0f;
        var root = Root("Cone", pos);
        Color orange = new Color(0.98f, 0.48f, 0.05f);

        Box(root, "Base", Y(0.06f), new Vector3(0.46f, 0.12f, 0.46f), orange, 0.30f);
        Box(root, "Mid",  Y(0.28f), new Vector3(0.30f, 0.34f, 0.30f), orange, 0.30f);
        Box(root, "Top",  Y(0.56f), new Vector3(0.12f, 0.18f, 0.12f), orange, 0.30f);
        Box(root, "Band", Y(0.25f), new Vector3(0.32f, 0.06f, 0.32f),
            new Color(0.97f, 0.97f, 0.97f), 0.78f);

        Consumable(root, 0.4f, 1, 8f, ObjectCategory.Prop, 0.25f, mass: 0.06f);
    }

    void PlaceBench(Vector3 pos)
    {
        pos.y = 0f;
        var root = Root("Bench", pos);
        root.transform.rotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
        Color wood  = new Color(0.52f, 0.36f, 0.18f);
        Color metal = new Color(0.55f, 0.55f, 0.58f);

        Box(root, "Seat", Y(0.42f), new Vector3(1.10f, 0.08f, 0.42f), wood, 0.10f);
        Box(root, "Back", new Vector3(0f, 0.72f, -0.18f),
            new Vector3(1.10f, 0.52f, 0.06f), wood, 0.10f);
        float[] lx = {  0.45f, -0.45f,  0.45f, -0.45f };
        float[] lz = {  0.15f,  0.15f, -0.15f, -0.15f };
        for (int i = 0; i < 4; i++)
            Box(root, "Leg", new Vector3(lx[i], 0.20f, lz[i]),
                new Vector3(0.06f, 0.40f, 0.06f), metal, 0.30f, 0.20f);

        Consumable(root, 1.0f, 2, 20f, ObjectCategory.Prop, 0.55f, mass: 0.6f);
    }

    // ── Cars ──────────────────────────────────────────────────────────────
    void SpawnCars(int count)
    {
        float block = GameManager.BLOCK, half = GameManager.HALF;
        const float MIN_GAP = 5f; // min center-to-center distance
        var placed = new System.Collections.Generic.List<Vector3>(count);
        int attempts = 0;

        while (placed.Count < count && attempts < count * 10)
        {
            attempts++;
            bool    horiz = Random.value < 0.5f;
            float   rc    = Mathf.Round(Random.Range(-3, 4)) * block;
            float   along = Random.Range(-half + 5f, half - 5f);
            Vector3 pos   = horiz ? new Vector3(along, 0f, rc) : new Vector3(rc, 0f, along);

            bool clear = true;
            foreach (var p in placed)
            {
                float dx = p.x - pos.x, dz = p.z - pos.z;
                if (dx * dx + dz * dz < MIN_GAP * MIN_GAP) { clear = false; break; }
            }

            if (!clear) continue;
            placed.Add(pos);
            SpawnCar(pos, horiz);
        }
    }

    void SpawnCar(Vector3 pos, bool horizontal)
    {
        Color col   = CAR_COLORS[Random.Range(0, CAR_COLORS.Length)];
        Color dark  = Sc(col, 0.52f);
        Color glass = new Color(0.14f, 0.22f, 0.32f);

        var root = Root("Car", pos);
        if (!horizontal) root.transform.rotation = Quaternion.Euler(0f, 90f, 0f);

        // Body and cabin
        Box(root, "Body",  Y(0.52f), new Vector3(1.70f, 0.62f, 3.50f), col, 0.60f, 0.10f);
        Box(root, "Cabin", Y(1.18f), new Vector3(1.52f, 0.54f, 2.20f), col, 0.55f, 0.08f);

        // Windshields
        Box(root, "FGlass", new Vector3(0f, 1.18f,  1.08f), new Vector3(1.35f, 0.42f, 0.08f), glass, 0.88f, 0.12f);
        Box(root, "RGlass", new Vector3(0f, 1.18f, -1.08f), new Vector3(1.35f, 0.42f, 0.08f), glass, 0.88f, 0.12f);

        // Bumpers
        Box(root, "FBump", new Vector3(0f, 0.35f,  1.78f), new Vector3(1.55f, 0.30f, 0.20f), dark, 0.30f);
        Box(root, "RBump", new Vector3(0f, 0.35f, -1.78f), new Vector3(1.55f, 0.30f, 0.20f), dark, 0.30f);

        // Headlights / taillights — emissive so they glow at night
        Color headBase = new Color(0.95f, 0.95f, 0.88f);
        Color headEmit = new Color(2.0f,  1.95f, 1.60f);   // bright white-warm HDR
        Color tailBase = new Color(0.80f, 0.05f, 0.05f);
        Color tailEmit = new Color(1.80f, 0.08f, 0.08f);   // saturated red HDR
        EmissiveBox(root, "HLR", new Vector3( 0.60f, 0.55f,  1.80f), new Vector3(0.34f, 0.22f, 0.06f), headBase, headEmit, 0.80f);
        EmissiveBox(root, "HLL", new Vector3(-0.60f, 0.55f,  1.80f), new Vector3(0.34f, 0.22f, 0.06f), headBase, headEmit, 0.80f);
        EmissiveBox(root, "TLR", new Vector3( 0.60f, 0.55f, -1.80f), new Vector3(0.34f, 0.22f, 0.06f), tailBase, tailEmit, 0.70f);
        EmissiveBox(root, "TLL", new Vector3(-0.60f, 0.55f, -1.80f), new Vector3(0.34f, 0.22f, 0.06f), tailBase, tailEmit, 0.70f);

        // Wheels: 4 tires + hubcaps
        Color tire = new Color(0.08f, 0.08f, 0.09f);
        Color rim  = new Color(0.75f, 0.75f, 0.80f);
        Vector3[] wPos = {
            new Vector3( 0.85f, 0.28f,  1.15f),
            new Vector3(-0.85f, 0.28f,  1.15f),
            new Vector3( 0.85f, 0.28f, -1.15f),
            new Vector3(-0.85f, 0.28f, -1.15f),
        };
        Quaternion wheelRot = Quaternion.Euler(0f, 0f, 90f);
        foreach (var wp in wPos)
        {
            Prim(PrimitiveType.Cylinder, root, "Tire", wp, wheelRot,
                new Vector3(0.52f, 0.22f, 0.52f), tire, 0.05f);
            Prim(PrimitiveType.Cylinder, root, "Rim",
                wp + new Vector3(Mathf.Sign(wp.x) * 0.26f, 0f, 0f), wheelRot,
                new Vector3(0.36f, 0.04f, 0.36f), rim, 0.75f, 0.55f);
        }

        Consumable(root, 1.8f, 3, 50f, ObjectCategory.Car, 1.6f, mass: 1.2f);
    }

    // ── Ground / road boxes ───────────────────────────────────────────────
    void GroundBox(string name, Vector3 pos, Vector3 size, Color col)
    {
        var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
        go.name = name;
        go.transform.SetParent(_cityRoot, false);
        go.transform.position   = pos;
        go.transform.localScale = size;
        go.GetComponent<Renderer>().sharedMaterial = MkGroundMat(col);
        Destroy(go.GetComponent<Collider>());
    }

    // ── Consumable registration ───────────────────────────────────────────
    void Consumable(GameObject go, float size, int tier, float value, ObjectCategory cat,
                    float footprintRadius = 0f, float mass = 0f)
    {
        var co = go.AddComponent<ConsumableObject>();
        co.Init(size, tier, value, cat, footprintRadius);
        GameManager.Instance.AllObjects.Add(co);

        // Objects with mass take part in physics: debris that slams into them
        // knocks them around, and they push back on the debris in equal
        // measure — PhysX resolves both sides by mass, so heavy and light
        // objects react differently for free.  Bodies start asleep so the
        // city costs nothing until something actually hits.
        if (mass > 0f)
        {
            var rends = go.GetComponentsInChildren<Renderer>();
            if (rends.Length > 0)
            {
                Bounds wb = rends[0].bounds;
                for (int i = 1; i < rends.Length; i++)
                    wb.Encapsulate(rends[i].bounds);

                var bc = go.AddComponent<BoxCollider>();
                bc.center = go.transform.InverseTransformPoint(wb.center);
                Vector3 ls = go.transform.InverseTransformVector(wb.size);
                bc.size = new Vector3(Mathf.Abs(ls.x), Mathf.Abs(ls.y), Mathf.Abs(ls.z));
            }

            var rb = go.AddComponent<Rigidbody>();
            rb.mass           = mass;
            rb.linearDamping  = 0.05f;
            rb.angularDamping = 0.8f;
            rb.maxLinearVelocity = 25f;
            rb.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
            rb.Sleep();
        }
    }

    // ── Primitive helpers ─────────────────────────────────────────────────
    GameObject Root(string name, Vector3 worldPos)
    {
        var go = new GameObject(name);
        go.transform.SetParent(_cityRoot, false);
        go.transform.position = worldPos;
        return go;
    }

    static GameObject Box(GameObject parent, string name, Vector3 lp, Vector3 ls,
                           Color col, float sm = 0.15f, float mt = 0f)
        => Prim(PrimitiveType.Cube, parent, name, lp, Quaternion.identity, ls, col, sm, mt);

    static GameObject Prim(PrimitiveType type, GameObject parent, string name,
                            Vector3 lp, Quaternion lr, Vector3 ls,
                            Color col, float sm = 0.15f, float mt = 0f)
    {
        var go = GameObject.CreatePrimitive(type);
        go.name = name;
        go.transform.SetParent(parent.transform, false);
        go.transform.localPosition = lp;
        go.transform.localRotation = lr;
        go.transform.localScale    = ls;
        go.GetComponent<Renderer>().sharedMaterial = MkLitMat(col, sm, mt);
        Destroy(go.GetComponent<Collider>());
        return go;
    }

    // ── Material cache ────────────────────────────────────────────────────
    static Material MkLitMat(Color c, float sm, float mt)
    {
        string key = $"{(int)(c.r*255)},{(int)(c.g*255)},{(int)(c.b*255)},{(int)(sm*100)},{(int)(mt*100)}";
        if (!_litCache.TryGetValue(key, out var mat))
        {
            mat = new Material(Shader.Find("Universal Render Pipeline/Lit"));
            mat.SetColor("_BaseColor", c);
            mat.SetFloat("_Smoothness", sm);
            mat.SetFloat("_Metallic",   mt);
            _litCache[key] = mat;
        }
        return mat;
    }

    static Material MkGroundMat(Color c)
    {
        string key = $"{(int)(c.r*255)},{(int)(c.g*255)},{(int)(c.b*255)}";
        if (!_groundCache.TryGetValue(key, out var mat))
        {
            mat = new Material(Shader.Find("DowntownDevour/GroundMasked"));
            mat.SetColor("_BaseColor", c);
            _groundCache[key] = mat;
        }
        return mat;
    }

    // Emissive material — used for lit windows, lamp globes, car lights.
    // emissiveColor should be the raw HDR colour (values > 1 are fine — URP bloom picks them up).
    static Material MkEmissiveMat(Color baseColor, Color emissiveColor, float sm = 0.5f)
    {
        string key = $"e{(int)(emissiveColor.r*255)},{(int)(emissiveColor.g*255)},{(int)(emissiveColor.b*255)},{(int)(emissiveColor.a*100)}";
        if (!_emissiveCache.TryGetValue(key, out var mat))
        {
            mat = new Material(Shader.Find("Universal Render Pipeline/Lit"));
            mat.SetColor("_BaseColor",  baseColor);
            mat.SetFloat("_Smoothness", sm);
            mat.SetFloat("_Metallic",   0f);
            mat.EnableKeyword("_EMISSION");
            mat.SetColor("_EmissionColor", emissiveColor);
            mat.globalIlluminationFlags = MaterialGlobalIlluminationFlags.RealtimeEmissive;
            _emissiveCache[key] = mat;
        }
        return mat;
    }

    // Emissive primitive helper — places a glowing mesh child
    static GameObject EmissivePrim(PrimitiveType type, GameObject parent, string name,
                                   Vector3 lp, Quaternion lr, Vector3 ls,
                                   Color baseColor, Color emissiveColor, float sm = 0.5f)
    {
        var go = GameObject.CreatePrimitive(type);
        go.name = name;
        go.transform.SetParent(parent.transform, false);
        go.transform.localPosition = lp;
        go.transform.localRotation = lr;
        go.transform.localScale    = ls;
        go.GetComponent<Renderer>().sharedMaterial = MkEmissiveMat(baseColor, emissiveColor, sm);
        Destroy(go.GetComponent<Collider>());
        return go;
    }

    static GameObject EmissiveBox(GameObject parent, string name, Vector3 lp, Vector3 ls,
                                  Color baseColor, Color emissiveColor, float sm = 0.5f)
        => EmissivePrim(PrimitiveType.Cube, parent, name, lp, Quaternion.identity, ls,
                        baseColor, emissiveColor, sm);

    // ── Color + position utilities ────────────────────────────────────────
    static Color  Sc(Color c, float f)
        => new Color(Mathf.Clamp01(c.r*f), Mathf.Clamp01(c.g*f), Mathf.Clamp01(c.b*f), c.a);
    static Vector3 Y(float y) => new Vector3(0f, y, 0f);
}
