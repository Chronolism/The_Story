using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : EntityState
{
    public override void Init(Entity entity)
    {
        base.Init(entity);
        ChangeState<NormalState>();
    }

    private void FixedUpdate()
    {
        if (!entity.ifPause && isServer)
        {
            if (entity.giddyTime > 0)
            {
                entity.giddyTime -= Time.deltaTime;
                ChangeState<GiddyState>();
            }
            else if (entity.ifInhibit)
            {
                ChangeState<InhibitState>();
            }
            else
            {
                ChangeState<NormalState>();
            }
            state.OnUpdata();
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
            foreach (var anim in entity.animators)
            {
                anim.speed = 1;
            }
        }
        public override void OnUpdata()
        {

        }
    }
    public class InhibitState : StateBase
    {
        Player player;
        public override void OnEnter(Entity entity)
        {
            if (player == null)
            {
                player = entity as Player;
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
