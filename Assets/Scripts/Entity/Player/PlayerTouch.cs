using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class PlayerTouch : AttackBase
{
    Player player;
    private void Awake()
    {
        player = transform.parent.GetComponent<Player>();
        perant = player;
        ifServer = NetworkServer.active;
    }
    public override void Attack(Entity entity)
    {
        if (player.isServer)
        {
            player.TouchEntity(entity, new ATKData(1, 1, player.atk, 0, player.atkpre, AtkType.atk));
            entity.TouchEntity(player, new ATKData(1, 1, entity.atk, 0, entity.atkpre, AtkType.atk));
            //if (entity is Servitor servitor)
            //{
            //    if (player.canRewrite)
            //    {
            //        player.RewriteServitor(servitor);
            //    }
            //    else
            //    {
            //        if (servitor.parent != player)
            //        {
            //            servitor.AtkEntity(player, new ATKData(1, 1, servitor.atk, 0, servitor.atkpre, AtkType.atk));
            //        }
            //    }
            //}
            //else if (entity is Player p && player.canRewrite) 
            //{
            //    player.AtkEntity(p, new ATKData(1, 1, player.atk, 0, player.atkpre, AtkType.atk));
            //}
        }
    }
}
