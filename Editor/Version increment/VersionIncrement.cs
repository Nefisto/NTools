#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;

public static class VersionManager
{
    [MenuItem("Tools/NTools/Version/Increment Patch", priority = 1)]
    public static void IncrementPatch() => UpdateVersion(v => new Version(v.Major, v.Minor, v.Build + 1));

    [MenuItem("Tools/NTools/Version/Increment Minor", priority = 2)]
    public static void IncrementMinor() => UpdateVersion(v => new Version(v.Major, v.Minor + 1, 0));

    [MenuItem("Tools/NTools/Version/Increment Major", priority = 3)]
    public static void IncrementMajor() => UpdateVersion(v => new Version(v.Major + 1, 0, 0));

    private static void UpdateVersion (Func<Version, Version> updaterCallback)
    {
        var newVersion = updaterCallback(new Version(PlayerSettings.bundleVersion));

        PlayerSettings.bundleVersion = newVersion.ToString();
        Debug.Log($"Version updated to: v{newVersion}");

        AssetDatabase.SaveAssets();
    }
}
#endif