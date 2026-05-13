using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

/// <summary>
/// Scaffolds placeholder door cubes and invisible door-blocker colliders for all 12 rooms.
/// Run once: Tools > Scaffold Room Doors
///
/// After running:
///   1. Position each Door_R* cube in its actual doorway.
///   2. Position each DoorBlocker_R* so it fills the opening completely.
///   3. Wire Door_R* into RoomState.door and DoorBlocker_R* into RoomLock.blockerCollider.
/// </summary>
public static class RoomSceneSetup
{
    [MenuItem("Tools/Scaffold Room Doors")]
    static void ScaffoldDoors()
    {
        int created = 0;
        int skipped = 0;

        for (int i = 1; i <= 12; i++)
        {
            string doorName    = $"Door_R{i}";
            string blockerName = $"DoorBlocker_R{i}";

            // Door visual — a flat cube the player can see swing open
            if (GameObject.Find(doorName) == null)
            {
                var door = GameObject.CreatePrimitive(PrimitiveType.Cube);
                door.name = doorName;
                door.transform.localScale = new Vector3(1.5f, 3f, 0.1f);
                door.transform.position   = Vector3.zero; // position manually per room
                // Remove the auto-added BoxCollider; the blocker handles blocking
                Object.DestroyImmediate(door.GetComponent<BoxCollider>());
                Undo.RegisterCreatedObjectUndo(door, "Scaffold Door");
                created++;
            }
            else skipped++;

            // Invisible blocker — solid BoxCollider, no renderer
            if (GameObject.Find(blockerName) == null)
            {
                var blocker = new GameObject(blockerName);
                var col = blocker.AddComponent<BoxCollider>();
                col.size      = new Vector3(1.6f, 3.2f, 0.2f);
                col.isTrigger = false;
                blocker.transform.position = Vector3.zero;
                Undo.RegisterCreatedObjectUndo(blocker, "Scaffold Door Blocker");
                created++;
            }
            else skipped++;
        }

        if (created > 0)
            EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());

        EditorUtility.DisplayDialog("Scaffold Room Doors",
            $"Created {created} object(s). Skipped {skipped} that already existed.\n\n" +
            "Next steps:\n" +
            "1. Position each Door_R* in its doorway\n" +
            "2. Position each DoorBlocker_R* to fill the opening\n" +
            "3. Wire them into RoomState + RoomLock in the Inspector",
            "OK");
    }
}
