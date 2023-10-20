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
    public uint runtime_id = 400; public bool needChangeID = true;
    public bool networkEnable = true; //{ get => !GameManager.Instance.isOfflineLocalTest; set => GameManager.Instance.isOfflineLocalTest = !value; }网络端与离线测试同一后使用此行代码
    Player _networkPlayer;
    /// <summary>
    /// 玩家数据集，应该在进入时获取
    /// </summary>
    public D_Base_Player PlayerData;
    //较常变动的数据留在本地
    /// <summary>
    /// 帧位移方向（非逻辑帧级别）
    /// </summary>
    public Vector3 displacementThisFrameDirctionTrue;
    //注册表
    public GameObject inputAndMove;
    float _timer;
    void Awake()
    {
        
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //如果角色ID发生更改，即时赋值
        if (needChangeID)
        {
            PlayerData = PlayerManager.Instance.GetPlayerDataWithRuntime_Id(runtime_id);
            if (PlayerData != null) PlayerData.runtime_Player = this.gameObject;
            needChangeID = false;
        }
        //这里写需要用到玩家信息的画面帧
        if (PlayerData != null) DisplayUpdate();
        _timer += Time.deltaTime;
        if (_timer >= 1){ _timer = 0; SecUpdate(); }
        //网络ID同步与获取
        



    }
    private void FixedUpdate()
    {
        //if (PlayerData != null) LogicUpdate();
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
        /*
        if (MapManager.Instance.runtimeGrid == null) return;//临时用来判断游戏是否在运行时
        //核心移动显示公式（逻辑帧更新——暂且与物理系统同步）
        this.transform.position += Time.deltaTime * displacementThisFrameDirctionTrue;
        */
        //网络id同步
        if (networkEnable && _networkPlayer != null)
        {
            if (_networkPlayer.isLocalPlayer)
            {
                runtime_id = _networkPlayer.netId;
                PlayerManager.Instance.LocalPlayer.runtime_id = _networkPlayer.netId;
            }
            else
            {
                runtime_id = _networkPlayer.netId;
                PlayerManager.Instance.AddOtherPlayer(_networkPlayer.netId, PlayerData);
            }
            needChangeID = true;
            networkEnable = false;
        }
        //每个逻辑帧更新该玩家的网格位置（V2）
        PlayerData.runtime_gird_Position = new V2(MapManager.Instance.runtimeGrid.WorldToCell(this.transform.position).x, MapManager.Instance.runtimeGrid.WorldToCell(this.transform.position).y);
        //每个逻辑帧试图将自身传入中枢
        if (PlayerData.runtime_Player == null) PlayerData.runtime_Player = this.gameObject;


    }
    /// <summary>
    /// 秒行为
    /// </summary>
    void SecUpdate()
    {
        //每个秒Debug玩家的网格位置（V2）
        //print(PlayerManager.Instance.LocalPlayer.runtime_gird_Position.x + "," + PlayerManager.Instance.LocalPlayer.runtime_gird_Position.y);
        //试图获取Net信息
        if (networkEnable)
            TryGetComponent<Player>(out _networkPlayer);
        
    }
}
