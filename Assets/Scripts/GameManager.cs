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
        //���ر��ص����ѡ��Ľ�ɫ
        if (CharacterManager.Instance.characterDic[CharacterCode] == null) return (!ThrowError(501));
        CharacterManager.Instance.characterDic[CharacterCode].InitLocalPlayCharacter();
        //���ص�ͼ
        if (!MapManager.Instance.LoadMapCompletelyToScene(MapName))return (!ThrowError(502));
        AStarMgr.Instance.InitMapInfo(MapManager.Instance.mapColliderData);
        //������Ϸģʽ
        if (GameRuntimeManager.Instance.gameModeDic[GameMode] == null) return (!ThrowError(503));
        GameRuntimeManager.Instance.gameModeDic[GameMode].SetSelfAsNowaGameMode();
        //������ҳ�ʼλ��
        ThrowError(500, "��Ҫ�ṩ������ҳ�ʼλ�õ�����");
        return true;
    }
    public static bool ThrowError(int errorCode,string errorMessage = "")
    {
        switch (errorCode)
        {
            case 500: Debug.LogWarning(errorMessage); return true;
            case 501: Debug.LogWarning("û�гɹ�����ָ����ɫ"); return true;
            case 502: Debug.LogWarning("û�гɹ�����ָ����ͼ"); return true;
            case 503: Debug.LogWarning("û�гɹ�����ָ����Ϸģʽ"); return true;
            default:return false;
        }
    }
}
