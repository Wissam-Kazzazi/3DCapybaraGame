using UnityEngine;
using UnityEngine.Video;

[RequireComponent(typeof(BoxCollider))]
public class RoomVideoTrigger : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public string triggerTag = "Player";
    public int roomNumber;

    [SerializeField] Transform tttsWaypoint;

    public static VideoPlayer ActivePlayer => s_active;
    public static int ActiveRoom => s_activeRoom;

    static VideoPlayer s_active;
    static int s_activeRoom;

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
        if (s_active == videoPlayer)
        {
            s_active = null;
            s_activeRoom = 0;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (videoPlayer == null) return;
        if (!other.CompareTag(triggerTag)) return;

        if (s_active != null && s_active != videoPlayer)
        {
            bool prevComplete = RoomManager.Instance != null &&
                RoomManager.Instance.roomCompleted[s_activeRoom];
            if (!prevComplete) s_active.Stop();
        }

        bool thisComplete = RoomManager.Instance != null &&
            RoomManager.Instance.roomCompleted[roomNumber];
        if (!thisComplete) videoPlayer.Play();

        s_active = videoPlayer;
        s_activeRoom = roomNumber;

        if (RoomManager.Instance != null)
            RoomManager.Instance.PlayerEnteredRoom(roomNumber, videoPlayer, tttsWaypoint);
    }

    void OnTriggerExit(Collider other)
    {
        if (videoPlayer == null) return;
        if (!other.CompareTag(triggerTag)) return;

        bool complete = RoomManager.Instance != null &&
            RoomManager.Instance.roomCompleted[roomNumber];
        if (!complete) videoPlayer.Stop();

        if (s_active == videoPlayer)
        {
            s_active = null;
            s_activeRoom = 0;
        }

        if (RoomManager.Instance != null)
            RoomManager.Instance.PlayerExitedRoom();
    }
}