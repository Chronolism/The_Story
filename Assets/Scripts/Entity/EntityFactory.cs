using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class EntityFactory : BaseManager<EntityFactory>
{
    public List<NetworkIdentity> behaviours = new List<NetworkIdentity>();
    /// <summary>
    /// 清除网络实体
    /// </summary>
    public void ClearNetworkEntity()
    {
        foreach(NetworkIdentity identity in behaviours)
        {
            if(identity != null)
            {
                GameObject.Destroy(identity.gameObject);
            }
        }
        behaviours.Clear();
    }
    /// <summary>
    /// 生成使魔
    /// </summary>
    /// <param name="position">位置</param>
    /// <param name="ifPause">是否为暂停状态</param>
    /// <returns></returns>
    [Server]
    public Servitor CreatServitor(Vector3 position , bool ifPause = false)
    {
        GameObject servitorGb = GameObject.Instantiate(DataMgr.Instance.GetEntity(1002).gameObject);
        servitorGb.transform.position = position;
        Servitor servitor = servitorGb.GetComponent<Servitor>();
        servitor.ifPause = ifPause;
        NetworkServer.Spawn(servitorGb);
        behaviours.Add(servitor.netIdentity);
        return servitor;
    }
    /// <summary>
    /// 生成房间实体
    /// </summary>
    /// <returns></returns>
    [Server]
    public RoomData CreatRoomData()
    {
        GameObject roomDataGB = GameObject.Instantiate(DataMgr.Instance.GetEntity(1003).gameObject);
        RoomData roomData = roomDataGB.GetComponent<RoomData>();
        NetworkServer.Spawn(roomDataGB);
        return roomData;
    }
    /// <summary>
    /// 生成玩家
    /// </summary>
    /// <param name="user">玩家拥有者</param>
    /// <param name="position">位置</param>
    /// <returns></returns>
    [Server]
    public Player CreatPlayer(RoomUserData user, Vector3 position)
    {
        GameObject playerGB = GameObject.Instantiate(DataMgr.Instance.GetEntity(1001).gameObject);
        playerGB.transform.position = position;
        Player player = playerGB.GetComponent<Player>();
        CharacterData characterData = DataMgr.Instance.GetCharacter(user.characterId);
        if(characterData == null)
        {
            characterData = DataMgr.Instance.GetCharacter(101);
        }
        player.userName = user.name;
        NetworkServer.Spawn(playerGB);
        player.InitPlayer(characterData, user.skills);
        behaviours.Add(player.netIdentity);
        return player;
    }
    /// <summary>
    /// 生成道具底座
    /// </summary>
    /// <param name="position">位置</param>
    /// <returns></returns>
    [Server]
    public Prop CreatProp(Vector3 position)
    {
        GameObject propGB = GameObject.Instantiate(DataMgr.Instance.GetEntity(1004).gameObject);
        propGB.transform.position = position;
        Prop prop = propGB.GetComponent<Prop>();
        NetworkServer.Spawn(propGB);
        behaviours.Add(prop.netIdentity);
        return prop;
    }
    /// <summary>
    /// 生成攻击
    /// </summary>
    /// <param name="entity">攻击所有者</param>
    /// <param name="id">攻击id</param>
    /// <param name="pos">攻击位置</param>
    /// <param name="dir">攻击方向</param>
    /// <param name="floats">攻击额外参数</param>
    /// <returns></returns>
    [Server]
    public AttackBase CreatAttack(Entity entity , int id, Vector3 pos , Vector3 dir, List<float> floats = null)
    {
        AttackData attackData = DataMgr.Instance.GetAttackData(id);
        GameObject attackGB = GameObject.Instantiate(attackData.gameObject);
        AttackBase attack = attackGB.GetComponent<AttackBase>();
        attack.Init(entity, pos, dir, floats);
        attack.atkId = attackData.id;
        NetworkServer.Spawn(attackGB);
        behaviours.Add(attack.netIdentity);
        return attack;
    }
    /// <summary>
    /// 通用实体生成
    /// </summary>
    /// <typeparam name="T">泛型类型</typeparam>
    /// <param name="id">实体id</param>
    /// <param name="pos">实体位置</param>
    /// <returns></returns>
    [Server]
    public T CreatEntity<T>(int id , Vector3 pos)
    {
        GameObject gb = GameObject.Instantiate(DataMgr.Instance.GetEntity(id).gameObject);
        gb.transform.position = pos;
        NetworkServer.Spawn(gb);
        behaviours.Add(gb.GetComponent<NetworkIdentity>());
        return gb.GetComponent<T>();
    }
}
