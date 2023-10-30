using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePanel : BasePanel
{
    Player player;
    Image imgBlood;
    Text txtBlood;
    public override void Init()
    {
        imgBlood = GetControl<Image>("imgBlood");
        txtBlood = GetControl<Text>("txtBlood");
        player = DataMgr.Instance.activePlayer; 
    }

    private void OnEnable()
    {
        EventMgr.StartGame += StartGame;
    }

    private void OnDisable()
    {
        EventMgr.StartGame -= StartGame;
    }
    /// <summary>
    /// 若显示面板与事件激活同帧执行，该方法不会执行，请在Init中执行
    /// </summary>
    private void StartGame()
    {
        player = DataMgr.Instance.activePlayer;
    }

    private void FixedUpdate()
    {
        if (player == null) return;
        imgBlood.fillAmount = player.blood / player.MaxBlood;
        txtBlood.text = player.blood + "/" + player.MaxBlood;
    }
}
