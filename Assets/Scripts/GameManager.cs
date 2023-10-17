using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{

}
public class GameManager : BaseManager<GameManager>
{
    float _timer;
    public GameManager()
    {
        MonoMgr.Instance.AddUpdateListener(GameCentreUpdate);
    }
    public void GameCentreUpdate()
    {




        _timer += Time.deltaTime;
    }
}
