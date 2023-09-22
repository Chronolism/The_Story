using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class MapManager : BaseManager<MapManager>
{
    //玩家正在运行的地图源数据
    public D_MapDataDetailOriginal nowPlaying_d_MapDataDetailOriginal;
    Dictionary<string, Dictionary<Vector3Int, int>> _collusionTestMapCellData;//测试用例，从Bottom生成碰撞箱
    //等确认了再改成Application.persistentDataPath
    public string mapSaveDirectoryAddress = Application.streamingAssetsPath + "/MapData";

    //加载地图到场景

    //地图规格化（多种方法）
    /// <summary>
    /// 根据传入的数据的最大范围，填补空隙为0，返回一个地图尺寸V2
    /// </summary>
    /// <param name="original"></param>
    /// <returns></returns>
    public V2 AutoFilledEmpty(ref Dictionary<Vector3Int, int> original)
    {
        if(original == null)
        {
            return new V2(0,0);
        }
        int MaxX = 0;
        int MinX = 0;
        int MaxY = 0;
        int MinY = 0;
        bool ifFirstCell = true;
        foreach (var item in original)
        {
            if (ifFirstCell)
            {
                MaxX = item.Key.x;
                MinX = item.Key.x;
                MaxY = item.Key.y;
                MinY = item.Key.y;
                ifFirstCell = false;
            }
            else
            {
                if (item.Key.x >= MaxX) MaxX = item.Key.x;
                if (item.Key.x <= MinX) MinX = item.Key.x;
                if (item.Key.y >= MaxY) MaxY = item.Key.y;
                if (item.Key.y <= MinY) MinY = item.Key.y;
            }
        }
        for (int i = MinX; i <= MaxX; i++)
        {
            for (int j = MinY; j <= MaxY; j++)
            {
                if(!original.ContainsKey(new Vector3Int(i, j, 0)))
                {
                    original.Add(new Vector3Int(i, j, 0),0);
                }
            }
        }
        return new V2(MaxX - MinX, MaxY - MinY);
    }

    /// <summary>
    /// 二进制存储地图
    /// </summary>
    /// <param name="d_MapDataDetailOriginal">待存储数据</param>
    /// <returns></returns>
    public bool SaveMap(D_MapDataDetailOriginal d_MapDataDetailOriginal)
    {
        D_MapDataDetailOriginal_Serializable tempData = MapDataSerialize(d_MapDataDetailOriginal);
        if (!Directory.Exists(mapSaveDirectoryAddress)) Directory.CreateDirectory(mapSaveDirectoryAddress);
        using (FileStream fileStream = new FileStream(mapSaveDirectoryAddress + "/" + tempData.name + ".thestorymap", FileMode.Create, FileAccess.Write, FileShare.ReadWrite)) 
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            binaryFormatter.Serialize(fileStream, tempData);
            fileStream.Flush();
            fileStream.Close();
        }
        return true;//用于判断重名什么，便于拓展
    }
    /// <summary>
    /// 二进制加载地图
    /// </summary>
    /// <param name="mapName">地图名称</param>
    /// <param name="errorCode">返回错误码</param>
    /// <returns></returns>
    public D_MapDataDetailOriginal LoadMap(string mapName,out int errorCode)
    {
        D_MapDataDetailOriginal target;
        D_MapDataDetailOriginal_Serializable target_Serializable;
        if (!Directory.Exists(mapSaveDirectoryAddress)) Directory.CreateDirectory(mapSaveDirectoryAddress);
        if (!File.Exists(mapSaveDirectoryAddress + "/" + mapName + ".thestorymap"))
        {
            errorCode = 404;
            return null;
        }
        using (FileStream fileStream = File.Open(mapSaveDirectoryAddress + "/" + mapName + ".thestorymap",FileMode.Open,FileAccess.Read,FileShare.ReadWrite))
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            target_Serializable = binaryFormatter.Deserialize(fileStream) as D_MapDataDetailOriginal_Serializable;
            fileStream.Close();
        }
        target = MapDataDeserialize(target_Serializable);
        errorCode = 0 ;
        //D_MapDataDetailOriginal tempData = MapDataSerialize();
        return target;
    }
    /// <summary>
    /// 地图数据剥离unity自有内容
    /// </summary>
    /// <param name="d_MapDataDetailOriginal">未剥离数据</param>
    /// <returns></returns>
    public D_MapDataDetailOriginal_Serializable MapDataSerialize(D_MapDataDetailOriginal d_MapDataDetailOriginal)
    {
        D_MapDataDetailOriginal_Serializable target = new D_MapDataDetailOriginal_Serializable();
        target.ID = d_MapDataDetailOriginal.ID;
        target.name = d_MapDataDetailOriginal.name;
        target.description = d_MapDataDetailOriginal.description; ;
        if(d_MapDataDetailOriginal.mapCellData != null)
        {
            target.mapCellDataSerializable = new Dictionary<string, Dictionary<V2, int>>();
            foreach (var item in d_MapDataDetailOriginal.mapCellData)
            {
                if (d_MapDataDetailOriginal.mapCellData[item.Key] != null)
                {
                    target.mapCellDataSerializable.Add(item.Key, new Dictionary<V2, int>());
                    foreach (var element in d_MapDataDetailOriginal.mapCellData[item.Key])
                    {
                        target.mapCellDataSerializable[item.Key].Add(V2.V3to2(element.Key), element.Value);
                    }
                }               
            }
        }    
        return target;
    }
    /// <summary>
    /// 地图数据还原unity自有内容
    /// </summary>
    /// <param name="d_MapDataDetailOriginal_Serializable">已剥离数据</param>
    /// <returns></returns>
    public D_MapDataDetailOriginal MapDataDeserialize(D_MapDataDetailOriginal_Serializable d_MapDataDetailOriginal_Serializable)
    {
        D_MapDataDetailOriginal target = new D_MapDataDetailOriginal();
        target.ID = d_MapDataDetailOriginal_Serializable.ID;
        target.name = d_MapDataDetailOriginal_Serializable.name;
        target.description = d_MapDataDetailOriginal_Serializable.description; ;
        if (d_MapDataDetailOriginal_Serializable.mapCellDataSerializable != null)
        {
            target.mapCellData = new Dictionary<string, Dictionary<Vector3Int, int>>();
            foreach (var item in d_MapDataDetailOriginal_Serializable.mapCellDataSerializable)
            {
                if (d_MapDataDetailOriginal_Serializable.mapCellDataSerializable[item.Key] != null)
                {
                    target.mapCellData.Add(item.Key, new Dictionary<Vector3Int, int>());
                    foreach (var element in d_MapDataDetailOriginal_Serializable.mapCellDataSerializable[item.Key])
                    {
                        target.mapCellData[item.Key].Add(V2.V2to3(element.Key), element.Value);
                    }
                }
            }
        }
        return target;
    }
}
