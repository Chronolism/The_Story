using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Servitor;

public class ServitorState : EntityState
{

    public override void Init(Entity entity)
    {
        base.Init(entity);
        ChangeState<NormalState>();
    }

    private void FixedUpdate()
    {
        if (isServer && !entity.ifPause)
        {
            if (entity.ifDie)
            {
                ChangeState<DieState>();
            }
            //if (ifBack)
            //{
            //    ChangeState<BackState>();
            //}
            //else
            //{
            //    ChangeState<NormalState>();
            //}
            state.OnUpdata();
        }
    }

    public class NormalState : StateBase
    {
        Servitor servitor;
        public override void OnEnter(Entity entity)
        {
            if (servitor == null)
            {
                servitor = entity as Servitor;
            }
            foreach (var anim in servitor.animators)
            {
                //anim.Play("idle");
            }

        }

        public override void OnExit(Entity entity)
        {

        }

        public override void OnUpdata()
        {
            servitor.FindTarget();
            servitor.Movement();
        }
    }

    public class BackState : StateBase
    {
        Servitor servitor;
        public override void OnEnter(Entity entity)
        {
            if (servitor == null)
            {
                servitor = entity as Servitor;
            }
            foreach (var anim in servitor.animators)
            {
                //anim.Play("idle");
            }

        }

        public override void OnExit(Entity entity)
        {

        }

        public override void OnUpdata()
        {
            servitor.BackFind();
            servitor.Movement();
        }
    }

    public class DieState : StateBase
    {
        Servitor servitor;
        public override void OnEnter(Entity entity)
        {
            if (servitor == null)
            {
                servitor = entity as Servitor;
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
