using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityComponent : NetworkBehaviour
{
    public Entity entity;

    public virtual void Awake()
    {
        entity = GetComponent<Entity>();
    }
}
