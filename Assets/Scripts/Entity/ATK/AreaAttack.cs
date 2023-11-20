using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaAttack : AttackBase
{
    public override void Init(Entity entity, Vector3 pos, Vector3 dir, List<float> floats = null)
    {
        base.Init(entity, pos, dir, floats);
        lifeTime = floats[0];
    }

    public override void Attack(Entity entity)
    {
        if(entity is Servitor servitor&&servitor.parent == perant)
        {
            foreach(var buff in perant.buff.buffList)
            {
                servitor.AddBuff(buff.buffData.id, buff.Amount, 1, perant);
            }
            foreach(var buff in perant.skill.buffList)
            {
                servitor.AddBuff(buff.buffData.id, buff.Amount, 1, perant);
            }
        }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        if (!isServer) return;
        transform.position = perant.transform.position;
    }
}
