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
        //���ر��ص����ѡ��Ľ�ɫ
        if (CharacterManager.Instance.characterDic[CharacterCode] == null) return (!ThrowError(501));
        CharacterManager.Instance.characterDic[CharacterCode].InitLocalPlayCharacter();
        //���ص�ͼ
        //if (!MapManager.Instance.LoadMapCompletelyToScene(MapName))return (!ThrowError(502));
        //AStarMgr.Instance.InitMapInfo(MapManager.Instance.mapColliderData);
        //������Ϸģʽ
        if (GameRuntimeManager.Instance.gameModeDic[GameMode] == null) return (!ThrowError(503));
        GameRuntimeManager.Instance.gameModeDic[GameMode].SetSelfAsNowaGameMode();
        //������Ϸģʽ���ر�����Ϸ��ʼ��Ϣ
        if (PlayerManager.Instance.LocalPlayer == null) return (!ThrowError(504));
        PlayerManager.Instance.InitLocalPlayer();
        //�������ݽ��ڱ������߲�����Ч
        if (isOfflineLocalTest)
        {
            PlayerManager.Instance.AddOtherPlayerForOfflineMode();
            //����һ�µ�ͼ��������
            GameRuntimeManager.Instance.InitBaseCellsFunction();
            Object.Instantiate(Resources.Load("Player/Player"),MapManager.Instance.runtimeGrid.CellToWorld(GameRuntimeManager.Instance.nowaGameMode.GetPlayerStartPos()),Quaternion.identity);
            Object.Instantiate(Resources.Load("Player/PlayerVirtual"), MapManager.Instance.runtimeGrid.CellToWorld(GameRuntimeManager.Instance.nowaGameMode.GetPlayerStartPos()), Quaternion.identity);
            Object.Instantiate(Resources.Load("Servitor/ServitorCommon"), MapManager.Instance.runtimeGrid.CellToWorld(GameRuntimeManager.Instance.nowaGameMode.cellsForServitorBorn[0]), Quaternion.identity);
            //������Ϸ����ʱ
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
            case 501: Debug.LogWarning("û�гɹ�����ָ����ɫ"); return true;
            case 502: Debug.LogWarning("û�гɹ�����ָ����ͼ"); return true;
            case 503: Debug.LogWarning("û�гɹ�����ָ����Ϸģʽ"); return true;
            case 504: Debug.LogWarning("û�гɹ�����ģʽ��ʼ�����ؽ�ɫ"); return true;
            case 505: Debug.LogWarning("û�гɹ�����ʹħ"); return true;
            default:return false;
        }
    }
}
