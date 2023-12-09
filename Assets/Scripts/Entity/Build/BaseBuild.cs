using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBuild : NetworkBehaviour
{
    protected Player entityParent;

    [SerializeField] protected float timeMax = 15;
    [SyncVar]
    public float time;

    public virtual void ReSetBuild()
    {
        time = timeMax;
        entityParent = null;
    }
}
