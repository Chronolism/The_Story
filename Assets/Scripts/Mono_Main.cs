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

        


        TestConsole.Instance.AddCommand("LocalhpUP10", () => { PlayerManager.Instance.LocalPlayer.runtime_HP += 10; });
        TestConsole.Instance.AddCommand("Showhp", () => { TestConsole.Instance.WriteLine("±¾µØÍæ¼ÒHP "+ PlayerManager.Instance.LocalPlayer.runtime_HP.ToString()); });
        TestConsole.Instance.AddCommand("TestModeLoad", () => { GameRuntimeManager.Instance.LoadGameMode<TestGameMode>(); });
        TestConsole.Instance.AddCommand("CreateLocalMaxHp", () => { PlayerManager.Instance.HP_Max = 200; });
        TestConsole.Instance.AddCommand("InitLocalMaxHp", () => { PlayerManager.Instance.InitLocalPlayer(); });
        TestConsole.Instance.AddCommand("NetHPChangeTo10", () => { GamePlayNetControl.Instance. Local_Net_HP = 10 ; });

        TestConsole.Instance.AddCommand("MapTest", () => { UIManager.Instance.ShowPanel<MapEditPanel>(); });



    }

    // Update is called once per frame
    void Update()
    {
        if (IEnableInput.GetKey(E_PlayKeys.E))
            print(IEnableInput.GetKeyCode(E_PlayKeys.E));
    }

}
