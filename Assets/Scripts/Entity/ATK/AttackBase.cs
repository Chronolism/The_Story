using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBase : NetworkBehaviour
{
    public int id;
    public Vector3 v3;
    public List<float> floats;
    public Entity perant;
    public int atkId;
    public float atk_pre;
    [Tooltip("�������ͣ���Ϊrewrite������д����")]
    public AtkType atkType;
    [Tooltip("��������ʱ�䣬��λ��")]
    public float lifeTime;
    [Tooltip("�Ƿ����������Ӧ�ù������")]
    public bool continuouslyEffective;
    [Tooltip("�������ʱ�䣬��λ����")]
    public int atkTime;
    List<AtkEntity> onTrigerEntities = new List<AtkEntity>();
    List<AtkEntity> trigerEntities = new List<AtkEntity>();
    Entity targer;
    Stack<AtkEntity> stack = new Stack<AtkEntity>();
    public virtual void Init(Entity entity ,Vector3 pos , Vector3 dir , List<float> floats = null)
    {
        perant = entity;
        transform.position = pos;
        v3 = dir;
        this.floats = floats;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isServer)
        {
            targer = collision.GetComponent<Entity>();
            if (targer != null && targer != perant) 
            {
                AtkEntity atkEntity = trigerEntities.Find(i => i.entity == targer);
                if (atkEntity == null)
                {
                    atkEntity = new AtkEntity(DateTime.Now.Ticks / 10000, targer);
                    trigerEntities.Add(atkEntity);
                    onTrigerEntities.Add(atkEntity);
                    Attack(targer);
                }
                else
                {
                    onTrigerEntities.Add(atkEntity);
                }
                
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (isServer)
        {
            targer = collision.GetComponent<Entity>();
            if (targer != null && targer != perant)
            {
                for(int i = 0; i < onTrigerEntities.Count; i++)
                {
                    if (onTrigerEntities[i].entity == targer)
                    {
                        onTrigerEntities.RemoveAt(i);
                        break;
                    }
                }
                
            }
        }
    }

    public virtual void FixedUpdate()
    {
        if (!isServer) return;
        if (!perant.ifPause)
        {
            if (continuouslyEffective && onTrigerEntities.Count > 0)
            {
                foreach (var i in onTrigerEntities)
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
                    onTrigerEntities.Remove(stack.Pop());
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
    /// ����Ŀ��
    /// </summary>
    /// <param name="entity"></param>
    public virtual void Attack(Entity entity)
    {

    }
    /// <summary>
    /// ������ʼ��������
    /// </summary>
    public virtual void StartAttack() { }
    /// <summary>
    /// ������������
    /// </summary>
    public virtual void StopAttack() 
    {
        //PoolMgr.Instance.PushObj("Prefab/Attack/" + id, this.gameObject);
        Destroy(gameObject);
    }

    public static Quaternion LookAt2D(Vector3 start, Vector3 end)
    {
        return Quaternion.AngleAxis((Vector3.Cross(start.normalized, end.normalized).z > 0 ? 1 : -1) * Mathf.Acos(Vector3.Dot(start.normalized, end.normalized)) * Mathf.Rad2Deg, new Vector3(0, 0, 1));
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