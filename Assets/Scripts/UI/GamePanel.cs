using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GamePanel : BasePanel
{
    public Player player;
    Image _HeartGroup;

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
    [Header("Color")]
    public Color Character101Color;
    public Color Character102Color;
    public Color Character103Color;
    public Color Character104Color;
    public Color Character105Color;
    public Color Character106Color;
    [Header("Heart")]
    [Range(0, 1)]
    public float targetHeartFillAmount;
    public float realHeartFillAmount;

    Text _tipsFollowMouse;

    bool _isUIUpdating = false;
    public override void Init()
    {
        _HeartGroup = GetControl<Image>("HeartPoints");
        player = DataMgr.Instance.activePlayer;

        _Ink_BackGround = GetControl<Image>("Ink_BackGround");
        _Ink_Now = GetControl<Image>("Ink_Now");

        _Prop_BackGround = GetControl<Image>("Prop_BackGround");
        _Prop_Now = GetControl<Image>("Prop_Now");

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

        if (!DataMgr.Instance.roomData.ifPause)
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
        InitUIDisplay();

    }

    private void FixedUpdate()
    {
        if (player == null) return;
        if (_isUIUpdating)
        {
            //血量
            targetHeartFillAmount = player.blood / player.MaxBlood;
            if (Mathf.Abs(targetHeartFillAmount - realHeartFillAmount) > 0.01f)
                realHeartFillAmount = Mathf.Lerp(realHeartFillAmount, targetHeartFillAmount, UIAnimationSpeed);
            else
                realHeartFillAmount = targetHeartFillAmount;           
            float eachHeartEqual = 1f / _HeartGroup.transform.childCount;
            int nowHeartIndex = (int)((realHeartFillAmount - realHeartFillAmount % eachHeartEqual)/ eachHeartEqual);
            for (int i = 0; i < _HeartGroup.transform.childCount; i++)
            {
                if (i == nowHeartIndex)
                {
                    _HeartGroup.transform.GetChild(i).GetComponent<Image>().fillAmount = (realHeartFillAmount - i * eachHeartEqual) / eachHeartEqual;
                }
                else if (i < nowHeartIndex)
                    _HeartGroup.transform.GetChild(i).GetComponent<Image>().fillAmount = 1;
                else
                    _HeartGroup.transform.GetChild(i).GetComponent<Image>().fillAmount = 0;
            }


            //墨水与充能
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

            //道具
            if (player.playerProp != null) _Prop_Now.sprite = player.playerProp.icon; 
            else _Prop_Now.sprite = null;


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
    public void InitUIDisplay()
    {      
        _Passive_Now.sprite = player.skill.PassiveSkill(0).buffData.img;
        _Ultimate_Skill_Now.sprite = Resources.Load<SpriteRenderer>("Icons/skill" + player.characterCode).sprite;
        _Ultimate_Skill_Charge_Progress.sprite = Resources.Load<SpriteRenderer>("Icons/skill" + player.characterCode).sprite;
        switch (player.characterCode)
        {
            case 101: _Ultimate_Skill_Charge_Progress.color = Character101Color; break;
            case 102: _Ultimate_Skill_Charge_Progress.color = Character102Color; break;
            case 103: _Ultimate_Skill_Charge_Progress.color = Character103Color; break;
            case 104: _Ultimate_Skill_Charge_Progress.color = Character104Color; break;
            case 105: _Ultimate_Skill_Charge_Progress.color = Character105Color; break;
            case 106: _Ultimate_Skill_Charge_Progress.color = Character106Color; break;
        }
    }
    
}
