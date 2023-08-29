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

    //��ǰ��Ϸģʽ
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
