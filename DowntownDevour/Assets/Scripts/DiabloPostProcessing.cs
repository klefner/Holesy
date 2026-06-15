using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

// Attaches to the main camera. Creates a global post-processing Volume at runtime
// that gives the game its visual identity. The grade is time-of-day aware:
// daytime uses ACES filmic tonemapping; evening/night switch to Neutral with a
// lifted exposure so the dark city stays readable instead of crushing to black.
public class DiabloPostProcessing : MonoBehaviour
{
    Tonemapping      _tone;
    Bloom            _bloom;
    ColorAdjustments _ca;
    Vignette         _vig;

    // Built in Awake so the references exist before GameManager.Start() applies
    // the initial time-of-day grade (AddComponent runs Awake synchronously).
    void Awake()
    {
        SetupCamera();
        CreateVolume();
    }

    void SetupCamera()
    {
        var urpData = GetComponent<UniversalAdditionalCameraData>();
        if (urpData == null) urpData = gameObject.AddComponent<UniversalAdditionalCameraData>();
        urpData.renderPostProcessing = true;
        urpData.volumeLayerMask = ~0;   // pick up volumes on all layers
    }

    void CreateVolume()
    {
        var go   = new GameObject("DiabloVolume");
        go.layer = 0;

        var volume      = go.AddComponent<Volume>();
        volume.isGlobal = true;
        volume.priority = 100f;

        var profile    = ScriptableObject.CreateInstance<VolumeProfile>();
        volume.profile = profile;

        _tone = profile.Add<Tonemapping>(true);
        _tone.mode.overrideState = true;
        _tone.mode.value         = TonemappingMode.ACES;

        _bloom = profile.Add<Bloom>(true);
        _bloom.threshold.overrideState = true;
        _bloom.threshold.value         = 1.1f;
        _bloom.intensity.overrideState = true;
        _bloom.intensity.value         = 0.7f;
        _bloom.scatter.overrideState   = true;
        _bloom.scatter.value           = 0.6f;

        _ca = profile.Add<ColorAdjustments>(true);
        _ca.postExposure.overrideState = true;
        _ca.postExposure.value         = 0.1f;
        _ca.contrast.overrideState     = true;
        _ca.contrast.value             = 6f;
        _ca.saturation.overrideState   = true;
        _ca.saturation.value           = 4f;

        _vig = profile.Add<Vignette>(true);
        _vig.color.overrideState      = true;
        _vig.color.value              = new Color(0.10f, 0.12f, 0.16f);
        _vig.intensity.overrideState  = true;
        _vig.intensity.value          = 0.15f;
        _vig.smoothness.overrideState = true;
        _vig.smoothness.value         = 0.55f;
    }

    // Called by GameManager whenever the time-of-day changes. Daytime keeps the
    // filmic ACES look; dark modes switch to Neutral tonemapping (preserves
    // midtones) with lifted exposure and a lower bloom threshold so lit windows,
    // lamps and headlights glow without the rest of the scene going black.
    public void SetGrade(bool darkMode, float exposure, float bloomThreshold,
                         float bloomIntensity, float vignette)
    {
        if (_tone == null) return;   // not built yet — defensive
        _tone.mode.value      = darkMode ? TonemappingMode.Neutral : TonemappingMode.ACES;
        _ca.postExposure.value = exposure;
        _bloom.threshold.value = bloomThreshold;
        _bloom.intensity.value = bloomIntensity;
        _vig.intensity.value   = vignette;
    }
}
