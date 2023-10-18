using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;
using UnityEngine.Events;

public class Entity : NetworkBehaviour
{
    public Collider collider;
    public Rigidbody2D rb;
    public Animator[] animators;
    public StateBase state;

    public int dir;
    public bool ifAtk = false;
    public bool ifDie = false;

    public Dictionary<Type, StateBase> stateDic = new Dictionary<Type, StateBase>();

    public Dictionary<int, BuffBase> buffDic = new Dictionary<int, BuffBase>();
    #region 属性
    [SyncVar]
    public float blood = 100;
    [SyncVar]
    public float maxSpeed = 5;
    #endregion
    #region 事件接口
    public UnityAction<Entity, Entity, ATKData> BeforeAttack;
    public UnityAction<Entity, Entity, ATKData> OnAttack;
    public UnityAction<Entity, Entity, ATKData> AfterAttack;
    public UnityAction<Entity, Entity, ATKData> BeforeAttacked;
    public UnityAction<Entity, Entity, ATKData> OnAttacked;
    public UnityAction<Entity, Entity, ATKData> AfterAttacked;
    public UnityAction<Entity, Entity, ATKData> BeforeHurt;
    public UnityAction<Entity, Entity, ATKData> OnHurt;
    public UnityAction<Entity, Entity, ATKData> AfterHurt;
    public UnityAction<Entity, Entity, ATKData> BeforeCure;
    public UnityAction<Entity, Entity, ATKData> OnCure;
    public UnityAction<Entity, Entity, ATKData> AfterCure;
    public UnityAction<Entity, Entity, ATKData> BeforeCured;
    public UnityAction<Entity, Entity, ATKData> OnCured;
    public UnityAction<Entity, Entity, ATKData> AfterCured;
    public UnityAction<Entity, Entity, ATKData> BeforeCureBlood;
    public UnityAction<Entity, Entity, ATKData> OnCureBlood;
    public UnityAction<Entity, Entity, ATKData> AfterCureBlood;
    public UnityAction<Entity, Entity> BeforeKill;
    public UnityAction<Entity, Entity> OnKill;
    public UnityAction<Entity, Entity> AfterKill;
    public UnityAction<Entity, Entity, ATKData> BeforeDie;
    public UnityAction<Entity, Entity, ATKData> OnDie;
    public UnityAction<Entity, Entity, ATKData> AfterDie;
    public UnityAction<Entity, InventoryItem> OnPickUpItem;
    public UnityAction<Entity, InventoryItem> OnPickCollection;

    public void Atk(Entity target, ATKData atkData)
    {
        BeforeAttack?.Invoke(this, target, atkData);
        OnAttack?.Invoke(this, target, atkData);
        AfterAttack?.Invoke(this, target, atkData);
    }

    public void Atked(Entity target, ATKData atkData)
    {
        BeforeAttacked?.Invoke(this, target, atkData);
        OnAttacked?.Invoke(this, target, atkData);
        AfterAttacked?.Invoke(this, target, atkData);
    }

    public void Hurt(Entity target, ATKData atkData)
    {
        BeforeHurt?.Invoke(this, target, atkData);
        OnHurt?.Invoke(this, target, atkData);
        AfterHurt?.Invoke(this, target, atkData);
        ChangeBlood(atkData);
        if (blood <= 0)
        {
            if (Die(target, atkData))
            {
                target.Kill(this);
            }
        }
    }

    public void Kill(Entity target)
    {
        BeforeKill?.Invoke(this, target);
        OnKill?.Invoke(this, target);
        AfterKill?.Invoke(this, target);
    }

    public bool Die(Entity target, ATKData atkData)
    {
        ifDie = true;
        BeforeDie?.Invoke(this, target, atkData);
        OnDie?.Invoke(this, target, atkData);
        DieCommand();
        if (ifDie)
        {
            EntityDie();
            AfterDie?.Invoke(this, target, atkData);
        }
        return ifDie;
    }

