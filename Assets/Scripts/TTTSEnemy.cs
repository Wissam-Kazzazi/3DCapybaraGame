using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

/// <summary>
/// Tung Tung Tung Sahur — capsule placeholder enemy.
/// Waits at the exit doorway of whichever room the player is currently in,
/// swinging a bat (child Transform) back and forth.
/// If the player leaves the room before the video finishes, TTTS chases them.
/// Touching the player reloads the scene (full game restart).
///
/// Place on a Capsule at scene root. Add a thin child cube named "Bat".
/// Wire: player, batTransform, and all RoomState references in the scene.
/// TTTSEnemy builds its internal maps automatically from RoomState components at Start.
/// </summary>
public class TTTSEnemy : MonoBehaviour
{
    [Header("References")]
    [Tooltip("The player's Transform.")]
    public Transform player;

    [Tooltip("Child Transform used as the bat prop. Rotates during idle.")]
    public Transform batTransform;

    [Header("Movement")]
    [Tooltip("Speed when moving to/from the doorway standby position.")]
    public float normalSpeed = 2f;

    [Tooltip("Speed when chasing the player after an early exit.")]
    public float chaseSpeed = 8f;

    [Tooltip("Distance at which TTTS catches the player and restarts the game.")]
    public float catchDistance = 1.5f;

    [Tooltip("How far outside the door TTTS stands while waiting.")]
    public float doorwayOffset = 1.5f;

    [Header("Bat swing")]
    public float batSwingSpeed = 2.5f;
    public float batSwingAngle = 50f;

    // Maps VideoPlayer -> RoomState and VideoPlayer -> door Transform
    readonly Dictionary<VideoPlayer, RoomState>  _roomMap = new();
    readonly Dictionary<VideoPlayer, Transform>  _doorMap = new();

    bool      _isChasing;
    Vector3   _waitPosition;
    bool      _hasWaitPosition;

    void Start()
    {
        foreach (var rs in Object.FindObjectsByType<RoomState>(FindObjectsSortMode.None))
        {
            var vp = rs.VideoPlayerRef;
            if (vp == null) continue;
            _roomMap[vp] = rs;
            if (rs.door != null) _doorMap[vp] = rs.door.transform;
        }

        RoomState.OnRoomCompleted += OnRoomCompleted;
    }

    void OnDestroy()
    {
        RoomState.OnRoomCompleted -= OnRoomCompleted;
    }

    void Update()
    {
        SwingBat();
        UpdateChaseState();

        if (_isChasing)
            Chase();
        else
            MoveToWaitPosition();
    }

    void SwingBat()
    {
        if (batTransform == null) return;
        float angle = Mathf.Sin(Time.time * batSwingSpeed) * batSwingAngle;
        batTransform.localRotation = Quaternion.Euler(angle, 0f, 0f);
    }

    void UpdateChaseState()
    {
        if (_isChasing) return; // stays chasing until next room entry resets it

        var activeVP = RoomVideoTrigger.ActivePlayer;
        if (activeVP == null)
        {
            // Player is between room triggers. If we know the last room wasn't complete, chase.
            // We keep _isChasing as-is; it was set when player exited.
            return;
        }

        if (!_roomMap.TryGetValue(activeVP, out var room)) return;

        // Update standby position to this room's door
        if (_doorMap.TryGetValue(activeVP, out var doorT))
        {
            _waitPosition    = doorT.position + doorT.forward * doorwayOffset;
            _hasWaitPosition = true;
        }

        // Only chase if player is inside a room and that room's video isn't done.
        // (If the video is done, the door is open and TTTS should step aside.)
        _isChasing = false;
    }

    void OnRoomCompleted(int _)
    {
        _isChasing = false;
    }

    // Called by RoomVideoTrigger indirectly: when player exits an incomplete room
    // the ActivePlayer becomes null while the room is not complete -> we need to detect that.
    // We hook into Update: when ActivePlayer goes null AND the last known room was incomplete.
    // To do this cleanly, track the last seen room state.
    RoomState _lastRoom;

    void LateUpdate()
    {
        var activeVP = RoomVideoTrigger.ActivePlayer;

        if (activeVP != null && _roomMap.TryGetValue(activeVP, out var room))
        {
            _lastRoom = room;
        }
        else if (activeVP == null && _lastRoom != null && !_lastRoom.IsComplete)
        {
            // Player has left the trigger zone of an incomplete room → chase!
            _isChasing = true;
        }
    }

    void Chase()
    {
        if (player == null) return;

        transform.position = Vector3.MoveTowards(
            transform.position, player.position, chaseSpeed * Time.deltaTime);

        FaceTarget(player.position);

        if (Vector3.Distance(transform.position, player.position) < catchDistance)
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void MoveToWaitPosition()
    {
        if (!_hasWaitPosition) return;

        transform.position = Vector3.MoveTowards(
            transform.position, _waitPosition, normalSpeed * Time.deltaTime);

        FaceTarget(_waitPosition);
    }

    void FaceTarget(Vector3 target)
    {
        Vector3 dir = target - transform.position;
        dir.y = 0f;
        if (dir.sqrMagnitude > 0.001f)
            transform.rotation = Quaternion.LookRotation(dir);
    }
}
