using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityFactory : BaseManager<EntityFactory>
{
    [Server]
    public Servitor CreatServitor(Vector3 position , bool ifPause = false)
    {
        GameObject servitorGb = ResMgr.Instance.Load<GameObject>("Servitor/ServitorNet");
        servitorGb.transform.position = position;
        Servitor servitor = servitorGb.GetComponent<Servitor>();
        servitor.ifPause = ifPause;
        NetworkServer.Spawn(servitorGb);
        return servitor;
    }

    [Server]
    public RoomData CreatRoomData()
    {
        GameObject roomDataGB = ResMgr.Instance.Load<GameObject>("Prefab/RoomData");
        RoomData roomData = roomDataGB.GetComponent<RoomData>();
        NetworkServer.Spawn(roomDataGB);
        return roomData;
    }
}
