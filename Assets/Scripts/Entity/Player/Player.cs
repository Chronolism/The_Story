using Mirror;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : Entity
{

    private float inputX, inputY;
    private int inputDir;
    private Vector2 movementInput;

    private Vector2 movement;

    private float speedper;

    [SyncVar]
    private bool isMoving;
    [SyncVar]
    private bool inputDisable;

    private void Start()
    {
        movement = this.transform.position;
    }

    private void Update()
    {
        if (isLocalPlayer)
        {
            if (!inputDisable)
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
        if (!inputDisable && isLocalPlayer)
        {
            state.OnUpdata();
        }
    }


    private void PlayerInput()
    {
        inputX = Input.GetAxisRaw("Horizontal");
        inputY = Input.GetAxisRaw("Vertical");
    }
    private void Movement()
    {
        speedper = Mathf.Sqrt(inputX * inputX + inputY * inputY);
        if (speedper > 1)
        {
            inputX /= speedper;
            inputY /= speedper;
        }
        movementInput = new Vector2(inputX, inputY);

        isMoving = movementInput != Vector2.zero;

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


        if (Vector2.Distance(rb.position, movement) > 0.1)
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
