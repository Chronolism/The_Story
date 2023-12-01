using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    //���ʺ��ģ�PlayerRuntime
    PlayerRuntime _playerRuntime;
    /// <summary>
    /// ��Ҳ���ģʽ
    /// </summary>
    public E_PlayerControlMode playerControlMode = E_PlayerControlMode.Pacman;
    /// <summary>
    /// ֡λ��
    /// </summary>
    public Vector3 displacementThisFrameDirction;
    /// <summary>
    /// ��¼��һ֡���൱�Ĳ�����Ϊ����ʼĬ������
    /// </summary>
    Vector3Int playInputLastFrame = new Vector3Int(1, 0, 0);
    //�����ƶ���̬���������߼�֡����
    public int UpDown = 0;
    public int RightLeft = 0;
    public int Special = 0;

    private void Awake()
    {
        //_playerRuntime = this.transform.parent.GetComponent<PlayerRuntime>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpDown = 0;
        RightLeft = 0;
        Special = 0;
        if (IEnableInput.GetKey(E_PlayKeys.W)) UpDown += 1;
        if (IEnableInput.GetKey(E_PlayKeys.S)) UpDown += -1;
        if (IEnableInput.GetKey(E_PlayKeys.D)) RightLeft += 1;
        if (IEnableInput.GetKey(E_PlayKeys.A)) RightLeft += -1;
        if (IEnableInput.GetKey(E_PlayKeys.E)) Special = 1;
        if (IEnableInput.GetKey(E_PlayKeys.Q)) Special = 2;
        //����Q���������QE�����ȴ���Q

        /*
         //������Ҳ���ģʽ������Ľ�ɫ�ٿ������߼�֡���£�
         switch (playerControlMode)
         {
             case E_PlayerControlMode.Pacman:
                 if ((UpDown == playInputLastFrame.x && RightLeft == 0)
                     || (RightLeft == playInputLastFrame.y && UpDown == 0)
                     || (UpDown == 0 && RightLeft == 0))
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
                 displacementThisFrameDirction = new Vector3(RightLeft, UpDown, 0);
                 break;
             case E_PlayerControlMode.Free8:
                 displacementThisFrameDirction = new Vector3(RightLeft, UpDown, 0);
                 break;
         }
         _playerRuntime.displacementThisFrameDirctionTrue = displacementThisFrameDirction;
         //��ײ̽��
         //if (MapManager.Instance.runtimeGrid != null) AbstractLogicManager.Instance.CellProbe(ref UpDown, ref RightLeft, _playerRuntime.PlayerData.runtime_gird_Position);
         //��¼��һ�߼�֡���൱�Ĳ�����Ϊ����Pacmanģʽ��������һֱ�ڰ�ĳһ����        
         playInputLastFrame = new Vector3Int(UpDown, RightLeft, Special);
        */
    }
    private void FixedUpdate()
    {
        
    }
}

