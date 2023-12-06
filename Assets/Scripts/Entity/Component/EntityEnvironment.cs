using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EntityEnvironment : EntityComponent
{
    public Dictionary<string,int> environments = new Dictionary<string, int>();

    IEnvironwentMap env;

    UnityAction<Entity> updataEnv;

    public MapColliderType landColliderType = MapColliderType.None;
    public AStarNode LastLand;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isServer) return;
        if (collision.CompareTag("MapEnvironwent")&&collision.TryGetComponent<IEnvironwentMap>(out env))
        {
            AddEnvironments(env);
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (!isServer) return;
        if (collision.CompareTag("MapEnvironwent") && collision.TryGetComponent<IEnvironwentMap>(out env))
        {
            RemoveEnvironments(env);
        }
    }

    float time = 0.3f;
    private void FixedUpdate()
    {
        if (isServer && entity.ifPause)
        {
            time-=Time.deltaTime;
            if(time < 0)
            {
                time = 0.3f;
                AStarNode aStarNode = AStarMgr.Instance.GetNode(entity.rb.position.x, entity.rb.position.y);
                if (aStarNode != null && aStarNode.ChackType(landColliderType))
                {
                    LastLand = aStarNode;
                }
            }
            updataEnv?.Invoke(entity);
        }
    }

    public void AddEnvironments(IEnvironwentMap baseMap)
    {
        string name = baseMap.GetType().Name;
        if (environments.ContainsKey(name))
        {
            if(environments[name] == 0)
            {
                baseMap.OnEnter(entity);
            }
            environments[name]++;
        }
        else
        {
            environments.Add(name, 1);
            baseMap.OnEnter(entity);
        }
        updataEnv += baseMap.OnUpdate;
    }

    public void RemoveEnvironments(IEnvironwentMap baseMap)
    {
        string name = baseMap.GetType().Name;
        environments[name]--;
        if (environments[name] == 0)
        {
            baseMap.OnExit(entity);
        }
        updataEnv -= baseMap.OnUpdate;
    }
}
