using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Progressive destruction — no all-or-nothing trigger.
// Every frame, any piece whose XZ footprint overlaps a hole breaks off that
// same frame (or is fragmented into smaller pieces first).  Parts at the hole
// edge topple outward; parts over the void drop straight in.  Chunks that miss
// the hole land and sleep as persistent debris.
public class BuildingCollapse : MonoBehaviour
{
    const float CHUNK_SIZE = 2.0f;
    const int   MAX_CHUNKS = 6;

    static Material _concrete;

    readonly List<Transform> _parts = new();
    float _minY          = float.MaxValue;
    float _maxY          = float.MinValue;
    float _footprintRadius;
    float _strength = 1f; // per-building structural integrity
    bool  _damaged;
    float _minHoleRadius;  // smaller holes only rattle the building

    // Undersized-hole shake state
    Vector3 _basePos;
    bool    _baseSet;
    float   _shakePhase;
    float   _nibbleTimer;
    float   _nextNibble = 1.5f;

    public void Init(float footprintRadius, float minHoleRadius)
    {
        _footprintRadius = footprintRadius;
        _minHoleRadius   = minHoleRadius;
        // Per-building personality: weak structures chain-react and crumble
        // wholesale when struck; strong ones only lose what the hole
        // actually touches and the rest keeps standing.
        _strength = Random.Range(0.6f, 2.4f);
    }

    public void RegisterPart(Transform part)
    {
        _parts.Add(part);
        // Standing parts are solid: debris slams into them instead of passing
        // through, and Impact() decides whether the hit dislodges them.
        if (part.GetComponent<Collider>() == null)
            part.gameObject.AddComponent<BoxCollider>();
        float y = part.position.y;
        if (y < _minY) _minY = y;
        if (y > _maxY) _maxY = y;
    }

    // ── Impact from flying debris ─────────────────────────────────────────────

    const float KNOCK_RESIST    = 6f;  // impulse per unit mass needed to dislodge
    const float MAX_KNOCK_SPEED = 9f;  // m/s cap on a dislodged part's launch
    const float MAX_DEBRIS_SPEED = 25f; // hard ceiling on any debris velocity

