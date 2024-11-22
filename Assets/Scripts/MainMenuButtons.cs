using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuButtons : MonoBehaviour
{

    public TextMeshProUGUI high_score_text;

    private void Update()
    {
        if (PlayerPrefs.HasKey("HighScore"))
        {
            SaveTheGame.high_score = PlayerPrefs.GetInt("HighScore");
            Debug.Log("SUCCESS: " + SaveTheGame.high_score);
        }
        high_score_text.SetText("High score: " + SaveTheGame.high_score);
    }

    public void start_game()
    {
        SceneManager.LoadScene("Game");
    }
    public void clear_result()
    {
        if (PlayerPrefs.HasKey("HighScore"))
        {
            PlayerPrefs.SetInt("HighScore", 0);
        }
    }

}
