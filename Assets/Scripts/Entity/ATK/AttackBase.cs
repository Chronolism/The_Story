using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBase : MonoBehaviour
{
    public int id;
    bool ifServer;
    public Vector3 v3;
    public Entity perant;
    public int atkId;
    public float atk_pre;
    [Tooltip("攻击类型，若为rewrite则进入改写流程")]
    public AtkType atkType;
    [Tooltip("攻击持续时间，单位秒")]
    public float lifeTime;
    [Tooltip("是否持续攻击，应用攻击间隔")]
    public bool continuouslyEffective;
    [Tooltip("攻击间隔时间，单位毫秒")]
    public int atkTime;
    List<AtkEntity> entities = new List<AtkEntity>();
    Entity targer;

    public virtual void Init(Entity entity ,Vector3 vector3)
    {
        perant = entity;
        ifServer = NetworkServer.active;
        v3 = vector3;
        lifeTime = 99999;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (ifServer)
        {
            targer = collision.GetComponent<Entity>();
            if (targer != null)
            {
                entities.Add(new AtkEntity(DateTime.Now.Ticks/10000, targer));
                Attack(targer);
            }
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (ifServer)
        {
            targer = collision.GetComponent<Entity>();
            if (targer != null)
            {
                for(int i = 0; i < entities.Count; i++)
                {
                    if (entities[i].entity == targer)
                    {
                        entities.RemoveAt(i);
                        break;
                    }
                }
                
            }
        }
    }
    /// <summary>
    /// 攻击目标
    /// </summary>
    /// <param name="entity"></param>
    public virtual void Attack(Entity entity)
    {

    }
    Stack<AtkEntity> stack = new Stack<AtkEntity>();
    public virtual void FixedUpdate()
    {
        if (!perant.ifPause)
        {
            if (continuouslyEffective && ifServer && entities.Count > 0)
            {
                foreach (var i in entities)
                {
                    if ((DateTime.Now.Ticks / 10000 - i.time) > atkTime)
                    {
                        if (i.entity == null) {
                            stack.Push(i);
                            continue;
                        } 
                        Attack(i.entity);
                        i.time = DateTime.Now.Ticks / 10000;
                        if (perant.ifDie) return;
                    }
                }
                while(stack.Count > 0)
                {
                    entities.Remove(stack.Pop());
                }
            }
            lifeTime -= Time.deltaTime;
            if (lifeTime <= 0)
            {
                StopAttack();
            }
        }
    }
    /// <summary>
    /// 动画开始攻击调用
    /// </summary>
    public virtual void StartAttack() { }
    /// <summary>
    /// 结束攻击调用
    /// </summary>
    public virtual void StopAttack() 
    {
        PoolMgr.Instance.PushObj("Prefab/Attack/" + id, this.gameObject);
    }
}

public class AtkEntity
{
    public long time;
    public Entity entity;
    public AtkEntity(long time, Entity entity)
    {
        this.time = time;
        this.entity = entity;
    }
}