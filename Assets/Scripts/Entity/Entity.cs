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
    /// 实体buff组件
    /// </summary>
    public EntityBuff buff;
    /// <summary>
    /// 实体的buff列表
    /// </summary>
    public List<BuffBase> BuffList => buff.buffList;
    /// <summary>
    /// 实体使魔组件
    /// </summary>
    public EntityServitor entityServitor;
    /// <summary>
    /// 实体拥有的使魔列表
    /// </summary>
    public List<Servitor> Servitors => entityServitor.Servitors;
    /// <summary>
    /// 实体技能组件
    /// </summary>
    public EntitySkill skill;
    /// <summary>
    /// 实体的主要技能
    /// </summary>
    public BuffBase MainSkill => skill.mainSkill;
    /// <summary>
    /// 用户名字
    /// </summary>
    [SyncVar]
    public string userName;
    [SyncVar]
    public int dir;             //方向
    [SyncVar]
    public bool ifAtk = false;  //是否攻击
    [SyncVar]
    public bool ifDie = false;  //是否死亡
    [SyncVar]
    public bool ifGiddy = false;//是否眩晕
    public float giddyTime = 0; //眩晕时间
    [SyncVar]
    public bool ifPause = true; //是否暂停
    [Header("用户输入")]
    public float inputX, inputY;
    public float fire1, fire2;
    [Header("实体属性")]
    #region 属性
    [SyncVar]
    public float blood = 100;   //血量
    public float MaxBlood => maxBlood * maxBlood_Pre;   //最大血量
    [SyncVar]
    public float maxBlood = 100;//最大基础血量
    [SyncVar]
    public float maxBlood_Pre = 1;//血量倍率
    public float speed => maxSpeed * maxSpeed_Pre;      //速度
    [SyncVar]
    public float maxSpeed = 5;  //最大速度
    [SyncVar]
    public float maxSpeed_Pre = 1;//速度倍率
    [SyncVar]
    public float atk;           //攻击
    [SyncVar]
    public float atkpre;        //攻击倍率
    [SyncVar]
    public float inkAmount;     //墨水量
    [SyncVar]
    public float inkMaxAmount;  //最大墨水量
    [SyncVar]
    public float inkCost;       //改写消耗量
    [SyncVar]
    public float inkCostRate;   //改写消耗倍率
    [SyncVar]
    public float energyGet;     //能量获取数量
    [SyncVar]
    public float energyGetRate; //能量获取倍率

    public bool canRewrite => inkAmount > 0 && canTurn;
    /// <summary>
    /// 可以改写
    /// </summary>
    [SyncVar]
    public bool canTurn;
    /// <summary>
    /// 可以受伤
    /// </summary>
    [SyncVar]
    public bool canHurt;
    /// <summary>
    /// 可以拾取道具
    /// </summary>
    [SyncVar]
    public bool canPickProp;
    /// <summary>
    /// 可以获取能量
    /// </summary>
    [SyncVar]
    public bool canAddEnergy;
    /// <summary>
    /// 可以使用技能
    /// </summary>
    [SyncVar]
    public bool canUseSkill;

    #endregion
    #region 事件接口
    /// <summary>
    /// 获得墨水时触发
    /// InkData中 inkAmount：获取墨水量  ifTurn：是否获取
    /// </summary>
    public UnityAction<Entity, InkData> OnGetInk;
    public UnityAction<Entity, InkData> AfterGetInk;
    /// <summary>
    /// 添加移除buff时触发
    /// float :添加移除数量
    /// </summary>
    public UnityAction<Entity, BuffBase ,float> OnAddBuff;
    public UnityAction<Entity, BuffBase, float> OnRemoveBuff;
    /// <summary>
    /// 能改写目标时触发（不稳定）
    /// </summary>
    public UnityAction<Entity> OnHaveTurn;
    /// <summary>
    /// 不能改写目标时触发（不稳定）
    /// </summary>
    public UnityAction<Entity> OnRemoveTurn;
    /// <summary>
    /// 改写目标时触发
    /// 第二个entity为改写的实体
    /// InkData中 inkAmount：消耗墨水量  energyAmount：获取的能量  ifTurn：是否改写
    /// </summary>
    public UnityAction<Entity, Entity, InkData> BeforeReWrite;
    public UnityAction<Entity, Entity, InkData> OnReWrite;
    public UnityAction<Entity, Entity, InkData> AfterReWrite;
    /// <summary>
    /// 被改写拥有的实体时触发
    /// 第二个entity为被改写的实体
    /// 第三个entity为触发改写的实体
    /// /// InkData中 inkAmount：消耗墨水量  energyAmount：获取的能量  ifTurn：是否改写
    /// </summary>
    public UnityAction<Entity, Entity, Entity, InkData> BeforeReWrited;
    public UnityAction<Entity, Entity, Entity, InkData> OnReWrited;
    public UnityAction<Entity, Entity, Entity, InkData> AfterReWrited;
    /// <summary>
    /// 被改写时触发
    /// 第二个entity为触发改写的实体
    /// InkData中 inkAmount：消耗墨水量  energyAmount：获取的能量  ifTurn：是否改写
    /// </summary>
    public UnityAction<Entity, Entity, InkData> BeforeTurn;
    public UnityAction<Entity, Entity, InkData> OnTurn;
    public UnityAction<Entity, Entity, InkData> AfterTurn;
    /// <summary>
    /// 添加使魔时触发
    /// 第二个entity为被添加的实体
    /// </summary>
    public UnityAction<Entity, Entity> OnAddServitor;
    /// <summary>
    /// 移除使魔时触发
    /// 第二个entity为被移除的实体
    /// </summary>
    public UnityAction<Entity, Entity> OnRemoveServitor;
    /// <summary>
    /// 获得能量时触发
    /// InkData中 energyAmount：获取的能量  ifTurn：是否获取
    /// </summary>
    public UnityAction<Entity, InkData> OnGetEnergy;
    /// <summary>
    /// 触碰时触发
    /// 第二个entity为被接触目标的实体
    /// ATKData 攻击数据（攻击已经完成）
    /// </summary>
    public UnityAction<Entity, Entity, ATKData> OnTouchEntity;
    /// <summary>
    /// 进出地形触发
    /// TileData 地图数据
    /// </summary>
    public UnityAction<Entity, TileData> OnEnterTile;
    public UnityAction<Entity, TileData> OnLeaveTile;
    /// <summary>
    /// 攻击时触发
    /// 第二个entity为攻击的目标实体
    /// </summary>
    public UnityAction<Entity, Entity, ATKData> OnAtk;
    /// <summary>
    /// 被攻击时触发
    /// 第二个entity为触发攻击的目标实体
    /// </summary>
    public UnityAction<Entity, Entity, ATKData> OnAtked;
    /// <summary>
    /// 伤害实体时触发
    /// 第二个entity为伤害的目标实体
    /// </summary>
    public UnityAction<Entity, Entity, ATKData> OnToHurt;
    /// <summary>
    /// 受到伤害时触发
    /// 第二个entity为触发伤害的目标实体
    /// </summary>
    public UnityAction<Entity, Entity, ATKData> OnBeHurt;
    /// <summary>
    /// 使用道具时触发
    /// </summary>
    public UnityAction<Entity, PropData> OnUseProp;
    /// <summary>
    /// 使用技能时触发
    /// </summary>
    public UnityAction<Entity, BuffBase> OnUseSkill;

    private void EntityTouch(Entity target, ATKData atkData)
    {
        OnTouchEntity?.Invoke(this, target, atkData);
    }

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

    #region 网络行为



    #endregion

    public void TouchEntity(Entity target, ATKData atkData)
    {
        if(this is Player)
        {
            if (canRewrite)
            {
                if (target is Servitor servitor)
                {
                    this.RewriteServitor(servitor);
                }
                else
                {
                    AtkEntity(target, atkData);
                }
            }
            else
            {
                atkData.canAtk = false;
            }
            EntityTouch(target, atkData);
        }
        else if(this is Servitor servitor)
        {
            if (target != servitor.parent)
            {
                if (target.canRewrite)
                {
                    atkData.canAtk = false;
                }
                else
                {
                    AtkEntity(target, atkData);
                }
                EntityTouch(target, atkData);
            }
        }
    }

    /// <summary>
    /// 攻击实体
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
    /// 伤害实体
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
    /// 被目标眩晕
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
    /// 修改血量
    /// </summary>
    /// <param name="target"></param>
    /// <param name="atkData"></param>
    public void ChangeBlood(Entity target, ATKData atkData)
    {
        if(atkData.atkType == AtkType.atk)
        {
            blood -= atkData.AtkValue;
            blood = Mathf.Clamp(blood, 0, maxBlood);
            if (blood <= 0)
            {
                EntityDie();
            }
        }
        else if(atkData.atkType == AtkType.cure)
        {
            blood += atkData.AtkValue;
            blood = Mathf.Clamp(blood, 0, maxBlood);
            if (blood <= 0)
            {
                EntityDie();
            }
        }

    }
    /// <summary>
    /// 修改墨水量
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
    /// 添加能量
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
    /// 改写使魔
    /// </summary>
    /// <param name="servitor"></param>
    public void RewriteServitor(Servitor servitor , bool unconditional = false)
    {
        entityServitor?.RewriteServitor(servitor , unconditional);
    }
    /// <summary>
    /// 添加使魔（若无必要，请使用RewriteServitor）
    /// </summary>
    /// <param name="servitor"></param>
    public void AddServitor(Servitor servitor)
    {
        entityServitor?.AddServers(servitor);
    }
    /// <summary>
    /// 移除使魔
    /// </summary>
    /// <param name="servitor"></param>
    public void RemoveServitor(Servitor servitor)
    {
        entityServitor?.RemoveServers(servitor);
    }
    /// <summary>
    /// 清空使魔
    /// </summary>
    public void ClearServitor()
    {
        entityServitor?.ClearServer();
    }
    /// <summary>
    /// 实体死亡
    /// </summary>
    public virtual void EntityDie()
    {
        ifDie = true;
        Destroy(gameObject);
    }
    /// <summary>
    /// 添加buff
    /// </summary>
    /// <param name="buffId">buffid</param>
    /// <param name="value">buff附加值</param>
    /// <param name="own">施加buff的实体</param>
    public void AddBuff(int buffId, float value, Entity own)
    {
        buff?.AddBuff(buffId, value, own);
    }
    /// <summary>
    /// 添加持续buff
    /// </summary>
    /// <param name="buffId">buffid</param>
    /// <param name="value">buff附加值</param>
    /// <param name="time">持续时间</param>
    /// <param name="own">施加buff的实体</param>

    public void AddBuff(int buffId, float value ,float time, Entity own)
    {
        buff?.AddBuff(buffId, value, time, own);
    }
    /// <summary>
    /// 移除指定buffid的buff（若不知施加者，请先用FindBuff或FindBuffs找到，再用RemoveBuff(BuffBase buffBase)）
    /// </summary>
    /// <param name="buffId">buffid</param>
    /// <param name="value">数量</param>
    /// <param name="own">buff施加者</param>
    public void RemoveBuff(int buffId, float value, Entity own)
    { 
        buff?.RemoveBuff(buffId, value, own);
    }
    /// <summary>
    /// 移除值定buff
    /// </summary>
    /// <param name="buffBase"></param>
    public void RemoveBuff(BuffBase buffBase)
    {
        buff?.RemoveBuff(buffBase);
    }
    /// <summary>
    /// 移除指定buffid的buff（不安全，建议使用RemoveBuff(int buffId, float value, Entity own)）
    /// </summary>
    /// <param name="buffId"></param>
    /// <param name="value"></param>
    public void RemoveBuff(int buffId, float value)
    {
        buff?.RemoveBuff(buffId, value);
    }
    /// <summary>
    /// 找到指定id的buff
    /// </summary>
    /// <param name="buffId"></param>
    /// <returns></returns>
    public BuffBase FindBuff(int buffId)
    {
        return buff?.FindBuff(buffId);
    }
    /// <summary>
    /// 找到所有指定id的buff
    /// </summary>
    /// <param name="buffId"></param>
    /// <returns></returns>
    public List<BuffBase> FindBuffs(int buffId)
    {
        return buff?.FindBuffs(buffId);
    }
    /// <summary>
    /// 释放攻击实体
    /// </summary>
    /// <param name="type">攻击id</param>
    /// <param name="v3">攻击附加参数</param>
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
    /// 修改状态
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
