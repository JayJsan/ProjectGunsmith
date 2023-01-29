using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverScreen : MonoBehaviour
{
    // Text taken and modified from https://youtu.be/K4uOjb5p3Io
    public TextMeshProUGUI scoreText;
    public void Setup(int score)
    {
        gameObject.SetActive(true);
        scoreText.text = "SCORE: " + score.ToString();
    }

    public void RetryButton()
    {
        SceneManager.LoadScene("ArenaTest");
    }

    public void ReturnToMenuButton()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
