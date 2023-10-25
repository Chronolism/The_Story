using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalRoom : RoomLogicBase
{
    public NormalRoom(RoomData roomData):base(roomData)
    {
    }

    public override void EndGame()
    {
        
    }

    public override void LoadMap()
    {
        if (cellsForToolsBorn.Count > 0)
            EntityFactory.Instance.CreatProp(new Vector3(cellsForToolsBorn[0].x + 0.5f, cellsForToolsBorn[0].y + 0.5f, 0)).ShowProp(DataMgr.Instance.GetPropData(1));
        if(cellsForToolsBorn.Count>1)
            EntityFactory.Instance.CreatProp(new Vector3(cellsForToolsBorn[1].x + 0.5f, cellsForToolsBorn[1].y + 0.5f, 0)).ShowProp(DataMgr.Instance.GetPropData(2));
        EntityFactory.Instance.CreatServitor(cellsForServitorBorn[0], true);
    }

    public override void LoadPlayer()
    {
        foreach (var user in roomData.roomUser)
        {
            EntityFactory.Instance.CreatPlayer(user.Value, cellsForPlayerBorn[0]);
        }
    }

    public override void StartGame()
    {
        
    }

    public override void Updata()
    {
        
    }
}
