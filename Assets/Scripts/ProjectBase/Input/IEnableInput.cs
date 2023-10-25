using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ͨ��IEnableInput.GetKeyCode��IEnableInput.GetKey��IEnableInput.GetKeyDown��IEnableInput.GetKeyUpʵ�ֻ�����λӳ��
/// ����дAsKeyDown��AsKeyUpʵ�ָ����ӵ��߼�����
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
/// ��Ҽ�λ�ṹ��ָ������ļ��ּ�
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
