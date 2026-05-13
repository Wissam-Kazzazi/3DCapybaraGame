using UnityEngine;

[RequireComponent(typeof(Collider))]
public class WinOrbTrigger : MonoBehaviour
{
    [Header("Win Settings")]
    public string playerTag = "Player";
    public WinScreenManager winScreenManager;

    [Header("Optional Effects")]
    public GameObject touchEffect;
    public AudioSource winSound;
    public bool hideOrbAfterWin = true;

    private bool hasWon = false;

    void Reset()
    {
        // Automatically make the collider a trigger when this script is added.
        GetComponent<Collider>().isTrigger = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (hasWon) return;
        if (!other.CompareTag(playerTag)) return;

        hasWon = true;

        if (touchEffect != null)
        {
            Instantiate(touchEffect, transform.position, Quaternion.identity);
        }

        if (winSound != null)
        {
            winSound.Play();
        }

        if (winScreenManager != null)
        {
            winScreenManager.ShowWinScreen();
        }
        else
        {
            Debug.LogWarning("WinOrbTrigger needs a WinScreenManager assigned.");
        }

        if (hideOrbAfterWin)
        {
            gameObject.SetActive(false);
        }
    }
}
