using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuillotineBuild : BaseBuild, IEntityInteractive, IEntityTouch
{
    public GameObject E;

    public float inkAmountCost = 20;

    public float getEnergy = 10;

    public float atkEnergy = 10;

    SpriteRenderer sr;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
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

    public void Interactive(Entity entity)
    {
        if (entityParent == null && entity is Player player)
        {
            ChangeParent(player);
        }
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
        if (entity is Player player)
        {
            if (entity.inkAmount >= inkAmountCost)
            {
                entity.ChangeInkAmount(-inkAmountCost);
                ChangeParent(player);
            }
        }else if(entity is Servitor servitor)
        {
            servitor.ChangeBlood(entityParent, new ATKData(1, 1, servitor.MaxBlood, 0, 1, AtkType.atk));
            if(servitor.blood <= 0)
            {
                entityParent.AddEnergy(new InkData(0, atkEnergy, true));
            }
            time = timeMax;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        ReSetBuild();
    }


    public void ChangeParent(Player player)
    {
        player.AddEnergy(new InkData(0, getEnergy, true));
        entityParent = player;
        ChangeColorRpc(new Color(Random.Range(0, 1), Random.Range(0, 1), Random.Range(0, 1), 1));
    }
}
