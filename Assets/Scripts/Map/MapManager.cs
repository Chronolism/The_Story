using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class MapManager : BaseManager<MapManager>
{
    //等确认了再改成Application.persistentDataPath
    public string mapSaveDirectoryAddress = Application.streamingAssetsPath + "/MapData";
    //二进制存储地图
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
