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

    [SyncVar]
    public string userName;

    public int dir;
    public bool ifAtk = false;
    public bool ifDie = false;
    [SyncVar]
    public bool ifPause = true;

    public Dictionary<Type, StateBase> stateDic = new Dictionary<Type, StateBase>();
    
    public EntityBuff buff;
    public List<BuffBase> buffList => buff.buffList;

    public float inputX, inputY;
    public float fire1, fire2;

    #region 属性
    [SyncVar]
    public float blood = 100;
    [SyncVar]
    public float maxBlood = 100;
    [SyncVar]
    public float maxBlood_Pre = 1;
    public float speed => maxSpeed * maxSpeed_Pre;
    [SyncVar]
    public float maxSpeed = 5;
    [SyncVar]
    public float maxSpeed_Pre = 1;
    [SyncVar]
    public float atk;
    [SyncVar]
    public float atkpre;
    #endregion
    #region 事件接口



    #endregion

    public virtual void Awake()
    {
        
    }

    #region 网络行为



    #endregion


    public void AddBuff(int buffId, float value, Entity own)
    {
        buff.AddBuff(buffId, value, own);
    }

    public void AddBuff(int buffId, float value ,float time, Entity own)
    {
        buff.AddBuff(buffId, value, time, own);
    }

    public void RemoveBuff(int buffId, float value, Entity own)
    { 
        buff.RemoveBuff(buffId, value, own);
    }

    public void RemoveBuff(BuffBase buffBase)
    {
        buff.RemoveBuff(buffBase);
    }

    public void RemoveBuff(int buffId, float value)
    {
        buff.RemoveBuff(buffId, value);
    }

    public BuffBase FindBuff(int buffId)
    {
        return buff.FindBuff(buffId);
    }

    public List<BuffBase> FindBuffs(int buffId)
    {
        return buff.FindBuffs(buffId);
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

    public virtual void OnEnable()
    {
        EventMgr.PauseGame += PauseGame;
        EventMgr.StartGame += StartGame;
    }
    public virtual void OnDisable()
    {
        EventMgr.PauseGame -= PauseGame;
        EventMgr.StartGame -= StartGame;
    }

    void PauseGame()
    {
        ifPause = true;
    }
    void StartGame()
    {
        ifPause = false;
    }

}
