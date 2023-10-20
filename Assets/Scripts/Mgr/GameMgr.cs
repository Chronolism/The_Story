using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMgr : BaseManager<GameMgr>
{

    public void StartGame()
    {
        //MapManager.Instance.LoadMapCompletelyToScene(mapName);

        AStarMgr.Instance.InitMapInfo(MapManager.Instance.mapColliderData);
    }
}
