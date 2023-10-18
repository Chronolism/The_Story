using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum GameState
{

}
public class GameManager : BaseManager<GameManager>
{
    public float ScreenHight;
    public float ScreenWidth;
    float _timer;

    public event UnityAction GameUpdate;
    public GameManager()
    {
        MonoMgr.Instance.AddUpdateListener(_gameCentreUpdate);
    }
    void _gameCentreUpdate()
    {
        _timer += Time.deltaTime;
        if (true) GameUpdate?.Invoke();
    }
    public void GameInit()
    {

    }
    public bool GameAccountInit()
    {
        return false;
    }
    public bool GameStart(string MapName = "400",int CharacterCode = 405,string GameMode = "TestGameMode")
    {
        //加载本地的玩家选择的角色
        if (CharacterManager.Instance.characterDic[CharacterCode] == null) return (!ThrowError(501));
        CharacterManager.Instance.characterDic[CharacterCode].InitLocalPlayCharacter();
        //加载地图
        if (!MapManager.Instance.LoadMapCompletelyToScene(MapName))return (!ThrowError(502));
        AStarMgr.Instance.InitMapInfo(MapManager.Instance.mapColliderData);
        //加载游戏模式
        if (GameRuntimeManager.Instance.gameModeDic[GameMode] == null) return (!ThrowError(503));
        GameRuntimeManager.Instance.gameModeDic[GameMode].SetSelfAsNowaGameMode();
        //加载玩家初始位置
        ThrowError(500, "需要提供承载玩家初始位置的容器");
        return true;
    }
    public static bool ThrowError(int errorCode,string errorMessage = "")
    {
        switch (errorCode)
        {
            case 500: Debug.LogWarning(errorMessage); return true;
            case 501: Debug.LogWarning("没有成功加载指定角色"); return true;
            case 502: Debug.LogWarning("没有成功加载指定地图"); return true;
            case 503: Debug.LogWarning("没有成功加载指定游戏模式"); return true;
            default:return false;
        }
    }
}
