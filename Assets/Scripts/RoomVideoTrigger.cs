using UnityEngine;
using UnityEngine.Video;

/// <summary>
/// Drop on a GameObject with a BoxCollider (trigger) covering the room interior.
/// When the player enters, plays whichever is wired up:
///   1) MockVideoScreen (testing without real video files), OR
///   2) VideoPlayer (real video, once clips are assigned).
/// On exit, stops it.
/// Also detects players that are already inside the trigger at scene load
/// (Unity doesn't fire OnTriggerEnter for that case).
/// </summary>
[RequireComponent(typeof(BoxCollider))]
public class RoomVideoTrigger : MonoBehaviour
{
    [Header("Wire up ONE of these (mock wins if both set)")]
    public MockVideoScreen mockScreen;
    public VideoPlayer videoPlayer;

    [Header("Behavior")]
    [Tooltip("If true, video/mock restarts from beginning each time the player enters. If false, pauses and resumes.")]
    public bool restartOnEnter = true;

    [Tooltip("Tag of the object that activates the screen. Default 'Player'.")]
    public string triggerTag = "Player";

    BoxCollider _box;
    bool _playerInside;

    void Reset()
    {
        var col = GetComponent<BoxCollider>();
        if (col != null) col.isTrigger = true;
    }

    void Awake()
    {
        _box = GetComponent<BoxCollider>();
        if (_box != null) _box.isTrigger = true;

        if (mockScreen == null && videoPlayer == null && transform.parent != null)
        {
            mockScreen  = transform.parent.GetComponentInChildren<MockVideoScreen>();
            videoPlayer = transform.parent.GetComponentInChildren<VideoPlayer>();
        }
        if (videoPlayer != null) videoPlayer.playOnAwake = false;
    }

    void Start()
    {
        // Catch the case where the player spawned already inside this trigger.
        // OnTriggerEnter doesn't fire for objects that were already overlapping.
        var hits = Physics.OverlapBox(
            _box.bounds.center, _box.bounds.extents,
            Quaternion.identity, ~0, QueryTriggerInteraction.Ignore);
        foreach (var h in hits)
            if (h.CompareTag(triggerTag)) { Activate(); break; }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(triggerTag)) return;
        Activate();
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag(triggerTag)) return;
        Deactivate();
    }

    void Activate()
    {
        if (_playerInside) return;
        _playerInside = true;

        if (mockScreen != null) { mockScreen.Play(); return; }
        if (videoPlayer != null)
        {
            if (restartOnEnter) videoPlayer.Stop();
            videoPlayer.Play();
        }
    }

    void Deactivate()
    {
        if (!_playerInside) return;
        _playerInside = false;

        if (mockScreen != null) { mockScreen.Stop(); return; }
        if (videoPlayer != null)
        {
            if (restartOnEnter) videoPlayer.Stop();
            else videoPlayer.Pause();
        }
    }
}
