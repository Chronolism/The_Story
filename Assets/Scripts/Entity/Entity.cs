using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;
using UnityEngine.Events;

public class Entity : NetworkBehaviour
{
    public Collider2D entityCollider;
    public Rigidbody2D rb;
    public Animator[] animators;
    protected StateBase state;
    private Dictionary<Type, StateBase> stateDic = new Dictionary<Type, StateBase>();
    /// <summary>
    /// ʵ��buff���
    /// </summary>
    public EntityBuff buff;
    /// <summary>
    /// ʵ���buff�б�
    /// </summary>
    public List<BuffBase> BuffList => buff.buffList;
    /// <summary>
    /// ʵ��ʹħ���
    /// </summary>
    public EntityServitor entityServitor;
    /// <summary>
    /// ʵ��ӵ�е�ʹħ�б�
    /// </summary>
    public List<Servitor> Servitors => entityServitor.Servitors;
    /// <summary>
    /// ʵ�弼�����
    /// </summary>
    public EntitySkill skill;
    /// <summary>
    /// ʵ�����Ҫ����
    /// </summary>
    public BuffBase MainSkill => skill.mainSkill;
    /// <summary>
    /// �û�����
    /// </summary>
    [SyncVar]
    public string userName;
    [SyncVar]
    public int dir;             //����
    [SyncVar]
    public bool ifAtk = false;  //�Ƿ񹥻�
    [SyncVar]
    public bool ifDie = false;  //�Ƿ�����
    [SyncVar]
    public bool ifGiddy = false;//�Ƿ�ѣ��
    public float giddyTime = 0;
    [SyncVar]
    public bool ifPause = true;
    [Header("�û�����")]
    public float inputX, inputY;
    public float fire1, fire2;
    [Header("ʵ������")]
    #region ����
    [SyncVar]
    public float blood = 100;
    public float MaxBlood => maxBlood * maxBlood_Pre;
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
    [SyncVar]
    public float inkAmount;
    [SyncVar]
    public float inkMaxAmount;
    [SyncVar]
    public float inkCost;
    [SyncVar]
    public float inkCostRate;
    [SyncVar]
    public float energyGet;
    [SyncVar]
    public float energyGetRate;

    public bool canRewrite => inkAmount > 0 && canTurn;
    /// <summary>
    /// ���Ը�д
    /// </summary>
    [SyncVar]
    public bool canTurn;
    /// <summary>
    /// ��������
    /// </summary>
    [SyncVar]
    public bool canHurt;
    /// <summary>
    /// ����ʰȡ����
    /// </summary>
    [SyncVar]
    public bool canPickProp;
    /// <summary>
    /// ���Ի�ȡ����
    /// </summary>
    [SyncVar]
    public bool canAddEnergy;
    /// <summary>
    /// ����ʹ�ü���
    /// </summary>
    [SyncVar]
    public bool canUseSkill;

    #endregion
    #region �¼��ӿ�
    /// <summary>
    /// ���īˮʱ����
    /// </summary>
    public UnityAction<Entity, InkData> OnGetInk;
    public UnityAction<Entity, InkData> AfterGetInk;

    public UnityAction<Entity, BuffBase ,float> OnAddBuff;

