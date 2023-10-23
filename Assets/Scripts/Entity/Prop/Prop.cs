using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prop : NetworkBehaviour
{
    Collider collider;
    SpriteRenderer spriteRenderer;
    private void Awake()
    {
        collider = GetComponent<Collider>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public PropData propData;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isServer)
        {
            if(collision.TryGetComponent<Player>(out Player player))
            {
                switch (propData.propType)
                {
                    case PropType.TrigerTool:
                        foreach(var i in propData.value)
                        {
                            DataMgr.Instance.GetBuff(i.buffId).OnTriger(player, i.buffValue);
                        }
                        break;
                    case PropType.UseTool:
                        if (player.AddProp(propData))
                        {
                            HideProp();
                        }
                        break;
                }
            }    
        }
    }
    [Server]
    public void HideProp()
    {
        ChangeProp(false);
        HidePropRpc();
    }
    [ClientRpc]
    public void HidePropRpc()
    {
        if (isServer) return;
        ChangeProp(false);
    }


    [Server]
    public void ShowProp(PropData prop)
    {
        propData = prop;
        ChangeProp(true);
        ShowPropRpc(prop.id);
    }
    [ClientRpc]
    public void ShowPropRpc(int proDataId)
    {
        if (isServer) return;
        propData = DataMgr.Instance.GetPropData(proDataId);
        ChangeProp(true);
    }

    public void ChangeProp(bool ifShow)
    {
        if (ifShow)
        {
            collider.enabled = true;
            spriteRenderer.enabled = true;
        }
        else
        {
            collider.enabled = false;
            spriteRenderer.enabled = false;
        }
    }
}
