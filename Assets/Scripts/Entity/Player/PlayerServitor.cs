using Mirror;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerServitor : NetworkBehaviour
{
    Player player;
    public List<Servitor> Servers = new List<Servitor>();

    private void Awake()
    {
        player = GetComponent<Player>();
    }
    [Server]
    public void AddServers(Servitor servitor)
    {
        Servers.Add(servitor);
        servitor.parent = player;
        servitor.GetComponent<SpriteRenderer>().color = Color.red;
    }
    [Server]
    public void RemoveServers(Servitor servitor)
    {
        Servers.Remove(servitor);
        servitor.parent = null;
    }
    [Server]
    public void ClearServer()
    {
        Servers.Clear();
    }
    [ClientRpc]
    public void AddServersRpc(uint netId)
    {
        if (isServer) return;
        Servitor servitor = Mirror.Utils.GetSpawnedInServerOrClient(netId).GetComponent<Servitor>();
        Servers.Add(servitor);
        servitor.parent = player;
    }
    [ClientRpc]
    public void RemoveServersRpc(uint netId)
    {
        if (isServer) return;
        Servitor servitor = Mirror.Utils.GetSpawnedInServerOrClient(netId).GetComponent<Servitor>();
        Servers.Remove(servitor);
        servitor.parent = null;
    }
    [ClientRpc]
    public void ClearServerRpc()
    {
        if (isServer) return;
        Servers.Clear();
    }
}
