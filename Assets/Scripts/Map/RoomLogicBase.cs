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

    public void ToStartGame()
    {
        roomData.LoadMap(roomData.mapName);
        cellsForPlayerBorn = MapManager.Instance.GetMapBaseFunction(2);
        cellsForFeatherPenBorn = MapManager.Instance.GetMapBaseFunction(0);
        cellsForToolsBorn = MapManager.Instance.GetMapBaseFunction(0);
        cellsForServitorBorn = MapManager.Instance.GetMapBaseFunction(3);
        LoadMap();
        LoadPlayer();
    }


    public abstract void StartGame();
    public abstract void LoadMap();
    public abstract void LoadPlayer();
    public abstract void Updata();

    public abstract void EndGame();
}