    public void Cure(Entity target, ATKData atkData)
    {
        BeforeCure?.Invoke(this, target, atkData);
        OnCure?.Invoke(this, target, atkData);
        AfterCure?.Invoke(this, target, atkData);
    }

    public void Cured(Entity target, ATKData atkData)
    {
        BeforeCured?.Invoke(this, target, atkData);
        OnCured?.Invoke(this, target, atkData);
        AfterCured?.Invoke(this, target, atkData);
    }

    public void CureBlood(Entity target, ATKData atkData)
    {
        BeforeCureBlood?.Invoke(this, target, atkData);
        OnCureBlood?.Invoke(this, target, atkData);
        AfterCureBlood?.Invoke(this, target, atkData);
    }

    public void PickUpItem(InventoryItem item)
    {
        OnPickUpItem?.Invoke(this, item);
    }

    public void PickUpCollection(InventoryItem item)
    {
        OnPickCollection?.Invoke(this, item);
    }

    public virtual List<InventoryItem> GetDropItems(Entity target)
    {
        return new List<InventoryItem>();
    }
    #endregion

    #region 网络行为
    [Command(requiresAuthority = false)]
    public virtual void ChangeBlood(ATKData value)
    {
        blood += value.AtkValue;
        
    }

    [Command(requiresAuthority = false)]
    public virtual void CureBlood(ATKData value)
    {
        blood += value.AtkValue;
    }

    [Command(requiresAuthority = false)]
    public virtual void DieCommand()
    {

    }

    [Command(requiresAuthority = false)]
    public virtual void EntityDie()
    {

    }
    #endregion

    [Server]
    public void ChangeState(int buffId, int value, bool ifAdd)
    {
        if (ifAdd)
        {
            if (buffDic.ContainsKey(buffId))
            {
                buffDic[buffId].Amount += value;
                buffDic[buffId].OnAdd(this, value);
            }
            else
            {
                BuffBase buffBase = DataMgr.Instance.GetBuff(buffId);
                buffDic.Add(buffId, buffBase);
                buffBase.Amount += value;
                buffBase.OnStart(this, value);
                buffBase.OnAdd(this, value);
            }

        }
        else
        {
            if (buffDic.ContainsKey(buffId))
            {

                buffDic[buffId].OnRemove(this, value);
                buffDic[buffId].Amount -= value;
                if (buffDic[buffId].Amount <= 0)
                {
                    buffDic[buffId].OnEnd(this, value);
                    buffDic.Remove(buffId);
                }

            }
        }
        ChangeStateRpc(buffId, value, ifAdd);
    }
    [ClientRpc]
    public void ChangeStateRpc(int buffId, int value, bool ifAdd)
    {
        if (isServer) return;
        if (ifAdd)
        {
            if (buffDic.ContainsKey(buffId))
            {
                buffDic[buffId].Amount += value;
                buffDic[buffId].OnAdd(this, value);
            }
            else
            {
                BuffBase buffBase = DataMgr.Instance.GetBuff(buffId);
                buffDic.Add(buffId, buffBase);
                buffBase.Amount += value;
                buffBase.OnStart(this, value);
                buffBase.OnAdd(this, value);
            }

        }
        else
        {
            if (buffDic.ContainsKey(buffId))
            {

                buffDic[buffId].OnRemove(this, value);
                buffDic[buffId].Amount -= value;
                if (buffDic[buffId].Amount <= 0)
                {
                    buffDic[buffId].OnEnd(this, value);
                    buffDic.Remove(buffId);
                }

            }
        }
    }

    public void ChangeState<T>() where T : StateBase, new()
    {
        if (state == null || state.GetType() != typeof(T))
        {
            state?.OnExit(this);

            if (stateDic.ContainsKey(typeof(T)))
            {
                stateDic[typeof(T)].OnEnter(this);
            }
            else
            {
                StateBase state = new T();
                stateDic.Add(typeof(T), state);
                state.OnEnter(this);
            }

            state = stateDic[typeof(T)];
        }
    }

    public virtual void OnDisable()
    {
        
    }
}
