using Mirror;
using Steamworks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomPanel : BasePanel,Observer<RoomData>
{
    public RoomData roomData;

    public CharacterData characterData;
    public RoomUserData roomUserData;

    public Text txtUserList;
    public Text txtCharacterName;
    public Text txtCharacterSkill;
    public Text txtCharacterPassiveSkill;
    public Button btnStart;

    bool ifchange;
    int passiveSkillIndex = 1;
    public override void Init()
    {
        txtUserList = GetControl<Text>("txtUserList");
        txtCharacterName = GetControl<Text>("txtCharacterName");
        txtCharacterSkill = GetControl<Text>("txtCharacterSkill");
        txtCharacterPassiveSkill = GetControl<Text>("txtCharacterPassiveSkill");
        btnStart = GetControl<Button>("btnStart");
        StartCoroutine(findRoomData());

        
    }

    protected override void Update()
    {
        base.Update();
        if (Input.GetKeyDown(KeyCode.W))
        {
            passiveSkillIndex++;
            if(passiveSkillIndex >= characterData.skill_Index.Count)
            {
                passiveSkillIndex = 1;
            }
            txtCharacterPassiveSkill.text = DataMgr.Instance.GetBuffData(characterData.skill_Index[passiveSkillIndex].buffId).name;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            passiveSkillIndex--;
            if (passiveSkillIndex < 1 )
            {
                passiveSkillIndex = characterData.skill_Index.Count - 1;
            }
            txtCharacterPassiveSkill.text = DataMgr.Instance.GetBuffData(characterData.skill_Index[passiveSkillIndex].buffId).name;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            UpdataOwnUserData(DataMgr.Instance.GetCharacter(characterData.character_Code + 1));
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            UpdataOwnUserData(DataMgr.Instance.GetCharacter(characterData.character_Code - 1));
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            List<BuffDetile> buff = new List<BuffDetile>();
            buff.Add(characterData.skill_Index[0]);
            buff.Add(characterData.skill_Index[passiveSkillIndex]);
            roomData.ChangeRoomUserData(new RoomUserData(roomUserData.connectId, characterData.character_Code, roomUserData.name,true , buff, roomUserData.tags));
        }
        if (ifchange)
        {
            ifchange = false;
            UpdataRoomUserData();
        }
    }

    public void InitData(RoomData roomData)
    {
        this.roomData = roomData;
        roomData.roomUser.Callback += RoomUserCallBack;
        UpdataRoomUserData();
    }

    void RoomUserCallBack(SyncList<RoomUserData>.Operation op, int itemIndex, RoomUserData oldItem, RoomUserData newItem)
    {
        ifchange = true;
    }

    private void OnDisable()
    {
        if(roomData != null)
        {
            roomData.roomUser.Callback -= RoomUserCallBack;
        }
    }

    public void UpdataRoomUserData()
    {
        txtUserList.text = "";
        bool ifsure = true;
        foreach (var user in roomData.roomUser) 
        {
            txtUserList.text += user.name + "\n"; 
            if(user.name == DataMgr.Instance.playerData.account)
            {
                UpdataOwnUserData(user);
            }
            if (user.ifSure == false) ifsure = false;
        }
        if (roomData.HostUser == DataMgr.Instance.playerData.account)
        {
            btnStart.gameObject.SetActive(ifsure);
        }
        else
        {
            btnStart.gameObject.SetActive(false);
        }
    }

    public void UpdataOwnUserData(RoomUserData own)
    {
        roomUserData = own;
        if (own.characterId!= characterData.character_Code)
        {
            characterData = DataMgr.Instance.GetCharacter(own.characterId);
            txtCharacterName.text = characterData.characterName;
            txtCharacterSkill.text = DataMgr.Instance.GetBuffData(own.skills[0].buffId).name;
            txtCharacterPassiveSkill.text = DataMgr.Instance.GetBuffData(own.skills[1].buffId).name;
            for(int i = 0; i < characterData.skill_Index.Count; i++)
            {
                if (characterData.skill_Index[i].buffId == own.skills[1].buffId)
                {
                    passiveSkillIndex = i;
                    break;
                }
            }
        }
    }

    public void UpdataOwnUserData(CharacterData characterData)
    {
        if (characterData == null) return;
        this.characterData = characterData;
        txtCharacterName.text = characterData.characterName;
        txtCharacterSkill.text = DataMgr.Instance.GetBuffData(characterData.skill_Index[0].buffId).name;
        txtCharacterPassiveSkill.text = DataMgr.Instance.GetBuffData(characterData.skill_Index[1].buffId).name;
        passiveSkillIndex = 1;
    }

    protected override void OnClick(string btnName)
    {
        switch (btnName)
        {
            case "btnStart":
                roomData.OpenGame();
                break;
        }
    }


    public void ToUpdate(RoomData value)
    {
        
    }

    IEnumerator findRoomData()
    {
        while(DataMgr.Instance.roomData == null)
        {
            yield return null;
        }
        InitData(DataMgr.Instance.roomData);
    }
}
