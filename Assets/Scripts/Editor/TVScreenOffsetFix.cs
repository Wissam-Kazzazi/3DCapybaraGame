using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

/// <summary>
/// Nudges every "Screen" MeshRenderer that uses M_Screen forward by 0.05 units
/// on local Z so it stops z-fighting with the wall.
/// Run once: Tools > Fix TV Screen Z Offset
/// </summary>
public static class TVScreenOffsetFix
{
    [MenuItem("Tools/Fix TV Screen Z Offset")]
    static void FixScreens()
    {
        var renderers = Object.FindObjectsByType<MeshRenderer>(FindObjectsSortMode.None);
        int fixed_ = 0;

        foreach (var mr in renderers)
        {
            if (mr.gameObject.name != "Screen") continue;
            if (mr.sharedMaterial == null) continue;
            if (!mr.sharedMaterial.name.Contains("M_Screen")) continue;

            Undo.RecordObject(mr.transform, "Fix Screen Z Offset");
            var lp = mr.transform.localPosition;
            lp.z = -0.05f; // pull toward the viewer (away from wall)
            mr.transform.localPosition = lp;
            fixed_++;
        }

        if (fixed_ > 0)
            EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());

        EditorUtility.DisplayDialog("Fix TV Screen Z Offset",
            fixed_ > 0
                ? $"Adjusted {fixed_} screen(s) by -0.05 on local Z. Save the scene (Ctrl+S).\n\n" +
                  "If screens moved the WRONG direction, undo (Ctrl+Z) and change the sign of lp.z in TVScreenOffsetFix.cs."
                : "No 'Screen' objects with M_Screen material found.",
            "OK");
    }
}
