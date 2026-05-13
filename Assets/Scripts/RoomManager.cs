using UnityEngine;
using UnityEngine.Video;

// tracks player room state and video completion
public class RoomManager : MonoBehaviour
{
    public static RoomManager Instance;

    public int currentRoom = 0;
    public int lastEnteredRoom = 0;
    public bool[] roomCompleted = new bool[13];
    public Transform currentWaypoint;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void PlayerEnteredRoom(int roomNumber, VideoPlayer vp, Transform waypoint)
    {
        currentRoom = roomNumber;
        lastEnteredRoom = roomNumber;
        currentWaypoint = waypoint;
        vp.loopPointReached += (v) => OnVideoComplete(roomNumber);
    }

    public void PlayerExitedRoom()
    {
        currentRoom = 0;
    }

    void OnVideoComplete(int roomNumber)
{
    roomCompleted[roomNumber] = true;

    // check if all 12 rooms are done
    bool allDone = true;
    for (int i = 1; i <= 12; i++)
    {
        if (!roomCompleted[i]) { allDone = false; break; }
    }
    if (allDone)
        UnityEngine.SceneManagement.SceneManager.LoadScene("WinScreen");
}

    public bool IsCurrentRoomComplete()
    {
        if (currentRoom == 0) return true;
        return roomCompleted[currentRoom];
    }
}