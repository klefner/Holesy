using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Attached to building roots by CityGenerator.
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
            float dx       = t.position.x - holePos.x;
            float dz       = t.position.z - holePos.z;
            float dist     = Mathf.Sqrt(dx * dx + dz * dz);
            float nY       = (y - minY) / range;
            bool  overHole = dist <= holeRadius;
            // Lower floors collapse first; outside parts wait longer based on distance
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

        Vector3 s     = part.lossyScale;
        float   vol   = s.x * s.y * s.z;
        float   value = Mathf.Clamp(vol * 1.2f, 4f, 80f);

        if (overHole)
        {
            // Parts directly over the hole fall and shrink into the void —
            // same visual as any other consumed object so the player sees it happening.
            StartCoroutine(SuckIntoHole(part));
        }
        else
        {
            // Parts outside the hole topple outward and land as collectible debris.
            if (part.GetComponent<Collider>() == null)
                part.gameObject.AddComponent<BoxCollider>();

            var rb = part.gameObject.AddComponent<Rigidbody>();
            rb.mass           = Mathf.Clamp(vol * 0.25f, 0.3f, 40f);
            rb.linearDamping  = 0.03f;
            rb.angularDamping = 0.08f;

            // Outward force — upper sections fly further
            float   mag  = Mathf.Lerp(1.5f, 5.0f, nY);
            Vector3 away = new Vector3(part.position.x - holePos.x, 0f,
                                       part.position.z - holePos.z);
            if (away.sqrMagnitude < 0.01f)
                away = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f));
            Vector3 force = away.normalized * mag;
            force.x += Random.Range(-0.5f, 0.5f);
            force.z += Random.Range(-0.5f, 0.5f);
            if (nY > 0.5f && Random.value < 0.4f)
                force.y = Random.Range(0.5f, 2.5f);
            rb.AddForce(force, ForceMode.VelocityChange);

            // XZ-only torque — no Y-axis spin so parts don't spiral
            Vector3 tq = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f));
            if (tq.sqrMagnitude < 0.01f) tq = Vector3.right;
            rb.AddTorque(tq.normalized * Random.Range(0.5f, 2.0f), ForceMode.VelocityChange);

            float minDim = Mathf.Min(s.x, Mathf.Min(s.y, s.z));
            float size   = Mathf.Max(0.4f, minDim * 0.45f);
            RegisterDebris(part.gameObject, size, value);
        }
    }

    // Falls straight down while shrinking to zero — taller parts take longer.
    IEnumerator SuckIntoHole(Transform part)
    {
        if (part == null) yield break;
        Vector3 startScale = part.localScale;
        float   fallVel    = 2f;
        float   elapsed    = 0f;
        float   duration   = Mathf.Clamp(part.position.y * 0.12f + 0.25f, 0.25f, 1.2f);

        while (part != null && elapsed < duration)
        {
            elapsed        += Time.deltaTime;
            fallVel        += 14f * Time.deltaTime;
            part.position  += Vector3.down * (fallVel * Time.deltaTime);
            part.localScale = Vector3.Lerp(startScale, Vector3.zero, elapsed / duration);
            yield return null;
        }

        if (part != null) Destroy(part.gameObject);
    }

    static void RegisterDebris(GameObject go, float size, float value)
    {
        var co = go.AddComponent<ConsumableObject>();
        co.Init(size, 1, value, ObjectCategory.Building);
        GameManager.Instance.AllObjects.Add(co);
    }
}
