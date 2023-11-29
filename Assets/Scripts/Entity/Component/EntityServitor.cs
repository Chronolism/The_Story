using Mirror;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EntityServitor : EntityComponent
{
    public List<Servitor> Servitors = new List<Servitor>();

    #region 事件接口




    #endregion

    public void RewriteServitor(Servitor servitor , bool unconditional)
    {
        if (unconditional)
        {
            InkData inkData = new InkData(0, 0, true);
            servitor.BeTurn( entity, inkData);
            if (!inkData.ifTurn) return;
            //entity.ChangeInkAmount(-inkData.inkAmount);
            //entity.AddEnergy(inkData);
            servitor.parent?.RemoveServitor(servitor);
            AddServers(servitor);
        }
        else
        {
            if (servitor.parent != entity && entity.canTurn && entity.inkAmount > 0)
            {
                InkData inkData = new InkData(entity.inkCost, entity.energyGet, true);
                entity.OnReWrite?.Invoke(entity, servitor, inkData);
                if (servitor.parent != null && inkData.ifTurn)
                {
                    servitor.parent.OnReWrited?.Invoke(servitor.parent, servitor, entity, inkData);
                }
                if (inkData.ifTurn) servitor.BeTurn(entity, inkData);
                if (!inkData.ifTurn) return;
                entity.ChangeInkAmount(-inkData.inkAmount);
                entity.AddEnergy(inkData);
                servitor.parent?.RemoveServitor(servitor);
                AddServers(servitor);
            }
        }

    }

    [Server]
    public void AddServers(Servitor servitor)
    {
        Servitors.Add(servitor);
        servitor.parent = entity;
        entity.OnAddServitor?.Invoke(entity, servitor);
    }
    [Server]
    public void RemoveServers(Servitor servitor)
    {
        Servitors.Remove(servitor);
        servitor.parent = null;
        entity.OnRemoveServitor?.Invoke(entity, servitor);
    }
    [Server]
    public void ClearServer()
    {
        Servitors.Clear();
    }
    [ClientRpc]
    public void AddServersRpc(uint netId)
    {
        if (isServer) return;
        Servitor servitor = Mirror.Utils.GetSpawnedInServerOrClient(netId).GetComponent<Servitor>();
        Servitors.Add(servitor);
        servitor.parent = entity;
    }
    [ClientRpc]
    public void RemoveServersRpc(uint netId)
    {
        if (isServer) return;
        Servitor servitor = Mirror.Utils.GetSpawnedInServerOrClient(netId).GetComponent<Servitor>();
        Servitors.Remove(servitor);
        servitor.parent = null;
    }
    [ClientRpc]
    public void ClearServerRpc()
    {
        if (isServer) return;
        Servitors.Clear();
    }
}
