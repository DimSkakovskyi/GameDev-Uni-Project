using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuController : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    public void OnStartClick()
    {
        SoundEffectManager.Play("Click");
        Time.timeScale = 1;
        SceneManager.LoadScene("Generator");
    }

    public void OnContinueClick()
    {
        SoundEffectManager.Play("Click");
        gameManager.ResumeGame();
    }

    public void OnExitClick()
    {
        SoundEffectManager.Play("Click");

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
}
