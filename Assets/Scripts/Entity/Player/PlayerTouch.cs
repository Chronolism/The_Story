using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTouch : AttackBase
{
    Player player;
    private void Awake()
    {
        player = transform.parent.GetComponent<Player>();
        Init(player, Vector3.zero, Vector3.zero);
    }
    public override void Attack(Entity entity)
    {
        if (player.isServer)
        {
            if (entity is Servitor servitor)
            {
                if (player.canRewrite)
                {
                    player.RewriteServitor(servitor);
                }
                else
                {
                    if (servitor.parent != player)
                    {
                        servitor.AtkEntity(player, new ATKData(1, 1, servitor.atk, 0, servitor.atkpre, AtkType.atk));
                    }
                }
                
            }
            else if (entity is Player p && player.canRewrite) 
            {
                player.AtkEntity(p, new ATKData(1, 1, player.atk, 0, player.atkpre, AtkType.atk));
            }
        }
    }
}
