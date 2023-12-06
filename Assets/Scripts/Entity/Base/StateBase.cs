using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateBase 
{
    public abstract void OnEnter(Entity entity);
    public abstract void OnExit(Entity entity);
    public abstract void OnUpdata();
}
