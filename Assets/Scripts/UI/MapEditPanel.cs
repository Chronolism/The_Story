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
    //�ƶ���Ļ���
    Vector3 _thisFrameMousePostion;
    Vector3 _lastFrameMousePostion;
    //UI�ؼ�
    Text _focusCell;//����������ʾ����
    Dropdown _mapLayerChoose;//��ǰλ�ڵ�ͼ��һ��
    Button _confirmAndClear;//ȷ�ϵ�ǰ���ͼ�༭��ʽ�����
    InputField _cellData;//�ɱ༭��ѡ�����ݵ�����
    InputField _mapName;//��ͼ����
    Button _saveMap;//�����ͼ
    Button _loadMap;//���ص�ͼ
    Button _cellWriteIn;//д�뼤��ĵ�Ԫ�񵽵�ͼ
    //��������
    Dictionary<Vector3Int, bool> _testTileMapData;
    TileBase _testTile_Confirm;
    TileBase _testTile_Focus;
    Vector3Int _LastFocusCell = new Vector3Int(0, 0, 0);
    Vector3Int _ThisFrameFocusCell;
    //���ڰ汾��ͼ����
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

        _testTileMapData = new Dictionary<Vector3Int, bool>();//��ʼ��
        _testTileMapData.Add(new Vector3Int(0, 0, 0), false);
        _editingTilemap = _MapGridGameObject.GetComponentInChildren<Tilemap>();//��ȡ��Ҫ�޸ĵ�Tile�������ã����Ľ�
        _testTile_Focus = Resources.Load<TileBase>("Map/MapEdit/TestIconLowAlpha");//�۽���ʾ����
        _testTile_Confirm = Resources.Load<TileBase>("Map/MapEdit/TestIcon");//ѡ����ʾ����

        //Ŀǰֻ����������
        _localMapDataDetailOriginal = new D_MapDataDetailOriginal();
        _localMapDataDetailOriginal.mapCellData = new Dictionary<string, Dictionary<Vector3Int, int>>
        {
            {"Bottom" ,null},
            {"Middle" ,null},
            {"Top" ,null},
        };
        _localLayerMapCellData = new Dictionary<Vector3Int, int>();

        //UI�ؼ���ȡ
        _focusCell = GetControl<Text>("FocusCell");//����������ʾ����(Update)
        _mapLayerChoose = GetControl<Dropdown>("MapLayerChoose");//��ǰλ�ڵ�ͼ��һ��(onValueChanged)
        _confirmAndClear = GetControl<Button>("ConfirmAndClear");//ȷ�ϵ�ǰ���ͼ�༭��ʽ�����
        _cellData = GetControl<InputField>("CellData");//�ɱ༭��ѡ�����ݵ�����(onEditEnd)
        _mapName = GetControl<InputField>("MapName");//��ͼ����(onEditEnd)
        _saveMap = GetControl<Button>("MapSave");//�����ͼ
        _loadMap = GetControl<Button>("MapLoad");//���ص�ͼ
        _cellWriteIn = GetControl<Button>("CellWriteIn");//д�뼤��ĵ�Ԫ�񵽵�ͼ

        //UI�ؼ���ʼ��
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

        TestConsole.Instance.AddCommand("MapFill", () => { MapManager.Instance.AutoFilledEmpty(ref _localLayerMapCellData,out _); },"������ʱ��������������漰����");//��ʱ�����Ժ���AutoFilledEmpty
    }

    protected override void Update()
    {
        base.Update();
        _ThisFrameFocusCell = _editingGrid.WorldToCell(_camera.ScreenToWorldPoint(Input.mousePosition));
        //��ʵʱ��ʾ����
        _focusCell.transform.position = Input.mousePosition;
        int tempThisCellData;
        string tempThisCellDataExpress = "null";
        if(_localLayerMapCellData.TryGetValue(_ThisFrameFocusCell,out tempThisCellData)) 
            tempThisCellDataExpress = tempThisCellData.ToString();
        _focusCell.text = " (" + _ThisFrameFocusCell.x+ "," + _ThisFrameFocusCell.y + "," + tempThisCellDataExpress + ")";
        //��ͼ��ȡ���ݣ�ʧ���ȴ���һ��
        _testTileMapData.TryAdd(_ThisFrameFocusCell, false);
        //�޸����ݵ���Ϊ
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
        if (!_testTileMapData[_ThisFrameFocusCell])//�����ݲ�Ϊ��ʱ��ʾFocus��ʽ
            _editingTilemap.SetTile(_ThisFrameFocusCell, _testTile_Focus);
        if (_LastFocusCell != _ThisFrameFocusCell && !_testTileMapData[_LastFocusCell])//ֻ�����ݲ�Ϊ�棬����겻������ʱ�������ʽ
            _editingTilemap.SetTile(_LastFocusCell, null);
        _LastFocusCell = _ThisFrameFocusCell;
        //����ɹ������˻������Ƿ��б���cell�е�ֵ�Ķ�Ӧ��ϵ
        _ViewMoveAndScale();
    }
    //���testTileMapData ������д��
    void _ConfirmAndClear()
    {
        //��������һ���Ƿ��ϵģ���Ϊ����ʹ������_nowEditingCellDataText������
        //����������ֵ�������⣬��һǵ��һ��
        Dictionary<Vector3Int, int> temp = new Dictionary<Vector3Int, int>();
        foreach (var item in _localLayerMapCellData)
        {
            temp.TryAdd(item.Key, item.Value);
        }
        _localMapDataDetailOriginal.mapCellData[_nowEditingMapLayer] = temp;
        //_localLayerMapCellData.Clear();Ҫ��ʾ�Ѿ����ĵ�ֵ
        _testTileMapData.Clear();
        _editingTilemap.ClearAllTiles();
    }
    //����ǰ��ͼ����
    void _SaveMap()
    {
        _localMapDataDetailOriginal.name = _nowEditingMap;
        MapManager.Instance.SaveMap(_localMapDataDetailOriginal);
        print(MapManager.Instance.mapSaveDirectoryAddress);
    }
    //�������ֽ���ͼ����
    void _LoadMap()
    {
        int errorCode;
        _localMapDataDetailOriginal = MapManager.Instance.LoadMap(_nowEditingMap,out errorCode);
        if (_localMapDataDetailOriginal == null)
        {
            print("��ȡ����" + errorCode);
            return;
        }
        _nowEditingMap = _localMapDataDetailOriginal.name;//��Ϊϰ�ߣ�ʵ����ͨ�����ּ���ʱ�˸�ֵ������
        _mapName.text = _nowEditingMap;//����ͨ�����Ƽ���ʱ����Ҫͬ�������Ϣ��
        _LoadMapLayer();
    }
    //С���ߣ����ظò�ĵ�ͼ����_localLayerMapCellData
    void _LoadMapLayer()
    {
        print(_localMapDataDetailOriginal.mapCellData[_nowEditingMapLayer]);
        //����keyһ���Ƿ��ϵģ���Ϊֻ�����л������һ�μ��ص�ͼ����ʱʹ�ã������ܱ���
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
    //д�뼤��ĵ�Ԫ�񵽵�ͼ
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
            TestConsole.Instance.ThrowError("��ʽ����");
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
