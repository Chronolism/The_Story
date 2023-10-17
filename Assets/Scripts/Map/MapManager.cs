using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.Tilemaps;

public class MapManager : BaseManager<MapManager>
{
    public Grid runtimeGrid;
    GameObject runtimeGirdGameObject;
    Dictionary<string,Tilemap> runtimeTilemaps = new Dictionary<string, Tilemap>();
    //测试用例
    TileBase _testTile;
    #region 地图配置规则
    //地图映射规格
    /// <summary>
    /// 用于生成碰撞的地图层名称，用于生成碰撞的值默认为1
    /// </summary>
    public string mapColliderLayerName = "Bottom";
    /// <summary>
    /// 用于寻路的数据集，只有bool
    /// </summary>
    public Dictionary<Vector3Int, bool> mapColliderData = new Dictionary<Vector3Int, bool>();
    /// <summary>
    /// 用于生成基础功能的地图层名称
    /// </summary>
    public string baseFunctionLayerName = "Middle";
    /// <summary>
    /// 目前暂定：0为改写笔与道具的刷新点，2为玩家出生点，3为使魔刷新点
    /// </summary>
    public Dictionary<Vector3Int, int> baseFunctionData = new Dictionary<Vector3Int, int>();
    #endregion
    //玩家正在运行的地图源数据
    public D_MapDataDetailOriginal nowPlaying_d_MapDataDetailOriginal;
    Dictionary<string, Dictionary<Vector3Int, int>> _runtimeCellData;//测试用例，从Bottom生成碰撞箱
    //等确认了再改成Application.persistentDataPath
    public string mapSaveDirectoryAddress = Application.streamingAssetsPath + "/MapData";

    //加载地图到场景
    public bool LoadMapCompletelyToScene(string mapName)
    {
        runtimeTilemaps.Clear();
        //如果是测试用例，则只加载Bottom中有数据地块作为碰撞箱到场景
        if (mapName == "401")
        {
            runtimeGirdGameObject = Object.Instantiate<GameObject>(Resources.Load<GameObject>("Map/MapEdit/Grid"));
            runtimeGrid = runtimeGirdGameObject.GetComponent<Grid>();
            runtimeTilemaps.TryAdd("Bottom", runtimeGrid.GetComponentInChildren<Tilemap>());
            _testTile = Resources.Load<TileBase>("Map/MapEdit/TestIcon");
            nowPlaying_d_MapDataDetailOriginal = LoadMap("400", out int errorCodeTest);
            runtimeTilemaps["Bottom"].gameObject.AddComponent<TilemapCollider2D>();//添加碰撞箱
            Dictionary<Vector3Int, int> tempMapCollider = nowPlaying_d_MapDataDetailOriginal.mapCellData["Bottom"];
            V2 tempLogicToDisplayOffset;
            AbstractLogicManager.Instance.logicMapSize = AutoFilledEmpty(ref tempMapCollider,out tempLogicToDisplayOffset);//自动填充空白并将大小给予抽象地图
            AbstractLogicManager.Instance.logicToDisplayOffset = tempLogicToDisplayOffset;//抽象地图原点一定为 0,0 ，记录该实际原点与 0,0 的偏移量
            if (tempLogicToDisplayOffset == new V2(0, 0))
            {
                foreach (var item in nowPlaying_d_MapDataDetailOriginal.mapCellData["Bottom"])
                {
                    if (item.Value == 1)
                    {
                        runtimeTilemaps["Bottom"].SetTile(item.Key, _testTile);
                        AbstractLogicManager.Instance.runtimeLogicMap[item.Key.x, item.Key.y].type = E_Node_Type.Stop;
                    }
                }
            }
            else
            {
                Debug.LogWarning("地图最小坐标不在原点,而在 " + tempLogicToDisplayOffset.x + "," + tempLogicToDisplayOffset.y);
                foreach (var item in nowPlaying_d_MapDataDetailOriginal.mapCellData["Bottom"])
                {
                    if (item.Value == 1)
                    {
                        runtimeTilemaps["Bottom"].SetTile(item.Key, _testTile);
                        GameRuntimeManager.Instance.runtimeLogicMap[item.Key.x - tempLogicToDisplayOffset.x, item.Key.y - tempLogicToDisplayOffset.y].type = E_Node_Type.Stop;
                    }
                }
            }
            return true;
        }

        if(runtimeGirdGameObject == null) runtimeGirdGameObject = Object.Instantiate<GameObject>(Resources.Load<GameObject>("Map/MapEdit/Grid")); ;                
        runtimeGrid = runtimeGirdGameObject.GetComponent<Grid>();                                              //初始化Grid

        nowPlaying_d_MapDataDetailOriginal = LoadMap(mapName, out int errorCode);                              //加载地图全部数据
        _runtimeCellData = nowPlaying_d_MapDataDetailOriginal.mapCellData;                                     //使用地图网格数据
        
        
        //这里留着加载地块数据
        _testTile = Resources.Load<TileBase>("Map/MapEdit/TestIcon");

        //这里留着加功能或碰撞
        // runtimeTilemaps["Bottom"].gameObject.AddComponent<TilemapCollider2D>();//添加碰撞箱


        foreach (var item in _runtimeCellData)
        {
            var o = Object.Instantiate<GameObject>(Resources.Load<GameObject>("Map/MapEdit/Tilemap"));
            if (o != null)
            {
                o.transform.parent = runtimeGrid.transform;
                runtimeTilemaps.TryAdd(item.Key, o.GetComponent<Tilemap>());
                foreach (var element in item.Value)
                {
                    if (element.Value == 1)
                    {
                        runtimeTilemaps[item.Key].SetTile(element.Key, _testTile);
                    }
                }
                o.name = item.Key;
                if (o.name == mapColliderLayerName)//在内部处理碰撞器
                {
                    mapColliderData.Clear();
                    Dictionary<Vector3Int, int> orignalMapCollider = item.Value;
                    AutoFilledEmpty(ref orignalMapCollider, out _);
                    foreach (var element in orignalMapCollider)
                    {
                        if (element.Value == 0) mapColliderData.TryAdd(element.Key, false);
                        else mapColliderData.TryAdd(element.Key, true);
                    }
                }
                if (o.name == baseFunctionLayerName) baseFunctionData = item.Value;
            }
        }
        return true;
    }
    public bool LoadMapLayerAs()
    {
        return true;
    }
    /// <summary>
    /// 获得一个列表，获取baseFunctionData具有指定值的所有地块
    /// </summary>
    /// <param name="code"></param>
    /// <returns></returns>
    public List<Vector3Int> GetMapBaseFunction(int code)
    {
        List<Vector3Int> temp = new List<Vector3Int>();
        if (baseFunctionData == null) return null;
        foreach (var item in baseFunctionData)
        {
            if (item.Value == code) temp.Add(item.Key);
        }
        return temp;
    }
    //地图规格化（多种方法）
    /// <summary>
    /// 根据传入的数据的最大范围，填补空隙为0，返回一个地图尺寸V2
    /// </summary>
    /// <param name="original"></param>
    /// <returns></returns>
    public V2 AutoFilledEmpty(ref Dictionary<Vector3Int, int> original,out V2 startCellOffset)
    {
        if(original == null)
        {
            startCellOffset = new V2(0, 0);
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
        startCellOffset = new V2(MinX, MinY);
        return new V2(MaxX - MinX + 1, MaxY - MinY + 1);
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
