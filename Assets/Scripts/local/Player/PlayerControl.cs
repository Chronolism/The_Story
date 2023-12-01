using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    //访问核心：PlayerRuntime
    PlayerRuntime _playerRuntime;
    /// <summary>
    /// 玩家操作模式
    /// </summary>
    public E_PlayerControlMode playerControlMode = E_PlayerControlMode.Pacman;
    /// <summary>
    /// 帧位移
    /// </summary>
    public Vector3 displacementThisFrameDirction;
    /// <summary>
    /// 记录上一帧的相当的操作行为，初始默认向右
    /// </summary>
    Vector3Int playInputLastFrame = new Vector3Int(1, 0, 0);
    //这是移动动态参数（非逻辑帧级别）
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
        //这里Q在最后，则按下QE会优先触发Q

        /*
         //根据玩家操纵模式，初版的角色操控器（逻辑帧更新）
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
         //碰撞探针
         //if (MapManager.Instance.runtimeGrid != null) AbstractLogicManager.Instance.CellProbe(ref UpDown, ref RightLeft, _playerRuntime.PlayerData.runtime_gird_Position);
         //记录上一逻辑帧的相当的操作行为，如Pacman模式就是视作一直在按某一方向        
         playInputLastFrame = new Vector3Int(UpDown, RightLeft, Special);
        */
    }
    private void FixedUpdate()
    {
        
    }
}

