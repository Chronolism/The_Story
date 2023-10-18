using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using UnityEngine.Events;

public class Mono_Main : MonoBehaviour
{
    void Start()
    {
        UIManager.Instance.ShowPanel<ConsolePanel>();

        TestConsole.Instance.AddCommand("GameStart", () => { GameManager.Instance.GameStart(); }, "默认的游戏开始");

        TestConsole.Instance.AddCommand("LocalhpUP10", () => { PlayerManager.Instance.LocalPlayer.runtime_HP += 10; }, "游戏运行时血量提升");
        TestConsole.Instance.AddCommand("Showhp", () => { TestConsole.Instance.WriteLine("本地玩家HP "+ PlayerManager.Instance.LocalPlayer.runtime_HP.ToString()); }, "显示游戏运行时血量");
        TestConsole.Instance.AddCommand("TestModeLoad", () => { GameRuntimeManager.Instance.LoadGameMode<TestGameMode>(); }, "加载游戏模式");
        TestConsole.Instance.AddCommand("CreateLocalMaxHp", () => { PlayerManager.Instance.HP_Max = 200; }, "设置角色模版:血量上限为200");
        TestConsole.Instance.AddCommand("InitLocalMaxHp", () => { PlayerManager.Instance.InitLocalPlayer(); }, "根据游戏模式加载已经设置的本地角色模版");
        TestConsole.Instance.AddCommand("NetHPChangeTo10", () => { GamePlayNetControl.Instance. Local_Net_HP = 10 ; });

        TestConsole.Instance.AddCommand("MapTest", () => { UIManager.Instance.ShowPanel<MapEditPanel>(); }, "进入地图编辑器");
        TestConsole.Instance.AddCommand("LoadMapCollusionTest", () => { MapManager.Instance.LoadMapCompletelyToScene("400"); }, "加载400碰撞测试地图");

        //TestConsole.Instance.AddCommand("TryCatchTest", () => { int x = 1;int y = 1; AbstractLogicManager.Instance.CellProbe(ref x, ref y,new V2 (0,0)); });
    }

    // Update is called once per frame
    void Update()
    {
        if (IEnableInput.GetKey(E_PlayKeys.E))
            print(IEnableInput.GetKeyCode(E_PlayKeys.E));
    }

}
