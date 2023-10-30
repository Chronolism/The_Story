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


    public void InitSkill(List<BuffDetile> buffIdList , float energyAmount , float maxEnergyAmount)
    {
        this.energyAmount = energyAmount;
        this.maxEnergyAmount = maxEnergyAmount;
        if(buffList == null|| buffIdList.Count == 0)
        {
            Debug.Log(entity.netId + "û�м���");
            return;
        }
        foreach(var i in buffIdList)
        {
            AddBuff(i.buffId, i.buffValue, entity);
        }
    }

    /// <summary>
    /// ����
    /// </summary>
    public void Triger()
    {
        if(entity.canUseSkill && energyAmount >= maxEnergyAmount)
        {
            mainSkill?.OnTriger(entity, mainSkill.Amount);
            energyAmount -= maxEnergyAmount;
            entity.OnUseSkill?.Invoke(entity, mainSkill);
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
            energyAmount = energyAmount + value.energyAmount * entity.energyGetRate > maxEnergyAmount ? maxEnergyAmount : energyAmount + value.energyAmount * entity.energyGetRate;
        }
    }
}
