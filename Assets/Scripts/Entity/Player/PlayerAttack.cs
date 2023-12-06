using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : AttackBase
{
    Player player;
    private void Awake()
    {
        player = transform.parent.GetComponent<Player>();
        perant = player;
    }
    public override void Attack(Entity entity)
    {
        if (player.isServer)
        {
            player.TouchEntity(entity, new ATKData(1, 1, player.atk, 0, player.atkpre, AtkType.atk));
            entity.TouchEntity(player, new ATKData(1, 1, entity.atk, 0, entity.atkpre, AtkType.atk));
        }
    }
}
