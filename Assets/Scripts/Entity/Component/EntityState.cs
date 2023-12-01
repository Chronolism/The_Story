using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityState : EntityComponent
{
    protected StateBase state;
    private Dictionary<Type, StateBase> stateDic = new Dictionary<Type, StateBase>();
    public void ChangeState<T>() where T : StateBase, new()
    {
        if (state == null || state.GetType() != typeof(T))
        {
            state?.OnExit(entity);

            if (stateDic.ContainsKey(typeof(T)))
            {
                stateDic[typeof(T)].OnEnter(entity);
            }
            else
            {
                StateBase state = new T();
                stateDic.Add(typeof(T), state);
                state.OnEnter(entity);
            }

            state = stateDic[typeof(T)];
        }
    }
}
