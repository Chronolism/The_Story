using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using UnityEngine.Events;

public class Mono_Main : MonoBehaviour,IEnableInput
{
    void Start()
    {
        UIManager.Instance.ShowPanel<ConsolePanel>();

        


        TestConsole.Instance.AddCommand("LocalhpUP10", () => { PlayerManager.Instance.LocalPlayer.runtime_HP += 10; }, "��Ϸ����ʱѪ������");
        TestConsole.Instance.AddCommand("Showhp", () => { TestConsole.Instance.WriteLine("�������HP "+ PlayerManager.Instance.LocalPlayer.runtime_HP.ToString()); }, "��ʾ��Ϸ����ʱѪ��");
        TestConsole.Instance.AddCommand("TestModeLoad", () => { GameRuntimeManager.Instance.LoadGameMode<TestGameMode>(); }, "������Ϸģʽ");
        TestConsole.Instance.AddCommand("CreateLocalMaxHp", () => { PlayerManager.Instance.HP_Max = 200; }, "���ý�ɫģ��");
        TestConsole.Instance.AddCommand("InitLocalMaxHp", () => { PlayerManager.Instance.InitLocalPlayer(); }, "������Ϸģʽ�����Ѿ����õı��ؽ�ɫģ��");
        TestConsole.Instance.AddCommand("NetHPChangeTo10", () => { GamePlayNetControl.Instance. Local_Net_HP = 10 ; });

        TestConsole.Instance.AddCommand("MapTest", () => { UIManager.Instance.ShowPanel<MapEditPanel>(); }, "�����ͼ�༭��");

        

    }

    // Update is called once per frame
    void Update()
    {
        if (IEnableInput.GetKey(E_PlayKeys.E))
            print(IEnableInput.GetKeyCode(E_PlayKeys.E));
    }

}
