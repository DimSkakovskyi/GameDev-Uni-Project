using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private GameObject levelFinishedScreen;
    [SerializeField] private GameObject pauseScreen;

    [SerializeField] private GameObject coinTextUI;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Time.timeScale == 1)
                PauseGame();
            else
                ResumeGame();
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        pauseScreen.SetActive(true);
        coinTextUI.SetActive(false); // Hide the coin counter
    }
    public void ResumeGame()
    {
        Time.timeScale = 1;
        pauseScreen.SetActive(false);
        coinTextUI.SetActive(true); // Show it again
    }

    public void ShowGameOver()
    {
        coinTextUI.SetActive(false);
        gameOverScreen.SetActive(true);
        Time.timeScale = 0; // optional pause
    }

    public void ShowLevelFinished()
    {
        coinTextUI.SetActive(false);
        levelFinishedScreen.SetActive(true);
        Time.timeScale = 0; // optional pause
    }
}
