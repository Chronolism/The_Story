using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RoomLogicBase
{
    public RoomData roomData;

    public List<Vector3> cellsForPlayerBorn = new List<Vector3>();
    public List<Vector3> cellsForFeatherPenBorn = new List<Vector3>();
    public List<Vector3> cellsForToolsBorn = new List<Vector3>();
    public List<Vector3> cellsForServitorBorn = new List<Vector3>();

    public RoomLogicBase(RoomData roomData)
    {
        this.roomData = roomData;
    }

    public void LoadMapData(string mapName)
    {
        roomData.LoadMap(mapName);
        cellsForPlayerBorn.Clear();
        cellsForFeatherPenBorn.Clear();
        cellsForToolsBorn.Clear();
        cellsForServitorBorn.Clear();
        // 目前暂定：10为改写笔刷新点，11为道具刷新点，12为玩家刷新点，13为使魔刷新点
        foreach (var v3 in MapManager.Instance.GetMapBaseFunction(12))
        {
            cellsForPlayerBorn.Add(v3 + new Vector3(0.5f, 0.5f, 0));
        }
        foreach (var v3 in MapManager.Instance.GetMapBaseFunction(10))
        {
            cellsForFeatherPenBorn.Add(v3 + new Vector3(0.5f, 0.5f, 0));
        }
        foreach (var v3 in MapManager.Instance.GetMapBaseFunction(11))
        {
            cellsForToolsBorn.Add(v3 + new Vector3(0.5f, 0.5f, 0));
        }
        foreach (var v3 in MapManager.Instance.GetMapBaseFunction(13))
        {
            cellsForServitorBorn.Add(v3 + new Vector3(0.5f, 0.5f, 0));
        }
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
