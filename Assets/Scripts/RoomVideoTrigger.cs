using UnityEngine;
using UnityEngine.Video;

/// <summary>
/// Drop on a GameObject with a BoxCollider (trigger) covering the room interior.
/// Only ONE VideoPlayer in the whole scene plays at a time -- entering a new
/// room's trigger stops the previously playing one before starting this one.
/// </summary>
[RequireComponent(typeof(BoxCollider))]
public class RoomVideoTrigger : MonoBehaviour
{
    [Tooltip("VideoPlayer to control. If left blank, auto-finds one nearby.")]
    public VideoPlayer videoPlayer;

    [Tooltip("Tag of the object that activates the video.")]
    public string triggerTag = "Player";

    static VideoPlayer s_active;

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

    void OnDisable()
    {
        if (s_active == videoPlayer) s_active = null;
    }

    void OnTriggerEnter(Collider other)
    {
        if (videoPlayer == null) return;
        if (!other.CompareTag(triggerTag)) return;

        if (s_active != null && s_active != videoPlayer)
            s_active.Stop();

        videoPlayer.Play();
        s_active = videoPlayer;
    }

    void OnTriggerExit(Collider other)
    {
        if (videoPlayer == null) return;
        if (!other.CompareTag(triggerTag)) return;

        videoPlayer.Stop();
        if (s_active == videoPlayer) s_active = null;
    }
}
