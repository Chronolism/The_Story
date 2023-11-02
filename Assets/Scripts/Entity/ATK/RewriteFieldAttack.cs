using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewriteFieldAttack : AttackBase
{
    public override void Init(Entity entity, Vector3 vector3, List<float> floats = null)
    {
        base.Init(entity, vector3, floats);
        lifeTime = vector3.x;
    }

    public override void Attack(Entity entity)
    {
        if(entity is Servitor servitor)
        {
            perant.RewriteServitor(servitor);
        }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        transform.position = perant.transform.position;
    }

}
