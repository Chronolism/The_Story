using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class EntityFactory : BaseManager<EntityFactory>
{
    public List<NetworkBehaviour> behaviours = new List<NetworkBehaviour>();
    /// <summary>
    /// �������ʵ��
    /// </summary>
    public void ClearNetworkEntity()
    {
        foreach(NetworkBehaviour behaviour in behaviours)
        {
            if(behaviour != null)
            {
                GameObject.Destroy(behaviour.gameObject);
            }
        }
        behaviours.Clear();
    }
    /// <summary>
    /// ����ʹħ
    /// </summary>
    /// <param name="position">λ��</param>
    /// <param name="ifPause">�Ƿ�Ϊ��ͣ״̬</param>
    /// <returns></returns>
    [Server]
    public Servitor CreatServitor(Vector3 position , bool ifPause = false)
    {
        GameObject servitorGb = ResMgr.Instance.Load<GameObject>("Servitor/ServitorNet");
        servitorGb.transform.position = position;
        Servitor servitor = servitorGb.GetComponent<Servitor>();
        servitor.ifPause = ifPause;
        NetworkServer.Spawn(servitorGb);
        behaviours.Add(servitor);
        return servitor;
    }
    /// <summary>
    /// ���ɷ���ʵ��
    /// </summary>
    /// <returns></returns>
    [Server]
    public RoomData CreatRoomData()
    {
        GameObject roomDataGB = ResMgr.Instance.Load<GameObject>("Prefab/RoomData");
        RoomData roomData = roomDataGB.GetComponent<RoomData>();
        NetworkServer.Spawn(roomDataGB);
        return roomData;
    }
    /// <summary>
    /// �������
    /// </summary>
    /// <param name="user">���ӵ����</param>
    /// <param name="position">λ��</param>
    /// <returns></returns>
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
        NetworkServer.Spawn(playerGB);
        player.InitPlayer(characterData, user.skills);
        behaviours.Add(player);
        return player;
    }
    /// <summary>
    /// ���ɵ��ߵ���
    /// </summary>
    /// <param name="position">λ��</param>
    /// <returns></returns>
    [Server]
    public Prop CreatProp(Vector3 position)
    {
        GameObject propGB = ResMgr.Instance.Load<GameObject>("Prefab/PropNet");
        propGB.transform.position = position;
        Prop prop = propGB.GetComponent<Prop>();
        NetworkServer.Spawn(propGB);
        behaviours.Add(prop);
        return prop;
    }
    /// <summary>
    /// ���ɹ���
    /// </summary>
    /// <param name="entity">����������</param>
    /// <param name="id">����id</param>
    /// <param name="pos">����λ��</param>
    /// <param name="dir">��������</param>
    /// <param name="floats">�����������</param>
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
        behaviours.Add(attack);
        return attack;
    }
}
