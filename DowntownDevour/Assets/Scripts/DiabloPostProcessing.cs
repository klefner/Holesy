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

        // Bloom — halos on lights and emissives; start aggressive so it's clearly visible
        var bloom = profile.Add<Bloom>(true);
        bloom.threshold.value         = 0.5f;
        bloom.threshold.overrideState = true;
        bloom.intensity.value         = 1.5f;
        bloom.intensity.overrideState = true;
        bloom.scatter.value           = 0.7f;
        bloom.scatter.overrideState   = true;

        // Color grade — underexposed, punchy contrast, mild desaturation
        var ca = profile.Add<ColorAdjustments>(true);
        ca.postExposure.value         = -0.3f;
        ca.postExposure.overrideState = true;
        ca.contrast.value             = 22f;
        ca.contrast.overrideState     = true;
        ca.saturation.value           = -12f;
        ca.saturation.overrideState   = true;

        // Vignette — darken edges toward deep purple
        var vig = profile.Add<Vignette>(true);
        vig.color.value           = new Color(0.04f, 0.0f, 0.07f);
        vig.color.overrideState   = true;
        vig.intensity.value       = 0.38f;
        vig.intensity.overrideState = true;
        vig.smoothness.value      = 0.5f;
        vig.smoothness.overrideState = true;
    }
}
