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

    [Server]
    public Player CreatPlayer(RoomUserData user, Vector3 position)
    {
        GameObject playerGB = ResMgr.Instance.Load<GameObject>("Player/PlayerNet");
        Player player = playerGB.GetComponent<Player>();
        CharacterData characterData = DataMgr.Instance.GetCharacter(user.characterId);
        player.InitPlayer(characterData);
        NetworkServer.Spawn(playerGB, user.con);
        return player;
    }
}
