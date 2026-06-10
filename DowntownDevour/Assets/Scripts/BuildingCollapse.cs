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

        Destroy(gameObject, 8f);
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

        // Register the launched part so the hole can consume it
        RegisterDebris(part.gameObject, 0.9f, 12f);

        Vector3 s   = part.lossyScale;
        float   vol = s.x * s.y * s.z;
        if (vol > 0.8f)
        {
            int   fragCount  = Mathf.RoundToInt(
                Mathf.Clamp(Mathf.Log(vol + 1f) * 1.8f + Random.Range(-0.5f, 0.5f), 2, 8));
            float fractureAt = Random.Range(0.08f, 1.3f);
            StartCoroutine(Fracture(part.gameObject, fragCount, fractureAt));
        }
        else
        {
            Destroy(part.gameObject, 8f);
        }
    }

    IEnumerator Fracture(GameObject piece, int fragCount, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (piece == null) yield break;

        var     rb   = piece.GetComponent<Rigidbody>();
        Vector3 vel  = rb != null ? rb.linearVelocity  : Vector3.zero;
        Vector3 angV = rb != null ? rb.angularVelocity : Vector3.zero;
        Vector3 pos  = piece.transform.position;
        Vector3 scl  = piece.transform.lossyScale;
        Material mat = piece.GetComponent<Renderer>()?.sharedMaterial;

        for (int i = 0; i < fragCount; i++)
        {
            float fx = Random.Range(0.28f, 0.78f);
            float fy = Random.Range(0.28f, 0.78f);
            float fz = Random.Range(0.28f, 0.78f);

            var frag = GameObject.CreatePrimitive(PrimitiveType.Cube);
            frag.transform.position   = pos + new Vector3(
                Random.Range(-scl.x * 0.38f, scl.x * 0.38f),
                Random.Range(-scl.y * 0.28f, scl.y * 0.28f),
                Random.Range(-scl.z * 0.38f, scl.z * 0.38f));
            frag.transform.localScale = new Vector3(scl.x * fx, scl.y * fy, scl.z * fz);
            frag.transform.rotation   = piece.transform.rotation *
                Quaternion.Euler(Random.Range(-30f, 30f), Random.Range(-30f, 30f), Random.Range(-30f, 30f));

            if (mat != null)
                frag.GetComponent<Renderer>().sharedMaterial = mat;

            var fragRb = frag.AddComponent<Rigidbody>();
            fragRb.linearVelocity  = vel + Random.insideUnitSphere * Random.Range(1f, 4f);
            fragRb.angularVelocity = angV + Random.insideUnitSphere * Random.Range(3f, 8f);
            fragRb.mass            = 0.4f;
            fragRb.linearDamping   = 0.2f;
            fragRb.angularDamping  = 0.5f;

            // Register each shard so any hole can consume it
            RegisterDebris(frag, 0.5f, 6f);

            Destroy(frag, 8f);
        }

        Destroy(piece);
    }

    static void RegisterDebris(GameObject go, float size, float value)
    {
        var co = go.AddComponent<ConsumableObject>();
        co.Init(size, 1, value, ObjectCategory.Building);
        GameManager.Instance.AllObjects.Add(co);
    }
}
