using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapMgr : BaseManager<MapMgr>
{
    public MapData mapData;

    Grid grid;

    Dictionary<V2,MapColliderType> colliders = new Dictionary<V2, MapColliderType>();
    public Dictionary<MapType,MapTileDetile> tileDetile = new Dictionary<MapType,MapTileDetile>();

    public void LoadMap(string mapName)
    {
        mapData = ResMgr.Instance.LoadBinaryWithMirror<MapData>(Application.persistentDataPath + "\\MapData\\" + mapName + ".MapData");
        if (mapData == null) Debug.Log("地图不存在");

        if(grid != null)GameObject.Destroy(grid.gameObject);
        grid = ResMgr.Instance.Load<GameObject>("Map/MapEdit/Grid").GetComponent<Grid>();

        foreach(MapDetile mapDetile in mapData.mapDetiles)
        {
            Tilemap tilemap = ResMgr.Instance.Load<GameObject>("Map/MapEdit/Tilemap").GetComponent<Tilemap>();
            tilemap.gameObject.transform.SetParent(grid.transform,false);
            tilemap.gameObject.name = mapDetile.name;
            tilemap.GetComponent<TilemapRenderer>().sortingLayerID = SortingLayer.NameToID(mapDetile.layer);
            foreach(var kvp in mapDetile.MapTileDetiles)
            {
                tilemap.SetTile(kvp.Key.ToV3Int(), kvp.Value.TileData.tileBase);
                if (colliders.ContainsKey(kvp.Key))
                {
                    colliders[kvp.Key] |= kvp.Value.TileData.type;
                }
                else
                {
                    colliders[kvp.Key] = kvp.Value.TileData.type;
                }
                if (kvp.Value.ifHaveValue)
                {
                    try
                    {
                        tilemap.GetInstantiatedObject(kvp.Key.ToV3Int()).GetComponent<BaseMap>().Init(mapDetile.tileValue[kvp.Key]);
                    }
                    catch
                    {
                        Debug.Log("数据不符");
                    }
                }
            }
        }
        AStarMgr.Instance.InitMapInfo(colliders);
    }
}