    public void Impact(Transform part, Vector3 impulse)
    {
        int i = _parts.IndexOf(part);
        if (i < 0) return;

        Vector3 s    = part.lossyScale;
        float   mass = Mathf.Clamp(s.x * s.y * s.z * 0.25f, 0.3f, 40f);

        // Pressure vs weight: the hit only dislodges the part when the
        // collision impulse exceeds what its own mass can absorb.  Heavy
        // sections shrug off hits that send light panels flying, and each
        // building's structural strength scales the bar — weak buildings
        // chain-react, sturdy ones barely shed.
        if (impulse.magnitude < mass * KNOCK_RESIST * _strength) return;

        _parts.RemoveAt(i);
        MarkDamaged();

        if (NeedsFragmenting(s)) { Fragment(part); return; }

        var rb = MakeDebris(part);
        // The engine has already resolved the collision for the striker, so
        // only a damped share of the impulse carries into the part — and the
        // resulting velocity is capped so light panels never become rockets.
        Vector3 dv = impulse * (0.5f / mass);
        if (dv.magnitude > MAX_KNOCK_SPEED)
            dv = dv.normalized * MAX_KNOCK_SPEED;
        rb.AddForce(dv, ForceMode.VelocityChange);
        Vector3 tq = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f));
        if (tq.sqrMagnitude < 0.01f) tq = Vector3.right;
        rb.AddTorque(tq.normalized * Random.Range(0.5f, 1.5f), ForceMode.VelocityChange);
    }

    // ── Per-frame scan ────────────────────────────────────────────────────────

    void Update()
    {
        if (GameManager.Instance == null ||
            GameManager.Instance.State != GameManager.GameState.Playing) return;

        bool rattled = false;
        foreach (var hole in GameManager.Instance.AllHoles)
        {
            if (!hole.Alive) continue;

            // Cheap circle-vs-circle reject before touching _parts
            Vector3 hp  = hole.transform.position;
            float   dx  = transform.position.x - hp.x;
            float   dz  = transform.position.z - hp.z;
            float   lim = hole.Radius + _footprintRadius;
            if (dx * dx + dz * dz > lim * lim) continue;

            // An undersized hole can't bring the structure down — it only
            // rattles the building and occasionally shakes a small piece off.
            if (hole.Radius >= _minHoleRadius) SweepHole(hole);
            else                               rattled |= Nibble(hole);
        }

        ApplyShake(rattled);

        if (_parts.Count == 0) Destroy(gameObject);
    }

    // ── Undersized hole: tremble + occasional shed ────────────────────────────

    bool Nibble(HoleBase hole)
    {
        Vector3 hp = hole.transform.position;
        float   r2 = hole.Radius * hole.Radius;

        bool      touching = false;
        Transform smallest = null;
        float     bestVol  = float.MaxValue;

        foreach (Transform part in _parts)
        {
            if (part == null) continue;
            Vector3 s = part.lossyScale;
            Vector3 c = part.position;
            float nx  = Mathf.Clamp(hp.x, c.x - 0.5f * s.x, c.x + 0.5f * s.x);
            float nz  = Mathf.Clamp(hp.z, c.z - 0.5f * s.z, c.z + 0.5f * s.z);
            float ndx = nx - hp.x;
            float ndz = nz - hp.z;
            if (ndx * ndx + ndz * ndz >= r2) continue;

            touching = true;
            float vol = s.x * s.y * s.z;
            if (vol < 1.5f && vol < bestVol) { bestVol = vol; smallest = part; }
        }

        if (!touching) return false;

        _nibbleTimer += Time.deltaTime;
        if (_nibbleTimer >= _nextNibble && smallest != null)
        {
            _nibbleTimer = 0f;
            _nextNibble  = Random.Range(1.2f, 2.6f);

            _parts.Remove(smallest);
            MarkDamaged();
            var rb = MakeDebris(smallest);
            rb.AddForce(new Vector3(Random.Range(-1f, 1f), Random.Range(0.5f, 1.5f),
                                    Random.Range(-1f, 1f)), ForceMode.VelocityChange);
            Vector3 tq = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f));
            if (tq.sqrMagnitude < 0.01f) tq = Vector3.right;
            rb.AddTorque(tq.normalized * Random.Range(0.3f, 1.0f), ForceMode.VelocityChange);
        }
        return true;
    }

    void ApplyShake(bool rattled)
    {
        if (rattled)
        {
            if (!_baseSet) { _basePos = transform.position; _baseSet = true; }
            _shakePhase += Time.deltaTime * 30f;
            const float A = 0.05f;
            transform.position = _basePos + new Vector3(
                Mathf.Sin(_shakePhase) * A, 0f, Mathf.Sin(_shakePhase * 1.31f) * A);
        }
        else if (_baseSet)
        {
            transform.position = _basePos;
            _baseSet = false;
        }
    }

    void SweepHole(HoleBase hole)
    {
        Vector3 hp    = hole.transform.position;
        float   range = Mathf.Max(_maxY - _minY, 1f);

        for (int i = _parts.Count - 1; i >= 0; i--)
        {
            Transform part = _parts[i];
            if (part == null) { _parts.RemoveAt(i); continue; }

            Vector3 s = part.lossyScale;
            Vector3 c = part.position;

            // Exact rectangle-vs-circle test: clamp the hole center to the
            // part's XZ box.  A corner clip releases only that corner — a
            // bounding circle here made one grazing touch release parts (and
            // fragment whole cores) the hole never actually reached.
            float nx  = Mathf.Clamp(hp.x, c.x - 0.5f * s.x, c.x + 0.5f * s.x);
            float nz  = Mathf.Clamp(hp.z, c.z - 0.5f * s.z, c.z + 0.5f * s.z);
            float ndx = nx - hp.x;
            float ndz = nz - hp.z;
            if (ndx * ndx + ndz * ndz >= hole.Radius * hole.Radius) continue;

            float pdx = c.x - hp.x;
            float pdz = c.z - hp.z;
            float sqD = pdx * pdx + pdz * pdz;

            _parts.RemoveAt(i);
            MarkDamaged();

            if (NeedsFragmenting(s))
            {
                Fragment(part);   // next frame, the smaller chunks get caught
                continue;
            }

            float dist = Mathf.Sqrt(sqD);
            float nY   = Mathf.Clamp01((part.position.y - _minY) / range);
            // Wave delay: parts near the touched edge release a hair later so the
            // collapse looks like structural failure spreading, not a pop.
            // Never more than 0.15 s so it never reads as a pause.
            float edgeFrac = Mathf.Clamp01(dist / Mathf.Max(hole.Radius, 0.01f));
            float delay    = edgeFrac * 0.12f;

            StartCoroutine(ReleasePart(part, delay, nY, dist, hole.Radius, hp));
        }
    }

    // ── Structural integrity ──────────────────────────────────────────────────
    // Once a building is damaged, a slow loop checks whether each standing
    // part is still held up.  Undermined sections sag and topple under
    // gravity alone — no launch — so partial hits leave partial ruins, and
    // anything left unsupported slowly leans over and falls.

    void MarkDamaged()
    {
        if (_damaged) return;
        _damaged = true;
        StartCoroutine(SupportLoop());
    }

    IEnumerator SupportLoop()
    {
        var wait = new WaitForSeconds(0.3f);
        while (_parts.Count > 0)
        {
            yield return wait;
            ReleaseUnsupportedParts();
        }
    }

    void ReleaseUnsupportedParts()
    {
        for (int i = _parts.Count - 1; i >= 0; i--)
        {
            Transform p = _parts[i];
            if (p == null) { _parts.RemoveAt(i); continue; }
            if (IsSupported(p)) continue;

            _parts.RemoveAt(i);
            // Gravity alone, plus a whisper of torque so columns lean and
            // keel over rather than dropping perfectly straight.  Only parts
            // the hole actually touches get the energetic launch.
            var rb = MakeDebris(p);
            Vector3 tq = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f));
            if (tq.sqrMagnitude < 0.01f) tq = Vector3.right;
            rb.AddTorque(tq.normalized * Random.Range(0.1f, 0.4f), ForceMode.VelocityChange);
        }
    }

    bool IsSupported(Transform part)
    {
        Vector3 s = part.lossyScale;
        Vector3 c = part.position;
        float bottom = c.y - 0.5f * s.y;
        if (bottom <= 0.25f) return true; // standing on the ground

        float ex = 0.5f * s.x, ez = 0.5f * s.z;

        for (int j = 0; j < _parts.Count; j++)
        {
            Transform o = _parts[j];
            if (o == null || o == part) continue;
            Vector3 os = o.lossyScale;
            Vector3 oc = o.position;

            float gapX = Mathf.Abs(c.x - oc.x) - (ex + 0.5f * os.x);
            float gapZ = Mathf.Abs(c.z - oc.z) - (ez + 0.5f * os.z);
            float oTop = oc.y + 0.5f * os.y;
            float oBot = oc.y - 0.5f * os.y;

            // Resting on top of a standing part below (needs real XZ overlap)
            float rest = bottom - oTop;
            if (rest > -0.05f && rest < 0.35f && gapX < -0.05f && gapZ < -0.05f)
                return true;

            // Attached to the face of a standing part (windows, bands, slabs
            // hang on the structure they decorate; side-by-side chunks at the
            // same level do NOT count — their bottoms align, so a floating
            // layer can't hold itself up)
            if (bottom > oBot + 0.05f && bottom < oTop - 0.05f &&
                gapX < 0.12f && gapZ < 0.12f)
                return true;
        }
        return false;
    }

    // ── Fragmentation ─────────────────────────────────────────────────────────

    static bool NeedsFragmenting(Vector3 s)
    {
        int nx = Chunks(s.x), ny = Chunks(s.y), nz = Chunks(s.z);
        return nx * ny * nz > 1 && s.x * s.y * s.z > 1.5f;
    }

    static int Chunks(float d) =>
        Mathf.Clamp(Mathf.RoundToInt(d / CHUNK_SIZE), 1, MAX_CHUNKS);

    void Fragment(Transform part)
    {
        Vector3 s  = part.lossyScale;
        Vector3 p  = part.position;
        int nx = Chunks(s.x), ny = Chunks(s.y), nz = Chunks(s.z);
        Vector3 cs = new Vector3(s.x / nx, s.y / ny, s.z / nz);
        Material facade = part.GetComponent<Renderer>().sharedMaterial;

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

            bool isOuter = ix == 0 || ix == nx - 1 ||
                           iz == 0 || iz == nz - 1 ||
                           iy == ny - 1;
            chunk.GetComponent<Renderer>().sharedMaterial =
                isOuter ? facade : ConcreteMat();

            _parts.Add(chunk.transform);
        }

        Destroy(part.gameObject);
    }

    static PhysicsMaterial _debrisMat;

    static PhysicsMaterial DebrisMat()
    {
        if (_debrisMat == null)
        {
            _debrisMat = new PhysicsMaterial("Debris")
            {
                staticFriction  = 0.9f,
                dynamicFriction = 0.85f,
                bounciness      = 0f,
                frictionCombine = PhysicsMaterialCombine.Maximum,
                bounceCombine   = PhysicsMaterialCombine.Minimum,
            };
        }
        return _debrisMat;
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

    // ── Release ───────────────────────────────────────────────────────────────

    // Turns a static building part into live physics debris the holes can eat.
    Rigidbody MakeDebris(Transform part)
    {
        part.SetParent(null);

        var col = part.GetComponent<Collider>();
        if (col == null)
            col = part.gameObject.AddComponent<BoxCollider>();
        // High friction stops chunks sliding once they land; drag must stay
        // near zero so gravity accelerates them naturally while airborne.
        col.material = DebrisMat();

        Vector3 s   = part.lossyScale;
        float   vol = s.x * s.y * s.z;

        var rb = part.gameObject.AddComponent<Rigidbody>();
        rb.mass           = Mathf.Clamp(vol * 0.25f, 0.3f, 40f);
        rb.linearDamping  = 0.02f;
        rb.angularDamping = 1.0f;
        rb.maxLinearVelocity = MAX_DEBRIS_SPEED;
        // Swept collision so fast debris can't cross a wall in one physics
        // step and come out the other side untouched
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;

        float minDim = Mathf.Min(s.x, Mathf.Min(s.y, s.z));
        float size   = Mathf.Max(0.3f, minDim * 0.45f);
        float value  = Mathf.Clamp(vol * 0.6f, 2f, 50f);

        var co = part.gameObject.AddComponent<ConsumableObject>();
        co.Init(size, 1, value, ObjectCategory.Building,
                footprintRadius: 0.5f * Mathf.Max(s.x, s.z));
        GameManager.Instance.AllObjects.Add(co);

        return rb;
    }

    IEnumerator ReleasePart(Transform part, float delay, float nY,
                            float distFromCenter, float holeRadius, Vector3 holePos)
    {
        if (delay > 0f) yield return new WaitForSeconds(delay);
        if (part == null) yield break;

        var rb = MakeDebris(part);

        // edgeFrac = 0 → part center is at hole center (falls straight in)
        //          = 1 → part center is at hole rim (topples outward)
        float edgeFrac = Mathf.Clamp01(distFromCenter / Mathf.Max(holeRadius, 0.01f));

        Vector3 force;
        if (edgeFrac < 0.55f)
        {
            // Over the void: drop with a tiny jitter so pieces don't stack
            float j = Random.Range(0.1f, 0.5f);
            force = new Vector3(Random.Range(-j, j), 0f, Random.Range(-j, j));
            // Upper floors get a slight upward pop before dropping — top doesn't
            // fall as one rigid pillar
            force.y = nY * Random.Range(0.5f, 3.0f);
        }
        else
        {
            // At the hole edge: topple outward; upper sections travel further
            Vector3 away = new Vector3(part.position.x - holePos.x, 0f,
                                       part.position.z - holePos.z);
            if (away.sqrMagnitude < 0.01f)
                away = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f));
            float outMag = Mathf.Lerp(1.0f, 5.0f, nY) * edgeFrac;
            force = away.normalized * outMag;
            force.x += Random.Range(-0.5f, 0.5f);
            force.z += Random.Range(-0.5f, 0.5f);
            // Height-proportional lift so the building top arcs outward, not collapses
            force.y = nY * Random.Range(0.5f, 2.5f) * edgeFrac;
        }

        rb.AddForce(force, ForceMode.VelocityChange);

        // XZ-only tumble — Y-axis spin causes the spiral the user complained about
        Vector3 tq = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f));
        if (tq.sqrMagnitude < 0.01f) tq = Vector3.right;
        rb.AddTorque(tq.normalized * Random.Range(0.5f, 2.5f), ForceMode.VelocityChange);
    }
}
