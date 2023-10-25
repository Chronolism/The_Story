using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.Events;
using System;

public class EntityBuff : NetworkBehaviour
{
    public Entity entity;

    UnityAction<Entity> UpdataBuff;
    public List<BuffBase> buffList = new List<BuffBase>();

    List<BuffBase> fastBuffList = new List<BuffBase>();
    List<BuffBase> slowBuffList = new List<BuffBase>();
    Stack<BuffBase> waitRemove = new Stack<BuffBase>();
    float slowTime;

    private void Awake()
    {
        Init(GetComponent<Entity>());
    }

    public virtual void FixedUpdate()
    {
        if (!isServer) return;
        UpdataBuff?.Invoke(entity);
        if (fastBuffList.Count > 0)
        {
            foreach (BuffBase buff in fastBuffList)
            {
                buff.time -= Time.deltaTime;
                if (buff.time <= 0)
                {
                    RemoveBuff(buff);
                    waitRemove.Push(buff);
                }
                else if (buff.time > 0.5f)
                {
                    waitRemove.Push(buff);
                    slowBuffList.Add(buff);
                }
            }
            while (waitRemove.Count > 0)
            {
                fastBuffList.Remove(waitRemove.Pop());
            }
        }
        if (slowTime > 0)
        {
            slowTime -= Time.deltaTime;
        }
        else
        {
            slowTime = 0.5f;
            foreach (BuffBase buff in slowBuffList)
            {
                buff.time -= 0.5f;
                if (buff.time <= 0.5f)
                {
                    waitRemove.Push(buff);
                    fastBuffList.Add(buff);
                }
            }
            while (waitRemove.Count > 0)
            {
                slowBuffList.Remove(waitRemove.Pop());
            }
        }


    }

    public void Init(Entity entity)
    {
        this.entity = entity;
    }

    [Server]
    public void AddBuff(int buffId, float value,Entity own)
    {
        string buffName = buffId.ToString() + own.netId.ToString();
        BuffBase buffBase = buffList.Find(i => i.BuffID == buffName);
        if (buffBase != null)
        {
            buffBase.Amount += value;
            buffBase.OnAdd(entity, value);
        }
        else
        {
            buffBase = DataMgr.Instance.GetBuff(buffId);
            buffList.Add(buffBase);
            buffBase.BuffID = buffName;
            buffBase.buffOwn = own;
            buffBase.Amount += value;
            buffBase.buffData = DataMgr.Instance.GetBuffData(buffId);
            buffBase.OnStart(entity, value);
            buffBase.OnAdd(entity, value);
            if (buffBase is IUpdataBuff updatabuff)
            {
                UpdataBuff += updatabuff.Updata;
            }
        }
        AddBuffRpc(buffId, value, own.netId);
    }
    [Server]
    public void AddBuff(int buffId, float value , float time, Entity own)
    {
        string buffName = buffId.ToString() + own.netId.ToString();
        BuffBase buffBase = buffList.Find(i => i.BuffID == buffName);
        if (buffBase != null)
        {
            buffBase.Amount += value;
            buffBase.time += time;
            buffBase.OnAdd(entity, value);
        }
        else
        {
            buffBase = DataMgr.Instance.GetBuff(buffId);
            buffList.Add(buffBase);
            buffBase.BuffID = buffName;
            buffBase.buffOwn = own;
            buffBase.Amount += value;
            buffBase.buffData = DataMgr.Instance.GetBuffData(buffId);
            buffBase.time = time;
            if (buffBase.time <= 0.5)
            {
                fastBuffList.Add(buffBase);
            }
            else
            {
                slowBuffList.Add(buffBase);
            }
            buffBase.OnStart(entity, value);
            buffBase.OnAdd(entity, value);
            if (buffBase is IUpdataBuff updatabuff)
            {
                UpdataBuff += updatabuff.Updata;
            }
        }
        AddBuffRpc(buffId, value, time, own.netId);
    }

