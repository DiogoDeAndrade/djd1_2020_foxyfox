using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMng : MonoBehaviour
{
    static GameMng instance;

    int highscore = 0;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    public void BackToMainMenu(float time)
    {
        StartCoroutine(BackToMainMenuCR(time));
    }

    IEnumerator BackToMainMenuCR(float time)
    {
        yield return new WaitForSeconds(time);

        SceneManager.LoadScene("MainMenu");
    }

    public void UpdateHighscore(int score)
    {
        if (highscore < score)
        {
            highscore = score;
        }
    }

    public int GetHighscore()
    {
        return highscore;
    }
}
