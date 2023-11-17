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
        // Ŀǰ�ݶ���10Ϊ��д��ˢ�µ㣬11Ϊ����ˢ�µ㣬12Ϊ���ˢ�µ㣬13Ϊʹħˢ�µ�
        cellsForPlayerBorn = MapManager.Instance.GetMapBaseFunction(12);
        cellsForFeatherPenBorn = MapManager.Instance.GetMapBaseFunction(10);
        cellsForToolsBorn = MapManager.Instance.GetMapBaseFunction(11);
        cellsForServitorBorn = MapManager.Instance.GetMapBaseFunction(13);
    }

    public abstract void OpenGame();

    /// <summary>
    /// ��ʼ�Ծ� ������߼�
    /// </summary>
    public abstract void StartGame();
    /// <summary>
    /// ��ʼ�Ծ� �ͻ����߼�(������������ͬ�߼���ʹ��if (!roomData.isServer) { }����)
    /// </summary>
    public abstract void StartGameClient();
    /// <summary>
    /// ��ʽ��ʼ ������߼�
    /// </summary>
    public abstract void BeginGame();
    /// <summary>
    /// ��ʽ��ʼ �ͻ����߼�(������������ͬ�߼���ʹ��if (!roomData.isServer) { }����)
    /// </summary>
    public abstract void BeginGameClient();
    /// <summary>
    /// һ����Ϸ���� ������߼�
    /// </summary>
    public abstract void FinishGame();
    /// <summary>
    /// һ����Ϸ���� �ͻ����߼�(������������ͬ�߼���ʹ��if (!roomData.isServer) { }����)
    /// </summary>
    public abstract void FinishGameClient();
    /// <summary>
    /// ���ص�ͼ
    /// </summary>
    public abstract void LoadMap();
    /// <summary>
    /// �������
    /// </summary>
    public abstract void LoadPlayer();
    /// <summary>
    /// ����� �������ͣʱ ���и���
    /// </summary>
    public abstract void Updata();
    /// <summary>
    /// ��Ϸ���� ������߼�
    /// </summary>
    public abstract void EndGame();
    /// <summary>
    /// ��Ϸ���� �ͻ����߼�(������������ͬ�߼���ʹ��if (!roomData.isServer) { }����)
    /// </summary>
    public abstract void EndGameClient();
}
