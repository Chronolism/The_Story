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
            roomData.ChangeRoomUserData(new RoomUserData(roomUserData.connectId, characterData.character_Code, roomUserData.name, buff, roomUserData.tags));
        }
    }

    public void InitData(RoomData roomData)
    {
        this.roomData = roomData;
        roomData.roomUser.Callback += (a, b, c) =>
        {
            UpdataRoomUserData();
        };
        UpdataRoomUserData();
    }

    public void UpdataRoomUserData()
    {
        txtUserList.text = "";
        foreach (var user in roomData.roomUser) 
        {
            txtUserList.text += user.Key + "\n"; 
            if(user.Value.name == DataMgr.Instance.playerData.account)
            {
                UpdataOwnUserData(user.Value);
            }
        }
        if (roomData.HostUser == DataMgr.Instance.playerData.account)
        {
            btnStart.gameObject.SetActive(true);
        }
        else
        {
            Debug.Log(false);
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
                roomData.StartGame();
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
