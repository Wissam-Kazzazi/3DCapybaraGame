using UnityEngine;

/// <summary>
/// Keeps an invisible BoxCollider blocking the room exit until the video finishes.
/// The CharacterController cannot pass solid (non-trigger) colliders, so no player
/// script changes are needed — just enable/disable this collider.
///
/// Place on the same GameObject as RoomState, or any child. Wire both fields.
/// The blockerCollider should be a BoxCollider (isTrigger=false) sized to fill the doorway.
/// </summary>
public class RoomLock : MonoBehaviour
{
    [SerializeField] RoomState roomState;
    [SerializeField] Collider  blockerCollider;

    void OnEnable()
    {
        RoomState.OnRoomCompleted += HandleRoomCompleted;
        if (blockerCollider != null)
            blockerCollider.enabled = !roomState.IsComplete;
    }

    void OnDisable()
    {
        RoomState.OnRoomCompleted -= HandleRoomCompleted;
    }

    void HandleRoomCompleted(int roomNumber)
    {
        if (roomState == null) return;
        if (roomNumber == roomState.roomNumber && blockerCollider != null)
            blockerCollider.enabled = false;
    }
}