    public UnityAction<Entity, BuffBase, float> OnRemoveBuff;
    /// <summary>
    /// �ܸ�дĿ��ʱ����
    /// </summary>
    public UnityAction<Entity> OnHaveTurn;
    /// <summary>
    /// ���ܸ�дĿ��ʱ����
    /// </summary>
    public UnityAction<Entity> OnRemoveTurn;
    /// <summary>
    /// ��дĿ��ʱ����
    /// </summary>
    public UnityAction<Entity, Entity, InkData> BeforeReWrite;
    public UnityAction<Entity, Entity, InkData> OnReWrite;
    public UnityAction<Entity, Entity, InkData> AfterReWrite;
    /// <summary>
    /// ����дĿ��ʱ����
    /// </summary>
    public UnityAction<Entity, Entity, Entity, InkData> BeforeReWrited;
    public UnityAction<Entity, Entity, Entity, InkData> OnReWrited;
    public UnityAction<Entity, Entity, Entity, InkData> AfterReWrited;
    /// <summary>
    /// ����дʱ����
    /// </summary>
    public UnityAction<Entity, Entity, InkData> BeforeTurn;
    public UnityAction<Entity, Entity, InkData> OnTurn;
    public UnityAction<Entity, Entity, InkData> AfterTurn;
    /// <summary>
    /// ���ʹħʱ����
    /// </summary>
    public UnityAction<Entity, Entity> OnAddServitor;
    /// <summary>
    /// �Ƴ�ʹħʱ����
    /// </summary>
    public UnityAction<Entity, Entity> OnRemoveServitor;
    /// <summary>
    /// �������ʱ����
    /// </summary>
    public UnityAction<Entity, InkData> OnGetEnergy;
    /// <summary>
    /// ����ʱ����
    /// </summary>
    public UnityAction<Entity, Entity, ATKData> OnAtk;
    /// <summary>
    /// ������ʱ����
    /// </summary>
    public UnityAction<Entity, Entity, ATKData> OnAtked;
    /// <summary>
    /// �˺�ʵ��ʱ����
    /// </summary>
    public UnityAction<Entity, Entity, ATKData> OnToHurt;
    /// <summary>
    /// �ܵ��˺�ʱ����
    /// </summary>
    public UnityAction<Entity, Entity, ATKData> OnBeHurt;
    /// <summary>
    /// ʹ�õ���ʱ����
    /// </summary>
    public UnityAction<Entity, PropData> OnUseProp;
    /// <summary>
    /// ʹ�ü���ʱ����
    /// </summary>
    public UnityAction<Entity, BuffBase> OnUseSkill;

    private void Atk(Entity target , ATKData atkData)
    {
        OnAtk?.Invoke(this, target, atkData);
        if (atkData.canAtk)
            EntityAtk(target , atkData);
    }
    protected virtual void EntityAtk(Entity target, ATKData atkData) { }
    private void Atked(Entity target, ATKData atkData)
    {
        OnAtked?.Invoke(this, target, atkData);
        if (atkData.canAtk)
            EntityAtked(target , atkData);
    }
    protected virtual void EntityAtked(Entity target, ATKData atkData) { }
    private void ToHurt(Entity target , ATKData atkData)
    {
        OnToHurt?.Invoke(this, target, atkData);
        if (atkData.canAtk)
            EntityToHurt(target , atkData);
    }
    protected virtual void EntityToHurt(Entity target, ATKData atkData) { }
    private void BeHurt(Entity target, ATKData atkData)
    {
        if (!canHurt) atkData.canAtk = false;
        else
        {
            OnBeHurt?.Invoke(this, target, atkData);
            if(atkData.canAtk)
                EntityBeHurt(target, atkData);
        }
    }
    protected virtual void EntityBeHurt(Entity target, ATKData atkData) { }
    #endregion

    public virtual void Awake()
    {
        
    }

    #region ������Ϊ



    #endregion
    /// <summary>
    /// ����ʵ��
    /// </summary>
    /// <param name="target"></param>
    /// <param name="atkData"></param>
    public void AtkEntity(Entity target, ATKData atkData)
    {
        if (target.canRewrite) return;
        Atk(target, atkData);
        if (atkData.canAtk)
        {
            if (atkData.atkType == AtkType.rewrite && target is Servitor servitor)
            {
                RewriteServitor(servitor);
                return;
            }
            target.Atked(this, atkData);
            if (atkData.canAtk)
            {
                if (atkData.atkType == AtkType.rewrite && target is Servitor servitor1)
                {
                    RewriteServitor(servitor1);
                    return;
                }
                HurtEntity(target, atkData);
            }
        }
    }
    /// <summary>
    /// �˺�ʵ��
    /// </summary>
    /// <param name="target"></param>
    /// <param name="atkData"></param>
    public void HurtEntity(Entity target, ATKData atkData)
    {
        ToHurt(target, atkData);
        if (atkData.canAtk)
        {
            target.BeHurt(this, atkData);
            if (atkData.canAtk)
            {
                target.ChangeBlood(target, atkData);
            }
        }
    }
    /// <summary>
    /// ѣ��Ŀ��
    /// </summary>
    /// <param name="target"></param>
    /// <param name="time"></param>
    public void GiddyEntity(Entity target, float time)
    {
        giddyTime = giddyTime > time ? giddyTime : time;
        Giddy(time);
    }

