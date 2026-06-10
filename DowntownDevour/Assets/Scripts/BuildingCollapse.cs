using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Added to building roots by CityGenerator. ConsumableObject calls Collapse()
// instead of the default shrink animation when this component is present.
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

        _parts.Sort((a, b) => a.worldY.CompareTo(b.worldY));
        float minY  = _parts[0].worldY;
        float maxY  = _parts[_parts.Count - 1].worldY;
        float range = Mathf.Max(maxY - minY, 1f);

        foreach (var (t, y) in _parts)
        {
            float nY    = (y - minY) / range;
            float delay = nY * 0.35f;
            StartCoroutine(LaunchPart(t, holePos, delay, nY));
        }

        Destroy(gameObject, 6f);
        yield break;
    }

    IEnumerator LaunchPart(Transform part, Vector3 holePos, float delay, float heightFactor)
    {
        if (delay > 0f) yield return new WaitForSeconds(delay);
        if (part == null) yield break;

        part.SetParent(null);

        if (part.GetComponent<Collider>() == null)
            part.gameObject.AddComponent<BoxCollider>();

        var rb = part.gameObject.AddComponent<Rigidbody>();
        rb.mass           = 1f + heightFactor * 3f;
        rb.linearDamping  = 0.15f;
        rb.angularDamping = 0.4f;

        Vector3 outward = part.position - holePos;
        outward.y = 0f;
        if (outward.sqrMagnitude < 0.01f)
            outward = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f));
        outward.Normalize();

        float upKick   = Mathf.Lerp(2f, 9f, heightFactor);
        float outSpeed = Mathf.Lerp(3f, 11f, heightFactor);
        rb.AddForce(outward * outSpeed + Vector3.up * upKick, ForceMode.VelocityChange);
        rb.AddTorque(Random.insideUnitSphere * 6f, ForceMode.VelocityChange);

        Destroy(part.gameObject, 5f);
    }
}
