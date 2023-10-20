using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMgr : BaseManager<GameMgr>
{

    public void StartGame()
    {
        LoadMap(DataMgr.Instance.roomData.mapName);
        DataMgr.Instance.roomData.LoadMap(DataMgr.Instance.roomData.mapName);
        DataMgr.Instance.roomData.AllLoadCharacter();
        DataMgr.Instance.roomData.BeginGane();
    }

    public void LoadMap(string name)
    {
        MapManager.Instance.LoadMapCompletelyToScene(DataMgr.Instance.roomData.mapName);

        AStarMgr.Instance.InitMapInfo(MapManager.Instance.mapColliderData);
    }

    
}
