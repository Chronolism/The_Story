using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;

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
    //测试用例
    Dictionary<Vector3Int, bool> _testTileMapData;
    TileBase _testTile_Confirm;
    TileBase _testTile_Focus;
    Vector3Int _LastFocusCell = new Vector3Int(0, 0, 0);
    Vector3Int _ThisFrameFocusCell;
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

        //GetControl<>

    }

    protected override void Update()
    {
        base.Update();
        _ThisFrameFocusCell = _editingGrid.WorldToCell(_camera.ScreenToWorldPoint(Input.mousePosition));

        //试图获取数据，失败先创建一个
        _testTileMapData.TryAdd(_ThisFrameFocusCell, false);
        //修改数据的行为
        if (Input.GetMouseButtonDown(0))
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

        _ViewMoveAndScale();
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