    [ClientRpc]
    public void Giddy(float time)
    {
        if (isServer) return;
        giddyTime = giddyTime > time ? giddyTime : time;
    }
    /// <summary>
    /// �޸�Ѫ��
    /// </summary>
    /// <param name="target"></param>
    /// <param name="atkData"></param>
    public void ChangeBlood(Entity target, ATKData atkData)
    {
        if(atkData.atkType == AtkType.atk)
        {
            blood -= atkData.AtkValue;
            if (blood <= 0)
            {
                EntityDie();
            }
        }
        else if(atkData.atkType == AtkType.cure)
        {
            blood += atkData.AtkValue;
            if (blood <= 0)
            {
                EntityDie();
            }
        }

    }
    /// <summary>
    /// �޸�īˮ��
    /// </summary>
    /// <param name="value"></param>
    public void ChangeInkAmount(float value)
    {
        InkData inkData = new InkData(value, 0, true);
        OnGetInk?.Invoke(this, inkData);
        if (inkData.ifTurn)
        {
            if (inkAmount <= 0)
            {
                float i = inkAmount;
                inkAmount += inkData.inkAmount * inkCostRate;
                inkAmount = Mathf.Clamp(inkAmount, 0, inkMaxAmount);
                inkData.inkAmount = inkAmount - i;
                AfterGetInk?.Invoke(this, inkData);
                if (inkAmount > 0 && canTurn)
                {
                    
                    OnHaveTurn?.Invoke(this);
                }

            }
            if (inkAmount > 0)
            {
                float i = inkAmount;
                inkAmount += inkData.inkAmount * inkCostRate;
                inkAmount = Mathf.Clamp(inkAmount, 0, inkMaxAmount);
                inkData.inkAmount = inkAmount - i;
                AfterGetInk?.Invoke(this, inkData);
                if (inkAmount <= 0)
                {
                    inkAmount = 0;
                    if (canTurn)
                    {
                        OnRemoveTurn?.Invoke(this);
                    }
                }
            }
        }

    }
    /// <summary>
    /// �������
    /// </summary>
    /// <param name="value"></param>
    public void AddEnergy(InkData value)
    {
        skill?.AddEnergy(value);
    }

    public void BeTurn(Entity target, InkData inkData)
    {
        BeforeTurn?.Invoke(this,target, inkData);
        if (inkData.ifTurn) OnTurn?.Invoke(this, target, inkData);
        if(inkData.ifTurn) AfterTurn?.Invoke(this,target, inkData);
    }
    /// <summary>
    /// ��дʹħ
    /// </summary>
    /// <param name="servitor"></param>
    public void RewriteServitor(Servitor servitor , bool unconditional = false)
    {
        entityServitor?.RewriteServitor(servitor , unconditional);
    }
    /// <summary>
    /// ���ʹħ�����ޱ�Ҫ����ʹ��RewriteServitor��
    /// </summary>
    /// <param name="servitor"></param>
    public void AddServitor(Servitor servitor)
    {
        entityServitor?.AddServers(servitor);
    }
    /// <summary>
    /// �Ƴ�ʹħ
    /// </summary>
    /// <param name="servitor"></param>
    public void RemoveServitor(Servitor servitor)
    {
        entityServitor?.RemoveServers(servitor);
    }
    /// <summary>
    /// ���ʹħ
    /// </summary>
    public void ClearServitor()
    {
        entityServitor?.ClearServer();
    }
    /// <summary>
    /// ʵ������
    /// </summary>
    public virtual void EntityDie()
    {
        ifDie = true;
        Destroy(gameObject);
    }
    /// <summary>
    /// ���buff
    /// </summary>
    /// <param name="buffId">buffid</param>
    /// <param name="value">buff����ֵ</param>
    /// <param name="own">ʩ��buff��ʵ��</param>
    public void AddBuff(int buffId, float value, Entity own)
    {
        buff?.AddBuff(buffId, value, own);
    }
    /// <summary>
    /// ��ӳ���buff
    /// </summary>
    /// <param name="buffId">buffid</param>
    /// <param name="value">buff����ֵ</param>
    /// <param name="time">����ʱ��</param>
    /// <param name="own">ʩ��buff��ʵ��</param>

