using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface BaseEnvironwentMap
{
    public string mapName { get;}
    public void OnEnter(Entity entity);
    public void OnExit(Entity entity);
    public void OnUpdate(Entity entity);
}
