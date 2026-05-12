using UnityEngine;
using UnityEngine.Video;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

/// <summary>
/// Editor utility: scans Assets/Video for clips named after rooms (R1.mp4, R2.mp4, ...)
/// and plugs each one into the matching TV_R* GameObject's VideoPlayer.
/// Run it via: Tools -> Auto-Assign Room Videos
/// </summary>
public static class RoomVideoAutoAssigner
{
    const string VideoFolder = "Assets/Video";

    [MenuItem("Tools/Auto-Assign Room Videos")]
    public static void Assign()
    {
        var players = Object.FindObjectsByType<VideoPlayer>(FindObjectsSortMode.None);
        if (players.Length == 0)
        {
            EditorUtility.DisplayDialog("Auto-Assign Room Videos",
                "No VideoPlayers found in the open scene. Open Game.unity and try again.",
                "OK");
            return;
        }

        if (!AssetDatabase.IsValidFolder(VideoFolder))
        {
            EditorUtility.DisplayDialog("Auto-Assign Room Videos",
                "Folder '" + VideoFolder + "' does not exist. Create it and drop your mp4s in.",
                "OK");
            return;
        }

        // Build map: "R1" -> clip, "R2" -> clip, ...
        var guids = AssetDatabase.FindAssets("t:VideoClip", new[] { VideoFolder });
        var clipsByRoom = new Dictionary<string, VideoClip>(System.StringComparer.OrdinalIgnoreCase);
        foreach (var guid in guids)
        {
            var path = AssetDatabase.GUIDToAssetPath(guid);
            var name = Path.GetFileNameWithoutExtension(path);
            var clip = AssetDatabase.LoadAssetAtPath<VideoClip>(path);
            if (clip != null && !clipsByRoom.ContainsKey(name))
                clipsByRoom[name] = clip;
        }

        int assigned = 0;
        var skipped = new List<string>();

        foreach (var vp in players)
        {
            // Walk up to find a parent named TV_R*
            var t = vp.transform;
            string roomKey = null;
            while (t != null)
            {
                if (t.name.StartsWith("TV_") && t.name.Length > 3)
                {
                    roomKey = t.name.Substring(3); // "R1"
                    break;
                }
                t = t.parent;
            }
            if (roomKey == null) continue;

            if (!clipsByRoom.TryGetValue(roomKey, out var clip))
            {
                skipped.Add(roomKey + "  (need " + VideoFolder + "/" + roomKey + ".mp4)");
                continue;
            }

            Undo.RecordObject(vp, "Assign Room Video");
            vp.source = VideoSource.VideoClip;
            vp.clip = clip;
            EditorUtility.SetDirty(vp);
            assigned++;
        }

        if (assigned > 0)
        {
            UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(
                UnityEngine.SceneManagement.SceneManager.GetActiveScene());
        }

        var msg = "Assigned " + assigned + " video clip(s).";
        if (skipped.Count > 0)
            msg += "\n\nSkipped " + skipped.Count + ":\n  " + string.Join("\n  ", skipped);
        if (assigned > 0)
            msg += "\n\nScene marked dirty. Press Ctrl+S to save.";
        EditorUtility.DisplayDialog("Auto-Assign Room Videos", msg, "OK");
    }
}
