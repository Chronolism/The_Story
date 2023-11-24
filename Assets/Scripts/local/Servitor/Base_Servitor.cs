using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base_Servitor : MonoBehaviour
{
    public D_Servitor d_servitor;
    
    public virtual void InitServitor()
    {
        d_servitor = new D_Servitor();
        //主人的id(0指完全中立)
        d_servitor.master_runtime_id = 0;
        //由游戏模式管理赋予显示的使魔式样(-1指完全中立)
        d_servitor.servitorDisplay = -1;
    }
}
