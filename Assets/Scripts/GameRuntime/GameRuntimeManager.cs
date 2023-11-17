using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameRuntimeManager : BaseManager<GameRuntimeManager>
{
    //模式列表
    public Dictionary<string, Base_GameMode> gameModeDic = new Dictionary<string, Base_GameMode>()
        {
        {"TestGameMode",new TestGameMode()}

        };
    //游戏运行时数据集
    private D_GameRuntime _gameRuntime = new D_GameRuntime();
    //获得游戏运行时数据集
    public D_GameRuntime GameRuntimeData => _gameRuntime;
    //运行中的逻辑地图，非常重要
    public PathNode[,] runtimeLogicMap => null;    
    //当前游戏模式
    public Base_GameMode nowaGameMode = new Base_GameMode();
    public void InitBaseCellsFunction()
    {
        // 目前暂定：10为改写笔刷新点，11为道具刷新点，12为玩家刷新点，13为使魔刷新点
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
        //这里暂时不使用缓存池
        GameObject.Destroy(gameObject);
    }
}
