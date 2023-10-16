using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class MapEditPanel : BasePanel
{
    Camera _camera;
    float _defaultCameraScale;
    Vector3 _defaultCameraVector3;
    GameObject _MapGridGameObject;
    Grid _editingGrid;
    Tilemap _editingTilemap;
    //移动屏幕相关
    Vector3 _thisFrameMousePostion;
    Vector3 _lastFrameMousePostion;
    //UI控件
    Text _focusCell;//跟随鼠标的显示内容
    Dropdown _mapLayerChoose;//当前位于地图哪一层
    Button _confirmAndClear;//确认当前层地图编辑样式并清空
    InputField _cellData;//可编辑，选定内容的数据
    InputField _mapName;//地图名称
    Button _saveMap;//保存地图
    Button _loadMap;//加载地图
    Button _cellWriteIn;//写入激活的单元格到地图
    //测试用例
    Dictionary<Vector3Int, bool> _testTileMapData;
    TileBase _testTile_Confirm;
    TileBase _testTile_Focus;
    Vector3Int _LastFocusCell = new Vector3Int(0, 0, 0);
    Vector3Int _ThisFrameFocusCell;
    //早期版本地图用例
    D_MapDataDetailOriginal _localMapDataDetailOriginal;
    Dictionary<Vector3Int, int> _localLayerMapCellData;
    string _nowEditingMap = "Default";
    string _nowEditingMapLayer = "Bottom";
    string _nowEditingCellDataText = "(None)";

    public override void Init()
    {
        _camera = Camera.main;
        _defaultCameraScale = _camera.orthographicSize;
        _defaultCameraVector3 = _camera.transform.position;

        _MapGridGameObject = Instantiate<GameObject>(Resources.Load<GameObject>("Map/MapEdit/Grid"));
        _editingGrid = _MapGridGameObject.GetComponent<Grid>();

        _testTileMapData = new Dictionary<Vector3Int, bool>();//初始化
        _testTileMapData.Add(new Vector3Int(0, 0, 0), false);
        _editingTilemap = _MapGridGameObject.GetComponentInChildren<Tilemap>();//获取需要修改的Tile，测试用，待改进
        _testTile_Focus = Resources.Load<TileBase>("Map/MapEdit/TestIconLowAlpha");//聚焦显示内容
        _testTile_Confirm = Resources.Load<TileBase>("Map/MapEdit/TestIcon");//选定显示内容

        //目前只设置三个层
        _localMapDataDetailOriginal = new D_MapDataDetailOriginal();
        _localMapDataDetailOriginal.mapCellData = new Dictionary<string, Dictionary<Vector3Int, int>>
        {
            {"Bottom" ,null},
            {"Middle" ,null},
            {"Top" ,null},
        };
        _localLayerMapCellData = new Dictionary<Vector3Int, int>();

        //UI控件获取
        _focusCell = GetControl<Text>("FocusCell");//跟随鼠标的显示内容(Update)
        _mapLayerChoose = GetControl<Dropdown>("MapLayerChoose");//当前位于地图哪一层(onValueChanged)
        _confirmAndClear = GetControl<Button>("ConfirmAndClear");//确认当前层地图编辑样式并清空
        _cellData = GetControl<InputField>("CellData");//可编辑，选定内容的数据(onEditEnd)
        _mapName = GetControl<InputField>("MapName");//地图名称(onEditEnd)
        _saveMap = GetControl<Button>("MapSave");//保存地图
        _loadMap = GetControl<Button>("MapLoad");//加载地图
        _cellWriteIn = GetControl<Button>("CellWriteIn");//写入激活的单元格到地图

        //UI控件初始化
        _confirmAndClear.onClick.AddListener(_ConfirmAndClear);
        _saveMap.onClick.AddListener(_SaveMap);
        _loadMap.onClick.AddListener(_LoadMap);
        _cellWriteIn.onClick.AddListener(_CellWriteIn);
        _mapLayerChoose.AddOptions(new List<string>() { "Bottom", "Middle", "Top" });
        _mapLayerChoose.onValueChanged.AddListener((o) => { _nowEditingMapLayer = _mapLayerChoose.options[o].text; _LoadMapLayer(); });
        _cellData.text = _nowEditingCellDataText;
        _cellData.onEndEdit.AddListener((o) => { _nowEditingCellDataText = o; });
        _mapName.text = _nowEditingMap;
        _mapName.onEndEdit.AddListener((o) => { _nowEditingMap = o; });

        TestConsole.Instance.AddCommand("MapFill", () => { MapManager.Instance.AutoFilledEmpty(ref _localLayerMapCellData,out _); },"测试临时用例：填充所有涉及方格");//临时：测试函数AutoFilledEmpty
    }

    protected override void Update()
    {
        base.Update();
        _ThisFrameFocusCell = _editingGrid.WorldToCell(_camera.ScreenToWorldPoint(Input.mousePosition));
        //先实时显示数据
        _focusCell.transform.position = Input.mousePosition;
        int tempThisCellData;
        string tempThisCellDataExpress = "null";
        if(_localLayerMapCellData.TryGetValue(_ThisFrameFocusCell,out tempThisCellData)) 
            tempThisCellDataExpress = tempThisCellData.ToString();
        _focusCell.text = " (" + _ThisFrameFocusCell.x+ "," + _ThisFrameFocusCell.y + "," + tempThisCellDataExpress + ")";
        //试图获取数据，失败先创建一个
        _testTileMapData.TryAdd(_ThisFrameFocusCell, false);
        //修改数据的行为
        if (Input.GetMouseButtonDown(0) && EventSystem.current.currentSelectedGameObject == null)
        {
            if (!_testTileMapData[_ThisFrameFocusCell])
            {
                _testTileMapData[_ThisFrameFocusCell] = true;
                _editingTilemap.SetTile(_LastFocusCell, _testTile_Confirm);
            }
            else
            {
                _testTileMapData[_ThisFrameFocusCell] = false;
                _editingTilemap.SetTile(_LastFocusCell, null);
            }              
        }
        if (!_testTileMapData[_ThisFrameFocusCell])//当数据不为真时显示Focus样式
            _editingTilemap.SetTile(_ThisFrameFocusCell, _testTile_Focus);
        if (_LastFocusCell != _ThisFrameFocusCell && !_testTileMapData[_LastFocusCell])//只有数据不为真，且鼠标不在这里时候清空样式
            _editingTilemap.SetTile(_LastFocusCell, null);
        _LastFocusCell = _ThisFrameFocusCell;
        //这里成功建立了画面中是否有笔与cell中的值的对应关系
        _ViewMoveAndScale();
    }
    //清空testTileMapData 并尝试写入
    void _ConfirmAndClear()
    {
        //这里数字一定是符合的，因为不会使用现在_nowEditingCellDataText的内容
        //引用类型与值类型问题，吃一堑长一智
        Dictionary<Vector3Int, int> temp = new Dictionary<Vector3Int, int>();
        foreach (var item in _localLayerMapCellData)
        {
            temp.TryAdd(item.Key, item.Value);
        }
        _localMapDataDetailOriginal.mapCellData[_nowEditingMapLayer] = temp;
        //_localLayerMapCellData.Clear();要显示已经更改的值
        _testTileMapData.Clear();
        _editingTilemap.ClearAllTiles();
    }
    //将当前地图保存
    void _SaveMap()
    {
        _localMapDataDetailOriginal.name = _nowEditingMap;
        MapManager.Instance.SaveMap(_localMapDataDetailOriginal);
        print(MapManager.Instance.mapSaveDirectoryAddress);
    }
    //将按名字将地图加载
    void _LoadMap()
    {
        int errorCode;
        _localMapDataDetailOriginal = MapManager.Instance.LoadMap(_nowEditingMap,out errorCode);
        if (_localMapDataDetailOriginal == null)
        {
            print("读取错误：" + errorCode);
            return;
        }
        _nowEditingMap = _localMapDataDetailOriginal.name;//作为习惯，实际上通过名字加载时此赋值无意义
        _mapName.text = _nowEditingMap;//而不通过名称加载时是需要同步面板信息的
        _LoadMapLayer();
    }
    //小工具，加载该层的地图进入_localLayerMapCellData
    void _LoadMapLayer()
    {
        print(_localMapDataDetailOriginal.mapCellData[_nowEditingMapLayer]);
        //这里key一定是符合的，因为只会在切换层与第一次加载地图数据时使用，但可能报空
        if (_localMapDataDetailOriginal.mapCellData[_nowEditingMapLayer] == null)
        {
            _localLayerMapCellData.Clear();
            return;
        }
        Dictionary<Vector3Int, int> temp = _localMapDataDetailOriginal.mapCellData[_nowEditingMapLayer];
        foreach (var item in temp)
        {
            _localLayerMapCellData.TryAdd(item.Key, item.Value);
            _localLayerMapCellData[item.Key] = item.Value;
        }
    }
    //写入激活的单元格到地图
    void _CellWriteIn()
    {
        int target;
        if (int.TryParse(_nowEditingCellDataText, out target))
        {
            foreach (var item in _testTileMapData)
            {
                if (item.Value)
                {
                    _localLayerMapCellData.TryAdd(item.Key, target);
                    _localLayerMapCellData[item.Key] = target;
                }
            }
        }
        else
        {
            TestConsole.Instance.ThrowError("格式错误");
        }

    }
    public override void HideMe(UnityAction callBack)
    {
        _camera.orthographicSize = _defaultCameraScale;
        _camera.transform.position = _defaultCameraVector3;

        Destroy(_MapGridGameObject);
        base.HideMe(callBack);
    }
    void _ViewMoveAndScale()
    {
        //用来在编辑地图的时候移动缩放屏幕的，写的有问题
        _camera.orthographicSize -= Input.mouseScrollDelta.y * Time.deltaTime * 5;
        if (Input.GetMouseButton(1))
        {
            _thisFrameMousePostion = Input.mousePosition;
            if (_lastFrameMousePostion == new Vector3(0, 0, 0))
            {
                _lastFrameMousePostion = _thisFrameMousePostion;
            }
            else
            {
                _camera.transform.position += (_lastFrameMousePostion - _thisFrameMousePostion).normalized * Time.deltaTime * 8;
                _lastFrameMousePostion = _thisFrameMousePostion;
            }
        }
    }

}
