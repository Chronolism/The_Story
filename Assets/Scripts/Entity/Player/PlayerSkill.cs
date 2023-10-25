using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class PlayerSkill : EntityBuff
{
    [SyncVar]
    public float energyAmount;
    [SyncVar]
    public float maxEnergyAmount;

    public BuffBase mainSkill => buffList[0];
    public int passiveCount => buffList.Count - 1;

    public BuffBase PassiveSkill(int i) => buffList[i + 1];
    public BuffBase this[int i]
    {
        get => buffList[i];
    }


    public void InitSkill(List<BuffDetile> buffIdList , float energyAmount , float maxEnergyAmount)
    {
        this.energyAmount = energyAmount;
        this.maxEnergyAmount = maxEnergyAmount;
        if(buffList == null|| buffIdList.Count == 0)
        {
            Debug.Log(entity.netId + "没有技能");
            return;
        }
        foreach(var i in buffIdList)
        {
            AddBuff(i.buffId, i.buffValue, entity);
        }
    }


    public void Triger()
    {
        if(energyAmount >= maxEnergyAmount)
        {
            mainSkill?.OnTriger(entity, 1);
            energyAmount -= maxEnergyAmount;
        }
    }

    public void AddEnergy(float value)
    {
        energyAmount = energyAmount + value > maxEnergyAmount ? maxEnergyAmount : energyAmount + value;
    }
}
