using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

// Forces every URP renderer asset in the project onto the Forward+ rendering path
// and ensures additional lights render per-pixel.
//
// WHY: classic Forward culls additional lights per-object and caps how many each
// object receives. A city full of lamp posts and car headlights leaves a giant
// ground mesh seeing almost none of them, so the streets stay black under every
// lamp no matter how bright the light. Forward+ culls lights per screen-space
// cluster with no per-object limit, so each lamp actually illuminates the area
// around it (the look we're after).
//
// The URP pipeline/renderer assets live only in the user's local project
// (Assets/Settings) and are not in source control, so we can't ship the change as
// an asset edit. Instead we configure them in code — on editor load and again
// before every build — mirroring how ShaderPreprocessor edits GraphicsSettings.
[InitializeOnLoad]
public class ForwardPlusSetup : IPreprocessBuildWithReport
{
    public int callbackOrder => 0;

    static ForwardPlusSetup()
    {
        // Defer until the asset database is ready after a domain reload.
        EditorApplication.delayCall += Configure;
    }

    public void OnPreprocessBuild(BuildReport report) => Configure();

    static void Configure()
    {
        bool changed = false;
        changed |= ConfigureRenderers();
        changed |= ConfigurePipelineAssets();

        if (changed)
        {
            AssetDatabase.SaveAssets();
            Debug.Log("ForwardPlusSetup: URP set to Forward+ with per-pixel additional lights.");
        }
    }

    // Set rendering path = Forward+ on every UniversalRendererData asset.
    static bool ConfigureRenderers()
    {
        bool changed = false;
        foreach (var guid in AssetDatabase.FindAssets("t:UniversalRendererData"))
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            var data = AssetDatabase.LoadAssetAtPath<UniversalRendererData>(path);
            if (data == null) continue;

            var so   = new SerializedObject(data);
            var prop = so.FindProperty("m_RenderingMode");
            if (prop != null && prop.intValue != (int)RenderingMode.ForwardPlus)
            {
                prop.intValue = (int)RenderingMode.ForwardPlus;
                so.ApplyModifiedProperties();
                EditorUtility.SetDirty(data);
                changed = true;
                Debug.Log($"ForwardPlusSetup: enabled Forward+ on {path}");
            }
        }
        return changed;
    }

    // Make sure additional lights aren't disabled / vertex-only — they must be
    // per-pixel for the clustered light loop (and our custom ground shader) to run.
    static bool ConfigurePipelineAssets()
    {
        bool changed = false;
        foreach (var guid in AssetDatabase.FindAssets("t:UniversalRenderPipelineAsset"))
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            var asset = AssetDatabase.LoadAssetAtPath<UniversalRenderPipelineAsset>(path);
            if (asset == null) continue;

            var so = new SerializedObject(asset);
            bool local = false;

            var mode = so.FindProperty("m_AdditionalLightsRenderingMode");
            if (mode != null && mode.intValue != (int)LightRenderingMode.PerPixel)
            {
                mode.intValue = (int)LightRenderingMode.PerPixel;
                local = true;
            }

            var limit = so.FindProperty("m_AdditionalLightsPerObjectLimit");
            if (limit != null && limit.intValue < 8)
            {
                limit.intValue = 8;
                local = true;
            }

            if (local)
            {
                so.ApplyModifiedProperties();
                EditorUtility.SetDirty(asset);
                changed = true;
                Debug.Log($"ForwardPlusSetup: configured additional lights on {path}");
            }
        }
        return changed;
    }
}
