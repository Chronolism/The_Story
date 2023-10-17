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
    //��������
    TileBase _testTile;
    #region ��ͼ���ù���
    //��ͼӳ����
    /// <summary>
    /// ����������ײ�ĵ�ͼ�����ƣ�����������ײ��ֵĬ��Ϊ1
    /// </summary>
    public string mapColliderLayerName = "Bottom";
    /// <summary>
    /// ����Ѱ·�����ݼ���ֻ��bool
    /// </summary>
    public Dictionary<Vector3Int, bool> mapColliderData = new Dictionary<Vector3Int, bool>();
    /// <summary>
    /// �������ɻ������ܵĵ�ͼ������
    /// </summary>
    public string baseFunctionLayerName = "Middle";
    /// <summary>
    /// Ŀǰ�ݶ���0Ϊ��д������ߵ�ˢ�µ㣬2Ϊ��ҳ����㣬3Ϊʹħˢ�µ�
    /// </summary>
    public Dictionary<Vector3Int, int> baseFunctionData = new Dictionary<Vector3Int, int>();
    #endregion
    //����������еĵ�ͼԴ����
    public D_MapDataDetailOriginal nowPlaying_d_MapDataDetailOriginal;
    Dictionary<string, Dictionary<Vector3Int, int>> _runtimeCellData;//������������Bottom������ײ��
    //��ȷ�����ٸĳ�Application.persistentDataPath
    public string mapSaveDirectoryAddress = Application.streamingAssetsPath + "/MapData";

    //���ص�ͼ������
    public bool LoadMapCompletelyToScene(string mapName)
    {
        runtimeTilemaps.Clear();
        //����ǲ�����������ֻ����Bottom�������ݵؿ���Ϊ��ײ�䵽����
        if (mapName == "401")
        {
            runtimeGirdGameObject = Object.Instantiate<GameObject>(Resources.Load<GameObject>("Map/MapEdit/Grid"));
            runtimeGrid = runtimeGirdGameObject.GetComponent<Grid>();
            runtimeTilemaps.TryAdd("Bottom", runtimeGrid.GetComponentInChildren<Tilemap>());
            _testTile = Resources.Load<TileBase>("Map/MapEdit/TestIcon");
            nowPlaying_d_MapDataDetailOriginal = LoadMap("400", out int errorCodeTest);
            runtimeTilemaps["Bottom"].gameObject.AddComponent<TilemapCollider2D>();//�����ײ��
            Dictionary<Vector3Int, int> tempMapCollider = nowPlaying_d_MapDataDetailOriginal.mapCellData["Bottom"];
            V2 tempLogicToDisplayOffset;
            AbstractLogicManager.Instance.logicMapSize = AutoFilledEmpty(ref tempMapCollider,out tempLogicToDisplayOffset);//�Զ����հײ�����С��������ͼ
            AbstractLogicManager.Instance.logicToDisplayOffset = tempLogicToDisplayOffset;//�����ͼԭ��һ��Ϊ 0,0 ����¼��ʵ��ԭ���� 0,0 ��ƫ����
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
                Debug.LogWarning("��ͼ��С���겻��ԭ��,���� " + tempLogicToDisplayOffset.x + "," + tempLogicToDisplayOffset.y);
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
        runtimeGrid = runtimeGirdGameObject.GetComponent<Grid>();                                              //��ʼ��Grid

        nowPlaying_d_MapDataDetailOriginal = LoadMap(mapName, out int errorCode);                              //���ص�ͼȫ������
        _runtimeCellData = nowPlaying_d_MapDataDetailOriginal.mapCellData;                                     //ʹ�õ�ͼ��������
        
        
        //�������ż��صؿ�����
        _testTile = Resources.Load<TileBase>("Map/MapEdit/TestIcon");

        //�������żӹ��ܻ���ײ
        // runtimeTilemaps["Bottom"].gameObject.AddComponent<TilemapCollider2D>();//�����ײ��


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
                if (o.name == mapColliderLayerName)//���ڲ�������ײ��
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
    /// ���һ���б���ȡbaseFunctionData����ָ��ֵ�����еؿ�
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
    //��ͼ��񻯣����ַ�����
    /// <summary>
    /// ���ݴ�������ݵ����Χ�����϶Ϊ0������һ����ͼ�ߴ�V2
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
    /// �����ƴ洢��ͼ
    /// </summary>
    /// <param name="d_MapDataDetailOriginal">���洢����</param>
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
        return true;//�����ж�����ʲô��������չ
    }
    /// <summary>
    /// �����Ƽ��ص�ͼ
    /// </summary>
    /// <param name="mapName">��ͼ����</param>
    /// <param name="errorCode">���ش�����</param>
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
    /// ��ͼ���ݰ���unity��������
    /// </summary>
    /// <param name="d_MapDataDetailOriginal">δ��������</param>
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
    /// ��ͼ���ݻ�ԭunity��������
    /// </summary>
    /// <param name="d_MapDataDetailOriginal_Serializable">�Ѱ�������</param>
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
