using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityEffect : EntityComponent
{
    Dictionary<int,GameObject> effects = new Dictionary<int,GameObject>();
    [Server]
    [ClientRpc]
    public void ShowEffect(int id)
    {
        ShowEffectOnClient(id);
    }
    public void ShowEffectOnClient(int id)
    {
        if (effects.ContainsKey(id))
        {
            effects[id].SetActive(true);
        }
        else
        {
            EffectData effectData = DataMgr.Instance.GetEffectData(id);
            GameObject effGB = GameObject.Instantiate(effectData.gb);
            effGB.transform.SetParent(entity.transform, false);
            if (effectData.ifContinue)
            {
                effects[id] = effGB;
            }
        }
    }

    public void HideEffectOnClient(int id)
    {
        if (effects.ContainsKey(id))
        {
            effects[id].SetActive(false);
        }
        else
        {
            
        }
    }
}
