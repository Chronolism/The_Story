using Mirror;
using System.Collections;
using System.Collections.Generic;

public class RoomData : NetworkBehaviour
{
    [SyncVar(hook = "UpDataDetile")]
    public string mapName = "400";
    public readonly SyncIDictionary<string, RoomUserData> roomUser = new SyncIDictionary<string, RoomUserData>(new Dictionary<string, RoomUserData>());

    public List<Observer<RoomData>> observers = new List<Observer<RoomData>>();
    // Start is called before the first frame update

    public void AddRoomUser(string name)
    {
        roomUser.Add(name, new RoomUserData() { name = name});
    }

    public void Awake()
    {
        DataMgr.Instance.roomData = this;
        UIManager.Instance.ShowPanel<RoomPanel>((o) => { o.InitData(this); });
    }

    public void UpDataDetile(string oldvalue , string  value)
    {
        foreach(var a in observers)
        {
            a.ToUpdate(this);
        }
    }
}
