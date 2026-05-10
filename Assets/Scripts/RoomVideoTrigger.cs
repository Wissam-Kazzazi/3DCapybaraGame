using UnityEngine;
using UnityEngine.Video;

/// <summary>
/// Drop on a GameObject with a BoxCollider (trigger) covering the room interior.
/// Plays the assigned VideoPlayer when an object tagged <see cref="triggerTag"/> enters.
/// Stops it when they leave.
/// </summary>
[RequireComponent(typeof(BoxCollider))]
public class RoomVideoTrigger : MonoBehaviour
{
    [Tooltip("VideoPlayer to control. If left blank, auto-finds one in this GameObject's siblings/parent's children.")]
    public VideoPlayer videoPlayer;

    [Tooltip("If true, video restarts from beginning each time the player enters. If false, it pauses and resumes.")]
    public bool restartOnEnter = true;

    [Tooltip("Tag of the object that activates the video. Default 'Player'.")]
    public string triggerTag = "Player";

    void Reset()
    {
        var col = GetComponent<BoxCollider>();
        if (col != null) col.isTrigger = true;
    }

    void Awake()
    {
        var col = GetComponent<BoxCollider>();
        if (col != null) col.isTrigger = true;

        if (videoPlayer == null)
        {
            videoPlayer = GetComponentInChildren<VideoPlayer>();
            if (videoPlayer == null && transform.parent != null)
                videoPlayer = transform.parent.GetComponentInChildren<VideoPlayer>();
        }
        if (videoPlayer != null) videoPlayer.playOnAwake = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (videoPlayer == null) return;
        if (!other.CompareTag(triggerTag)) return;
        if (restartOnEnter) videoPlayer.Stop();
        videoPlayer.Play();
    }

    void OnTriggerExit(Collider other)
    {
        if (videoPlayer == null) return;
        if (!other.CompareTag(triggerTag)) return;
        if (restartOnEnter) videoPlayer.Stop();
        else videoPlayer.Pause();
    }
}
