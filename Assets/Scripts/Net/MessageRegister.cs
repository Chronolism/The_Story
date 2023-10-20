using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageRegister:BaseManager<MessageRegister>
{
    public void RegisterMessage()
    {
        NetworkServer.RegisterHandler<C2S_ChoiceCharacter>(C2S_ChoiceCharacterHandler);
        NetworkServer.RegisterHandler<C2S_JionRoom>(C2S_JionRoomHandler);
    }

    void C2S_ChoiceCharacterHandler(NetworkConnectionToClient con , C2S_ChoiceCharacter msg)
    {

    }

    void C2S_JionRoomHandler(NetworkConnectionToClient con, C2S_JionRoom msg)
    {
        DataMgr.Instance.roomData.AddRoomUser(msg.name);
    }
}
