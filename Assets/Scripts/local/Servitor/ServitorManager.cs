using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ServitorManager : BaseManager<ServitorManager>
{
    //�泡�������е�ʹħ
    public List<Base_Servitor> runtimeServitorList = new List<Base_Servitor>();

    public void SpawnServitor<T>(Vector3Int spawnCell,UnityAction<T> callBack = null) where T : Base_Servitor
    {

    }




}