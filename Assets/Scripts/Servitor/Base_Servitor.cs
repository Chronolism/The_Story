using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base_Servitor : MonoBehaviour
{
    public D_Servitor d_servitor;

    public virtual void InitServitor()
    {
        //���˵�id(0ָ��ȫ����)
        d_servitor.master_runtime_id = 0;
        //����
        d_servitor.HP_Max = 100;
        d_servitor.runtime_HP_Max = 100;
        d_servitor.runtime_HP = 100;
        //�ٶ�
        d_servitor.Speed = 1;
        d_servitor.runtime_Speed = 1;
        d_servitor.runtime_Speed_Max = 1.5f;
        //����
        d_servitor.atkDamage = 1;
        //����д����īˮ��
        d_servitor.rewrite_ink_Need = 25;
        //����д���������
        d_servitor.rewrite_given_ultimate_Need = 10;
        //����Ϸģʽ��������ʾ��ʹħʽ��(-1ָ��ȫ����)
        d_servitor.ServitorDisplay = -1;
    }
}
