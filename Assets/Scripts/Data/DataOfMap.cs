using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;
using UnityEngine.Tilemaps;

[Serializable]
public class MapData
{
    public int ID;
    public string name;
    public string description;
    public List<MapDetile> mapDetiles;
}

/// <summary>
/// 地图瓦片信息
/// </summary>
[Serializable]
public class MapDetile
{
    public int id;
    public string name;
    public Dictionary<V2, MapTileDetile> MapTileDetiles;
}
[Serializable]
public class MapTileDetile
{
    public int id;
    public float blood = -1;
    private TileData m_tileData;
    public TileData TileData
    {
        get 
        {
            if (m_tileData == null) m_tileData = DataMgr.Instance.GetTileData(id);
            return m_tileData; 
        }
    }
}
[Serializable]
public class TileData
{
    public int id;
    public string name;
    public MapColliderType type;
    public TileBase tileBase;
}
[Serializable]
public enum MapColliderType
{
    None = 1,
    Wall = 2,
    Water = 4,
    Fire = 8
}

/// <summary>
/// 二元组
/// </summary>
[Serializable]
public struct V2
{
    public int x;
    public int y;

    public V2(int from_x,int from_y)
    {
        x = from_x;
        y = from_y;
    }
    public override bool Equals(object obj)
    {
        return obj is V2 v2 && x == v2.x && y == v2.y;
    }
    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
    public override string ToString()
    {
        return base.ToString();
    }

    public static bool operator ==(V2 a, V2 b)
    {
        if (a.x == b.x && a.y == b.y) return true;
        return false;
    }
    public static bool operator !=(V2 a, V2 b)
    {
        if (a.x == b.x && a.y == b.y) return false;
        return true;
    }
    public static V2 V3to2(Vector3 vector3) => new V2((int) vector3.x, (int) vector3.y);
    public static V2 V3to2(Vector3Int vector3) => new V2(vector3.x, vector3.y);
    public static Vector3Int V2to3(V2 v2) => new Vector3Int(v2.x, v2.y, 0);
    //隐式赋值重载
    public static explicit operator V2(Vector3 vector3)
    {
        return new V2((int)vector3.x, (int)vector3.y);
    }
    public static explicit operator Vector3(V2 v2)
    {
        return new Vector3(v2.x, v2.y, 0);
    }

    public static explicit operator V2(Vector3Int vector3)
    {
        return new V2(vector3.x, vector3.y);
    }
    public static explicit operator Vector3Int(V2 v2)
    {
        return new Vector3Int(v2.x, v2.y, 0);
    }
}
