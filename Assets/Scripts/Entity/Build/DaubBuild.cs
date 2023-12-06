using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaubBuild : BaseBuild, IEntityInteractive,IEntityTouch
{
    public GameObject E;

    public float inkAmountCost = 60;

    public float atk = 30;

    public float getEnergy = 10;

    public float atkEnergy = 20;

    SpriteRenderer sr;


    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        ReSetBuild();
    }

    private void FixedUpdate()
    {
        if (isServer)
        {
            if (time > 0)
            {
                time -= Time.deltaTime;
            }
        }
        E.SetActive(time <= 0);
    }

    public override void ReSetBuild()
    {
        base.ReSetBuild();
        ChangeColorRpc(Color.white);
    }
    [ClientRpc]
    public void ChangeColorRpc(Color color)
    {
        sr.color = color;
    }

    public void OnActive()
    {
        
    }

    public void OnNotActive()
    {
        
    }

    public void Touch(Entity entity)
    {
        if (entityParent == null) return;
        if (entity == entityParent) return;
        if(entity is Player player )
        {
            if (entity.inkAmount >= inkAmountCost)
            {
                entity.ChangeInkAmount(-inkAmountCost);
                ChangeParent(player);
            }
            else if (time <= 0)
            {
                time = timeMax;
                entityParent.AtkEntity(player, new ATKData(0, 1, atk, 0, 1, AtkType.atk));
                entityParent.AddEnergy(new InkData(0, atkEnergy, true));
            }
        }
    }

    public void Interactive(Entity entity)
    {
        if (entityParent == null && entity is Player player)
        {
            ChangeParent(player);
        }
    }

    public void ChangeParent(Player player)
    {
        player.AddEnergy(new InkData(0, getEnergy, true));
        entityParent = player;
        ChangeColorRpc(new Color(Random.Range(0, 1), Random.Range(0, 1), Random.Range(0, 1), 1));
    }
}
