using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonsEndGame : MonoBehaviour
{

    public void RestartScene()
    {
        SceneManager.LoadScene("Game");
    }

    public void MainMenuScene()
    {
        SceneManager.LoadScene("MainMenu");
    }

}
