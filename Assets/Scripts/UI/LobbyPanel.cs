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
        //���鰴ť
        GetControl<Button>("QuickRace").onClick.AddListener(QuickRace);
        GetControl<Button>("HostRoom").onClick.AddListener(HostRoom);
        GetControl<Button>("JoinRoom").onClick.AddListener(JoinRoom);
        GetControl<Button>("StoryMode").onClick.AddListener(StoryMode);
        GetControl<Button>("FabricateMode").onClick.AddListener(FabricateMode);
        GetControl<Button>("CreativeFactory").onClick.AddListener(CreativeFactory);
        GetControl<Button>("HandBook").onClick.AddListener(HandBook);
        //�̼�
        _accountAvatar = GetControl<Image>("AccountAvatar");
        _accountName = GetControl<Text>("AccountName");
        _notice = GetControl<Image>("Notice");
        _pagesNum = GetControl<Text>("PagesNum");
        _coinsNum = GetControl<Text>("CoinsNum");
        //Ť����
        //����ϵ���Ū����ģ����
        //�˳�
        GetControl<Button>("ExitButton").onClick.AddListener(()=> {
            UIManager.Instance.ShowPanel<LoadingPanel>((o) => {
                o.AddWhileEnterCompletelyBlack(() =>
                {
                    UIManager.Instance.HidePanel<LobbyPanel>();
                    UIManager.Instance.ShowPanel<MainMenuPanel>();
                });
            },true);
        });
        GetControl<Button>("SettingButton").onClick.AddListener(() => {
            UIManager.Instance.ShowPanel<LoadingPanel>(null,true);
        });
    }
    #region ���д��鰴ť�ĺ���
    public void QuickRace()
    {
        //���ٿ�ʼ
    }
    public void HostRoom()
    {
        GameMgr.Instance.CreatRoom();
    }
    public void JoinRoom()
    {
        UIManager.Instance.ShowPanel<LoadingPanel>((o) => {
            o.AddWhileEnterCompletelyBlack(() =>
            {
                UIManager.Instance.HidePanel<LobbyPanel>();
                UIManager.Instance.ShowPanel<SearchRoomPanel>();
            });
        }, true);
    }
    public void StoryMode()
    {
        //����ģʽ
    }
    public void FabricateMode()
    {
        //��׫ģʽ
    }
    public void CreativeFactory()
    {
        //ת����ͼ�༭������ʱ��
        UIManager.Instance.ShowPanel<LoadingPanel>((o) =>
        {
            o.AddWhileEnterCompletelyBlack(() =>
            {
                UIManager.Instance.HidePanel<LobbyPanel>();
                UIManager.Instance.ShowPanel<MapEditorPanel>((o) =>
                {
                    o.OnQuit.AddListener(() =>
                    {
                        UIManager.Instance.ShowPanel<LoadingPanel>((o) =>
                        {
                            o.AddWhileEnterCompletelyBlack(() =>
                            {
                                UIManager.Instance.HidePanel<MapEditorPanel>();
                                UIManager.Instance.ShowPanel<LobbyPanel>();
                            });
                        });
                    });
                });
            });
        });
    }
    public void HandBook()
    {
        //ת��ͼ��
    }
    #endregion
}
