using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

// Attaches to the main camera. Creates a global post-processing Volume at runtime
// that gives the game its Diablo dark-city visual identity.
public class DiabloPostProcessing : MonoBehaviour
{
    void Start()
    {
        SetupCamera();
        CreateVolume();
    }

    void SetupCamera()
    {
        var urpData = GetComponent<UniversalAdditionalCameraData>();
        if (urpData == null) urpData = gameObject.AddComponent<UniversalAdditionalCameraData>();
        urpData.renderPostProcessing = true;
        // Explicitly tell this camera to pick up volumes on all layers.
        urpData.volumeLayerMask = ~0;
    }

    void CreateVolume()
    {
        var go     = new GameObject("DiabloVolume");
        go.layer   = 0; // Default layer

        var volume      = go.AddComponent<Volume>();
        volume.isGlobal = true;
        volume.priority = 100f;

        // Use .profile (instance) not .sharedProfile so Unity treats it as runtime data.
        var profile  = ScriptableObject.CreateInstance<VolumeProfile>();
        volume.profile = profile;

        // Tonemapping — ACES filmic contrast
        var tone = profile.Add<Tonemapping>(true);
        tone.mode.value         = TonemappingMode.ACES;
        tone.mode.overrideState = true;

        // Bloom — daytime: only true HDR emissives (>1) should halo, not lit surfaces.
        var bloom = profile.Add<Bloom>(true);
        bloom.threshold.value         = 1.1f;   // high threshold — skip ordinary daylight surfaces
        bloom.threshold.overrideState = true;
        bloom.intensity.value         = 0.7f;   // gentle
        bloom.intensity.overrideState = true;
        bloom.scatter.value           = 0.6f;
        bloom.scatter.overrideState   = true;

        // Color grade — neutral daytime: no underexposure, mild contrast, full color.
        var ca = profile.Add<ColorAdjustments>(true);
        ca.postExposure.value         = 0.1f;
        ca.postExposure.overrideState = true;
        ca.contrast.value             = 6f;
        ca.contrast.overrideState     = true;
        ca.saturation.value           = 4f;
        ca.saturation.overrideState   = true;

        // Vignette — very light, just a soft frame; no black edge crush in daytime.
        var vig = profile.Add<Vignette>(true);
        vig.color.value              = new Color(0.10f, 0.12f, 0.16f);
        vig.color.overrideState      = true;
        vig.intensity.value          = 0.15f;
        vig.intensity.overrideState  = true;
        vig.smoothness.value         = 0.55f;
        vig.smoothness.overrideState = true;
    }
}
