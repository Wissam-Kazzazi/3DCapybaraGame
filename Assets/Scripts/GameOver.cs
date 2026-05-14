using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
   void Start()
   {
      Cursor.lockState = CursorLockMode.None;
      Cursor.visible = true;
   }

    public void RestartGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void MainMenuButton()
    {
        SceneManager.LoadScene("MainMenu");
    }
}