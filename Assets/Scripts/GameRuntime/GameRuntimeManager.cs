using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameRuntimeManager : BaseManager<GameRuntimeManager>
{
    //ģʽ�б�
    public Dictionary<string, Base_GameMode> gameModeDic = new Dictionary<string, Base_GameMode>()
        {
        {"TestGameMode",new TestGameMode()}

        };
    //��Ϸ����ʱ���ݼ�
    private D_GameRuntime _gameRuntime = new D_GameRuntime();
    //�����Ϸ����ʱ���ݼ�
    public D_GameRuntime GameRuntimeData => _gameRuntime;
    //�����е��߼���ͼ���ǳ���Ҫ
    public PathNode[,] runtimeLogicMap => null;    
    //��ǰ��Ϸģʽ
    public Base_GameMode nowaGameMode = new Base_GameMode();
    public void InitBaseCellsFunction()
    {
        // Ŀǰ�ݶ���10Ϊ��д��ˢ�µ㣬11Ϊ����ˢ�µ㣬12Ϊ���ˢ�µ㣬13Ϊʹħˢ�µ�
        nowaGameMode.cellsForPlayerBorn = MapManager.Instance.GetMapBaseFunction(12);
        nowaGameMode.cellsForFeatherPenBorn = MapManager.Instance.GetMapBaseFunction(10);
        nowaGameMode.cellsForToolsBorn = MapManager.Instance.GetMapBaseFunction(11);
        nowaGameMode.cellsForServitorBorn = MapManager.Instance.GetMapBaseFunction(13);
    }
    public void LoadGameMode<T>(UnityAction<T> callBack = null) where T : Base_GameMode
    {
        string gameModeName = typeof(T).Name;
        if (gameModeDic.ContainsKey(gameModeName))
        {
            nowaGameMode = gameModeDic[gameModeName];            
            if (callBack != null)
                callBack(gameModeDic[gameModeName] as T);
            return;
        }
    }
    

    public void GameRuntimeRemove(GameObject gameObject)
    {
        //������ʱ��ʹ�û����
        GameObject.Destroy(gameObject);
    }
}
