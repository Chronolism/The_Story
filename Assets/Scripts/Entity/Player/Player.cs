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

    #region �¼��ӿ�
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
    /// ��ʼ��Player
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
    /// ��ӵ���
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
    /// ʹ�õ���
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
        //�Ƿ�������
        isMoving = (inputX != 0) ||( inputY != 0);

        //ʱ�̼���Լ�λ�ã���������Ƭλ�ø���ʱ���ж��Ƿ�ΪĿ���λ������Ƭ
        //���� ˵�������Լ��ƶ����£���Ŀ���λ����Ϊ�Լ�λ��

        //��¼�Լ���Ƭλ��
        newTilePos.Init(rb.position);
        //����һ֡�ж�
        if (oldTilePos != newTilePos)
        {
            oldTilePos = newTilePos;
            //��Ŀ���λ��Ƭλ���ж�
            if (newTilePos != new V2(movement))
            {
                //������Ƭλ��
                movement = rb.position;
            }
            //���Ŀ���ĺ�����
            ChackMovement(ref movement);
        }
        //���뷽�����
        if (isMoving)
        {
            //�����ж�Ϊ��0123 ��Ӧ DWAS���������£�
            inputDir = Mathf.Abs(inputX) >= Mathf.Abs(inputY) ? inputX > 0 ? 0 : 2 : inputY > 0 ? 1 : 3;
            //�����뷽����ԭ���򲻷�ʱ�ж�
            if (dir != inputDir)
            {
                //��������ֱ�ӻ�ͷ��ת����Ŀ�����ת��
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
        //����ж�֪��ִ�Ŀ���
        if (Vector2.Dot(movement - rb.position, dirV2) > 0.01)
        {
            //��¼ÿһ֡����������Ա����ж�
            dirV2 = movement - rb.position;
            rb.AddForce((movement - rb.position).normalized * speed * 300);
        }
        else
        {
            //�ȼ�����뷽���Ƿ���ߣ��ټ���ƶ�����
            if (ChackMap(ref movement, inputDir))
            {
                //�������ƶ����
                dir = inputDir;
                dirV2 = movement - rb.position;
                rb.AddForce((movement - rb.position).normalized * speed * 300);
            }
            else if (ChackMap(ref movement, dir))
            {
                //�������ƶ����
                inputDir = dir;
                dirV2 = movement - rb.position;
                rb.AddForce((movement - rb.position).normalized * speed * 300);
            }
        }

    }
    /// <summary>
    /// ����ͼ�Ƿ���ƶ��������޸�Ŀ���λ
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
    /// ���Ŀ���λ�Ƿ�Ҫ��λ����Ƭ����
    /// ����Ŀ�������Ƭ���ĵ�ƫ�� ��������������Ƭ����������Ƿ��ͨ�У��������λ
    /// </summary>
    /// <param name="v2"></param>
    void ChackMovement(ref Vector2 v2)
    {
        switch (dir)
        {
            case 0:
                //�ж�������������Ƭ���������λ��ά�ȣ���һά�Ȳ��䣬�ܽ�����һ��
                if (AStarMgr.Instance.ChackType(v2.x + 1, v2.y, mapColliderType))
                {
                    //��¼��Ƭ��ά�����ĵ�
                    float i = Mathf.Floor(v2.y);
                    //�ж�����ƫ��
                    if (v2.y >= i + 0.5f) 
                    {
                        //����ͨ�����λ�����򲻶�
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
