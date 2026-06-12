using System.Linq;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

// Runs automatically before every build (including batch-mode builds).
// Ensures all shaders used via runtime Shader.Find() are in Always Included
// Shaders so they are never stripped from platform builds.  WebGL is the most
// aggressive stripper — any shader not referenced by a project material asset
// AND not listed here will be missing at runtime, producing invisible objects.
public class ShaderPreprocessor : IPreprocessBuildWithReport
{
    public int callbackOrder => 0;

    static readonly string[] Required =
    {
        "Universal Render Pipeline/Lit",
        "DowntownDevour/HoleMask",
        "DowntownDevour/GroundMasked",
    };

    public void OnPreprocessBuild(BuildReport report)
    {
        var gs = AssetDatabase.LoadAssetAtPath<Object>(
            "ProjectSettings/GraphicsSettings.asset");
        if (gs == null)
        {
            Debug.LogWarning("ShaderPreprocessor: GraphicsSettings.asset not found — skipping.");
            return;
        }

        var so   = new SerializedObject(gs);
        var list = so.FindProperty("m_AlwaysIncludedShaders");
        if (list == null)
        {
            Debug.LogWarning("ShaderPreprocessor: m_AlwaysIncludedShaders not found — skipping.");
            return;
        }

        bool changed = false;
        foreach (string name in Required)
        {
            var shader = Shader.Find(name);
            if (shader == null) { Debug.LogWarning($"ShaderPreprocessor: '{name}' not found."); continue; }

            bool already = false;
            for (int i = 0; i < list.arraySize; i++)
                if (list.GetArrayElementAtIndex(i).objectReferenceValue == shader)
                { already = true; break; }

            if (!already)
            {
                list.arraySize++;
                list.GetArrayElementAtIndex(list.arraySize - 1).objectReferenceValue = shader;
                changed = true;
                Debug.Log($"ShaderPreprocessor: added '{name}' to Always Included Shaders.");
            }
        }

        if (changed)
        {
            so.ApplyModifiedProperties();
            AssetDatabase.SaveAssets();
            Debug.Log("ShaderPreprocessor: GraphicsSettings saved.");
        }
    }
}
