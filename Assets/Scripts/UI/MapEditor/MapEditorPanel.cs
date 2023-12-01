using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class MapEditorPanel : BasePanel
{
    #region 组件
    [SerializeField] private TileData[] tiles;
    [SerializeField] private GameObject mapLayer;
    [SerializeField] private GameObject btnTile;
    [SerializeField] private GameObject btnTileType;
    [SerializeField] private GameObject btnMap;
    [SerializeField] private MapLayer mlPlayerSpawn, mlServitorSpawn, mlFeatherSpawn, mlToolSpawn;

    InputField ifMapName;
    InputField ifMapDescription;
    InputField ifMapLayerName;
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
    Button btnQuit;
    Button btnCancelTile;
    Button btnBackOrigin;
    Dropdown ddSortingLayer;
    Text txtMousePos;
    #endregion
    #region 数据集
    string mapDataPath;
    GameObject tilemap;
    List<TileDataList> tileDataLists;
    Dictionary<string, FileInfo> mapDataList = new Dictionary<string, FileInfo>();
    SortingLayer[] layers;
    List<string> layerNames = new List<string>();
    #endregion

    Grid grid;
    Tilemap uiTileMap;
    FileInfo activeMap;
    MapLayer activeMapLayer;
    TileData activeTile;
    Vector3 mousePos;
    MapData mapData;

    private UnityEvent m_onQuit = new UnityEvent();

    public UnityEvent OnQuit => m_onQuit;

    public override void Init()
    {
        layers = SortingLayer.layers;
        foreach (SortingLayer sortingLayer in layers)
        {
            layerNames.Add(sortingLayer.name);
        }
        mapDataPath = Application.persistentDataPath + "\\MapData\\";
        tilemap = Resources.Load<GameObject>("Map/MapEdit/Tilemap");
        uiTileMap = Instantiate(tilemap).GetComponent<Tilemap>();

        InitCompement();
        RegisterCompementEvent();
        LoadMapTileData();
        LoadAllMapData();
    }

    #region 核心逻辑
    protected override void Update()
    {
        base.Update();
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (activeMap == null) return;
        if (activeMapLayer == null) return;
        UpdataUITileMap();
        InputControl();
        UpdataUI();

    }

    bool ifChangTile = false;
    Vector3Int oldPos;
    Vector3Int newPos;
    private void UpdataUITileMap()
    {
        if (uiTileMap == null) return;
        newPos = uiTileMap.WorldToCell(mousePos);
        if(ifChangTile || newPos != oldPos)
        {
            ifChangTile = false;
            uiTileMap.SetTile(oldPos, null);
            oldPos = newPos;
            if(activeTile != null)
                uiTileMap.SetTile(newPos, activeTile.tileBase);
        }
    }

    private void UpdataUI()
    {
        txtMousePos.text = $"({newPos.x},{newPos.y})";
    }

    float scroll;
    float cameraSpeed = 1;
    Vector3 oldMousePos;
    BaseMap baseMap;
    private void InputControl()
    {
        if (Input.GetMouseButtonDown(0)&& EventSystem.current.currentSelectedGameObject == null)
        {
            if(activeTile == null)
            {
                baseMap = activeMapLayer.tilemap.GetInstantiatedObject(newPos)?.GetComponent<BaseMap>();
                if (baseMap != null)
                {
                    ShowPanel<BaseMapEditorPanel>((o) =>
                    {
                        baseMap.OnOpenEditor(o);
                    });
                }
            }
            else
            {
                SetTile(newPos);
            }

        }
        if (Input.GetMouseButtonDown(1) && EventSystem.current.currentSelectedGameObject == null)
        {
            RemoveTile(newPos);
        }
        if(Input.GetMouseButtonDown(2) && EventSystem.current.currentSelectedGameObject == null)
        {
            oldMousePos = mousePos;
        }
        if(Input.GetMouseButton(2) && EventSystem.current.currentSelectedGameObject == null)
        {
            Camera.main.transform.position -= mousePos - oldMousePos;
        }
        scroll = Input.GetAxisRaw("Mouse ScrollWheel");
        if(scroll != 0)
        {
            Camera.main.orthographicSize += scroll * cameraSpeed;
        }
    }
    #endregion

    #region 事件方法
    /// <summary>
    /// 初始化组件
    /// </summary>
    private void InitCompement()
    {
        ifMapName = GetControl<InputField>("ifMapName");
        ifMapDescription = GetControl<InputField>("ifMapDescription");
        ifMapLayerName = GetControl<InputField>("ifMapLayerName");
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
        btnQuit = GetControl<Button>("btnQuit");
        btnCancelTile = GetControl<Button>("btnCancelTile");
        btnBackOrigin = GetControl<Button>("btnBackOrigin");
        ddSortingLayer = GetControl<Dropdown>("ddSortingLayer");
        txtMousePos = GetControl<Text>("txtMousePos");

    }
    /// <summary>
    /// 注册组件内容
    /// </summary>
    private void RegisterCompementEvent()
    {
        ifMapName.onValueChanged.AddListener((o) =>
        {
            if (mapData != null)
            {
                mapData.name = o;
                
            }
        });
        ifMapDescription.onValueChanged.AddListener((o) =>
        {
            if (mapData != null) mapData.description = o;
        });
        ifMapLayerName.onValueChanged.AddListener((o) =>
        {
            if (activeMapLayer != null && activeMapLayer.mapDetile.id != -1)
            {
                activeMapLayer.mapDetile.name = o;
                activeMapLayer.ReLoad();
            }
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
        btnQuit.onClick.AddListener(() =>
        {
            Quit();
        });
        btnCancelTile.onClick.AddListener(() =>
        {
            CancelTile();
        });
        btnBackOrigin.onClick.AddListener(() =>
        {
            Camera.main.transform.position = new Vector3(0, 0, Camera.main.transform.position.z);
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
        ddSortingLayer.onValueChanged.AddListener((o) =>
        {
            if (activeMapLayer != null && activeMapLayer.mapDetile.id != -1)
            {
                activeMapLayer.mapDetile.layer = layers[o].name;
                activeMapLayer.ReLoad();
            }
        });
        ddSortingLayer.AddOptions(layerNames);
    }
    /// <summary>
    /// 加载所有地图文件信息
    /// </summary>
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
                string mapName = fileInfo.Name.Split('.')[0];
                btnMapCopy.GetComponentInChildren<Text>().text = mapName;
                mapDataList.Add(mapName, fileInfo);
            }
        }
    }
    /// <summary>
    /// 加载地图瓦片集信息
    /// </summary>
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
    /// <summary>
    /// 加载地图瓦片信息
    /// </summary>
    /// <param name="tileDataList"></param>
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
    /// <summary>
    /// 加载地图
    /// </summary>
    /// <param name="mapfile"></param>
    private void LoadMap(FileInfo mapfile)
    {
        Camera.main.transform.position = new Vector3(0, 0, Camera.main.transform.position.z);

        activeMap = mapfile;
        mapData = ResMgr.Instance.LoadBinaryWithMirror<MapData>(mapfile.FullName);
        ifMapName.text = mapData.name;
        ifMapDescription.text = mapData.description;
        itFeather.value = mapData.maxFeather;
        itTool.value = mapData.maxTool;


        if (grid != null) Destroy(grid.gameObject);
        grid = ResMgr.Instance.Load<GameObject>("Map/MapEdit/Grid").GetComponent<Grid>();
        uiTileMap = Instantiate(tilemap).GetComponent<Tilemap>();
        uiTileMap.gameObject.transform.SetParent(grid.transform, false);

        #region 常用4个点位注册
        Tilemap tilemapCopy = Instantiate(tilemap).GetComponent<Tilemap>();
        tilemapCopy.gameObject.transform.SetParent(grid.transform, false);
        tilemapCopy.gameObject.name = "PlayerSpawnPos";
        tilemapCopy.GetComponent<TilemapRenderer>().sortingLayerID = SortingLayer.NameToID("UI");
        foreach(V2 v2 in mapData.playerSpwnPos) tilemapCopy.SetTile(v2.ToV3Int(), tiles[0].tileBase);
        mlPlayerSpawn.Init(tilemapCopy);

        tilemapCopy = Instantiate(tilemap).GetComponent<Tilemap>();
        tilemapCopy.gameObject.transform.SetParent(grid.transform, false);
        tilemapCopy.gameObject.name = "ServitorSpawnPos";
        tilemapCopy.GetComponent<TilemapRenderer>().sortingLayerID = SortingLayer.NameToID("UI");
        foreach (V2 v2 in mapData.servitorSpwnPos) tilemapCopy.SetTile(v2.ToV3Int(), tiles[1].tileBase);
        mlServitorSpawn.Init(tilemapCopy);

        tilemapCopy = Instantiate(tilemap).GetComponent<Tilemap>();
        tilemapCopy.gameObject.transform.SetParent(grid.transform, false);
        tilemapCopy.gameObject.name = "FeatherSpawnPos";
        tilemapCopy.GetComponent<TilemapRenderer>().sortingLayerID = SortingLayer.NameToID("UI");
        foreach (V2 v2 in mapData.FeatherSpwnPos) tilemapCopy.SetTile(v2.ToV3Int(), tiles[2].tileBase);
        mlFeatherSpawn.Init(tilemapCopy);

        tilemapCopy = Instantiate(tilemap).GetComponent<Tilemap>();
        tilemapCopy.gameObject.transform.SetParent(grid.transform, false);
        tilemapCopy.gameObject.name = "ToolSpawnPos";
        tilemapCopy.GetComponent<TilemapRenderer>().sortingLayerID = SortingLayer.NameToID("UI");
        foreach (V2 v2 in mapData.ToolSpwnPos) tilemapCopy.SetTile(v2.ToV3Int(), tiles[3].tileBase);
        mlToolSpawn.Init(tilemapCopy);
        #endregion

        for (int i = 0; i < svMapLayerList.content.childCount; i++)
        {
            Destroy(svMapLayerList.content.GetChild(i).gameObject);
        }
        foreach (MapDetile mapDetile in mapData.mapDetiles)
        {
            LoadMapDetile(mapDetile);
        }
    }
    /// <summary>
    /// 加载地图层级
    /// </summary>
    /// <param name="mapDetile">层级信息</param>
    private void LoadMapDetile(MapDetile mapDetile)
    {
        MapDetile md = mapDetile;
        Tilemap tilemapCopy = Instantiate(tilemap).GetComponent<Tilemap>();
        tilemapCopy.gameObject.transform.SetParent(grid.transform, false);
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
    /// <summary>
    /// 保存地图文件
    /// </summary>
    private void SaveMap()
    {
        activeMap.MoveTo(activeMap.DirectoryName + "\\" + mapData.name + ".MapData");
        Debug.Log(mapData.mapDetiles.Count);
        ResMgr.Instance.SaveBinaryWithMirror(mapData, activeMap.FullName);
    }
    /// <summary>
    /// 创建新地图
    /// </summary>
    private void CreatMap()
    {
        mapData = new MapData();
        mapData.name = "New Map";
        mapData.description = "新地图";
        ResMgr.Instance.SaveBinaryWithMirror(mapData, mapDataPath + mapData.name + ".MapData");
        LoadAllMapData();
        LoadMap(mapDataList["New Map"]);
    }
    /// <summary>
    /// 删除地图
    /// </summary>
    /// <param name="mapfile"></param>
    private void DestroyMap(FileInfo mapfile)
    {
        if(mapfile != null)
            mapfile.Delete();
    }
    /// <summary>
    /// 改变选取瓦片
    /// </summary>
    /// <param name="tileData"></param>
    private void ChangeTile(TileData tileData)
    {
        if (activeMapLayer.type != 0) return;
        ifChangTile = true;
        activeTile = tileData;
    }

    private void CancelTile()
    {
        activeTile = null;
    }

    /// <summary>
    /// 设置瓦片
    /// </summary>
    /// <param name="pos"></param>
    private void SetTile(Vector3Int pos)
    {
        V2 v2 = new V2(pos);
        switch (activeMapLayer.type)
        {
            case 0:
                activeMapLayer.tilemap.SetTile(pos, activeTile.tileBase);
                if (activeTile.ifHaveValue)
                {
                    activeMapLayer.mapDetile.tileValue[v2] = activeMapLayer.tilemap.GetInstantiatedObject(pos)?.GetComponent<BaseMap>().OnSave();
                    activeMapLayer.mapDetile.MapTileDetiles[v2] = new MapTileDetile(activeTile.id, true);
                }
                else
                {
                    activeMapLayer.mapDetile.tileValue.Remove(v2);
                    activeMapLayer.mapDetile.MapTileDetiles[v2] = new MapTileDetile(activeTile.id, false);
                }
                break;
            case 1: 
                activeMapLayer.tilemap.SetTile(pos, activeTile.tileBase);
                if (!mapData.playerSpwnPos.Contains(v2)) mapData.playerSpwnPos.Add(v2);
                break;
            case 2:
                activeMapLayer.tilemap.SetTile(pos, activeTile.tileBase);
                if (!mapData.servitorSpwnPos.Contains(v2)) mapData.servitorSpwnPos.Add(v2);
                break;
            case 3:
                activeMapLayer.tilemap.SetTile(pos, activeTile.tileBase);
                if (!mapData.FeatherSpwnPos.Contains(v2)) mapData.FeatherSpwnPos.Add(v2);
                break;
            case 4:
                activeMapLayer.tilemap.SetTile(pos, activeTile.tileBase);
                if (!mapData.ToolSpwnPos.Contains(v2)) mapData.ToolSpwnPos.Add(v2);
                break;
        }

    }
    /// <summary>
    /// 删除瓦片
    /// </summary>
    /// <param name="pos"></param>
    private void RemoveTile(Vector3Int pos)
    {
        V2 v2 = new V2(pos);
        switch (activeMapLayer.type)
        {
            case 0:
                activeMapLayer.tilemap.SetTile(pos, null);
                activeMapLayer.mapDetile.tileValue.Remove(v2);
                activeMapLayer.mapDetile.MapTileDetiles.Remove(v2);
                break;
            case 1:
                activeMapLayer.tilemap.SetTile(pos, null);
                mapData.playerSpwnPos.Remove(v2);
                break;
            case 2:
                activeMapLayer.tilemap.SetTile(pos, null);
                mapData.servitorSpwnPos.Remove(v2);
                break;
            case 3:
                activeMapLayer.tilemap.SetTile(pos, null);
                mapData.FeatherSpwnPos.Remove(v2);
                break;
            case 4:
                activeMapLayer.tilemap.SetTile(pos, null);
                mapData.ToolSpwnPos.Remove(v2);
                break;
        }
    }
    /// <summary>
    /// 更改涂写层级
    /// </summary>
    /// <param name="mapLayer"></param>
    private void ChangeMapLayer(MapLayer mapLayer)
    {
        activeMapLayer = mapLayer;
        UpdataMapLayerData(mapLayer);
        switch (mapLayer.type)
        {
            case 1:
                activeTile = tiles[0];
                ifChangTile = true;
                break;
            case 2:
                activeTile = tiles[1];
                ifChangTile = true;
                break;
            case 3:
                activeTile = tiles[2];
                ifChangTile = true;
                break;
            case 4:
                activeTile = tiles[3];
                ifChangTile = true;
                break;
        }
    }
    /// <summary>
    /// 更新层级信息
    /// </summary>
    /// <param name="mapLayer"></param>
    private void UpdataMapLayerData(MapLayer mapLayer)
    {
        if (mapLayer.type != 0)
        {
            ifMapLayerName.SetTextWithoutNotify(mapLayer.txtName.text);
            ddSortingLayer.SetValueWithoutNotify(layerNames.FindIndex(i => i == "UI"));
        }
        else
        {
            ifMapLayerName.SetTextWithoutNotify(mapLayer.txtName.text);
            ddSortingLayer.SetValueWithoutNotify(layerNames.FindIndex(i => i == mapLayer.mapDetile.layer));
        }
        
    }
    /// <summary>
    /// 添加层级
    /// </summary>
    private void AddMapLayer()
    {
        if (mapData == null) return;
        MapDetile mapDetile = new MapDetile();
        mapDetile.name = "New MapLayer";
        mapDetile.layer = "Instance";
        mapData.mapDetiles.Add(mapDetile);
        LoadMapDetile(mapDetile);
    }
    /// <summary>
    /// 移除层级
    /// </summary>
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
    /// <summary>
    /// 退出
    /// </summary>
    private void Quit()
    {
        Destroy(grid.gameObject);
        m_onQuit?.Invoke();
    }
    #endregion
}
