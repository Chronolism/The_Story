using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyPanel : BasePanel
{
    [Header("AccountBar")]
    Image _accountAvatar;
    Text _accountName;
    Image _notice;
    [Header("AccountAsset")]
    Text _pagesNum;
    Text _coinsNum;
    public override void Init()
    {
        //大板块按钮
        GetControl<Button>("QuickRace").onClick.AddListener(QuickRace);
        GetControl<Button>("HostRoom").onClick.AddListener(HostRoom);
        GetControl<Button>("JoinRoom").onClick.AddListener(JoinRoom);
        GetControl<Button>("StoryMode").onClick.AddListener(StoryMode);
        GetControl<Button>("FabricateMode").onClick.AddListener(FabricateMode);
        GetControl<Button>("CreativeFactory").onClick.AddListener(CreativeFactory);
        GetControl<Button>("HandBook").onClick.AddListener(HandBook);
        //固件
        _accountAvatar = GetControl<Image>("AccountAvatar");
        _accountName = GetControl<Text>("AccountName");
        _notice = GetControl<Image>("Notice");
        _pagesNum = GetControl<Text>("PagesNum");
        _coinsNum = GetControl<Text>("CoinsNum");
        //扭蛋机
        //面板上的是弄着玩的，别管
    }
    #region 所有大板块按钮的函数
    public void QuickRace()
    {
        //快速开始
    }
    public void HostRoom()
    {
        //主持游戏
    }
    public void JoinRoom()
    {
        //加入游戏
    }
    public void StoryMode()
    {
        //故事模式
    }
    public void FabricateMode()
    {
        //杜撰模式
    }
    public void CreativeFactory()
    {
        //转到地图编辑器（暂时）
    }
    public void HandBook()
    {
        //转到图鉴
    }
    #endregion
}
