using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EventMgr
{
    public static event Action PauseGame;
    public static void CallPauseGame()
    {
        PauseGame?.Invoke();
    }

    public static event Action StartGame;
    public static void CallStartGame()
    {
        StartGame?.Invoke();
    }
}
