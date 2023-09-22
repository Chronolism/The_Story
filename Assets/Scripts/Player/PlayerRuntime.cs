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
    D_Base_Player _PlayerData;
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
            _PlayerData = PlayerManager.Instance.GetPlayerDataWithRuntime_Id(_runtime_id);
            _needChangeID = false;
        }
        //����д��Ҫ�õ������Ϣ�Ļ���֡
        if (_PlayerData != null) DisplayUpdate();
        //�����ƶ���ʾ��ʽ
        this.transform.position += Time.deltaTime * displacementThisFrameDirctionTrue;
    }
    private void FixedUpdate()
    {
        if (_PlayerData != null) LogicUpdate();
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

    }
}
