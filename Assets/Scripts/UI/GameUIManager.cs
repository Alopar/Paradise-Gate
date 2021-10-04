using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameUIManager : MonoBehaviour
{
    [SerializeField] private GameObject _hud;
    [SerializeField] private GameObject _pause;
    [SerializeField] private GameObject _win;
    [SerializeField] private GameObject _tutorial;
    [SerializeField] private Slider _flyIndicator;

    public static event Action OnClearPot;
    public static event Action OnLaunchRocket;

    public void ClearPot()
    {
        OnClearPot?.Invoke();
    }

    public void LaunchRocket()
    {
        AudioManager.Play("Click");
        OnLaunchRocket?.Invoke();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void SetFlyProgress(float percent)
    {
        _flyIndicator.value = percent;
    }

    public void ShowHud()
    {
        _hud.SetActive(true);
        _pause.SetActive(false);
        _win.SetActive(false);
        _tutorial.SetActive(false);
    }

    public void ShowPause()
    {
        _hud.SetActive(false);
        _pause.SetActive(true);
        _win.SetActive(false);
        _tutorial.SetActive(false);
    }

    public void ShowWin()
    {
        _hud.SetActive(false);
        _pause.SetActive(false);
        _win.SetActive(true);
        _tutorial.SetActive(false);
    }

    public void ShowTutorial()
    {
        _hud.SetActive(false);
        _pause.SetActive(false);
        _win.SetActive(false);
        _tutorial.SetActive(true);
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
