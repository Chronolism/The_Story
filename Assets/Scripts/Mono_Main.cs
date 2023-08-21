using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mono_Main : MonoBehaviour
{
   
    void Start()
    {
        UIManager.Instance.ShowPanel<ConsolePanel>();


        

        TestConsole.Instance.AddCommand("LocalhpUP10", () => { PlayerManager.Instance.LocalPlayer.runtime_HP += 10; });
        TestConsole.Instance.AddCommand("Showhp", () => { TestConsole.Instance.WriteLine("±¾µØÍæ¼ÒHP "+ PlayerManager.Instance.LocalPlayer.runtime_HP.ToString()); });
        TestConsole.Instance.AddCommand("TestModeLoad", () => { GameRuntimeManager.Instance.LoadGameMode<TestGameMode>(); });
        TestConsole.Instance.AddCommand("CreateLocalMaxHp", () => { PlayerManager.Instance.HP_Max = 200; });
        TestConsole.Instance.AddCommand("InitLocalMaxHp", () => { PlayerManager.Instance.InitLocalPlayer(); });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
