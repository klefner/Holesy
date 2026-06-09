using UnityEngine;
using UnityEngine.Rendering;

// Scene-level lighting. Currently daytime — bright warm sun, good ambient.
// The hole reads as a genuine black void by contrast against the bright city.
public static class LightingSetup
{
    public static void ApplyDiablo()
    {
        RenderSettings.ambientMode  = AmbientMode.Flat;
        RenderSettings.ambientLight = new Color(0.65f, 0.68f, 0.72f);
        RenderSettings.fog          = false;

        var sunGO = new GameObject("Sun");
        var sun   = sunGO.AddComponent<Light>();
        sun.type      = LightType.Directional;
        sun.color     = new Color(1.0f, 0.96f, 0.86f);
        sun.intensity = 1.4f;
        sun.shadows   = LightShadows.Soft;
        sunGO.transform.rotation = Quaternion.Euler(50f, -28f, 0f);
    }
}
