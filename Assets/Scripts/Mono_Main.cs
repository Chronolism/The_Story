using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using UnityEngine.Events;
using Mirror;

public class Mono_Main : MonoBehaviour
{
    bool _ifConsoleEnable = false;
    void Start()
    {


        UIManager.Instance.ShowPanel<MainMenuPanel>();

        TestConsole.Instance.AddCommand("GameStart", () => { GameManager.Instance.GameStart(); }, "Ĭ�ϵ���Ϸ��ʼ");
        TestConsole.Instance.AddCommand("401", () => { GameManager.Instance.GameStart("401", 405, "TestGameMode"); }, "Ĭ�ϵ���Ϸ��ʼ");
        TestConsole.Instance.AddCommand("402", () => { GameManager.Instance.GameStart("402", 405, "TestGameMode"); }, "Ĭ�ϵ���Ϸ��ʼ");

        TestConsole.Instance.AddCommand("LocalhpUP10", () => { PlayerManager.Instance.LocalPlayer.runtime_HP += 10; }, "��Ϸ����ʱѪ������");
        TestConsole.Instance.AddCommand("Showhp", () => { TestConsole.Instance.WriteLine("�������HP "+ PlayerManager.Instance.LocalPlayer.runtime_HP.ToString()); }, "��ʾ��Ϸ����ʱѪ��");
        TestConsole.Instance.AddCommand("TestModeLoad", () => { GameRuntimeManager.Instance.LoadGameMode<TestGameMode>(); }, "������Ϸģʽ");
        TestConsole.Instance.AddCommand("CreateLocalMaxHp", () => { PlayerManager.Instance.HP_Max = 200; }, "���ý�ɫģ��:Ѫ������Ϊ200");
        TestConsole.Instance.AddCommand("InitLocalMaxHp", () => { PlayerManager.Instance.InitLocalPlayer(); }, "������Ϸģʽ�����Ѿ����õı��ؽ�ɫģ��");
        TestConsole.Instance.AddCommand("NetHPChangeTo10", () => { GamePlayNetControl.Instance. Local_Net_HP = 10 ; });

        TestConsole.Instance.AddCommand("MapTest", () => { UIManager.Instance.ShowPanel<MapEditPanel>(); }, "�����ͼ�༭��");
        TestConsole.Instance.AddCommand("LoadMapCollusionTest", () => { MapManager.Instance.LoadMapCompletelyToScene("400"); }, "����400��ײ���Ե�ͼ");
        TestConsole.Instance.AddCommand("BackToMenu", () => { UIManager.Instance.ShowPanel<MainMenuPanel>(null,true); }, "ǿ����ʾ���˵�");
        TestConsole.Instance.AddCommand("kill", () => { Application.Quit(); }, "ɱ����Ϸ");
        //TestConsole.Instance.AddCommand("TryCatchTest", () => { int x = 1;int y = 1; AbstractLogicManager.Instance.CellProbe(ref x, ref y,new V2 (0,0)); });
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (!_ifConsoleEnable)
            {
                UIManager.Instance.ShowPanel<ConsolePanel>(null,true);
                _ifConsoleEnable = true;
            }
            else
            {
                UIManager.Instance.HidePanel<ConsolePanel>();
                _ifConsoleEnable = false;
            }
        }           
    }

}
