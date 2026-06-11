using System.Collections.Generic;
using UnityEngine;

// Progressive destruction. Every frame, any piece of the building whose
// footprint overlaps a hole breaks off IMMEDIATELY and falls under real
// gravity — no delays, no scripted swirl. Large boxes (the shaft, slabs,
// bands) lazily fragment into a grid of cube chunks the first time a hole
// touches them: outer chunks keep the facade material, inner chunks are
// concrete, so breaking the skin reveals interior mass. Chunks not over the
// hole stay frozen in place, so only the section above the hole collapses;
// the rest of the building keeps standing until the hole sweeps under it.
public class BuildingCollapse : MonoBehaviour
{
    const float CHUNK_TARGET   = 2.2f; // desired chunk edge length (m)
    const int   MAX_PER_AXIS   = 8;
    const float RELEASE_MARGIN = 0.3f;

    static Material _concrete;

    readonly List<Transform> _parts = new();
    float _minHoleRadius;   // hole must be at least this big to harm this building
    float _footprintRadius; // building bounding circle for cheap overlap rejection

    public void RegisterPart(Transform part) => _parts.Add(part);

    public void Init(float minHoleRadius, float footprintRadius)
    {
        _minHoleRadius   = minHoleRadius;
        _footprintRadius = footprintRadius;
    }

    void Update()
    {
        if (GameManager.Instance == null ||
            GameManager.Instance.State != GameManager.GameState.Playing) return;

        var holes = GameManager.Instance.AllHoles;
        for (int h = 0; h < holes.Count; h++)
        {
            var hole = holes[h];
            if (!hole.Alive || hole.Radius < _minHoleRadius) continue;

            Vector3 hp = hole.transform.position;
            float dx = transform.position.x - hp.x;
            float dz = transform.position.z - hp.z;
            float reach = hole.Radius + _footprintRadius;
            if (dx * dx + dz * dz > reach * reach) continue;

            ProcessHole(hole);
        }

        if (_parts.Count == 0) Destroy(gameObject);
    }

    void ProcessHole(HoleBase hole)
    {
        Vector3 hp = hole.transform.position;

        // Backwards so we can remove entries; chunks appended by Fragment()
        // land beyond the current index and get picked up next frame (16 ms).
        for (int i = _parts.Count - 1; i >= 0; i--)
        {
            var part = _parts[i];
            if (part == null) { _parts.RemoveAt(i); continue; }

            Vector3 s    = part.lossyScale;
            float   half = 0.5f * Mathf.Sqrt(s.x * s.x + s.z * s.z);
            float   dx   = part.position.x - hp.x;
            float   dz   = part.position.z - hp.z;
            float   dist = Mathf.Sqrt(dx * dx + dz * dz);

            if (dist > hole.Radius + half) continue; // hole not under this part

            if (NeedsFragmenting(s))
            {
                _parts.RemoveAt(i);
                Fragment(part);
                continue;
            }

            if (dist <= hole.Radius + RELEASE_MARGIN)
            {
                _parts.RemoveAt(i);
                Release(part);
            }
        }
    }

    // ── Fragmentation ─────────────────────────────────────────────────────────

    static bool NeedsFragmenting(Vector3 s)
    {
        int nx = AxisCount(s.x), ny = AxisCount(s.y), nz = AxisCount(s.z);
        return nx * ny * nz > 1 && s.x * s.y * s.z > 1.5f;
    }

    static int AxisCount(float d) =>
        Mathf.Clamp(Mathf.RoundToInt(d / CHUNK_TARGET), 1, MAX_PER_AXIS);

    // Replace one big axis-aligned box with a grid of cube chunks occupying the
    // same volume. Chunks stay frozen (no physics) as children of the building
    // until the hole reaches each one.
    void Fragment(Transform part)
    {
        Vector3 s = part.lossyScale;
        Vector3 p = part.position;
        int nx = AxisCount(s.x), ny = AxisCount(s.y), nz = AxisCount(s.z);
        var  cs     = new Vector3(s.x / nx, s.y / ny, s.z / nz);
        var  facade = part.GetComponent<Renderer>().sharedMaterial;

        for (int ix = 0; ix < nx; ix++)
        for (int iy = 0; iy < ny; iy++)
        for (int iz = 0; iz < nz; iz++)
        {
            var chunk = GameObject.CreatePrimitive(PrimitiveType.Cube);
            chunk.name = "Chunk";
            chunk.transform.SetParent(transform, true);
            chunk.transform.position = new Vector3(
                p.x - s.x * 0.5f + cs.x * (ix + 0.5f),
                p.y - s.y * 0.5f + cs.y * (iy + 0.5f),
                p.z - s.z * 0.5f + cs.z * (iz + 0.5f));
            chunk.transform.localScale = cs;

            bool outer = ix == 0 || ix == nx - 1 ||
                         iz == 0 || iz == nz - 1 ||
                         iy == ny - 1;
            chunk.GetComponent<Renderer>().sharedMaterial =
                outer ? facade : ConcreteMat();

            _parts.Add(chunk.transform);
        }

        Destroy(part.gameObject);
    }

    static Material ConcreteMat()
    {
        if (_concrete == null)
        {
            _concrete = new Material(Shader.Find("Universal Render Pipeline/Lit"));
            _concrete.SetColor("_BaseColor", new Color(0.68f, 0.62f, 0.50f));
            _concrete.SetFloat("_Smoothness", 0.10f);
        }
        return _concrete;
    }

    // ── Release: gravity takes over the same frame ────────────────────────────

    void Release(Transform part)
    {
        part.SetParent(null);

        if (part.GetComponent<Collider>() == null)
            part.gameObject.AddComponent<BoxCollider>();

        Vector3 s   = part.lossyScale;
        float   vol = s.x * s.y * s.z;

        var rb = part.gameObject.AddComponent<Rigidbody>();
        rb.mass           = Mathf.Clamp(vol * 0.25f, 0.3f, 40f);
        rb.linearDamping  = 0.03f;
        rb.angularDamping = 0.08f;

        // Tiny jitter so stacked pieces separate; gravity does the real work
        rb.AddForce(new Vector3(Random.Range(-0.3f, 0.3f), 0f,
                                Random.Range(-0.3f, 0.3f)), ForceMode.VelocityChange);

        // XZ-only tumble — never spin on Y, so no spiral
        Vector3 tq = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f));
        if (tq.sqrMagnitude < 0.01f) tq = Vector3.right;
        rb.AddTorque(tq.normalized * Random.Range(0.3f, 1.0f), ForceMode.VelocityChange);

        float minDim = Mathf.Min(s.x, Mathf.Min(s.y, s.z));
        float size   = Mathf.Max(0.4f, minDim * 0.45f);
        float value  = Mathf.Clamp(vol * 0.6f, 2f, 50f);

        var co = part.gameObject.AddComponent<ConsumableObject>();
        co.Init(size, 1, value, ObjectCategory.Building);
        GameManager.Instance.AllObjects.Add(co);
    }
}
