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
        DataMgr.Instance.players.Add(netId, this);
        ChangeState<NormalState>();
        
    }

    private void Update()
    {
        if (ifPause)
            isMoving = false;
        Light.SetActive(canRewrite);
    }

    private void FixedUpdate()
    {
        if (!ifPause&&isServer)
        {
            if(giddyTime > 0)
            {
                giddyTime -= Time.deltaTime;
            }
            if (giddyTime > 0)
            {
                ChangeState<GiddyState>();
            }
            else
            {
                ChangeState<NormalState>();
            }
            state.OnUpdata();
        }
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
            spriteRenderer.enabled =  Mathf.Sin(invincibleFrameTime * 4 * Mathf.PI) > 0 ? true : false;
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

    /// <summary>
    /// 使用道具
    /// </summary>
    private void UseProp()
    {
        if (fire2 > 0 && playerProp != null && playerProp.id != 0) 
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
        if(fire1 > 0&& skill != null)
        {
            skill.Triger();
        }
    }
    [ClientRpc]
    public void UsePropRpc()
    {
        playerProp = null;
    }

    Vector2 dirV2;
    private void Movement()
    {
        
        isMoving = (inputX != 0) ||( inputY != 0); 

        if (isMoving)
        {
            inputDir = Mathf.Abs(inputX) >= Mathf.Abs(inputY) ? inputX > 0 ? 0 : 2 : inputY > 0 ? 1 : 3;
            if (dir != inputDir)
            {
                if (Mathf.Abs(dir - inputDir) == 2)
                {
                    if (ChackMap(ref movement, inputDir))
                    {
                        dirV2 = movement - rb.position;
                        dir = inputDir;
                    }
                }
            }
            if (Vector2.Dot(movement - rb.position, dirV2) > 0.01)
            {
                dirV2 = movement - rb.position;
                rb.MovePosition(rb.position + (movement - rb.position).normalized * speed * Time.deltaTime);
            }
            else
            {
                if (ChackMap(ref movement, inputDir))
                {
                    dir = inputDir;
                    dirV2 = movement - rb.position;
                    rb.MovePosition(rb.position + (movement - rb.position).normalized * speed * Time.deltaTime);
                }
                else if (ChackMap(ref movement, dir))
                {
                    inputDir = dir;
                    dirV2 = movement - rb.position;
                    rb.MovePosition(rb.position + (movement - rb.position).normalized * speed * Time.deltaTime);
                }
            }
        }
    }

    bool ChackMap(ref Vector2 v2, int dir)
    {
        switch (dir)
        {
            case 0:
                if (AStarMgr.Instance.ChackType(v2.x + 1, v2.y, E_Node_Type.Walk))
                {
                    v2.x += 1;
                    return true;
                }
                break;
            case 1:
                if (AStarMgr.Instance.ChackType(v2.x, v2.y + 1, E_Node_Type.Walk))
                {
                    v2.y += 1;
                    return true;
                }
                break;
            case 2:
                if (AStarMgr.Instance.ChackType(v2.x - 1, v2.y, E_Node_Type.Walk))
                {
                    v2.x -= 1;
                    return true;
                }
                break;
            case 3:
                if (AStarMgr.Instance.ChackType(v2.x, v2.y - 1, E_Node_Type.Walk))
                {
                    v2.y -= 1;
                    return true;
                }
                break;
        }
        return false;
    }

    private void SwitchAnimation()
    {
        foreach (var anim in animators)
        {
            //anim.SetFloat("speed", speedper);
            if (isMoving)
            {
                anim.SetFloat("dir", dir);
                anim.SetFloat("x", inputX);
                anim.SetFloat("y", inputY);
            }
        }
    }


    public class NormalState : StateBase
    {
        Player player;
        public override void OnEnter(Entity entity)
        {
            if (player == null)
            {
                player = entity as Player;
            }
            foreach (var anim in entity.animators)
            {
                anim.speed = 1;
                //anim.Play("idle");
            }
        }
        public override void OnExit(Entity entity)
        {

        }
        public override void OnUpdata()
        {
            player.UseProp();
            player.Movement();
            player.SwitchAnimation();
        }
    }

    public class GiddyState : StateBase
    {
        Player player;
        public override void OnEnter(Entity entity)
        {
            if (player == null)
            {
                player = entity as Player;
                foreach (var anim in entity.animators)
                {
                    anim.speed = 0;
                }
            }
        }
        public override void OnExit(Entity entity)
        {

        }
        public override void OnUpdata()
        {
            
        }
    }
}
