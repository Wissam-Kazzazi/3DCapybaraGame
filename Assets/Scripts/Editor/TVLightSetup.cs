using UnityEngine;
using UnityEngine.Video;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

/// <summary>
/// Adds a warm PointLight child to every TV_R* in the open scene.
/// Run once: Tools > Add TV Point Lights
/// Safe to run again — skips TVs that already have a "TVLight" child.
/// </summary>
public static class TVLightSetup
{
    [MenuItem("Tools/Add TV Point Lights")]
    static void AddLights()
    {
        var players = Object.FindObjectsByType<VideoPlayer>(FindObjectsSortMode.None);
        if (players.Length == 0)
        {
            EditorUtility.DisplayDialog("Add TV Point Lights",
                "No VideoPlayers found. Open Game.unity and try again.", "OK");
            return;
        }

        int added = 0;
        foreach (var vp in players)
        {
            // Walk up to find the TV_R* parent
            Transform tvRoot = vp.transform;
            while (tvRoot != null && !tvRoot.name.StartsWith("TV_"))
                tvRoot = tvRoot.parent;
            if (tvRoot == null) continue;

            if (tvRoot.Find("TVLight") != null) continue; // already set up

            var go = new GameObject("TVLight");
            Undo.RegisterCreatedObjectUndo(go, "Add TV Light");
            go.transform.SetParent(tvRoot, false);
            // Offset slightly in front of the screen (local -Z = toward viewer for most TV orientations)
            go.transform.localPosition = new Vector3(0f, 0f, -0.5f);

            var light = go.AddComponent<Light>();
            light.type      = LightType.Point;
            light.color     = new Color(1f, 0.95f, 0.8f);
            light.intensity = 3f;
            light.range     = 5f;
            light.shadows   = LightShadows.None;

            added++;
        }

        if (added > 0)
            EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());

        EditorUtility.DisplayDialog("Add TV Point Lights",
            added > 0 ? $"Added {added} point light(s). Save the scene (Ctrl+S)."
                      : "All TVs already have lights. Nothing changed.",
            "OK");
    }
}
