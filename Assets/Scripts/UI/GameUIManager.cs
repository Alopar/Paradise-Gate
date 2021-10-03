using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUIManager : MonoBehaviour
{
    public static event Action OnClearPot;
    public static event Action OnLaunchRocket;

    public void ClearPot()
    {
        OnClearPot?.Invoke();
    }

    public void LaunchRocket()
    {
        OnLaunchRocket?.Invoke();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }
}
