using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuillotineMap : BaseMap
{
    public override void OnServerStart()
    {
        EntityFactory.Instance.CreatEntity<DaubBuild>(2002, transform.position);
    }
}
