using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 通过IEnableInput.GetKeyCode、IEnableInput.GetKey、IEnableInput.GetKeyDown、IEnableInput.GetKeyUp实现基础键位映射
/// 可重写AsKeyDown、AsKeyUp实现更复杂的逻辑控制
/// </summary>
public static class IEnableInput
{
    static public KeyCode GetKeyCode(E_PlayKeys key)
    {
        return InputMgr.Instance.keyPairs[key];
    }
    static public bool GetKey(E_PlayKeys key)
    {
        return Input.GetKey(InputMgr.Instance.keyPairs[key]);
    }
    static public bool GetKeyDown(E_PlayKeys key)
    {
        return Input.GetKeyDown(InputMgr.Instance.keyPairs[key]);
    }
    static public bool GetKeyUp(E_PlayKeys key)
    {
        return Input.GetKeyUp(InputMgr.Instance.keyPairs[key]);
    }
}
/// <summary>
/// 玩家键位结构，指玩家有哪几种键
/// </summary>
public enum E_PlayKeys
{
    W = 1,
    A = 2,
    S = 4,
    D = 8,
    E = 16,
    Q = 32
}
