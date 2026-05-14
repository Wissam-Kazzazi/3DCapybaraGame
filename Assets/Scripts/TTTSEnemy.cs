using UnityEngine;
using UnityEngine.SceneManagement;

public class TTTSEnemy : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] Transform bat;
    [SerializeField] float followSpeed = 0.5f;
    [SerializeField] float catchDistance = 1.5f;
    [SerializeField] float batSwingSpeed = 3f;
    [SerializeField] float batSwingAngle = 50f;

    void Update()
    {
        if (player == null) return;

        // follow player
        Vector3 dir = (player.position - transform.position).normalized;
        transform.position += dir * followSpeed * Time.deltaTime;

        // face player
        transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));

        // swing bat
        if (bat != null)
        {
            float angle = Mathf.Sin(Time.time * batSwingSpeed) * batSwingAngle;
            bat.localRotation = Quaternion.Euler(angle, 0, 0);
        }

        // catch player
        if (Vector3.Distance(transform.position, player.position) < catchDistance)
            SceneManager.LoadScene("GameOver");
    }
}