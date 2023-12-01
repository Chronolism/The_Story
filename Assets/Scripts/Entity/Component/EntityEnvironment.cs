using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EntityEnvironment : EntityComponent
{

    public List<BaseEnvironwentMap> map = new List<BaseEnvironwentMap>();

    public Dictionary<string,int> environments = new Dictionary<string, int>();

    BaseEnvironwentMap env;

    UnityAction<Entity> updataEnv;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("MapEnvironwent")&&collision.TryGetComponent<BaseEnvironwentMap>(out env))
        {
            AddEnvironments(env);
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("MapEnvironwent") && collision.TryGetComponent<BaseEnvironwentMap>(out env))
        {
            RemoveEnvironments(env);
        }
    }

    private void FixedUpdate()
    {
        if (isServer && entity.ifPause)
        {
            updataEnv?.Invoke(entity);
        }
    }

    public void AddEnvironments(BaseEnvironwentMap baseMap)
    {
        string name = baseMap.GetType().Name;
        if (environments.ContainsKey(name))
        {
            if(environments[name] == 0)
            {
                map.Add(baseMap);
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

    public void RemoveEnvironments(BaseEnvironwentMap baseMap)
    {
        string name = baseMap.GetType().Name;
        environments[name]--;
        if (environments[name] == 0)
        {
            map.Remove(baseMap);
            baseMap.OnExit(entity);
        }
        updataEnv -= baseMap.OnUpdate;
    }
}
