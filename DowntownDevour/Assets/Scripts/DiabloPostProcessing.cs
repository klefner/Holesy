using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

// Attaches to the main camera. Enables URP post-processing and builds a global
// Volume that gives the game its Diablo dark-isometric visual identity:
//   - ACES tonemapping for filmic contrast
//   - Bloom halos around every light and emissive surface
//   - Color grade that pushes shadows cool and highlights warm
//   - Vignette that pulls the eye to the centre of the action
public class DiabloPostProcessing : MonoBehaviour
{
    void Start()
    {
        EnablePostProcessing();
        BuildVolume();
    }

    void EnablePostProcessing()
    {
        var data = GetComponent<UniversalAdditionalCameraData>();
        if (data == null) data = gameObject.AddComponent<UniversalAdditionalCameraData>();
        data.renderPostProcessing = true;
    }

    void BuildVolume()
    {
        var go     = new GameObject("DiabloVolume");
        var volume = go.AddComponent<Volume>();
        volume.isGlobal = true;
        volume.priority = 10f;

        var profile   = ScriptableObject.CreateInstance<VolumeProfile>();
        volume.sharedProfile = profile;

        // Tonemapping — ACES gives rich shadow crush and warm highlight rolloff.
        var tone = profile.Add<Tonemapping>(true);
        tone.mode.Override(TonemappingMode.ACES);

        // Bloom — halos around streetlamps, hole rim, emissive windows.
        var bloom = profile.Add<Bloom>(true);
        bloom.threshold.Override(0.8f);
        bloom.intensity.Override(0.7f);
        bloom.scatter.Override(0.65f);
        bloom.tint.Override(new Color(1.0f, 0.85f, 0.55f));

        // Color grade — slightly underexposed, punchy contrast, mild desaturation.
        var ca = profile.Add<ColorAdjustments>(true);
        ca.postExposure.Override(-0.3f);
        ca.contrast.Override(22f);
        ca.saturation.Override(-12f);

        // Vignette — deep purple-black edges draw focus inward.
        var vig = profile.Add<Vignette>(true);
        vig.color.Override(new Color(0.04f, 0.0f, 0.07f));
        vig.intensity.Override(0.38f);
        vig.smoothness.Override(0.5f);
    }
}
