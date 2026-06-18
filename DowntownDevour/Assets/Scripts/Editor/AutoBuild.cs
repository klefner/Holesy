using System.Linq;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

// Command-line entry point that builds the Windows PC player and the Web
// player in a single batch session.  Invoked by Scripts/build-and-play-both:
//   Unity.exe -batchmode -quit -projectPath <proj> -executeMethod AutoBuild.BuildAll
public static class AutoBuild
{
    public static void BuildAll()
    {
        string[] scenes = EditorBuildSettings.scenes
            .Where(s => s.enabled)
            .Select(s => s.path)
            .ToArray();

        if (scenes.Length == 0)
        {
            Debug.LogError("AutoBuild: no enabled scenes in Build Profiles / Scene List.");
            EditorApplication.Exit(1);
            return;
        }

        if (!Build(scenes, BuildTarget.StandaloneWindows64,
                   "Builds/Windows/DowntownDevour.exe", "Windows PC")) return;

        if (!Build(scenes, BuildTarget.WebGL,
                   "Builds/Web", "Web")) return;

        Debug.Log("AutoBuild: both platforms built successfully.");
        EditorApplication.Exit(0);
    }

    static bool Build(string[] scenes, BuildTarget target, string path, string label)
    {
        Debug.Log($"AutoBuild: building {label} -> {path}");

        var report = BuildPipeline.BuildPlayer(new BuildPlayerOptions
        {
            scenes           = scenes,
            locationPathName = path,
            target           = target,
            options          = BuildOptions.None,
        });

        if (report.summary.result != BuildResult.Succeeded)
        {
            Debug.LogError($"AutoBuild: {label} build FAILED " +
                           $"({report.summary.totalErrors} errors).");
            EditorApplication.Exit(1);
            return false;
        }

        Debug.Log($"AutoBuild: {label} build succeeded " +
                  $"({report.summary.totalSize / (1024 * 1024)} MB).");
        return true;
    }
}
