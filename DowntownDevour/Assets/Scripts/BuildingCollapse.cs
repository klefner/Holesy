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

    public void Collapse(Vector3 holePos) =>
        StartCoroutine(DoCollapse(holePos));

    IEnumerator DoCollapse(Vector3 holePos)
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
            // Parts directly over the hole fall first.
            // Parts farther away and higher up fall progressively later.
            float dx    = t.position.x - holePos.x;
            float dz    = t.position.z - holePos.z;
            float dist  = Mathf.Sqrt(dx * dx + dz * dz);
            float nY    = (y - minY) / range;
            float delay = dist * 0.15f + nY * 0.25f;
            StartCoroutine(ReleasePart(t, delay));
        }

        // Root is now an empty container; destroy it once all parts are released
        Destroy(gameObject, 5f);
        yield break;
    }

    IEnumerator ReleasePart(Transform part, float delay)
    {
        if (delay > 0f) yield return new WaitForSeconds(delay);
        if (part == null) yield break;

        part.SetParent(null);

        // BoxCollider lets pieces land on the ground and interact with each other
        if (part.GetComponent<Collider>() == null)
            part.gameObject.AddComponent<BoxCollider>();

        var rb = part.gameObject.AddComponent<Rigidbody>();
        Vector3 s  = part.lossyScale;
        float   vol = s.x * s.y * s.z;

        // Mass proportional to volume so big chunks feel heavy
        rb.mass           = Mathf.Clamp(vol * 0.25f, 0.3f, 40f);
        rb.linearDamping  = 0.03f;
        rb.angularDamping = 0.08f;
        // Use Unity's default gravity — no custom downward push

        // Tiny random nudge to break perfect vertical symmetry.
        // Gravity does all the real work; this just prevents pieces
        // stacking in a perfect column.
        Vector3 nudge = new Vector3(
            Random.Range(-0.4f, 0.4f),
            0f,
            Random.Range(-0.4f, 0.4f));
        rb.AddForce(nudge, ForceMode.VelocityChange);

        // Gentle tumble — NOT explosive spin
        rb.AddTorque(
            Random.insideUnitSphere * Random.Range(0.1f, 0.6f),
            ForceMode.VelocityChange);

        // Register so any hole can eat this piece.
        // Size is based on the smallest dimension so thin panels are easy to eat
        // and chunky pieces require a larger hole.
        float minDim = Mathf.Min(s.x, Mathf.Min(s.y, s.z));
        float size   = Mathf.Max(0.4f, minDim * 0.45f);
        float value  = Mathf.Clamp(vol * 1.2f, 4f, 80f);
        RegisterDebris(part.gameObject, size, value);
        // No Destroy timer — debris stays until a hole eats it
    }

    static void RegisterDebris(GameObject go, float size, float value)
    {
        var co = go.AddComponent<ConsumableObject>();
        co.Init(size, 1, value, ObjectCategory.Building);
        GameManager.Instance.AllObjects.Add(co);
    }
}
