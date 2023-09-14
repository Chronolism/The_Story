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
    //�ƶ���Ļ���
    Vector3 _thisFrameMousePostion;
    Vector3 _lastFrameMousePostion;
    //��������
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

        _testTileMapData = new Dictionary<Vector3Int, bool>();//��ʼ��
        _testTileMapData.Add(new Vector3Int(0, 0, 0), false);
        _editingTilemap = _MapGridGameObject.GetComponentInChildren<Tilemap>();//��ȡ��Ҫ�޸ĵ�Tile�������ã����Ľ�
        _testTile_Focus = Resources.Load<TileBase>("Map/MapEdit/TestIconLowAlpha");//�۽���ʾ����
        _testTile_Confirm = Resources.Load<TileBase>("Map/MapEdit/TestIcon");//ѡ����ʾ����

        //GetControl<>

    }

    protected override void Update()
    {
        base.Update();
        _ThisFrameFocusCell = _editingGrid.WorldToCell(_camera.ScreenToWorldPoint(Input.mousePosition));

        //��ͼ��ȡ���ݣ�ʧ���ȴ���һ��
        _testTileMapData.TryAdd(_ThisFrameFocusCell, false);
        //�޸����ݵ���Ϊ
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
        if (!_testTileMapData[_ThisFrameFocusCell])//�����ݲ�Ϊ��ʱ��ʾFocus��ʽ
            _editingTilemap.SetTile(_ThisFrameFocusCell, _testTile_Focus);
        if (_LastFocusCell != _ThisFrameFocusCell && !_testTileMapData[_LastFocusCell])//ֻ�����ݲ�Ϊ�棬����겻������ʱ�������ʽ
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
        //�����ڱ༭��ͼ��ʱ���ƶ�������Ļ�ģ�д��������
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
