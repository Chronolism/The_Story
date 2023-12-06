using Mirror;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Prop : NetworkBehaviour,IEntityTouch
{
    public Collider2D propCollider;
    public SpriteRenderer spriteRenderer;
    public PropData propData;
    public int propid;
    public List<Observer<Prop>> observers = new List<Observer<Prop>>();


    public void Touch(Entity entity)
    {
        if (entity is Player player)
        {
            switch (propData.propType)
            {
                case PropType.TrigerTool:
                    foreach (var i in propData.value)
                    {
                        DataMgr.Instance.GetBuff(i.buffId).OnTriger(player, i.buffValue);
                    }
                    HideProp();
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (isServer)
        {
  
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
            propCollider.enabled = true;
            spriteRenderer.enabled = true;
            spriteRenderer.sprite = propData.icon;
            propid = propData.id;
        }
        else
        {
            propData = null;
            foreach (var i in observers)
            {
                i.ToUpdate(this);
            }
            propCollider.enabled = false;
            spriteRenderer.enabled = false;
        }
    }

}
