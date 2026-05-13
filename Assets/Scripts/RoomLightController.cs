using UnityEngine;
using UnityEngine.Video;

// attach to TriggerZone — changes lamp light red/green based on video state
public class RoomLightController : MonoBehaviour
{
    [SerializeField] Light roomLight;
    [SerializeField] VideoPlayer videoPlayer;
    [SerializeField] int roomNumber;

    bool _complete = false;

    void Start()
    {
        if (roomLight != null)
            roomLight.color = Color.red;

        if (videoPlayer != null)
            videoPlayer.loopPointReached += OnVideoComplete;
    }

    void OnVideoComplete(VideoPlayer vp)
    {
        if (_complete) return;
        _complete = true;
        if (roomLight != null)
            roomLight.color = Color.green;
    }
}