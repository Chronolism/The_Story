using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RoomLogicBase
{
    public RoomData roomData;

    public List<Vector3Int> cellsForPlayerBorn;
    public List<Vector3Int> cellsForFeatherPenBorn;
    public List<Vector3Int> cellsForToolsBorn;
    public List<Vector3Int> cellsForServitorBorn;

    public RoomLogicBase(RoomData roomData)
    {
        this.roomData = roomData;
    }

    public void LoadMapData(string mapName)
    {
        roomData.LoadMap(mapName);
        cellsForPlayerBorn = MapManager.Instance.GetMapBaseFunction(2);
        cellsForFeatherPenBorn = MapManager.Instance.GetMapBaseFunction(0);
        cellsForToolsBorn = MapManager.Instance.GetMapBaseFunction(1);
        cellsForServitorBorn = MapManager.Instance.GetMapBaseFunction(3);
    }

    public abstract void OpenGame();

    /// <summary>
    /// 开始对局 服务端逻辑
    /// </summary>
    public abstract void StartGame();
    /// <summary>
    /// 开始对局 客户端逻辑(若有与服务端相同逻辑请使用if (!roomData.isServer) { }包裹)
    /// </summary>
    public abstract void StartGameClient();
    /// <summary>
    /// 正式开始 服务端逻辑
    /// </summary>
    public abstract void BeginGame();
    /// <summary>
    /// 正式开始 客户端逻辑(若有与服务端相同逻辑请使用if (!roomData.isServer) { }包裹)
    /// </summary>
    public abstract void BeginGameClient();
    /// <summary>
    /// 一局游戏结束 服务端逻辑
    /// </summary>
    public abstract void FinishGame();
    /// <summary>
    /// 一局游戏结束 客户端逻辑(若有与服务端相同逻辑请使用if (!roomData.isServer) { }包裹)
    /// </summary>
    public abstract void FinishGameClient();
    /// <summary>
    /// 加载地图
    /// </summary>
    public abstract void LoadMap();
    /// <summary>
    /// 加载玩家
    /// </summary>
    public abstract void LoadPlayer();
    /// <summary>
    /// 服务端 房间非暂停时 运行更新
    /// </summary>
    public abstract void Updata();
    /// <summary>
    /// 游戏结束 服务端逻辑
    /// </summary>
    public abstract void EndGame();
    /// <summary>
    /// 游戏结束 客户端逻辑(若有与服务端相同逻辑请使用if (!roomData.isServer) { }包裹)
    /// </summary>
    public abstract void EndGameClient();
}
