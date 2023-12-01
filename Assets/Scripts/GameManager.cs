using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum GameState
{

}
public class GameManager : BaseManager<GameManager>
{
    public Camera camera;
    public float ScreenHight;
    public float ScreenWidth;
    public bool GameIsPause = false;
    float _timer;

    public bool isOfflineLocalTest = true;
    public event UnityAction GameUpdate;
    public GameManager()
    {
        MonoMgr.Instance.AddUpdateListener(_gameCentreUpdate);
    }
    void _gameCentreUpdate()
    {
        _timer += Time.deltaTime;
        if (!GameIsPause) GameUpdate?.Invoke();
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
        //if (!MapManager.Instance.LoadMapCompletelyToScene(MapName))return (!ThrowError(502));
        //AStarMgr.Instance.InitMapInfo(MapManager.Instance.mapColliderData);
        //加载游戏模式
        if (GameRuntimeManager.Instance.gameModeDic[GameMode] == null) return (!ThrowError(503));
        GameRuntimeManager.Instance.gameModeDic[GameMode].SetSelfAsNowaGameMode();
        //根据游戏模式加载本局游戏初始信息
        if (PlayerManager.Instance.LocalPlayer == null) return (!ThrowError(504));
        PlayerManager.Instance.InitLocalPlayer();
        //以下内容仅在本地离线测试生效
        if (isOfflineLocalTest)
        {
            PlayerManager.Instance.AddOtherPlayerForOfflineMode();
            //加载一下地图基本功能
            GameRuntimeManager.Instance.InitBaseCellsFunction();
            Object.Instantiate(Resources.Load("Player/Player"),MapManager.Instance.runtimeGrid.CellToWorld(GameRuntimeManager.Instance.nowaGameMode.GetPlayerStartPos()),Quaternion.identity);
            Object.Instantiate(Resources.Load("Player/PlayerVirtual"), MapManager.Instance.runtimeGrid.CellToWorld(GameRuntimeManager.Instance.nowaGameMode.GetPlayerStartPos()), Quaternion.identity);
            Object.Instantiate(Resources.Load("Servitor/ServitorCommon"), MapManager.Instance.runtimeGrid.CellToWorld(GameRuntimeManager.Instance.nowaGameMode.cellsForServitorBorn[0]), Quaternion.identity);
            //启动游戏运行时
            GameRuntimeManager.Instance.nowaGameMode.GameRuntimeStart();
            GameUpdate += GameRuntimeManager.Instance.nowaGameMode.GameRuntimeUpdate;
        }
        return true;
    }
    public bool GameEnd()
    {
        GameUpdate -= GameRuntimeManager.Instance.nowaGameMode.GameRuntimeUpdate;
        return false;
    }
    public static bool ThrowError(int errorCode,string errorMessage = "")
    {
        switch (errorCode)
        {
            case 500: Debug.LogWarning(errorMessage); return true;
            case 501: Debug.LogWarning("没有成功加载指定角色"); return true;
            case 502: Debug.LogWarning("没有成功加载指定地图"); return true;
            case 503: Debug.LogWarning("没有成功加载指定游戏模式"); return true;
            case 504: Debug.LogWarning("没有成功依据模式初始化本地角色"); return true;
            case 505: Debug.LogWarning("没有成功加载使魔"); return true;
            default:return false;
        }
    }
}
