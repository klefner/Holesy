using UnityEngine;
using UnityEngine.Rendering;

// Configures scene-level lighting for the Diablo dark-city aesthetic.
// Called once from GameManager.SetupLighting() — replaces the default daylight rig.
public static class LightingSetup
{
    public static void ApplyDiablo()
    {
        // Nearly black ambient so streetlamps and the hole rim are the dominant
        // light sources, just like Diablo's torch-lit dungeons.
        RenderSettings.ambientMode  = AmbientMode.Flat;
        RenderSettings.ambientLight = new Color(0.03f, 0.03f, 0.055f);

        // Linear depth fog — city fades to darkness at the far edge of the board.
        RenderSettings.fog              = true;
        RenderSettings.fogMode          = FogMode.Linear;
        RenderSettings.fogColor         = new Color(0.015f, 0.01f, 0.03f);
        RenderSettings.fogStartDistance = 90f;
        RenderSettings.fogEndDistance   = 200f;

        // Moonlight — cool blue-gray directional, low intensity, soft shadows.
        var moonGO = new GameObject("Moon");
        var moon   = moonGO.AddComponent<Light>();
        moon.type           = LightType.Directional;
        moon.color          = new Color(0.55f, 0.65f, 1.0f);
        moon.intensity      = 0.28f;
        moon.shadows        = LightShadows.Soft;
        moon.shadowStrength = 0.9f;
        moonGO.transform.rotation = Quaternion.Euler(48f, -135f, 0f);
    }
}
