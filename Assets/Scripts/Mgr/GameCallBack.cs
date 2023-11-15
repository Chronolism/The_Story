using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameCallBack : BaseManager<GameCallBack>
{
    public UnityAction SuccesJionRoom;
    public GameCallBack()
    {
        SuccesJionRoom = () => {
            UIManager.Instance.ClearAllPanel("GameLayer");
            //UIManager.Instance.GetPanel<LoadingPanel>().SetLoading(false);
            UIManager.Instance.ShowPanel<RoomPanel>();
        };
    }
}
