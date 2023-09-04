using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ͨ��IEnableInput.GetKeyCode��IEnableInput.GetKey��IEnableInput.GetKeyDown��IEnableInput.GetKeyUpʵ�ֻ�����λӳ��
/// ����дAsKeyDown��AsKeyUpʵ�ָ����ӵ��߼�����
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
    /// ��������ʽ���뺯����ͨ���ӿ�ͳһ�����ã������Բ�ʹ�ã���InputMgr��CheckKeyCode(KeyCode.W)���;
    /// ʹ�÷�������ȷ���Ѿ�ִ��
    ///     InputMgr.Instance.StartOrEndCheck(true);
    ///     EventCenter.Instance.AddEventListener<KeyCode>("ĳ������", AsKeyDown) ;
    ///     ������������ݴ����KeyCode���ж�ִ������
    /// </summary>
    /// <param name="keyCode">���ܵ���KeyCode</param>
    public virtual void AsKeyDown(KeyCode keyCode) { }
    /// <summary>
    /// ��������ʽ���뺯����ͨ���ӿ�ͳһ�����ã������Բ�ʹ�ã���InputMgr��CheckKeyCode(KeyCode.W)���;
    /// ʹ�÷�������ȷ���Ѿ�ִ��
    ///     InputMgr.Instance.StartOrEndCheck(true);
    ///     EventCenter.Instance.AddEventListener<KeyCode>("ĳ��̧��", AsKeyUp) ;
    ///     ������������ݴ����KeyCode���ж�ִ������
    /// </summary>
    /// <param name="keyCode">���ܵ���KeyCode</param>
    public virtual void AsKeyUp(KeyCode keyCode) { }
    //һЩ������:����Ϊ����д��Ȼ�ܹ������εĽ�������ܻ���Ϊ�ܹ��߼���һ�µ�����������Ƶ��
    //������������dop��oop���õĵ������̻��ҵĿ���
    //���Ҳϣ�����������������
}
/// <summary>
/// ��Ҽ�λ�ṹ��ָ������ļ��ּ�
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
