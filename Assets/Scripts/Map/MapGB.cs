using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGB : NetworkBehaviour
{
    [SyncVar]
    public string mapName = "400";
    // Start is called before the first frame update
    void Start()
    {
        MapManager.Instance.LoadMapCompletelyToScene(mapName);

        AStarMgr.Instance.InitMapInfo(MapManager.Instance.mapColliderData);

        StartCoroutine(StartGame());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator StartGame()
    {
        if(DataMgr.Instance.activePlayer == null)
        {
            yield return null;
        }
        DataMgr.Instance.activePlayer.inputDisable = false;
    }
}
