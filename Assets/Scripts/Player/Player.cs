using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour,IEnableInput
{
    /// <summary>
    /// ��Ҳ���ģʽ
    /// </summary>
    public E_PlayerControlMode playerControlMode = E_PlayerControlMode.Pacman;
    /// <summary>
    /// ����ʱid���״��Ǹ��裬֮������
    /// </summary>
    int _runtime_id = 0;
    /// <summary>
    /// ������ݼ���Ӧ���ڽ���ʱ��ȡ
    /// </summary>
    D_Base_Player _PlayerData { get =>PlayerManager.Instance.GetPlayerDataWithRuntime_Id(_runtime_id); }
    /// <summary>
    /// ֡λ��
    /// </summary>
    Vector3 displacementThisFrameDirction;
    /// <summary>
    /// ��¼��һ֡���൱�Ĳ�����Ϊ����ʼĬ������
    /// </summary>
    Vector3Int playInputLastFrame = new Vector3Int(1, 0, 0);
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        int UpDown = 0;
        int RightLeft = 0;
        int Special = 0;
        if (IEnableInput.GetKey(E_PlayKeys.W)) UpDown += 1;
        if (IEnableInput.GetKey(E_PlayKeys.S)) UpDown += -1;
        if (IEnableInput.GetKey(E_PlayKeys.D)) RightLeft += 1;
        if (IEnableInput.GetKey(E_PlayKeys.A)) RightLeft += -1;
        if (IEnableInput.GetKey(E_PlayKeys.E)) Special = 1;
        if (IEnableInput.GetKey(E_PlayKeys.Q)) Special = 2;
        //����Q���������QE�����ȴ���Q

        //������Ҳ���ģʽ������Ľ�ɫ�ٿ���
        switch (playerControlMode)
        {
            case E_PlayerControlMode.Pacman:
                if (   (UpDown == playInputLastFrame.x && RightLeft == 0)
                    || (RightLeft == playInputLastFrame.y && UpDown == 0) 
                    || (UpDown == 0 && RightLeft == 0)   )
                {
                    UpDown = playInputLastFrame.x;
                    RightLeft = playInputLastFrame.y;
                    displacementThisFrameDirction = new Vector3(RightLeft, UpDown, 0);
                    break;
                }                                   
                else
                {
                    if (UpDown != 0 && RightLeft != 0)
                        UpDown = 0;
                    displacementThisFrameDirction = new Vector3(RightLeft, UpDown, 0);
                }                   
                break;
            case E_PlayerControlMode.Free4:
                if (UpDown != 0 && RightLeft != 0)
                    UpDown = 0;                    
                displacementThisFrameDirction = new Vector3( RightLeft, UpDown, 0);
                break;
            case E_PlayerControlMode.Free8:
                displacementThisFrameDirction = new Vector3( RightLeft, UpDown, 0);
                break;
        }
        //���Ĳٿع�ʽ
        this.transform.position += Time.deltaTime * displacementThisFrameDirction;
        //��¼��һ֡���൱�Ĳ�����Ϊ����Pacmanģʽ��������һֱ�ڰ�ĳһ����
        playInputLastFrame = new Vector3Int(UpDown, RightLeft, Special);

        
    }
    
}
public enum E_PlayerControlMode
{
    Pacman,
    Free4,
    Free8,
}
