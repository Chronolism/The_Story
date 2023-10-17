using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.LightingExplorerTableColumn;

public class ATKData
{
    public int id;
    public float pre;
    public float value;
    public float valueAdd;
    public float valuePre;
    public AtkType atkType;
    public bool canAtk = true;
    public float AtkValue => pre * (value + valueAdd) * valuePre;
    public ATKData(int id, float pre, float value, float valueAdd, float valuePre, AtkType atkType)
    {
        this.id = id;
        this.pre = pre;
        this.value = value;
        this.valueAdd = valueAdd;
        this.valuePre = valuePre;
        this.atkType = atkType;
    }
    public ATKData()
    {

    }
}
