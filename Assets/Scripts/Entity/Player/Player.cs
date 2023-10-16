using Mirror;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : Entity
{

    private float inputX, inputY;
    private Vector2 movementInput;
    private float speedper;

    [SyncVar]
    private bool isMoving;
    [SyncVar]
    private bool inputDisable;

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

        if (Input.GetKey(KeyCode.LeftShift))
        {
            inputX = inputX * 0.5f;
            inputY = inputY * 0.5f;
        }
        movementInput = new Vector2(inputX, inputY);

        isMoving = movementInput != Vector2.zero;

        if (isMoving)
        {
            //if(Mathf.Abs(inputX)>= Mathf.Abs(inputY))
            //    dir = inputX > 0 ? 0 : 2;
            //else
            //    dir = inputY > 0 ? 1 : 3;
            dir = Mathf.Abs(inputX) >= Mathf.Abs(inputY) ? inputX > 0 ? 0 : 2 : inputY > 0 ? 1 : 3;
        }

        rb.MovePosition(rb.position + movementInput * maxSpeed * Time.deltaTime);
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
