using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;


[Serializable]
public class MapTileData
{
    public int ID;
    public string name;
    public string description;
    public string TileAsset;
    public float blood;
}

/// <summary>
/// 地图瓦片信息
/// </summary>
[Serializable]
public class MapDetile
{
    public V2 Size;
    public int type;
    public int id;
    public string name;
    public int beansMax;
    public float birthTime;
    public MapTileDetile[,] MapTileDetiles;
    public List<EnemyDetile> enemyDetiles = new List<EnemyDetile>();
    public List<V2> start = new List<V2>();
    public List<V2> end = new List<V2>();
}
[Serializable]
public class MapDetileToJson
{
    public V2 Size;
    public int type;
    public int id;
    public string name;
    public int beansMax;
    public float birthTime;
    public List<MapTileDetile> MapTileDetiles;
    public List<EnemyDetile> enemyDetiles = new List<EnemyDetile>();
    public List<V2> start = new List<V2>();
    public List<V2> end = new List<V2>();
}
/// <summary>
/// 地图单瓦片包含信息
/// </summary>
[Serializable]
public class MapTileDetile
{
    public int Tile_x;
    public int Tile_y;
    public int x;
    public int y;
    public float world_x;
    public float world_y;
    public float blood = -1;
    public Dictionary<string, int> tile = new Dictionary<string, int>();
    //public string TileName;
}
[Serializable]
public class PlayerData
{
    public string account;
    public string password;
    public string language = "Chinese";

    public float volume = 0.3f;
    public float SoundValue = 0.3f;
}

[Serializable]
public class EnemyData
{
    public int id;
    public string name;
    public GameObject obj;
}
[Serializable]
public class EnemyDetile
{
    public int id;
    public int actionType;
    public List<int> Limit_one = new List<int>();
    public List<V2> targets = new List<V2>();
}
/// <summary>
/// 重载二元组运算（那你为什么不用元组或者Vector3Int呢？）
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

}
[Serializable]
public class UIData
{
    public Dictionary<string, string> value;
}
[Serializable]
public class SettingData
{
    public string Language;
}