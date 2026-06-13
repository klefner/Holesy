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

        // Bloom — halos on lights and emissives; aggressive for Diablo night glow
        // Higher intensity and scatter so lamp halos and window glow halo out properly
        var bloom = profile.Add<Bloom>(true);
        bloom.threshold.value         = 0.4f;   // lower threshold catches more emissives
        bloom.threshold.overrideState = true;
        bloom.intensity.value         = 2.2f;   // stronger halo effect in dark scene
        bloom.intensity.overrideState = true;
        bloom.scatter.value           = 0.75f;  // slightly wider spread
        bloom.scatter.overrideState   = true;

        // Color grade — underexposed, punchy contrast, mild desaturation
        var ca = profile.Add<ColorAdjustments>(true);
        ca.postExposure.value         = -0.3f;
        ca.postExposure.overrideState = true;
        ca.contrast.value             = 22f;
        ca.contrast.overrideState     = true;
        ca.saturation.value           = -12f;
        ca.saturation.overrideState   = true;

        // Vignette — heavy purple-black edge crush, Diablo signature
        var vig = profile.Add<Vignette>(true);
        vig.color.value              = new Color(0.02f, 0.0f, 0.05f);
        vig.color.overrideState      = true;
        vig.intensity.value          = 0.48f;
        vig.intensity.overrideState  = true;
        vig.smoothness.value         = 0.45f;
        vig.smoothness.overrideState = true;

        // Chromatic aberration — slight lens fringing around emissive sources
        var chr = profile.Add<ChromaticAberration>(true);
        chr.intensity.value         = 0.12f;
        chr.intensity.overrideState = true;

        // Film grain — very subtle cinematic noise texture over the frame
        var grain = profile.Add<FilmGrain>(true);
        grain.type.value            = FilmGrainLookup.Thin1;
        grain.intensity.value       = 0.08f;
        grain.intensity.overrideState = true;
        grain.response.value        = 0.8f;
        grain.response.overrideState = true;
    }
}
