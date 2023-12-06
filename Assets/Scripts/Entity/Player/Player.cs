using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    public int inputDir;
    public Vector2 movement;
    public PropData playerProp;
    public SpriteRenderer spriteRenderer;
    public Animator bodyAnimator;
    public EntityInteractive entityInteractive;

    public GameObject Light;

    public List<Observer<Player>> observers = new List<Observer<Player>>();

    public CharacterData characterData;

    [SyncVar]
    public int characterCode;
    [SyncVar]
    private bool isMoving;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        entityInteractive = GetEntityComponent<EntityInteractive>();
        DataMgr.Instance.players.Add(netId, this);
    }

    private void Update()
    {
        if (ifPause)
            isMoving = false;
        Light.SetActive(canRewrite);
    }

    private void FixedUpdate()
    {
        
    }

    public override void OnDisable()
    {
        base.OnDisable();
        DataMgr.Instance.players.Remove(netId);
    }

    #region 事件接口
    [ClientRpc]
    protected override void EntityBeHurt(Entity target, ATKData atkData)
    {
        if (canShartInvincble)
        {
            StartCoroutine(InvincibleFrame());
        }
        else
        {
            invincibleFrameTime = 2;
        }
    }
    float invincibleFrameTime = 0;
    bool canShartInvincble = true;
    IEnumerator InvincibleFrame()
    {
        canShartInvincble = false;
        invincibleFrameTime = 2;
        canHurt = false;
        while (invincibleFrameTime > 0)
        {
            invincibleFrameTime -= Time.deltaTime;
            spriteRenderer.enabled =  Mathf.Sin(invincibleFrameTime * 8 * Mathf.PI) > 0 ? true : false;
            yield return null;
        }
        spriteRenderer.enabled = true;
        canHurt = true;
        canShartInvincble = true;
    }

    public override void EntityDie()
    {
        base.EntityDie();
        foreach (var i in observers)
        {
            i.ToUpdate(this);
        }
    }
    #endregion
    /// <summary>
    /// 初始化Player
    /// </summary>
    /// <param name="characterData"></param>
    /// <param name="skills"></param>
    public void InitPlayer(CharacterData characterData ,List<BuffDetile> skills)
    {
        characterCode = characterData.character_Code;
        this.characterData = characterData;
        blood = maxBlood = characterData.HP_Max;
        maxSpeed = characterData.Speed;
        atk = characterData.atkDamage;
        inkAmount = 0;
        inkMaxAmount = characterData.rewrite_ink_Max;
        inkCost = 20;
        inkCostRate = characterData.rewrite_ink_NeedRate;
        energyGet = 20;
        energyGetRate = 1;
        skill.InitSkill(skills, characterData.ultimate_Skill_Start, characterData.ultimate_Skill_Need);
        InitPlayerRpc(characterCode);
    }
    [ClientRpc]
    public void InitPlayerRpc(int characterCode)
    {
        if (!isServer)
        {
            this.characterCode = characterCode;
            this.characterData = DataMgr.Instance.GetCharacter(characterCode);
        }
        animators = rb.GetComponentsInChildren<Animator>();
        movement = this.transform.position;

        bodyAnimator.runtimeAnimatorController = characterData.controller;
    }
    /// <summary>
    /// 添加道具
    /// </summary>
    /// <param name="prop"></param>
    /// <returns></returns>
    public bool AddProp(PropData prop)
    {
        if (canPickProp && (playerProp == null || playerProp.id == 0))
        {
            playerProp = prop;
            AddPropRpc(prop.id);
            return true;
        }
        return false;
    }
    [ClientRpc]
    public void AddPropRpc(int id)
    {
        if (isServer) return;
        playerProp = DataMgr.Instance.GetPropData(id);
    }

    float oldfire1;
    float oldfire2;
    /// <summary>
    /// 使用道具
    /// </summary>
    public void UseProp()
    {
        if (oldfire2 == 0 && fire2 > 0 ) 
        {
            if(!entityInteractive.TrigerBuild() && playerProp != null && playerProp.id != 0)
            {
                foreach (var i in playerProp.value)
                {
                    DataMgr.Instance.GetBuff(i.buffId).OnTriger(this, i.buffValue);
                }
                PropData propData = playerProp;
                playerProp = null;
                UsePropRpc();
                OnUseProp?.Invoke(this, propData);
            }
        }
        oldfire2 = fire2;
        if (oldfire1 == 0 && fire1 > 0&& skill != null)
        {
            skill.Triger();
        }
        oldfire1 = fire1;
    }
    [ClientRpc]
    public void UsePropRpc()
    {
        playerProp = null;
    }

    Vector2 dirV2;
    V2 oldTilePos;
    V2 newTilePos;
    public void Movement()
    {
        //是否处于输入
        isMoving = (inputX != 0) ||( inputY != 0);

        //时刻检查自己位置，当所处瓦片位置更新时，判断是否为目标点位所在瓦片
        //不是 说明不是自己移动所致，将目标点位设置为自己位置

        //记录自己瓦片位置
        newTilePos.Init(rb.position);
        //与上一帧判断
        if (oldTilePos != newTilePos)
        {
            oldTilePos = newTilePos;
            //与目标点位瓦片位置判断
            if (newTilePos != new V2(movement))
            {
                //重置瓦片位置
                movement = rb.position;
            }
            //检查目标点的合理性
            ChackMovement(ref movement);
        }
        //输入方向更新
        if (isMoving)
        {
            //方向判断为：0123 对应 DWAS（左上右下）
            inputDir = Mathf.Abs(inputX) >= Mathf.Abs(inputY) ? inputX > 0 ? 0 : 2 : inputY > 0 ? 1 : 3;
            //当输入方向与原方向不符时判断
            if (dir != inputDir)
            {
                //反方向则直接回头，转向则到目标点再转向
                if (Mathf.Abs(dir - inputDir) == 2)
                {
                    if (ChackMap(ref movement, inputDir))
                    {
                        dirV2 = movement - rb.position;
                        dir = inputDir;
                    }
                }
            }
        }
        //点乘判断知否抵达目标点
        if (Vector2.Dot(movement - rb.position, dirV2) > 0.01)
        {
            //记录每一帧方向变量，以便点乘判断
            dirV2 = movement - rb.position;
            rb.AddForce((movement - rb.position).normalized * speed * 300);
        }
        else
        {
            //先检查输入方向是否可走，再检查移动方向
            if (ChackMap(ref movement, inputDir))
            {
                //将输入移动归等
                dir = inputDir;
                dirV2 = movement - rb.position;
                rb.AddForce((movement - rb.position).normalized * speed * 300);
            }
            else if (ChackMap(ref movement, dir))
            {
                //将输入移动归等
                inputDir = dir;
                dirV2 = movement - rb.position;
                rb.AddForce((movement - rb.position).normalized * speed * 300);
            }
        }

    }
    /// <summary>
    /// 检查地图是否可移动，是则修改目标点位
    /// </summary>
    /// <param name="v2"></param>
    /// <param name="dir"></param>
    /// <returns></returns>
    bool ChackMap(ref Vector2 v2, int dir)
    {
        switch (dir)
        {
            case 0:
                if (AStarMgr.Instance.ChackType(v2.x + 1, v2.y, mapColliderType))
                {
                    v2.x += 1;
                    return true;
                }
                break;
            case 1:
                if (AStarMgr.Instance.ChackType(v2.x, v2.y + 1, mapColliderType))
                {
                    v2.y += 1;
                    return true;
                }
                break;
            case 2:
                if (AStarMgr.Instance.ChackType(v2.x - 1, v2.y, mapColliderType))
                {
                    v2.x -= 1;
                    return true;
                }
                break;
            case 3:
                if (AStarMgr.Instance.ChackType(v2.x, v2.y - 1, mapColliderType))
                {
                    v2.y -= 1;
                    return true;
                }
                break;
        }
        return false;
    }
    /// <summary>
    /// 检查目标点位是否要归位到瓦片中心
    /// 根据目标点与瓦片中心的偏移 检查所朝反向的瓦片与其左或右是否可通行，不能则归位
    /// </summary>
    /// <param name="v2"></param>
    void ChackMovement(ref Vector2 v2)
    {
        switch (dir)
        {
            case 0:
                //判断先所朝方向瓦片，不能则归位改维度，另一维度不变，能进入下一步
                if (AStarMgr.Instance.ChackType(v2.x + 1, v2.y, mapColliderType))
                {
                    //记录瓦片该维度中心点
                    float i = Mathf.Floor(v2.y);
                    //判断左右偏移
                    if (v2.y >= i + 0.5f) 
                    {
                        //不可通行则归位，能则不动
                        if (!AStarMgr.Instance.ChackType(v2.x + 1, v2.y + 1, mapColliderType)) 
                        {
                            v2.y = i + 0.5f;
                            v2.x = Mathf.Floor(v2.x) + 0.5f;
                        }
                    }
                    else
                    {
                        if (AStarMgr.Instance.ChackType(v2.x + 1, v2.y - 1, mapColliderType))
                        {
                            v2.y = i + 0.5f;
                            v2.x = Mathf.Floor(v2.x) + 0.5f;
                        }
                    }
                }
                else
                {
                    v2.x = Mathf.Floor(v2.x) + 0.5f;
                }
                break;
            case 1:
                if (AStarMgr.Instance.ChackType(v2.x, v2.y + 1, mapColliderType))
                {
                    float i = Mathf.Floor(v2.x);
                    if (v2.x >= i + 0.5f)
                    {
                        if (!AStarMgr.Instance.ChackType(v2.x + 1, v2.y + 1, mapColliderType))
                        {
                            v2.x = i + 0.5f;
                            v2.y = Mathf.Floor(v2.y) + 0.5f;
                        }
                    }
                    else
                    {
                        if (AStarMgr.Instance.ChackType(v2.x - 1, v2.y + 1, mapColliderType))
                        {
                            v2.x = i + 0.5f;
                            v2.y = Mathf.Floor(v2.y) + 0.5f;
                        }
                    }
                }
                else
                {
                    v2.y = Mathf.Floor(v2.y) + 0.5f;
                }
                break;
            case 2:
                if (AStarMgr.Instance.ChackType(v2.x - 1, v2.y, mapColliderType))
                {
                    float i = Mathf.Floor(v2.y);
                    if (v2.y >= i + 0.5f)
                    {
                        if (!AStarMgr.Instance.ChackType(v2.x - 1, v2.y + 1, mapColliderType))
                        {
                            v2.y = i + 0.5f;
                            v2.x = Mathf.Floor(v2.x) + 0.5f;
                        }
                    }
                    else
                    {
                        if (AStarMgr.Instance.ChackType(v2.x - 1, v2.y - 1, mapColliderType))
                        {
                            v2.y = i + 0.5f;
                            v2.x = Mathf.Floor(v2.x) + 0.5f;
                        }
                    }
                }
                else
                {
                    v2.x = Mathf.Floor(v2.x) + 0.5f;
                }
                break;
            case 3:
                if (AStarMgr.Instance.ChackType(v2.x, v2.y - 1, mapColliderType))
                {
                    float i = Mathf.Floor(v2.x);
                    if (v2.x >= i + 0.5f)
                    {
                        if (!AStarMgr.Instance.ChackType(v2.x + 1, v2.y - 1, mapColliderType))
                        {
                            v2.x = i + 0.5f;
                            v2.y = Mathf.Floor(v2.y) + 0.5f;
                        }
                    }
                    else
                    {
                        if (AStarMgr.Instance.ChackType(v2.x - 1, v2.y - 1, mapColliderType))
                        {
                            v2.x = i + 0.5f;
                            v2.y = Mathf.Floor(v2.y) + 0.5f;
                        }
                    }
                }
                else
                {
                    v2.y = Mathf.Floor(v2.y) + 0.5f;
                }
                break;
        }

    }

    public void SwitchAnimation()
    {
        foreach (var anim in animators)
        {
            //anim.SetFloat("speed", speedper);
            anim.SetFloat("dir", dir);
            if (isMoving)
            {
                anim.SetFloat("x", inputX);
                anim.SetFloat("y", inputY);
            }
        }
    }
}
