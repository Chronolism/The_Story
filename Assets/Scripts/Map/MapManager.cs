using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class MapManager : BaseManager<MapManager>
{
    
    //�����ƴ洢��ͼ
    public bool SaveMap(D_MapDataDetailOriginal d_MapDataDetailOriginal)
    {
        return false;
    }
    public D_MapDataDetailOriginal LoadMap(string mapName)
    {
        return null;
    }
}
