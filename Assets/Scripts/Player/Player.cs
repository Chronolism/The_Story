using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour,IEnableInput
{
    /// <summary>
    /// 玩家操作模式
    /// </summary>
    public E_PlayerControlMode playerControlMode = E_PlayerControlMode.Pacman;
    /// <summary>
    /// 运行时id，首次是赋予，之后供索引
    /// </summary>
    int _runtime_id = 0;
    /// <summary>
    /// 玩家数据集，应该在进入时获取
    /// </summary>
    D_Base_Player _PlayerData { get =>PlayerManager.Instance.GetPlayerDataWithRuntime_Id(_runtime_id); }
    /// <summary>
    /// 帧位移
    /// </summary>
    Vector3 displacementThisFrameDirction;
    /// <summary>
    /// 记录上一帧的相当的操作行为，初始默认向右
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
        //这里Q在最后，则按下QE会优先触发Q

        //根据玩家操纵模式，初版的角色操控器
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
        //核心操控公式
        this.transform.position += Time.deltaTime * displacementThisFrameDirction;
        //记录上一帧的相当的操作行为，如Pacman模式就是视作一直在按某一方向
        playInputLastFrame = new Vector3Int(UpDown, RightLeft, Special);

        
    }
    
}
public enum E_PlayerControlMode
{
    Pacman,
    Free4,
    Free8,
}
