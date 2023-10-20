using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRuntime : MonoBehaviour
{
    //����ű��ǹ����������ɫԤ�������߿������ϣ����ڿ�����������ʱ����

    //�����䶯������ͨ��PlayerManager��ȡ
    /// <summary>
    /// ����ʱid���״��Ǹ��裬֮������
    /// </summary>
    public uint runtime_id = 400; public bool needChangeID = true;
    public bool networkEnable = true; //{ get => !GameManager.Instance.isOfflineLocalTest; set => GameManager.Instance.isOfflineLocalTest = !value; }����������߲���ͬһ��ʹ�ô��д���
    Player _networkPlayer;
    /// <summary>
    /// ������ݼ���Ӧ���ڽ���ʱ��ȡ
    /// </summary>
    public D_Base_Player PlayerData;
    //�ϳ��䶯���������ڱ���
    /// <summary>
    /// ֡λ�Ʒ��򣨷��߼�֡����
    /// </summary>
    public Vector3 displacementThisFrameDirctionTrue;
    //ע���
    public GameObject inputAndMove;
    float _timer;
    void Awake()
    {
        
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //�����ɫID�������ģ���ʱ��ֵ
        if (needChangeID)
        {
            PlayerData = PlayerManager.Instance.GetPlayerDataWithRuntime_Id(runtime_id);
            if (PlayerData != null) PlayerData.runtime_Player = this.gameObject;
            needChangeID = false;
        }
        //����д��Ҫ�õ������Ϣ�Ļ���֡
        if (PlayerData != null) DisplayUpdate();
        _timer += Time.deltaTime;
        if (_timer >= 1){ _timer = 0; SecUpdate(); }
        //����IDͬ�����ȡ
        



    }
    private void FixedUpdate()
    {
        //if (PlayerData != null) LogicUpdate();
    }
    /// <summary>
    /// ����֡����
    /// </summary>
    void DisplayUpdate()
    {

    }
    /// <summary>
    /// �߼�֡����
    /// </summary>
    void LogicUpdate()
    {
        /*
        if (MapManager.Instance.runtimeGrid == null) return;//��ʱ�����ж���Ϸ�Ƿ�������ʱ
        //�����ƶ���ʾ��ʽ���߼�֡���¡�������������ϵͳͬ����
        this.transform.position += Time.deltaTime * displacementThisFrameDirctionTrue;
        */
        //����idͬ��
        if (networkEnable && _networkPlayer != null)
        {
            if (_networkPlayer.isLocalPlayer)
            {
                runtime_id = _networkPlayer.netId;
                PlayerManager.Instance.LocalPlayer.runtime_id = _networkPlayer.netId;
            }
            else
            {
                runtime_id = _networkPlayer.netId;
                PlayerManager.Instance.AddOtherPlayer(_networkPlayer.netId, PlayerData);
            }
            needChangeID = true;
            networkEnable = false;
        }
        //ÿ���߼�֡���¸���ҵ�����λ�ã�V2��
        PlayerData.runtime_gird_Position = new V2(MapManager.Instance.runtimeGrid.WorldToCell(this.transform.position).x, MapManager.Instance.runtimeGrid.WorldToCell(this.transform.position).y);
        //ÿ���߼�֡��ͼ������������
        if (PlayerData.runtime_Player == null) PlayerData.runtime_Player = this.gameObject;


    }
    /// <summary>
    /// ����Ϊ
    /// </summary>
    void SecUpdate()
    {
        //ÿ����Debug��ҵ�����λ�ã�V2��
        //print(PlayerManager.Instance.LocalPlayer.runtime_gird_Position.x + "," + PlayerManager.Instance.LocalPlayer.runtime_gird_Position.y);
        //��ͼ��ȡNet��Ϣ
        if (networkEnable)
            TryGetComponent<Player>(out _networkPlayer);
        
    }
}
