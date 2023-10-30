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

    private void OnDisable()
    {
        EventMgr.StartGame -= StartGame;
    }

    private void StartGame()
    {
        entityControl = DataMgr.Instance.activePlayer.GetComponent<EntityControl>();
    }

    private void Update()
    {
        if (isLocalPlayer && entityControl != null) 
        {
            entityControl.PlayerInput();
        }
    }
}
