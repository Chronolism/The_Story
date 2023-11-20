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
        // Ŀǰ�ݶ���10Ϊ��д��ˢ�µ㣬11Ϊ����ˢ�µ㣬12Ϊ���ˢ�µ㣬13Ϊʹħˢ�µ�
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
