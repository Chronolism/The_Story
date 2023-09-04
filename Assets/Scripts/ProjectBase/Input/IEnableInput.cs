using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 通过IEnableInput.GetKeyCode、IEnableInput.GetKey、IEnableInput.GetKeyDown、IEnableInput.GetKeyUp实现基础键位映射
/// 可重写AsKeyDown、AsKeyUp实现更复杂的逻辑控制
/// </summary>
public interface IEnableInput
{
    static protected KeyCode GetKeyCode(E_PlayKeys key)
    {
        return InputMgr.Instance.keyPairs[key];
    }
    static protected bool GetKey(E_PlayKeys key)
    {
        return Input.GetKey(InputMgr.Instance.keyPairs[key]);
    }
    static protected bool GetKeyDown(E_PlayKeys key)
    {
        return Input.GetKeyDown(InputMgr.Instance.keyPairs[key]);
    }
    static protected bool GetKeyUp(E_PlayKeys key)
    {
        return Input.GetKeyUp(InputMgr.Instance.keyPairs[key]);
    }
    /// <summary>
    /// 生命周期式输入函数（通过接口统一被调用），可以不使用（与InputMgr的CheckKeyCode(KeyCode.W)相关;
    /// 使用方法：请确保已经执行
    ///     InputMgr.Instance.StartOrEndCheck(true);
    ///     EventCenter.Instance.AddEventListener<KeyCode>("某键按下", AsKeyDown) ;
    ///     函数体中请根据传入的KeyCode来判断执行内容
    /// </summary>
    /// <param name="keyCode">接受到的KeyCode</param>
    public virtual void AsKeyDown(KeyCode keyCode) { }
    /// <summary>
    /// 生命周期式输入函数（通过接口统一被调用），可以不使用（与InputMgr的CheckKeyCode(KeyCode.W)相关;
    /// 使用方法：请确保已经执行
    ///     InputMgr.Instance.StartOrEndCheck(true);
    ///     EventCenter.Instance.AddEventListener<KeyCode>("某键抬起", AsKeyUp) ;
    ///     函数体中请根据传入的KeyCode来判断执行内容
    /// </summary>
    /// <param name="keyCode">接受到的KeyCode</param>
    public virtual void AsKeyUp(KeyCode keyCode) { }
    //一些碎碎念:我认为这样写虽然能够更深层次的解耦，但可能会因为架构逻辑不一致导致正反调用频繁
    //而产生类似于dop与oop混用的导致流程混乱的可能
    //这边也希望问问其他的意见？
}
/// <summary>
/// 玩家键位结构，指玩家有哪几种键
/// </summary>
public enum E_PlayKeys
{
    W,
    A,
    S,
    D,
    E,
    Q
}