    public void AddBuff(int buffId, float value ,float time, Entity own)
    {
        buff?.AddBuff(buffId, value, time, own);
    }
    /// <summary>
    /// �Ƴ�ָ��buffid��buff������֪ʩ���ߣ�������FindBuff��FindBuffs�ҵ�������RemoveBuff(BuffBase buffBase)��
    /// </summary>
    /// <param name="buffId">buffid</param>
    /// <param name="value">����</param>
    /// <param name="own">buffʩ����</param>
    public void RemoveBuff(int buffId, float value, Entity own)
    { 
        buff?.RemoveBuff(buffId, value, own);
    }
    /// <summary>
    /// �Ƴ�ֵ��buff
    /// </summary>
    /// <param name="buffBase"></param>
    public void RemoveBuff(BuffBase buffBase)
    {
        buff?.RemoveBuff(buffBase);
    }
    /// <summary>
    /// �Ƴ�ָ��buffid��buff������ȫ������ʹ��RemoveBuff(int buffId, float value, Entity own)��
    /// </summary>
    /// <param name="buffId"></param>
    /// <param name="value"></param>
    public void RemoveBuff(int buffId, float value)
    {
        buff?.RemoveBuff(buffId, value);
    }
    /// <summary>
    /// �ҵ�ָ��id��buff
    /// </summary>
    /// <param name="buffId"></param>
    /// <returns></returns>
    public BuffBase FindBuff(int buffId)
    {
        return buff?.FindBuff(buffId);
    }
    /// <summary>
    /// �ҵ�����ָ��id��buff
    /// </summary>
    /// <param name="buffId"></param>
    /// <returns></returns>
    public List<BuffBase> FindBuffs(int buffId)
    {
        return buff?.FindBuffs(buffId);
    }
    /// <summary>
    /// �ͷŹ���ʵ��
    /// </summary>
    /// <param name="type">����id</param>
    /// <param name="v3">�������Ӳ���</param>
    //[Server]
    //public void Atttack(int type, Vector3 v3)
    //{
    //    //PoolMgr.Instance.GetObj("Prefab/Attack/" + type, (o) => { o.GetComponent<AttackBase>().Init(this, v3); });
    //    AtttackRpc(type, v3);
    //}
    //[ClientRpc]
    //private void AtttackRpc(int type, Vector3 v3)
    //{
    //    //PoolMgr.Instance.GetObj("Prefab/Attack/" + type, (o) => { o.GetComponent<AttackBase>().Init(this, v3); });
    //}
    /// <summary>
    /// �޸�״̬
    /// </summary>
    /// <typeparam name="T"></typeparam>
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
        EventMgr.ContinueGame += ContinueGame;
    }
    public virtual void OnDisable()
    {
        EventMgr.PauseGame -= PauseGame;
        EventMgr.ContinueGame -= ContinueGame;
    }

    void PauseGame()
    {
        ifPause = true;
    }
    void ContinueGame()
    {
        ifPause = false;
    }

}
