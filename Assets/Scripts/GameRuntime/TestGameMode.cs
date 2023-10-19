using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGameMode:Base_GameMode
{
    public int JoinPlayerNumMax = 3;
    float _timer;

    public override void InitPlayer(D_Base_Player Player)
    {
        //血量重置
        Player.runtime_HP = Player.HP_Max;
        Player.runtime_HP_Max = Player.HP_Max;
        //速度与速度上限重置
        Player.runtime_Speed = Player.Speed;
        Player.runtime_Speed_Max = Player.Speed;
        //终极技能初始化
        Player.runtime_ultimate_Skill = Player.ultimate_Skill_Start;
        Player.runtime_ultimate_Skill_Need = Player.ultimate_Skill_Need;
        //位置初始化
        Player.runtime_gird_Position = Player.gird_Position_Start;
        //大小归一
        Player.runtime_characterFigure = 1;
        //改写墨水归一
        Player.runtime_rewrite_ink_NeedRate = Player.rewrite_ink_NeedRate;
        Player.runtime_rewrite_ink_Max = Player.rewrite_ink_Max;
        Player.runtime_rewrite_ink = 0;
        //自己的使魔列表清空
        Player.runtime_myServitors = new List<D_Servitor>();
        //buff清空
        Player.runtime_Buff = new List<D_Buff>();
        //道具情况
        Player.runtime_Tools = new List<int>();

        if (Player is D_LocalPlayer) Player.ownServitorDisplay = 0;
        else Player.ownServitorDisplay = PlayerManager.Instance.OtherPlayers.Count + 1;

    }
    public override Vector3Int GetPlayerStartPos()
    {
        if (cellsForPlayerBorn == null || cellsForPlayerBorn.Count < 1) return Vector3Int.zero;
        return cellsForPlayerBorn[0];
    }
    //将自己设置为当前的游戏模式
    public override void SetSelfAsNowaGameMode()
    {
        GameRuntimeManager.Instance.nowaGameMode = this;
    }
    //游戏运行时逻辑
    public override void GameRuntimeStart()
    {
        runtimeFeatherPenCount = 0;
        gameRuntimeData.gameLast = 0;
        gameRuntimeData.featherPenSpawnRest = 5;
        //其他使用DataSet的初始值
        _timer = 0;
    }
    public override void GameRuntimeUpdate()
    {
        if(_timer> gameRuntimeData.featherPenSpawnRest && runtimeFeatherPenCount < 1)
        {           
            var o =  Object.Instantiate(Resources.Load("FeatherPen"), MapManager.Instance.runtimeGrid.CellToWorld(cellsForFeatherPenBorn[Random.Range(0, cellsForFeatherPenBorn.Count)]), Quaternion.identity);            
            _timer = 0;
        }

        gameRuntimeData.gameLast += Time.deltaTime;
        _timer += Time.deltaTime;
    }
    
}

