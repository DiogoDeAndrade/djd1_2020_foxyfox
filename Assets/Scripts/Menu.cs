using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Menu : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI highscoreText;

    private void Start()
    {
        GameMng gameManager = FindObjectOfType<GameMng>();
        int highscore = gameManager.GetHighscore();

        highscoreText.text = "Highscore: " + highscore;
    }

    public void StartGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
