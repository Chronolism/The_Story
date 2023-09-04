using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{

}
public class GameManager : BaseManager<GameManager>
{
    
    public GameManager()
    {
        MonoMgr.Instance.AddUpdateListener(()=> { });
    }
    

}
