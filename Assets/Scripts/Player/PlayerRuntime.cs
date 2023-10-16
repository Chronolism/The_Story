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
    int _runtime_id = 400;bool _needChangeID = true;
    /// <summary>
    /// ������ݼ���Ӧ���ڽ���ʱ��ȡ
    /// </summary>
    public D_Base_Player PlayerData;
    //�ϳ��䶯���������ڱ���
    /// <summary>
    /// ֡λ�Ʒ��򣨷��߼�֡����
    /// </summary>
    public Vector3 displacementThisFrameDirctionTrue;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //�����ɫID�������ģ���ʱ��ֵ
        if (_needChangeID)
        {
            PlayerData = PlayerManager.Instance.GetPlayerDataWithRuntime_Id(_runtime_id);
            //_PlayerData.runtime_Player = this.gameObject;
            _needChangeID = false;
        }
        //����д��Ҫ�õ������Ϣ�Ļ���֡
        if (PlayerData != null) DisplayUpdate();
        
    }
    private void FixedUpdate()
    {
        if (PlayerData != null) LogicUpdate();
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
        if (MapManager.Instance.runtimeGrid == null) return;//��ʱ�����ж���Ϸ�Ƿ�������ʱ
        //�����ƶ���ʾ��ʽ���߼�֡���¡�������������ϵͳͬ����
        this.transform.position += Time.deltaTime * displacementThisFrameDirctionTrue;
        //ÿ���߼�֡���¸���ҵ�����λ�ã�V2��
        PlayerData.runtime_gird_Position = new V2(MapManager.Instance.runtimeGrid.WorldToCell(this.transform.position).x, MapManager.Instance.runtimeGrid.WorldToCell(this.transform.position).y);
        print(PlayerManager.Instance.LocalPlayer.runtime_gird_Position.x + "," + PlayerManager.Instance.LocalPlayer.runtime_gird_Position.y);
    }
}
