using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.Events;
using System;

public class EntityBuff : EntityComponent
{

    UnityAction<Entity> UpdataBuff;
    public List<BuffBase> buffList = new List<BuffBase>();
    //快慢表，减少延时buff消耗
    List<BuffBase> fastBuffList = new List<BuffBase>();
    List<BuffBase> slowBuffList = new List<BuffBase>();
    Stack<BuffBase> waitRemove = new Stack<BuffBase>();
    float slowTime;

    public virtual void FixedUpdate()
    {
        if (!isServer||entity.ifPause) return;
        
        UpdataBuff?.Invoke(entity);
        //快表每帧更新，有内容就遍历迭代
        if (fastBuffList.Count > 0)
        {
            foreach (BuffBase buff in fastBuffList)
            {
                buff.time -= Time.deltaTime;
                if (buff.time <= 0)
                {
                    //buff时间归零就移除
                    RemoveBuff(buff,buff.temporaryAmount);
                    buff.temporaryAmount = 0;
                    waitRemove.Push(buff);
                }
                else if (buff.time > 0.5f)
                {
                    //大于快表限制，返回慢表
                    waitRemove.Push(buff);
                    slowBuffList.Add(buff);
                }
            }
            while (waitRemove.Count > 0)
            {
                //堆栈移除，请不要在迭代过程中移除列表元素
                fastBuffList.Remove(waitRemove.Pop());
            }
        }
        //慢表0.5s更新
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
                    //低于慢表阈值，进入快表
                    waitRemove.Push(buff);
                    fastBuffList.Add(buff);
                }
            }
            while (waitRemove.Count > 0)
            {
                //堆栈移除，请不要在迭代过程中移除列表元素
                slowBuffList.Remove(waitRemove.Pop());
            }
        }


    }
    /// <summary>
    /// 添加buff
    /// </summary>
    /// <param name="buffId">buffid</param>
    /// <param name="value">buff附加值</param>
    /// <param name="own">施加buff的实体</param>
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
            buffBase.Init(entity,buffName, value, own);
            buffBase.OnStart(entity, value);
            buffBase.OnAdd(entity, value);
            if (buffBase is IUpdataBuff updatabuff)
            {
                UpdataBuff += updatabuff.Updata;
            }
        }
        buffBase.OnAddEffect(entity, value);
        entity.OnAddBuff?.Invoke(entity, buffBase, value);
        AddBuffRpc(buffId, value, own.netId);
    }
    /// <summary>
    /// 添加持续buff
    /// </summary>
    /// <param name="buffId">buffid</param>
    /// <param name="value">buff附加值</param>
    /// <param name="time">持续时间</param>
    /// <param name="own">施加buff的实体</param>
    [Server]
    public void AddBuff(int buffId, float value , float time, Entity own)
    {
        string buffName = buffId.ToString() + own.netId.ToString();
        BuffBase buffBase = buffList.Find(i => i.BuffID == buffName);
        if (buffBase != null)
        {
            buffBase.Amount += value;
            buffBase.temporaryAmount += value;
            buffBase.time = Mathf.Max(buffBase.time, time);
            buffBase.OnAdd(entity, value);
        }
        else
        {
            buffBase = DataMgr.Instance.GetBuff(buffId);
            buffList.Add(buffBase);
            buffBase.Init(entity,buffName, value, own);
            buffBase.time = time;
            buffBase.temporaryAmount += value;
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
        buffBase.OnAddEffect(entity, value);
        entity.OnAddBuff?.Invoke(entity, buffBase, value);
        AddBuffRpc(buffId, value, time, own.netId);
    }
    /// <summary>
    /// 移除指定buffid的buff（若不知施加者，请先用FindBuff或FindBuffs找到，再用RemoveBuff(BuffBase buffBase)）
    /// </summary>
    /// <param name="buffId">buffid</param>
    /// <param name="value">数量</param>
    /// <param name="own">buff施加者</param>
    [Server]
    public void RemoveBuff(int buffId, float value,Entity own)
    {
        string buffName = buffId.ToString() + own.netId.ToString();
        BuffBase buffBase = buffList.Find(i => i.BuffID == buffName);
        if (buffBase != null)
        {
            buffBase.OnRemove(entity,Mathf.Min(buffBase.Amount, value));
            if (buffBase.Amount <= value)
            {
                buffBase.OnEnd(entity, Mathf.Min(buffBase.Amount, value));
                if (buffBase is IUpdataBuff updataBuff)
                {
                    UpdataBuff -= updataBuff.Updata;
                }
                buffBase.OnRemoveEffect(entity, buffBase.Amount);
                buffList.Remove(buffBase);
                RemoveBuffRpc(buffId, own.netId);
            }
            else
            {
                buffBase.Amount -= value;
                RemoveBuffRpc(buffId, value , own.netId);
            }
            entity.OnRemoveBuff?.Invoke(entity, buffBase, value);
        }
    }
    /// <summary>
    /// 移除指定buffid的buff（不安全，建议使用RemoveBuff(int buffId, float value, Entity own)）
    /// </summary>
    /// <param name="buffId"></param>
    /// <param name="value"></param>
    [Server]
    public void RemoveBuff(int buffId, float value)
    {
        BuffBase buffBase = buffList.Find(i => i.buffData.id == buffId);
        if (buffBase != null)
        {
            buffBase.OnRemove(entity, Mathf.Min(buffBase.Amount, value));
            if (buffBase.Amount <= value)
            {
                buffBase.OnEnd(entity, Mathf.Min(buffBase.Amount, value));
                if (buffBase is IUpdataBuff updataBuff)
                {
                    UpdataBuff -= updataBuff.Updata;
                }
                buffList.Remove(buffBase);
                buffBase.OnRemoveEffect(entity, buffBase.Amount);
                RemoveBuffRpc(buffId, buffBase.buffOwn.netId);
            }
            else
            {
                buffBase.Amount -= value;
                RemoveBuffRpc(buffId, value);
            }
            entity.OnRemoveBuff?.Invoke(entity, buffBase, value);
        }
    }
    /// <summary>
    /// 移除指定buff指定数量
    /// </summary>
    /// <param name="buffBase"></param>
    /// <param name="value">为-1则全部移除</param>
    [Server]
    public void RemoveBuff(BuffBase buffBase , float value = -1)
    {
        if (buffBase != null)
        {
            if(value == -1)
            {
                buffBase.OnRemove(entity, buffBase.Amount);
                buffBase.OnEnd(entity, buffBase.Amount);
                if (buffBase is IUpdataBuff updataBuff)
                {
                    UpdataBuff -= updataBuff.Updata;
                }
                buffList.Remove(buffBase);
                buffBase.OnRemoveEffect(entity, value);
                entity.OnRemoveBuff?.Invoke(entity, buffBase, buffBase.Amount);
                RemoveBuffRpc(buffBase.buffData.id, buffBase.buffOwn.netId);
            }
            else
            {
                buffBase.OnRemove(entity, Mathf.Min(buffBase.Amount, value));
                if (buffBase.Amount <= value)
                {
                    buffBase.OnEnd(entity, Mathf.Min(buffBase.Amount, value));
                    if (buffBase is IUpdataBuff updataBuff)
                    {
                        UpdataBuff -= updataBuff.Updata;
                    }
                    buffBase.OnRemoveEffect(entity, buffBase.Amount);
                    buffList.Remove(buffBase);
                    RemoveBuffRpc(buffBase.buffData.id, buffBase.buffOwn.netId);
                }
                else
                {
                    buffBase.Amount -= value;
                    RemoveBuffRpc(buffBase.buffData.id, value, buffBase.buffOwn.netId);
                }
                entity.OnRemoveBuff?.Invoke(entity, buffBase, Mathf.Min(value, buffBase.Amount));
            }

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
            buffBase.Init(entity,buffName, value, Mirror.Utils.GetSpawnedInServerOrClient(netId).GetComponent<Entity>());
            
        }
        buffBase.OnAddEffect(entity, value);
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
            buffBase.temporaryAmount += value;
            buffBase.time += time;
        }
        else
        {
            buffBase = DataMgr.Instance.GetBuff(buffId);
            buffList.Add(buffBase);
            buffBase.Init(entity, buffName, value, Mirror.Utils.GetSpawnedInServerOrClient(netId).GetComponent<Entity>());
            buffBase.time = time;
            buffBase.temporaryAmount += value;
            if (buffBase.time <= 0.5)
            {
                fastBuffList.Add(buffBase);
            }
            else
            {
                slowBuffList.Add(buffBase);
            }
        }
        buffBase.OnAddEffect(entity, value);
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
                buffList[i].OnRemoveEffect(entity, buffList[i].Amount);
                buffList.RemoveAt(i);
                return;
            }
        }
    }
    /// <summary>
    /// 找到指定id的buff
    /// </summary>
    /// <param name="buffId"></param>
    /// <returns></returns>
    public BuffBase FindBuff(int buffId)
    {
        return buffList.Find(i => i.buffData.id == buffId);
    }
    /// <summary>
    /// 找到所有指定id的buff
    /// </summary>
    /// <param name="buffId"></param>
    /// <returns></returns>
    public List<BuffBase> FindBuffs(int buffId)
    {
        return buffList.FindAll(i => i.buffData.id == buffId);
    }
    /// <summary>
    /// 找到所有指定buff名称的buff
    /// </summary>
    /// <param name="buffId"></param>
    /// <returns></returns>
    public BuffBase FindBuff(string buffName)
    {
        return buffList.Find(i => i.BuffID == buffName);
    }

    [Server]
    public void BuffSetActive(BuffBase buffBase , bool active)
    {
        if (buffBase.active != active)
        {
            if (active)
            {
                buffBase.OnEnable(entity);
            }
            else
            {
                buffBase.OnDisabled(entity);
            }
            buffBase.active = active;
            BuffSetActiveRpc(buffBase.buffData.id, buffBase.buffOwn.netId, active);
        }
    }
    [ClientRpc]
    public void BuffSetActiveRpc(int buffId, uint netid, bool active)
    {
        string buffName = buffId.ToString() + netId.ToString();
        FindBuff(buffName).active = active;
    }
}