    [Server]
    public void RemoveBuff(int buffId, float value,Entity own)
    {
        string buffName = buffId.ToString() + own.netId.ToString();
        BuffBase buffBase = buffList.Find(i => i.BuffID == buffName);
        if (buffBase != null)
        {
            buffBase.OnRemove(entity, value);
            buffBase.Amount -= value;
            if (buffBase.Amount <= 0)
            {
                buffBase.OnEnd(entity, value);
                if (buffBase is IUpdataBuff updataBuff)
                {
                    UpdataBuff -= updataBuff.Updata;
                }
                buffList.Remove(buffBase);
                RemoveBuffRpc(buffId, own.netId);
            }
            else
            {
                RemoveBuffRpc(buffId, value , own.netId);
            }
        }
    }
    [Server]
    public void RemoveBuff(int buffId, float value)
    {
        BuffBase buffBase = buffList.Find(i => i.buffData.id == buffId);
        if (buffBase != null)
        {
            buffBase.OnRemove(entity, value);
            buffBase.Amount -= value;
            if (buffBase.Amount <= 0)
            {
                buffBase.OnEnd(entity, value);
                if (buffBase is IUpdataBuff updataBuff)
                {
                    UpdataBuff -= updataBuff.Updata;
                }
                buffList.Remove(buffBase);
                RemoveBuffRpc(buffId, buffBase.buffOwn.netId);
            }
            else
            {
                RemoveBuffRpc(buffId, value);
            }
        }
    }

    [Server]
    public void RemoveBuff(BuffBase buffBase)
    {
        if (buffBase != null)
        {
            buffBase.OnRemove(entity, buffBase.Amount);
            buffBase.OnEnd(entity, buffBase.Amount);
            if (buffBase is IUpdataBuff updataBuff)
            {
                UpdataBuff -= updataBuff.Updata;
            }
            buffList.Remove(buffBase);
            RemoveBuffRpc(buffBase.buffData.id, buffBase.buffOwn.netId);
        }
    }

    [ClientRpc]
    public void AddBuffRpc(int buffId, float value, uint netId)
    {
        if (isServer) return;
        string buffName = buffId.ToString() + netId.ToString();
        BuffBase buffBase = buffList.Find(i => i.BuffID == buffName);
        if (buffBase != null)
        {
            buffBase.Amount += value;
        }
        else
        {
            buffBase = DataMgr.Instance.GetBuff(buffId);
            buffList.Add(buffBase);
            buffBase.BuffID = buffName;
            buffBase.buffOwn = Mirror.Utils.GetSpawnedInServerOrClient(netId).GetComponent<Entity>();
            buffBase.Amount += value;
            buffBase.buffData = DataMgr.Instance.GetBuffData(buffId);
        }
    }
    [ClientRpc]
    public void AddBuffRpc(int buffId, float value, float time, uint netId)
    {
        if (isServer) return;
        string buffName = buffId.ToString() + netId.ToString();
        BuffBase buffBase = buffList.Find(i => i.BuffID == buffName);
        if (buffBase != null)
        {
            buffBase.Amount += value;
            buffBase.time += time;
        }
        else
        {
            buffBase = DataMgr.Instance.GetBuff(buffId);
            buffList.Add(buffBase);
            buffBase.BuffID = buffName;
            buffBase.buffOwn = Mirror.Utils.GetSpawnedInServerOrClient(netId).GetComponent<Entity>();
            buffBase.Amount += value;
            buffBase.buffData = DataMgr.Instance.GetBuffData(buffId);
            buffBase.time = time;
            if (buffBase.time <= 0.5)
            {
                fastBuffList.Add(buffBase);
            }
            else
            {
                slowBuffList.Add(buffBase);
            }
        }
    }

    [ClientRpc]
    public void RemoveBuffRpc(int buffId, float value, uint netId)
    {
        if (isServer) return;
        string buffName = buffId.ToString() + netId.ToString();
        BuffBase buffBase = buffList.Find(i => i.BuffID == buffName);
        if (buffBase != null)
        {
            buffBase.Amount -= value;
        }
    }
    [ClientRpc]
    public void RemoveBuffRpc(int buffId, float value)
    {
        if (isServer) return;
        BuffBase buffBase = buffList.Find(i => i.buffData.id == buffId);
        if (buffBase != null)
        {
            buffBase.Amount -= value;
        }
    }

    [ClientRpc]
    public void RemoveBuffRpc(int buffId , uint netId)
    {
        if (isServer) return;
        string buffName = buffId.ToString() + netId.ToString();
        for(int i = 0; i < buffList.Count; i++)
        {
            if(buffList[i].BuffID == buffName)
            {
                buffList.RemoveAt(i);
                return;
            }
        }
    }

    public BuffBase FindBuff(int buffId)
    {
        return buffList.Find(i => i.buffData.id == buffId);
    }

    public List<BuffBase> FindBuffs(int buffId)
    {
        return buffList.FindAll(i => i.buffData.id == buffId);
    }
}
