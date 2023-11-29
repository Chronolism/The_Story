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

    public void LoadMap()
    {
        LoadMapData();
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
                    tilemap.GetInstantiatedObject(kvp.Key.ToV3Int()).GetComponent<BaseMap>().Init(mapDetile.tileValue[kvp.Key]);
                }
            }
        }
        AStarMgr.Instance.InitMapInfo(colliders);
    }


    void LoadMapData()
    {
        mapData = new MapData();
        MapDetile mapDetile = new MapDetile();
        mapData.mapDetiles.Add(mapDetile);
        mapDetile.name = "wall";
        mapDetile.layer = "Instance";
        for (int i = 0; i < 10; i++)
        {
            mapDetile.MapTileDetiles.Add(new V2(i,0),new MapTileDetile(0,false));
            mapDetile.MapTileDetiles.Add(new V2(i, 8), new MapTileDetile(0, false));
            mapDetile.MapTileDetiles.Add(new V2(-1, i), new MapTileDetile(0, false));
            mapDetile.MapTileDetiles.Add(new V2(10, i), new MapTileDetile(0, false));
        }
        mapDetile = new MapDetile();
        mapDetile.name = "water";
        mapDetile.layer = "Ground_1";
        for (int i = 3; i < 6; i++)
        {
            mapDetile.MapTileDetiles.Add(new V2(i, 5), new MapTileDetile(6, false));
        }
        mapData.mapDetiles.Add(mapDetile);
        mapData.playerSpwnPos.Add(new V2(1, 1));
        mapData.playerSpwnPos.Add(new V2(1, 2));
        mapData.playerSpwnPos.Add(new V2(1, 3));
        mapData.servitorSpwnPos.Add(new V2(4, 4));
        mapData.servitorSpwnPos.Add(new V2(6, 7));
        mapData.maxFeather = 1;
        mapData.maxTool = 1;
        mapData.FeatherSpwnPos.Add(new V2(5, 6));
        mapData.FeatherSpwnPos.Add(new V2(3, 6));
        mapData.ToolSpwnPos.Add(new V2(2, 6));
        mapData.ToolSpwnPos.Add(new V2(1, 6));
    }
}
