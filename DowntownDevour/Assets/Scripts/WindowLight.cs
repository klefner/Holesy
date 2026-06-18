using System.Collections;
using UnityEngine;

// Per-renderer emission controller for building window panes and storefronts.
// Uses a MaterialPropertyBlock so each pane is independent even when multiple
// panes share the same cached material.  GameManager calls SetNight(true/false)
// when the time-of-day changes to activate or kill the light.
public class WindowLight : MonoBehaviour
{
    Renderer              _rend;
    MaterialPropertyBlock _block;
    Color                 _emit;
    bool                  _nightOn;   // false → this window is always dark (unoccupied)
    bool                  _on;        // currently in the "on" state
    bool                  _moveDark;  // permanently extinguished after movement

    public void Init(Color emit, bool nightOn)
    {
        _rend    = GetComponent<Renderer>();
        _block   = new MaterialPropertyBlock();
        _emit    = emit;
        _nightOn = nightOn;
        SetEmission(Color.black);
    }

    // Called by GameManager.SetWindowLights() at every TOD change.
    public void SetNight(bool on)
    {
        if (_moveDark) return;
        StopAllCoroutines();
        _on = on && _nightOn;
        if (_on)
            StartCoroutine(FlickerOn());
        else
            SetEmission(Color.black);
    }

    void Update()
    {
        if (!_on || _moveDark) return;
        // If this pane's hierarchy gained a Rigidbody and started moving
        // (e.g. the building part it's attached to broke off as debris),
        // extinguish it with a brief death flicker.
        var rb = GetComponentInParent<Rigidbody>();
        if (rb != null && !rb.IsSleeping() && rb.linearVelocity.sqrMagnitude > 0.25f)
        {
            _moveDark = true;
            _on       = false;
            StopAllCoroutines();
            StartCoroutine(FlickerOff());
        }
    }

    // Staggered random delay then 1–3 quick on/off pulses before settling steady.
    IEnumerator FlickerOn()
    {
        yield return new WaitForSeconds(Random.Range(0f, 2f));
        int n = Random.Range(1, 4);
        for (int i = 0; i < n; i++)
        {
            SetEmission(_emit);
            yield return new WaitForSeconds(Random.Range(0.04f, 0.12f));
            SetEmission(Color.black);
            yield return new WaitForSeconds(Random.Range(0.06f, 0.22f));
        }
        SetEmission(_emit);   // steady on
    }

    // 1–3 death pulses then permanently dark.
    IEnumerator FlickerOff()
    {
        int n = Random.Range(1, 4);
        for (int i = 0; i < n; i++)
        {
            SetEmission(_emit);
            yield return new WaitForSeconds(Random.Range(0.04f, 0.10f));
            SetEmission(Color.black);
            yield return new WaitForSeconds(Random.Range(0.06f, 0.16f));
        }
        // Block stays set to black — permanently extinguished.
    }

    void SetEmission(Color c)
    {
        if (_rend == null) return;
        _block.SetColor("_EmissionColor", c);
        _rend.SetPropertyBlock(_block);
    }
}
