using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnvironwentMap : MonoBehaviour
{
    EntityEnvironment environment;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<EntityEnvironment>(out environment))
        {
            environment.AddEnvironments(this);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent<EntityEnvironment>(out environment))
        {
            environment.RemoveEnvironments(this);
        }
    }
    public virtual void OnEnter(Entity entity) { }
    public virtual void OnExit(Entity entity) { }
    public virtual void OnUpdate(Entity entity) { }
}
