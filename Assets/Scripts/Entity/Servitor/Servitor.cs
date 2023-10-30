using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Servitor : Entity
{
    public Vector2 movement;

    public Entity target;
    public Entity parent;


    [SyncVar]
    private bool isMoving;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animators = rb.GetComponentsInChildren<Animator>();
        this.transform.position = new Vector3(0.5f, 0.5f);
        movement = this.transform.position;
        ChangeState<NormalState>();
    }

    private void Update()
    {

    }

    private void FixedUpdate()
    {
        if (isServer&&!ifPause)
        {
            state.OnUpdata();
        }
    }

    public override void OnDisable()
    {
        base.OnDisable();
    }

    float time;
    private void FindTarget()
    {
        if (time >= 0.5)
        {
            time = 0;
            if (target == null || target == parent)
            {
                target = null;
                float distance = 999;
                foreach (var player in DataMgr.Instance.players)
                {
                    if (player.Value == parent) continue;
                    if (Vector3.Distance(transform.position, player.Value.transform.position) < distance)
                    {
                        distance = Vector3.Distance(transform.position, player.Value.transform.position);
                        target = player.Value;
                    }
                }
            }
            if(target != null) AStarMgr.Instance.FindPath(rb.position, target.rb.position, FindPathCallBack, false);
        }
        else
        {
            time += Time.deltaTime;
        }

    }

    List<AStarNode> path = new List<AStarNode>();
    int pathIndex = 0;
    bool havePath;
    void FindPathCallBack(List<AStarNode> aStarNodes)
    {
        if (aStarNodes == null||aStarNodes.Count == 0)
        {
            havePath = false;
            return;
        }

        path = aStarNodes;
        havePath = true;
        if (path.Count == 1)
        {
            movement = new Vector2(path[0].x, path[0].y);
            return;
        }

        if (Vector2.Distance(rb.position , path[1].pos) > Vector2.Distance(path[0].pos, path[1].pos))
        {
            pathIndex = 0;
            movement = path[0].pos;
        }
        else
        {
            pathIndex = 1;
            movement = path[1].pos;
        }

    }

    private void Movement()
    {

        if (Vector2.Distance(rb.position, movement) > 0.05)
        {
            rb.MovePosition(rb.position + (movement - rb.position).normalized * speed * Time.deltaTime);
        }
        else if(havePath)
        {
            if(pathIndex == path.Count - 1)
            {
                havePath = false;
            }
            else
            {
                pathIndex++;
                movement = path[pathIndex].pos;
                rb.MovePosition(rb.position + (movement - rb.position).normalized * speed * Time.deltaTime);
            }
        }
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
        Servitor servitor;
        public override void OnEnter(Entity entity)
        {
            if (servitor == null)
            {
                servitor = entity as Servitor;
            }
            foreach (var anim in servitor.animators)
            {
                anim.Play("idle");
            }

        }

        public override void OnExit(Entity entity)
        {

        }

        public override void OnUpdata()
        {
            servitor.FindTarget();
            servitor.Movement();
            servitor.SwitchAnimation();
        }
    }


}
