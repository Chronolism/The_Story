using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForeShadowAttack : AttackBase
{
    bool ifGiddy;
    float time;
    public List<Observer<ForeShadowAttack>> Observers = new List<Observer<ForeShadowAttack>>();
    public override void Init(Entity entity, Vector3 pos, Vector3 dir, List<float> floats = null)
    {
        base.Init(entity, pos, dir, floats);
        time = floats[0];
        ifGiddy = time > 0;
    }
    public override void Attack(Entity entity)
    {
        if(entity!=perant && entity is Player player && player.canRewrite)
        {
            player.ChangeInkAmount(-99999);
            if (ifGiddy)
            {
                entity.GiddyEntity(perant, time);
            }
            StopAttack();
        }
    }

    public override void StopAttack()
    {
        foreach(var observer in Observers)
        {
            observer.ToUpdate(this);
        }
        base.StopAttack();
    }
}
