using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base_Servitor : MonoBehaviour
{
    public D_Servitor d_servitor;
    
    public virtual void InitServitor()
    {
        d_servitor = new D_Servitor();
        //���˵�id(0ָ��ȫ����)
        d_servitor.master_runtime_id = 0;
        //����Ϸģʽ��������ʾ��ʹħʽ��(-1ָ��ȫ����)
        d_servitor.servitorDisplay = -1;
    }
}
