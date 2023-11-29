using System.Collections.Generic;
using UnityEngine;

public class EntityEnvironment : EntityComponent
{

    public List<BaseEnvironwentMap> map;

    public Dictionary<string,int> environments = new Dictionary<string, int>();

    public void AddEnvironments(BaseEnvironwentMap baseMap)
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
    }

    public void RemoveEnvironments(BaseEnvironwentMap baseMap)
    {
        string name = baseMap.GetType().Name;
        environments[name]--;
        if (environments[name] == 0)
        {
            baseMap.OnExit(entity);
        }
    }
}
