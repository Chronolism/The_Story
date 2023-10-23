using Mirror;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : Entity
{

    public float inputX, inputY;
    private float fire1 , fire2;
    private int inputDir;

    private Vector2 movement;

    public PropData playerProp;

    [SyncVar]
    private bool isMoving;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animators = rb.GetComponentsInChildren<Animator>();
        this.transform.position = new Vector3(0.5f, 0.5f);
        movement = this.transform.position;
        if (isLocalPlayer)
        {
            DataMgr.Instance.activePlayer = this;
        }
        DataMgr.Instance.players.Add(netId, this);
        ChangeState<NormalState>();
    }

    private void Update()
    {
        if (isOwned)
        {
            if (!ifPause)
            {
                PlayerInput();
            }
            else
            {
                isMoving = false;
            }
        }
    }

    private void FixedUpdate()
    {
        if (!ifPause && isOwned)
        {
            state.OnUpdata();
        }
    }

    public override void OnDisable()
    {
        base.OnDisable();
        DataMgr.Instance.players.Remove(netId);
    }
    
    public void InitPlayer(CharacterData characterData)
    {

    }

    public bool AddProp(PropData prop)
    {
        if(playerProp == null)
        {
            playerProp = prop;
            return true;
        }
        return false;
    }

    private void PlayerInput()
    {
        inputX = 0;
        inputY = 0;
        fire1 = 0;
        fire2 = 0;
        if (IEnableInput.GetKey(E_PlayKeys.W)) inputY += 1;
        if (IEnableInput.GetKey(E_PlayKeys.S)) inputY += -1;
        if (IEnableInput.GetKey(E_PlayKeys.D)) inputX += 1;
        if (IEnableInput.GetKey(E_PlayKeys.A)) inputX += -1;
        if (IEnableInput.GetKey(E_PlayKeys.E)) fire1 = 1;
        if (IEnableInput.GetKey(E_PlayKeys.Q)) fire2 = 1;

    }

    private void UseProp()
    {
        if (fire2 > 0)
        {
            foreach (var i in playerProp.value)
            {
                DataMgr.Instance.GetBuff(i.buffId).OnTriger(this, i.buffValue);
            }
            playerProp = null;
        }
    }

    private void Movement()
    {
        
        isMoving = (inputX != 0) ||( inputY != 0); 

        if (isMoving)
        {
            inputDir = Mathf.Abs(inputX) >= Mathf.Abs(inputY) ? inputX > 0 ? 0 : 2 : inputY > 0 ? 1 : 3;
            if (dir != inputDir)
            {
                if ((Mathf.Abs(dir - inputDir) & 1) != 1)
                {
                    if (ChackMap(ref movement, inputDir))
                    {
                        dir = inputDir;
                    }
                }
            }
        }


        if (Vector2.Distance(rb.position, movement) > 0.01)
        {
            rb.MovePosition(rb.position + (movement - rb.position).normalized * maxSpeed * Time.deltaTime);
        }
        else
        {
            if (ChackMap(ref movement, inputDir))
            {
                dir = inputDir;
                rb.MovePosition(rb.position + (movement - rb.position).normalized * maxSpeed * Time.deltaTime);
            }
            else if (ChackMap(ref movement, dir))
            {
                inputDir = dir;
                rb.MovePosition(rb.position + (movement - rb.position).normalized * maxSpeed * Time.deltaTime);
            }
        }
    }

    bool ChackMap(ref Vector2 v2, int dir)
    {
        switch (dir)
        {
            case 0:
                if (AStarMgr.Instance.ChackType((int)Mathf.Floor(v2.x + 1), (int)Mathf.Floor(v2.y), E_Node_Type.Walk))
                {
                    v2.x += 1;
                    return true;
                }
                break;
            case 1:
                if (AStarMgr.Instance.ChackType((int)Mathf.Floor(v2.x), (int)Mathf.Floor(v2.y + 1), E_Node_Type.Walk))
                {
                    v2.y += 1;
                    return true;
                }
                break;
            case 2:
                if (AStarMgr.Instance.ChackType((int)Mathf.Floor(v2.x - 1), (int)Mathf.Floor(v2.y), E_Node_Type.Walk))
                {
                    v2.x -= 1;
                    return true;
                }
                break;
            case 3:
                if (AStarMgr.Instance.ChackType((int)Mathf.Floor(v2.x), (int)Mathf.Floor(v2.y) - 1, E_Node_Type.Walk))
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
            //if (isMoving)
            //{
            //    anim.SetInteger("dir", dir);
            //    anim.SetFloat("x", inputX);
            //    anim.SetFloat("y", inputY);
            //}
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
                anim.Play("idle");
            }

        }

        public override void OnExit(Entity entity)
        {

        }

        public override void OnUpdata()
        {
            player.Movement();
            player.SwitchAnimation();
        }
    }
}
