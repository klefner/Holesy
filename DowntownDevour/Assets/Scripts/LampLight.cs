using System.Collections;
using UnityEngine;

// Sodium-vapour flicker on each street lamp: slow intensity drift layered with
// high-frequency shimmer applied to the point light.  When the lamp is consumed
// by a hole, a DieFlicker coroutine fires: 1–3 rapid on/off pulses on both the
// point light and the globe renderer, then permanent dark.
public class LampLight : MonoBehaviour
{
    Light   _spot;
    float   _baseIntensity;
    float   _phase;
    bool    _dying;

    // Emissive renderers on this lamp (the globe sphere) controlled via
    // MaterialPropertyBlock so the shared material is not globally affected.
    Renderer[]              _emRends;
    Color[]                 _emColors;
    MaterialPropertyBlock   _block;

    public void Init(Light spot,
                     Material outerMat, Color outerEmit,
                     Material innerMat, Color innerEmit)
    {
        _spot          = spot;
        _baseIntensity = spot != null ? spot.intensity : 1f;
        _phase         = Random.Range(0f, Mathf.PI * 2f);

        // Collect all emissive child renderers now so DieFlicker can use them.
        var list   = new System.Collections.Generic.List<Renderer>();
        var colors = new System.Collections.Generic.List<Color>();
        foreach (var r in GetComponentsInChildren<Renderer>())
        {
            var m = r.sharedMaterial;
            if (m == null || !m.HasProperty("_EmissionColor")) continue;
            Color ec = m.GetColor("_EmissionColor");
            if (ec.maxColorComponent < 0.01f) continue;
            list.Add(r);
            colors.Add(ec);
        }
        _emRends  = list.ToArray();
        _emColors = colors.ToArray();
        _block    = new MaterialPropertyBlock();
    }

    void Update()
    {
        if (_spot == null || !_spot.enabled || _dying) return;

        // When the lamp is consumed by a hole, switch to the die sequence.
        var co = GetComponent<ConsumableObject>();
        if (co != null && co.IsConsumed)
        {
            _dying = true;
            StartCoroutine(DieFlicker());
            return;
        }

        float t = Time.time;
        // Three-frequency flicker: slow swell, medium pulse, fast shimmer.
        float f = 1f
            + 0.05f * Mathf.Sin(t * 1.1f  + _phase)
            + 0.03f * Mathf.Sin(t * 4.3f  + _phase * 1.7f)
            + 0.02f * Mathf.Sin(t * 14.8f + _phase * 3.1f);

        _spot.intensity = _baseIntensity * f;
    }

    // 1–3 rapid pulses on both the point light and the globe, then permanent off.
    IEnumerator DieFlicker()
    {
        int n = Random.Range(1, 4);
        for (int i = 0; i < n; i++)
        {
            if (_spot != null) _spot.intensity = _baseIntensity;
            SetGlobeEmission(on: true);
            yield return new WaitForSeconds(Random.Range(0.04f, 0.10f));

            if (_spot != null) _spot.intensity = 0f;
            SetGlobeEmission(on: false);
            yield return new WaitForSeconds(Random.Range(0.06f, 0.16f));
        }
        if (_spot != null) { _spot.intensity = 0f; _spot.enabled = false; }
        // Globe stays dark — MaterialPropertyBlock keeps _EmissionColor = black.
    }

    void SetGlobeEmission(bool on)
    {
        if (_emRends == null) return;
        for (int j = 0; j < _emRends.Length; j++)
        {
            if (_emRends[j] == null) continue;
            _block.SetColor("_EmissionColor", on ? _emColors[j] : Color.black);
            _emRends[j].SetPropertyBlock(_block);
        }
    }
}
