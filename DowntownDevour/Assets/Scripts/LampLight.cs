using UnityEngine;

// Sodium-vapour flicker on each street lamp: slow intensity drift layered with
// high-frequency shimmer, applied to the spotlight AND both ground pool discs
// so the light and the ground move together. Bails when the spotlight is off
// (daytime) so it doesn't fight SetBuildingLights.
public class LampLight : MonoBehaviour
{
    Light    _spot;
    Material _outerMat;
    Material _innerMat;
    Color    _outerEmit;
    Color    _innerEmit;
    float    _baseIntensity;
    float    _phase;

    public void Init(Light spot,
                     Material outerMat, Color outerEmit,
                     Material innerMat, Color innerEmit)
    {
        _spot          = spot;
        _outerMat      = outerMat;
        _innerMat      = innerMat;
        _outerEmit     = outerEmit;
        _innerEmit     = innerEmit;
        _baseIntensity = spot.intensity;
        _phase         = Random.Range(0f, Mathf.PI * 2f);
    }

    void Update()
    {
        if (_spot == null || !_spot.enabled) return;

        float t = Time.time;
        // Three-frequency flicker: slow swell, medium pulse, fast shimmer.
        // Keeps it subtle — these are electric lights, not candles.
        float f = 1f
            + 0.05f * Mathf.Sin(t * 1.1f  + _phase)
            + 0.03f * Mathf.Sin(t * 4.3f  + _phase * 1.7f)
            + 0.02f * Mathf.Sin(t * 14.8f + _phase * 3.1f);

        _spot.intensity = _baseIntensity * f;

        if (_outerMat != null) _outerMat.SetColor("_EmissionColor", _outerEmit * f);
        if (_innerMat != null) _innerMat.SetColor("_EmissionColor", _innerEmit * f);
    }
}
