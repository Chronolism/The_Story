using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ServitorManager : BaseManager<ServitorManager>
{
    //存场景上所有的使魔
    public List<Base_Servitor> runtimeServitorList = new List<Base_Servitor>();

    public void SpawnServitor<T>(Vector3Int spawnCell,UnityAction<T> callBack = null) where T : Base_Servitor
    {

    }




}
