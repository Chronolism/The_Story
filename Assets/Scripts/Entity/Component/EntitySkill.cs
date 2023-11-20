using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class EntitySkill : EntityBuff
{
    [SyncVar]
    public float energyAmount;
    [SyncVar]
    public float maxEnergyAmount;
    /// <summary>
    /// ��������
    /// </summary>
    public BuffBase mainSkill => buffList[0];
    /// <summary>
    /// ������������
    /// </summary>
    public int passiveCount => buffList.Count - 1;
    /// <summary>
    /// ��ȡ��������
    /// </summary>
    /// <param name="i"></param>
    /// <returns></returns>
    public BuffBase PassiveSkill(int inedx) => buffList[inedx + 1];
    public BuffBase this[int i]
    {
        get => buffList[i];
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        if (!entity.ifPause&& mainSkill.cd > 0)
        {
            mainSkill.cd -= Time.deltaTime;
            if(mainSkill.cd < 0)
            {
                mainSkill.cd = 0;
            }
        }
    }

    public void InitSkill(List<BuffDetile> buffIdList , float energyAmount , float maxEnergyAmount)
    {
        //this.energyAmount = energyAmount;
        //this.maxEnergyAmount = maxEnergyAmount;
        if(buffList == null|| buffIdList.Count == 0)
        {
            Debug.Log(entity.netId + "û�м���");
            return;
        }
        foreach(var i in buffIdList)
        {
            AddBuff(i.buffId, i.buffValue, entity);
        }
        this.energyAmount = mainSkill.energy;
        this.maxEnergyAmount = mainSkill.maxEnergy;
    }

    /// <summary>
    /// ����
    /// </summary>
    public void Triger()
    {
        if (entity.canUseSkill && mainSkill.cd <= 0 && mainSkill.energy >= mainSkill.maxEnergy) 
        {
            mainSkill.cd = mainSkill.cdMax;
            mainSkill.energy -= mainSkill.maxEnergy;
            mainSkill?.OnTriger(entity, mainSkill.Amount);
            entity.OnUseSkill?.Invoke(entity, mainSkill);
            energyAmount = mainSkill.energy;
            maxEnergyAmount = mainSkill.maxEnergy;
        }
    }
    /// <summary>
    /// �������
    /// </summary>
    /// <param name="value"></param>
    public void AddEnergy(InkData value)
    {
        if (entity.canAddEnergy)
        {
            entity.OnGetEnergy?.Invoke(entity, value);
            mainSkill.energy = mainSkill.energy + value.energyAmount * entity.energyGetRate > mainSkill.maxEnergy ? mainSkill.maxEnergy : mainSkill.energy + value.energyAmount * entity.energyGetRate;
            energyAmount = mainSkill.energy;
            maxEnergyAmount = mainSkill.maxEnergy;
        }
    }
}
