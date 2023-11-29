using kcp2k;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class MapEditorPanel : BasePanel
{
    [SerializeField] private TileBase[] tiles;
    [SerializeField] private GameObject mapLayer;
    [SerializeField] private GameObject btnTile;
    [SerializeField] private GameObject btnTileType;
    [SerializeField] private GameObject btnMap;

    [SerializeField] private MapLayer mlPlayerSpawn, mlServitorSpawn, mlFeatherSpawn, mlToolSpawn;

    InputField ifMapName;
    InputField ifMapDescription;
    IntegerTriger itFeather;
    IntegerTriger itTool;
    ScrollRect svMapLayerList;
    ScrollRect svMapTileList;
    ScrollRect svMapTileTypeList;
    ScrollRect svMapList;
    Button btnCreatMap;
    Button btnDestroy;
    Button btnSave;
    Button btnAddMapLayer;
    Button btnRemoveMapLayer;

    string mapDataPath;
    GameObject tilemap;
    List<TileDataList> tileDataLists;
    Dictionary<string, FileInfo> mapDataList = new Dictionary<string, FileInfo>();

    Grid grid;
    FileInfo activeMap;
    MapLayer activeMapLayer;
    TileData activeTile;
    MapData mapData;

    private void InitCompement()
    {
        ifMapName = GetControl<InputField>("ifMapName");
        ifMapDescription = GetControl<InputField>("ifMapDescription");
        itFeather = GetControl<IntegerTriger>("itFeather");
        itTool = GetControl<IntegerTriger>("itTool");
        svMapLayerList = GetControl<ScrollRect>("svMapLayerList");
        svMapTileList = GetControl<ScrollRect>("svMapTileList");
        svMapTileTypeList = GetControl<ScrollRect>("svMapTileTypeList");
        svMapList = GetControl<ScrollRect>("svMapList");
        btnCreatMap = GetControl<Button>("btnCreatMap");
        btnDestroy = GetControl<Button>("btnDestroy");
        btnSave = GetControl<Button>("btnSave");
        btnAddMapLayer = GetControl<Button>("btnAddMapLayer");
        btnRemoveMapLayer = GetControl<Button>("btnRemoveMapLayer");

    }

    private void RegisterCompementEvent()
    {
        ifMapName.onValueChanged.AddListener((o) =>
        {
            if (mapData != null) mapData.name = o;
        });
        ifMapDescription.onValueChanged.AddListener((o) =>
        {
            if (mapData != null) mapData.description = o;
        });
        itFeather.OnValueChanged.AddListener((o) =>
        {
            if (mapData != null) mapData.maxFeather = o;
        });
        itTool.OnValueChanged.AddListener((o) =>
        {
            if (mapData != null) mapData.maxTool = o;
        });
        btnCreatMap.onClick.AddListener(() =>
        {
            CreatMap();
        });
        btnDestroy.onClick.AddListener(() =>
        {
            DestroyMap(activeMap);
        });
        btnSave.onClick.AddListener(() =>
        {
            SaveMap();
        });
        btnAddMapLayer.onClick.AddListener(() =>
        {
            AddMapLayer();
        });
        btnRemoveMapLayer.onClick.AddListener(() =>
        {
            RemoveMapLayer();
        });
        mlPlayerSpawn.OnClik.AddListener((o) =>
        {
            ChangeMapLayer(o);
        });
        mlServitorSpawn.OnClik.AddListener((o) =>
        {
            ChangeMapLayer(o);
        });
        mlFeatherSpawn.OnClik.AddListener((o) =>
        {
            ChangeMapLayer(o);
        });
        mlToolSpawn.OnClik.AddListener((o) =>
        {
            ChangeMapLayer(o);
        });
    }

    public override void Init()
    {
        mapDataPath = Application.persistentDataPath + "/MapData/";
        tilemap = Resources.Load<GameObject>("Map/MapEdit/Tilemap");
        InitCompement();
        RegisterCompementEvent();
        LoadMapTileData();
        LoadAllMapData();
    }

    private void LoadAllMapData()
    {
        if (!Directory.Exists(mapDataPath)) return;
        for (int i = 0; i < svMapList.content.childCount; i++)
        {
            Destroy(svMapList.content.GetChild(i).gameObject);
        }
        mapDataList.Clear();
        foreach (FileInfo fileInfo in Directory.CreateDirectory(mapDataPath).GetFiles())
        {
            if (fileInfo.Extension ==".MapData")
            {
                FileInfo fi = fileInfo;
                Button btnMapCopy = Instantiate(btnMap,svMapList.content).GetComponent<Button>();
                btnMapCopy.onClick.AddListener(() =>
                {
                    LoadMap(fi);
                });
                btnMapCopy.GetComponentInChildren<Text>().text = fileInfo.Name;
                mapDataList.Add(fileInfo.Name.Split('.')[0], fileInfo);
            }
        }
    }

    private void LoadMapTileData()
    {
        tileDataLists = DataMgr.Instance.TileDataSet;
        for (int i = 0; i < svMapTileTypeList.content.childCount; i++)
        {
            Destroy(svMapTileTypeList.content.GetChild(i).gameObject);
        }
        foreach (TileDataList tileDataList in tileDataLists)
        {
            TileDataList tdl = tileDataList;
            Button btnTileTypeCopy = Instantiate(btnTileType, svMapTileTypeList.content).GetComponent<Button>();
            btnTileTypeCopy.onClick.AddListener(() =>
            {
                LoadMapTileList(tdl);
            });
            btnTileTypeCopy.GetComponentInChildren<Text>().text = tileDataList.name;
        }
    }

    private void LoadMapTileList(TileDataList tileDataList)
    {
        for (int i = 0; i < svMapTileList.content.childCount; i++)
        {
            Destroy(svMapTileList.content.GetChild(i).gameObject);
        }
        foreach (TileData tileData in tileDataList.tileDatas)
        {
            TileData td = tileData;
            Button btnTileCopy = Instantiate(btnTile, svMapTileList.content).GetComponent<Button>();
            btnTileCopy.onClick.AddListener(() =>
            {
                ChangeTile(td);
            });
            btnTileCopy.GetComponentInChildren<Text>().text = tileData.name;
        }
    }

    private void LoadMap(FileInfo mapfile)
    {
        activeMap = mapfile;
        mapData = ResMgr.Instance.LoadBinaryWithMirror<MapData>(mapfile.FullName.Split(Application.persistentDataPath)[1]);
        ifMapName.text = mapData.name;
        ifMapDescription.text = mapData.description;
        itFeather.value = mapData.maxFeather;
        itTool.value = mapData.maxTool;
        #region 常用4个点位注册
        Tilemap tilemapCopy = Instantiate(tilemap).GetComponent<Tilemap>();
        tilemapCopy.gameObject.transform.SetParent(grid.transform, false);
        tilemapCopy.gameObject.name = "PlayerSpawnPos";
        tilemapCopy.GetComponent<TilemapRenderer>().sortingLayerID = SortingLayer.NameToID("UI");
        foreach(V2 v2 in mapData.playerSpwnPos) tilemapCopy.SetTile(v2.ToV3Int(), tiles[0]);
        mlPlayerSpawn.Init(tilemapCopy);

        tilemapCopy = Instantiate(tilemap).GetComponent<Tilemap>();
        tilemapCopy.gameObject.transform.SetParent(grid.transform, false);
        tilemapCopy.gameObject.name = "ServitorSpawnPos";
        tilemapCopy.GetComponent<TilemapRenderer>().sortingLayerID = SortingLayer.NameToID("UI");
        foreach (V2 v2 in mapData.servitorSpwnPos) tilemapCopy.SetTile(v2.ToV3Int(), tiles[1]);
        mlServitorSpawn.Init(tilemapCopy);

        tilemapCopy = Instantiate(tilemap).GetComponent<Tilemap>();
        tilemapCopy.gameObject.transform.SetParent(grid.transform, false);
        tilemapCopy.gameObject.name = "FeatherSpawnPos";
        tilemapCopy.GetComponent<TilemapRenderer>().sortingLayerID = SortingLayer.NameToID("UI");
        foreach (V2 v2 in mapData.FeatherSpwnPos) tilemapCopy.SetTile(v2.ToV3Int(), tiles[2]);
        mlFeatherSpawn.Init(tilemapCopy);

        tilemapCopy = Instantiate(tilemap).GetComponent<Tilemap>();
        tilemapCopy.gameObject.transform.SetParent(grid.transform, false);
        tilemapCopy.gameObject.name = "ToolSpawnPos";
        tilemapCopy.GetComponent<TilemapRenderer>().sortingLayerID = SortingLayer.NameToID("UI");
        foreach (V2 v2 in mapData.ToolSpwnPos) tilemapCopy.SetTile(v2.ToV3Int(), tiles[3]);
        mlToolSpawn.Init(tilemapCopy);
        #endregion

        if (grid != null) GameObject.Destroy(grid.gameObject);
        grid = ResMgr.Instance.Load<GameObject>("Map/MapEdit/Grid").GetComponent<Grid>();
        for (int i = 0; i < svMapLayerList.content.childCount; i++)
        {
            Destroy(svMapLayerList.content.GetChild(i).gameObject);
        }
        foreach (MapDetile mapDetile in mapData.mapDetiles)
        {
            LoadMapDetile(mapDetile);
        }
    }

    private void LoadMapDetile(MapDetile mapDetile)
    {
        MapDetile md = mapDetile;
        Tilemap tilemapCopy = Instantiate(tilemap).GetComponent<Tilemap>();
        tilemapCopy.gameObject.transform.SetParent(grid.transform, false);
        tilemapCopy.gameObject.name = md.name;
        tilemapCopy.GetComponent<TilemapRenderer>().sortingLayerID = md.layer.id;
        foreach (var kvp in md.MapTileDetiles)
        {
            tilemapCopy.SetTile(kvp.Key.ToV3Int(), kvp.Value.TileData.tileBase);
            if (kvp.Value.ifHaveValue)
            {
                tilemapCopy.GetInstantiatedObject(kvp.Key.ToV3Int()).GetComponent<BaseMap>().Init(md.tileValue[kvp.Key]);
            }
        }
        MapLayer mapLayerCopy = Instantiate(mapLayer, svMapLayerList.content).GetComponent<MapLayer>();
        mapLayerCopy.Init(tilemapCopy, md);
        mapLayerCopy.OnClik.AddListener((o) =>
        {
            ChangeMapLayer(o);
        });
    }

    private void SaveMap()
    {
        ResMgr.Instance.SaveBinaryWithMirror(mapData, mapDataPath + mapData.name + ".MapData");
    }

    private void CreatMap()
    {
        mapData = new MapData();
        mapData.name = "New Map";
        mapData.description = "新地图";
        ResMgr.Instance.SaveBinaryWithMirror(mapData, mapDataPath + mapData.name + ".MapData");
        LoadAllMapData();
        LoadMap(mapDataList["New Map"]);
    }

    private void DestroyMap(FileInfo mapfile)
    {
        if(mapfile != null)
            mapfile.Delete();
    }

    private void ChangeTile(TileData tileData)
    {
        activeTile = tileData;
    }

    private void ChangeMapLayer(MapLayer mapLayer)
    {
        activeMapLayer = mapLayer;
    }

    private void RemoveMapLayer()
    {
        if (mapData == null) return;
        if(activeMapLayer == null) return;
        if (activeMapLayer.type != 0) return;
        mapData.mapDetiles.Remove(activeMapLayer.mapDetile);
        Destroy(activeMapLayer.tilemap.gameObject);
        Destroy(activeMapLayer.gameObject);
        activeMapLayer = null;
    }

    private void AddMapLayer()
    {
        MapDetile mapDetile = new MapDetile();
        mapDetile.name = "New MapLayer";
        mapDetile.layer = new SortingLayer();
        LoadMapDetile(mapDetile);
    }
}
