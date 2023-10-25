using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

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
        playerGB.transform.position = position;
        Player player = playerGB.GetComponent<Player>();
        CharacterData characterData = DataMgr.Instance.GetCharacter(user.characterId);
        if(characterData == null)
        {
            characterData = DataMgr.Instance.GetCharacter(101);
        }
        player.userName = user.name;
        player.InitPlayer(characterData , user.skills);
        NetworkServer.Spawn(playerGB);
        return player;
    }

    [Server]
    public Prop CreatProp(Vector3 position)
    {
        GameObject propGB = ResMgr.Instance.Load<GameObject>("Prefab/PropNet");
        propGB.transform.position = position;
        Prop prop = propGB.GetComponent<Prop>();
        NetworkServer.Spawn(propGB);
        return prop;
    }
}
