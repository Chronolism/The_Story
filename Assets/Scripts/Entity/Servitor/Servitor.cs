using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Servitor : Entity
{
    public Vector2 movement;

    public Entity target;
    public Entity parent;

    private ServitorTurnAnimation turnAnimation;

    public Vector2 posAdd;
    public int addRate;

    public Vector2 bronPos;

    public bool ifBack;

    [SyncVar]
    private bool isMoving;

    public override void Awake()
    {
        base.Awake();
        turnAnimation = GetComponentInChildren<ServitorTurnAnimation>();
        rb = GetComponent<Rigidbody2D>();
        animators = rb.GetComponentsInChildren<Animator>();
        AfterTurn += (a, b, c) =>
        {
            TurnAnimation(b.netId);
        };
    }

    private void Start()
    {
        movement = this.transform.position;
        posAdd = Random.Range(0, 1f) > 0.5f ? new Vector2(Random.Range(-1, 2), 0) : new Vector3(0, Random.Range(-1, 2));
        addRate = Random.Range(1, 3);
        bronPos = this.transform.position;
    }
    [ClientRpc]
    public void TurnAnimation(uint netid)
    {
        if(Mirror.Utils.GetSpawnedInServerOrClient(netid).GetComponent<Entity>() is Player player)
        {
            if (player != DataMgr.Instance.activePlayer && player.characterCode == DataMgr.Instance.activePlayer.characterCode)
            {
                turnAnimation.StartTurn(DataMgr.Instance.RangeCharacter(player.characterCode).servitorController);
            }
            else
            {
                turnAnimation.StartTurn(player.characterData.servitorController);
            }
        }
    }

    private void Update()
    {

    }

    public override void OnDisable()
    {
        base.OnDisable();
    }

    float time;
    public void FindTarget()
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
            if(target != null)
            {
                //if (target.canRewrite)
                //{
                //    ifBack = true;
                //    return;
                //}
                for(int i = addRate; i >= 0; i--)
                {
                    if (AStarMgr.Instance.ChackType(target.transform.position.x + posAdd.x * i, target.transform.position.y + posAdd.y * i, mapColliderType))
                    {
                        AStarMgr.Instance.FindPath(rb.position, target.rb.position + posAdd * i, mapColliderType, FindPathCallBack, false);
                        break;
                    }
                }
            }
        }
        time += Time.deltaTime;

    }



    public void BackFind()
    {
        if (time >= 0.5)
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
            if(target != null )
            {
                if (!target.canRewrite)
                {
                    ifBack = false;
                    return;
                }
                time = 0;
                if (parent != null)
                {
                    AStarMgr.Instance.FindPath(rb.position, parent.rb.position, mapColliderType, FindPathCallBack, false);
                }
                else
                {
                    AStarMgr.Instance.FindPath(rb.position, bronPos, mapColliderType, FindPathCallBack, false);
                }
            }
        }
        time += Time.deltaTime;
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
            pathIndex = 0;
            movement = path[0].pos;
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

    public void Movement()
    {

        if (Vector2.Distance(rb.position, movement) > 0.05)
        {
            rb.AddForce((movement - rb.position).normalized * speed * 300);
            dir = (movement - rb.position).normalized.x > 0 ? 1 : -1;
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
                rb.AddForce((movement - rb.position).normalized * speed * 300);
                dir = (movement - rb.position).normalized.x > 0 ? 1 : -1;
            }
        }
    }


    //private void SwitchAnimation()
    //{
    //    foreach (var anim in animators)
    //    {
    //        //anim.SetFloat("speed", speedper);
    //        //if (isMoving)
    //        //{
    //        //    anim.SetInteger("dir", dir);
    //        //    anim.SetFloat("x", inputX);
    //        //    anim.SetFloat("y", inputY);
    //        //}
    //        anim.SetFloat("x", dir);
    //    }
    //}
    public void EatThis()
    {
        ifDie = true;
        EatThisRpc();
        turnAnimation.Eat();
    }
    [ClientRpc]
    private void EatThisRpc()
    {
        if (isServer) return;
        turnAnimation.Eat();
    }

    public override void EntityDie()
    {
        ifDie = true;
        EntityDieRpc();
        turnAnimation.Die();
    }
    [ClientRpc]
    private void EntityDieRpc()
    {
        if (isServer) return;
        turnAnimation.Die();
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}
