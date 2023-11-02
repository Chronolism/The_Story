using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowAttack : AttackBase
{
    public bool ifTurn;
    public float speed;
    public override void Init(Entity entity, Vector3 vector3, List<float> floats = null)
    {
        base.Init(entity, vector3, floats);
        transform.position = entity.transform.position;
        transform.rotation = LookAt2D(Vector3.up, vector3);
    }

    public override void Attack(Entity entity)
    {
        if(entity is Servitor servitor)
        {
            if (servitor.parent != entity) {
                if (ifTurn)
                {
                    entity.RewriteServitor(servitor, true);
                }
                else
                {
                    entity.AtkEntity(servitor, new ATKData(atkId, 1, 99999, 0, 1, atkType));
                }
            }
        }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        transform.position +=v3.normalized * speed * Time.deltaTime;
    }
}
