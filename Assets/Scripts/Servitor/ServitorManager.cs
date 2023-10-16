using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ServitorManager : BaseManager<ServitorManager>
{
    //模式列表
    public Dictionary<string, Base_Servitor> servitorDic = new Dictionary<string, Base_Servitor>()
        {
        

        };

    

    ////当前游戏模式
    //public Base_GameMode nowaGameMode = new Base_GameMode();
    //public void LoadGameMode<T>(UnityAction<T> callBack = null) where T : Base_GameMode
    //{
    //    string gameModeName = typeof(T).Name;
    //    if (gameModeDic.ContainsKey(gameModeName))
    //    {
    //        nowaGameMode = gameModeDic[gameModeName];



    //        if (callBack != null)
    //            callBack(gameModeDic[gameModeName] as T);

    //        return;
    //    }
    //}
}
