using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GamePanel : BasePanel
{
    public Player player;
    Image imgBlood;
    Text txtBlood;

    [Header("Ink")]
    Image _Ink_BackGround;
    Image _Ink_Now;
    [Header("Prop")]
    Image _Prop_BackGround;
    Image _Prop_Now;
    [Header("Passive")]
    Image _Passive_BackGround;
    Image _Passive_Now;
    [Header("Ultimate_Skill")]
    Image _Ultimate_Skill_BackGround;
    Image _Ultimate_Skill_Now;
    Image _Ultimate_Skill_Charge_Progress;

    Text _tipsFollowMouse;

    bool _isUIUpdating = false;
    public override void Init()
    {
        imgBlood = GetControl<Image>("imgBlood");
        txtBlood = GetControl<Text>("txtBlood");
        player = DataMgr.Instance.activePlayer;

        _Ink_BackGround = GetControl<Image>("Ink_BackGround");
        _Ink_Now = GetControl<Image>("Ink_Now");

        _Prop_BackGround = GetControl<Image>("Prop_BackGround");
        _Prop_Now = GetControl<Image>("Prop_BackGround");

        _Passive_BackGround = GetControl<Image>("Passive_BackGround");
        _Passive_Now = GetControl<Image>("Passive_Now");

        _Ultimate_Skill_BackGround = GetControl<Image>("Ultimate_Skill_BackGround");
        _Ultimate_Skill_Now = GetControl<Image>("Ultimate_Skill_Now");
        _Ultimate_Skill_Charge_Progress = GetControl<Image>("Ultimate_Skill_Charge_Progress");

        _tipsFollowMouse = GetControl<Text>("tipsFollowMouse");

        StartGame();
    }

    private void OnEnable()
    {
        EventMgr.StartGame += StartGame;
        _isUIUpdating = false;
    }

    private void OnDisable()
    {
        EventMgr.StartGame -= StartGame;
        _isUIUpdating = false;
    }
    /// <summary>
    /// 若显示面板与事件激活同帧执行，该方法不会执行，请在Init中执行
    /// </summary>
    private void StartGame()
    {
        player = DataMgr.Instance.activePlayer;
        _isUIUpdating = true;

    }

    private void FixedUpdate()
    {
        if (player == null) return;
        if (_isUIUpdating)
        {
            //待做：逐渐改变
            imgBlood.fillAmount = player.blood / player.MaxBlood;
            txtBlood.text = player.blood + "/" + player.MaxBlood;
            //待做：逐渐改变
            _Ink_Now.fillAmount = player.inkAmount / player.inkMaxAmount;
            _Ultimate_Skill_Charge_Progress.fillAmount = player.skill.energyAmount / player.skill.maxEnergyAmount;
        }
        if (EventSystem.current.currentSelectedGameObject != null) 
        {
            //鼠标
            _tipsFollowMouse.transform.position = Input.mousePosition;
        }
    }

}
