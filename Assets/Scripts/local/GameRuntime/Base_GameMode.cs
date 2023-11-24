using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base_GameMode
{
    //��ͼ�Ļ�������
    public List<Vector3Int> cellsForPlayerBorn ;
    public List<Vector3Int> cellsForFeatherPenBorn ;
    public List<Vector3Int> cellsForToolsBorn ;
    public List<Vector3Int> cellsForServitorBorn ;
    //����������ʱ����
    public int runtimeFeatherPenCount;
    //����GameRuntimeData
    protected D_GameRuntime gameRuntimeData => GameRuntimeManager.Instance.GameRuntimeData;
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
    public virtual void GameRuntimeStart()
    {

    }
    public virtual void GameRuntimeUpdate()
    {
        GameManager.ThrowError(503);
    }
}