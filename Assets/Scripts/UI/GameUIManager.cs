using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
