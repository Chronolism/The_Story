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

    //当前游戏模式
    public Base_GameMode nowaGameMode = new Base_GameMode();
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


}
