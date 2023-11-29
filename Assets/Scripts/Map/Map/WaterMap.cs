using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterMap : BaseEnvironwentMap
{
    int i = 0;
    public override void OnEnter(Entity entity)
    {
        i++;
        Debug.Log(i);
    }
}
