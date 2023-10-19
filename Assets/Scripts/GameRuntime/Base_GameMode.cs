using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base_GameMode
{
    //地图的基本功能
    public List<Vector3Int> cellsForPlayerBorn ;
    public List<Vector3Int> cellsForFeatherPenBorn ;
    public List<Vector3Int> cellsForToolsBorn ;
    public List<Vector3Int> cellsForServitorBorn ;
    public virtual void InitPlayer(D_Base_Player Player)
    {
        
    }
    public virtual Vector3Int GetPlayerStartPos()
    {
        return Vector3Int.zero;
    }
    public virtual void SetSelfAsNowaGameMode()
    {
        GameRuntimeManager.Instance.nowaGameMode = this;
    }
}
