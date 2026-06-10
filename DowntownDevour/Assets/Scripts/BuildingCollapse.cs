using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Added to building roots by CityGenerator.
// ConsumableObject.MarkConsumed() calls Collapse() instead of the shrink animation.
public class BuildingCollapse : MonoBehaviour
{
    readonly List<(Transform t, float worldY)> _parts = new();

    public void RegisterPart(Transform part) =>
        _parts.Add((part, part.position.y));

    public void Collapse(Vector3 holePos, float holeRadius) =>
        StartCoroutine(DoCollapse(holePos, holeRadius));

    IEnumerator DoCollapse(Vector3 holePos, float holeRadius)
    {
        if (_parts.Count == 0) { Destroy(gameObject, 0.1f); yield break; }

        float minY = float.MaxValue, maxY = float.MinValue;
        foreach (var (t, y) in _parts)
        {
            if (y < minY) minY = y;
            if (y > maxY) maxY = y;
        }
        float range = Mathf.Max(maxY - minY, 1f);

        foreach (var (t, y) in _parts)
        {
            if (t == null) continue;
            float dx      = t.position.x - holePos.x;
            float dz      = t.position.z - holePos.z;
            float dist    = Mathf.Sqrt(dx * dx + dz * dz);
            float nY      = (y - minY) / range;
            bool  overHole = dist <= holeRadius;
            // Lower floors fall first; outside parts wait longer based on distance
            float delay = nY * 0.20f + (overHole ? 0f : dist * 0.08f);
            StartCoroutine(ReleasePart(t, delay, nY, overHole, holePos));
        }

        Destroy(gameObject, 10f);
        yield break;
    }

    IEnumerator ReleasePart(Transform part, float delay, float nY, bool overHole, Vector3 holePos)
    {
        if (delay > 0f) yield return new WaitForSeconds(delay);
        if (part == null) yield break;

        part.SetParent(null);

        if (part.GetComponent<Collider>() == null)
            part.gameObject.AddComponent<BoxCollider>();

        var rb = part.gameObject.AddComponent<Rigidbody>();
        Vector3 s   = part.lossyScale;
        float   vol = s.x * s.y * s.z;

        rb.mass           = Mathf.Clamp(vol * 0.25f, 0.3f, 40f);
        rb.linearDamping  = 0.03f;
        rb.angularDamping = 0.08f;

        Vector3 force;
        if (overHole)
        {
            // Parts directly over the hole drop mostly straight down.
            // A tiny jitter breaks perfect symmetry so pieces don't stack.
            float j = Random.Range(0.1f, 0.4f);
            force = new Vector3(Random.Range(-j, j), 0f, Random.Range(-j, j));
            // Top quarter: occasional upward pop before dropping in
            if (nY > 0.75f && Random.value < 0.35f)
                force.y = Random.Range(1.5f, 4.0f);
        }
        else
        {
            // Parts outside the hole topple outward; upper sections travel further
            float   mag  = Mathf.Lerp(0.5f, 3.0f, nY);
            Vector3 away = new Vector3(part.position.x - holePos.x, 0f,
                                       part.position.z - holePos.z);
            if (away.sqrMagnitude < 0.01f)
                away = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f));
            force    = away.normalized * mag;
            force.x += Random.Range(-0.5f, 0.5f);
            force.z += Random.Range(-0.5f, 0.5f);
            // Upper outside parts sometimes fly upward before falling
            if (nY > 0.65f && Random.value < 0.4f)
                force.y = Random.Range(0.5f, 2.5f);
        }
        rb.AddForce(force, ForceMode.VelocityChange);

        // Tumble on XZ only — no Y-axis spin
        Vector3 tq = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f));
        if (tq.sqrMagnitude < 0.01f) tq = Vector3.right;
        rb.AddTorque(tq.normalized * Random.Range(0.2f, 1.2f), ForceMode.VelocityChange);

        float minDim = Mathf.Min(s.x, Mathf.Min(s.y, s.z));
        float size   = Mathf.Max(0.4f, minDim * 0.45f);
        float value  = Mathf.Clamp(vol * 1.2f, 4f, 80f);
        RegisterDebris(part.gameObject, size, value);
    }

    static void RegisterDebris(GameObject go, float size, float value)
    {
        var co = go.AddComponent<ConsumableObject>();
        co.Init(size, 1, value, ObjectCategory.Building);
        GameManager.Instance.AllObjects.Add(co);
    }
}
