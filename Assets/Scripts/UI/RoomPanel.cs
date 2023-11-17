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
    public ScrollRect srSteamFriendList;

    public Animator characterDisplay;
    public Image passiveIcon;
    public Image ultimateSkillIcon;

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

        srSteamFriendList = GetControl<ScrollRect>("srSteamFriendList");
        srSteamFriendList.gameObject.SetActive(false);
        GetControl<Button>("btnInvited").gameObject.SetActive(DataMgr.Instance.gameServerType == GameServerType.Steam);

        //按钮初始化
        GetControl<Button>("SwitchW").onClick.AddListener(SwitchPassiveLast);
        GetControl<Button>("SwitchS").onClick.AddListener(SwitchPassiveNext);
        GetControl<Button>("SwitchA").onClick.AddListener(SwitchCharacterLast);
        GetControl<Button>("SwitchD").onClick.AddListener(SwitchCharacterNext); 
        GetControl<Button>("PrepareButton").onClick.AddListener(PrepareButton);
        //UI控件初始化
        characterDisplay = GetControl<Image>("CharacterImage").GetComponent<Animator>();
        passiveIcon = GetControl<Image>("PassiveIcon");
        ultimateSkillIcon = GetControl<Image>("UltimateSkillIcon");
    }

    protected override void Update()
    {
        base.Update();
        if (IEnableInput.GetKeyDown(E_PlayKeys.W))
        {
            SwitchPassiveLast();
        }
        if (IEnableInput.GetKeyDown(E_PlayKeys.S))
        {
            SwitchPassiveNext();
        }
        if (IEnableInput.GetKeyDown(E_PlayKeys.A))
        {
            SwitchCharacterLast();
        }
        if (IEnableInput.GetKeyDown(E_PlayKeys.D))
        {
            SwitchCharacterNext();
        }
        if (IEnableInput.GetKeyDown(E_PlayKeys.E))
        {
            PrepareButton();
        }
        if (ifchange)
        {
            ifchange = false;
            UpdataRoomUserData();
        }
    }
    #region 包裹一层方便button调用同一逻辑
    void SwitchCharacterLast()
    {
        UpdataOwnUserData(DataMgr.Instance.GetCharacter(characterData.character_Code - 1));
        characterDisplay.Play(characterData.character_Code  + "UIDisplay");
        passiveIcon.sprite = Resources.Load<SpriteRenderer>("Icons/skill" + characterData.character_Code + "_" + passiveSkillIndex).sprite;
        ultimateSkillIcon.sprite = Resources.Load<SpriteRenderer>("Icons/skill" + characterData.character_Code).sprite;
    }
    void SwitchCharacterNext()
    {
        UpdataOwnUserData(DataMgr.Instance.GetCharacter(characterData.character_Code + 1));
        characterDisplay.Play(characterData.character_Code + "UIDisplay");
        passiveIcon.sprite = Resources.Load<SpriteRenderer>("Icons/skill" + characterData.character_Code + "_" + passiveSkillIndex).sprite;
        ultimateSkillIcon.sprite = Resources.Load<SpriteRenderer>("Icons/skill" + characterData.character_Code).sprite;
    }
    void SwitchPassiveLast()
    {
        passiveSkillIndex++;
        if (passiveSkillIndex >= characterData.skill_Index.Count)
        {
            passiveSkillIndex = 1;
        }
        txtCharacterPassiveSkill.text = DataMgr.Instance.GetBuffData(characterData.skill_Index[passiveSkillIndex].buffId).name;
        passiveIcon.sprite = Resources.Load<SpriteRenderer>("Icons/skill" + characterData.character_Code+"_"+ passiveSkillIndex).sprite;
    }
    void SwitchPassiveNext()
    {
        passiveSkillIndex--;
        if (passiveSkillIndex < 1)
        {
            passiveSkillIndex = characterData.skill_Index.Count - 1;
        }
        txtCharacterPassiveSkill.text = DataMgr.Instance.GetBuffData(characterData.skill_Index[passiveSkillIndex].buffId).name;
        passiveIcon.sprite = Resources.Load<SpriteRenderer>("Icons/skill" + characterData.character_Code + "_" + passiveSkillIndex).sprite;
    }
    void PrepareButton()
    {
        List<BuffDetile> buff = new List<BuffDetile>();
        buff.Add(characterData.skill_Index[0]);
        buff.Add(characterData.skill_Index[passiveSkillIndex]);
        roomData.ChangeRoomUserData(new RoomUserData(roomUserData.connectId, characterData.character_Code, roomUserData.name, true, buff, roomUserData.tags));
    }
    #endregion

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
            case "btnInvited":
                srSteamFriendList.gameObject.SetActive(true);
                for (int i = 0; i < srSteamFriendList.content.transform.childCount; i++)
                {
                    Destroy(srSteamFriendList.content.transform.GetChild(i).gameObject);
                }
                foreach (var friend in SteamMgr.GetOnLineFriend())
                {
                    BtnRoom btnRoom = ResMgr.Instance.Load<GameObject>("UI/Compenent/btnRoom").GetComponent<BtnRoom>();
                    btnRoom.Init(new FriendRoom() { steamIP = friend.steamID.m_SteamID }, friend.name);
                    btnRoom.transform.SetParent(srSteamFriendList.content, false);
                    btnRoom.btnRoom.onClick.AddListener(() =>
                    {
                        (MyNetworkManager.singleton as MyNetworkManager).InvitedSteamFriendToLobby(btnRoom.room.steamIP);
                    });
                }
                break;
            case "btnQuitInvited":
                srSteamFriendList.gameObject.SetActive(false);
                break;
            case "btnQuit":
                GameMgr.Instance.QuitRoom();
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
