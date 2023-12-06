using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMap : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if(DataMgr.Instance.mapLoadType == 0)
        {
            if (NetworkServer.active)
            {
                OnServerStart();
            }
            OnClientStart();
            gameObject.SetActive(false);
        }
    }

    public virtual void OnServerStart()
    {

    }

    public virtual void OnClientStart()
    {

    }
}
