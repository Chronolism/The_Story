using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRuntime : MonoBehaviour
{
    //这个脚本是挂载在任意角色预制体的最高空物体上，用于控制整个运行时数据

    //不常变动的数据通过PlayerManager获取
    /// <summary>
    /// 运行时id，首次是赋予，之后供索引
    /// </summary>
    int _runtime_id = 400;bool _needChangeID = true;
    /// <summary>
    /// 玩家数据集，应该在进入时获取
    /// </summary>
    public D_Base_Player PlayerData;
    //较常变动的数据留在本地
    /// <summary>
    /// 帧位移方向（非逻辑帧级别）
    /// </summary>
    public Vector3 displacementThisFrameDirctionTrue;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //如果角色ID发生更改，即时赋值
        if (_needChangeID)
        {
            PlayerData = PlayerManager.Instance.GetPlayerDataWithRuntime_Id(_runtime_id);
            //_PlayerData.runtime_Player = this.gameObject;
            _needChangeID = false;
        }
        //这里写需要用到玩家信息的画面帧
        if (PlayerData != null) DisplayUpdate();
        
    }
    private void FixedUpdate()
    {
        if (PlayerData != null) LogicUpdate();
    }
    /// <summary>
    /// 画面帧中枢
    /// </summary>
    void DisplayUpdate()
    {

    }
    /// <summary>
    /// 逻辑帧中枢
    /// </summary>
    void LogicUpdate()
    {
        if (MapManager.Instance.runtimeGrid == null) return;//临时用来判断游戏是否在运行时
        //核心移动显示公式（逻辑帧更新——暂且与物理系统同步）
        this.transform.position += Time.deltaTime * displacementThisFrameDirctionTrue;
        //每个逻辑帧更新该玩家的网格位置（V2）
        PlayerData.runtime_gird_Position = new V2(MapManager.Instance.runtimeGrid.WorldToCell(this.transform.position).x, MapManager.Instance.runtimeGrid.WorldToCell(this.transform.position).y);
        print(PlayerManager.Instance.LocalPlayer.runtime_gird_Position.x + "," + PlayerManager.Instance.LocalPlayer.runtime_gird_Position.y);
    }
}
