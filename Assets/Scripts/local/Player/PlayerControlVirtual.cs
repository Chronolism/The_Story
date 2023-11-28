using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControlVirtual : MonoBehaviour
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
    int UpDown = 0;
    int RightLeft = 0;
    //int Special = 0;

    private void Awake()
    {
        _playerRuntime = this.transform.parent.parent.GetComponent<PlayerRuntime>();
    }
    void Start()
    {
        if (_playerRuntime.runtime_id == 400) _playerRuntime.runtime_id = 406;
    }

    // Update is called once per frame
    void Update()
    {
        UpDown = 0;
        RightLeft = 0;
        //Special = 0;
        if (Input.GetKey(KeyCode.UpArrow)) UpDown += 1;
        if (Input.GetKey(KeyCode.DownArrow)) UpDown += -1;
        if (Input.GetKey(KeyCode.RightArrow)) RightLeft += 1;
        if (Input.GetKey(KeyCode.LeftArrow)) RightLeft += -1;
        //if (Input.GetKey(KeyCode.RightControl)) Special = 1;
        //if (Input.GetKey(KeyCode.RightAlt)) Special = 2;
        //����Q���������QE�����ȴ���Q



        this.transform.parent.GetComponent<FindTest>().InputX = RightLeft;
        this.transform.parent.GetComponent<FindTest>().InputY = UpDown;

    }
    private void FixedUpdate()
    {
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
        //��¼��һ�߼�֡���൱�Ĳ�����Ϊ����Pacmanģʽ��������һֱ�ڰ�ĳһ����
        playInputLastFrame = new Vector3Int(UpDown, RightLeft, Special);
        */
    }
}