using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuController : MonoBehaviour
{
    public void OnStartClick()
    {
        SoundEffectManager.Play("Click");
        SceneManager.LoadScene("Generator");
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
