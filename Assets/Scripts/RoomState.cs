using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

/// <summary>
/// Tracks whether the room's video has finished. When it does:
///   - shows the checkmark image on the TV's world-space canvas
///   - plays a ding sound
///   - rotates the door open
///   - fires OnRoomCompleted so RoomLock and TTTSEnemy can react
///
/// Place on the same GameObject as (or a parent of) the TV_R* VideoPlayer.
/// Wire all serialized fields in the Inspector.
/// </summary>
public class RoomState : MonoBehaviour
{
    [Header("Identity")]
    public int roomNumber;

    [Header("References")]
    public VideoPlayer videoPlayer;
    public GameObject door;
    public AudioSource dingSource;
    public Image checkmarkImage;

    [Header("Door")]
    public float doorOpenDuration = 1f;
    public Vector3 doorOpenEulers = new Vector3(0f, 90f, 0f);

    public bool IsComplete { get; private set; }
    public VideoPlayer VideoPlayerRef => videoPlayer;

    public static event Action<int> OnRoomCompleted;

    void Awake()
    {
        if (videoPlayer != null)
        {
            videoPlayer.isLooping = false;
            videoPlayer.playOnAwake = false;
        }

        if (dingSource != null)
        {
            dingSource.clip = AudioUtils.BuildDingClip();
            dingSource.loop = false;
            dingSource.playOnAwake = false;
            dingSource.spatialBlend = 1f;
            dingSource.maxDistance = 10f;
        }

        if (checkmarkImage != null)
        {
            var tex = AudioUtils.BuildCheckmarkTexture();
            checkmarkImage.sprite = Sprite.Create(
                tex,
                new Rect(0f, 0f, tex.width, tex.height),
                new Vector2(0.5f, 0.5f),
                100f
            );
            checkmarkImage.enabled = false;
        }
    }

    void Start()
    {
        if (videoPlayer != null)
            videoPlayer.loopPointReached += OnVideoFinished;
    }

    void OnDestroy()
    {
        if (videoPlayer != null)
            videoPlayer.loopPointReached -= OnVideoFinished;
        OnRoomCompleted = null;
    }

    void OnVideoFinished(VideoPlayer vp)
    {
        if (IsComplete) return;
        IsComplete = true;

        if (checkmarkImage != null) checkmarkImage.enabled = true;
        if (dingSource != null)     dingSource.Play();

        OnRoomCompleted?.Invoke(roomNumber);

        if (door != null) StartCoroutine(OpenDoor());
    }

    IEnumerator OpenDoor()
    {
        Quaternion start = door.transform.localRotation;
        Quaternion end   = start * Quaternion.Euler(doorOpenEulers);
        float elapsed = 0f;
        while (elapsed < doorOpenDuration)
        {
            elapsed += Time.deltaTime;
            door.transform.localRotation = Quaternion.Lerp(start, end, elapsed / doorOpenDuration);
            yield return null;
        }
        door.transform.localRotation = end;
    }
}
