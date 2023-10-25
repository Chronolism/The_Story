using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class RoomUser : NetworkBehaviour
{

    EntityControl entityControl;
    public override void OnStartClient()
    {
        base.OnStartClient();
    }

    private void OnEnable()
    {
        EventMgr.StartGame += StartGame;
    }

    private void StartGame()
    {
        foreach(var player in DataMgr.Instance.players)
        {
            if(player.Value.userName == DataMgr.Instance.playerData.account)
            {
                entityControl = player.Value.GetComponent<EntityControl>();
                return;
            }
        }
    }

    private void Update()
    {
        if (isLocalPlayer && entityControl != null) 
        {
            entityControl.PlayerInput();
        }
    }
}
