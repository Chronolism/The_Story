using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowAttack : AttackBase
{
    public bool ifTurn;
    public float speed;
    public override void Init(Entity entity, Vector3 pos , Vector3 dir, List<float> floats = null)
    {
        base.Init(entity, pos, dir, floats);
        transform.rotation = LookAt2D(Vector3.up, dir);
    }

    public override void Attack(Entity entity)
    {
        if(entity is Servitor servitor)
        {
            if (servitor.parent != entity) {
                if (ifTurn)
                {
                    Debug.Log("¼ý¸ÄÐ´");
                    perant.RewriteServitor(servitor, true);
                }
                else
                {
                    perant.AtkEntity(servitor, new ATKData(atkId, 1, 99999, 0, 1, atkType));
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
