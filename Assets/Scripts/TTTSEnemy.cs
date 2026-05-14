using UnityEngine;
using UnityEngine.SceneManagement;

public class TTTSEnemy : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] Transform bat;
    [SerializeField] float chaseSpeed = 6f;
    [SerializeField] float catchDistance = 1.5f;
    [SerializeField] float batSwingSpeed = 3f;
    [SerializeField] float batSwingAngle = 50f;

    bool _isChasing = false;
    Animator _animator;

    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (player == null) return;

        SwingBat();
        UpdateChaseState();
        MoveToTarget();

        if (_isChasing && Vector3.Distance(transform.position, player.position) < catchDistance)
            SceneManager.LoadScene("GameOver");
    }

    void SwingBat()
    {
        if (bat == null) return;
        float angle = Mathf.Sin(Time.time * batSwingSpeed) * batSwingAngle;
        bat.localRotation = Quaternion.Euler(angle, 0, 0);
    }

    void UpdateChaseState()
    {
        if (RoomManager.Instance == null) return;

        int currentRoom = RoomManager.Instance.currentRoom;

        if (currentRoom != 0)
        {
            _isChasing = false;
        }
        else
        {
            _isChasing = RoomManager.Instance.lastEnteredRoom > 0 &&
                         !RoomManager.Instance.roomCompleted[RoomManager.Instance.lastEnteredRoom];
        }

        if (_animator != null)
            _animator.SetBool("isChasing", _isChasing);
    }

    void MoveToTarget()
    {
        Transform waypoint = RoomManager.Instance?.currentWaypoint;
        if (waypoint == null) return;

        Vector3 target = _isChasing ? player.position : waypoint.position;
        float speed = _isChasing ? chaseSpeed : 2f;

        float dist = Vector3.Distance(transform.position, target);
        if (dist > 0.5f)
        {
            Vector3 dir = (target - transform.position).normalized;
            transform.position += dir * speed * Time.deltaTime;
            transform.LookAt(new Vector3(target.x, transform.position.y, target.z));
        }
    }
}