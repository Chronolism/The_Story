using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaubMap : BaseMap
{
    public override void OnServerStart()
    {
        EntityFactory.Instance.CreatEntity<DaubBuild>(2001, transform.position);
    }
}
