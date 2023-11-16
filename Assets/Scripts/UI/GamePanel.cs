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
    float _Ink_targetFillAmount;
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
    float _Ultimate_Skill_targetFillAmount;
    [Header("QA")]
    public float UIAnimationSpeed = 0.1f; 

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

        //添加悬停逻辑，有bug
        AddChangeItOnlyWhileStayIn(_Ink_BackGround, _tipsFollowMouse,"ink");
        AddChangeItOnlyWhileStayIn(_Passive_BackGround, _tipsFollowMouse, "passive");
        AddChangeItOnlyWhileStayIn(_Prop_BackGround, _tipsFollowMouse, "prop");
        AddChangeItOnlyWhileStayIn(_Ultimate_Skill_BackGround, _tipsFollowMouse, "ultimate");

        if (DataMgr.Instance.roomData.ifPause)
        {
            StartGame();
        }
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

            if (player.inkMaxAmount == 0) Debug.LogWarning("player.inkMaxAmount为0");
            if (player.skill.maxEnergyAmount == 0) Debug.LogWarning("skill.maxEnergyAmount为0");
            _Ink_targetFillAmount = player.inkAmount / player.inkMaxAmount;
            _Ultimate_Skill_targetFillAmount = player.skill.energyAmount / player.skill.maxEnergyAmount;
            

            if (Mathf.Abs(_Ink_targetFillAmount - _Ink_Now.fillAmount) > 0.01f)
                _Ink_Now.fillAmount = Mathf.Lerp(_Ink_Now.fillAmount, _Ink_targetFillAmount, UIAnimationSpeed);
            else
                _Ink_Now.fillAmount = _Ink_targetFillAmount;

            if (Mathf.Abs(_Ultimate_Skill_targetFillAmount - _Ultimate_Skill_Charge_Progress.fillAmount) > 0.01f)
                _Ultimate_Skill_Charge_Progress.fillAmount = Mathf.Lerp(_Ultimate_Skill_Charge_Progress.fillAmount, _Ultimate_Skill_targetFillAmount, UIAnimationSpeed);
            else
                _Ultimate_Skill_Charge_Progress.fillAmount = _Ultimate_Skill_targetFillAmount;


            
        }                     
        else
        {
            
        }
        _tipsFollowMouse.transform.position = Input.mousePosition;
    }
    public void AddChangeItOnlyWhileStayIn(UIBehaviour control, Text text, string changeText)
    {
        UIManager.AddCustomEventListener(control, EventTriggerType.PointerEnter, (o) => { (text as TextFollowMouse).DisplayIt(changeText); Debug.LogWarning("进"); });
        UIManager.AddCustomEventListener(control, EventTriggerType.PointerExit, (o) => { (text as TextFollowMouse).DisplayClear(); Debug.LogWarning("出"); });
    }

}
