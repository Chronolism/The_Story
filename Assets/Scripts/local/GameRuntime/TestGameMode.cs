using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGameMode:Base_GameMode
{
    public int JoinPlayerNumMax = 3;
    float _timer;

    public override void InitPlayer(D_Base_Player Player)
    {
        //Ѫ������
        Player.runtime_HP = Player.HP_Max;
        Player.runtime_HP_Max = Player.HP_Max;
        //�ٶ����ٶ���������
        Player.runtime_Speed = Player.Speed;
        Player.runtime_Speed_Max = Player.Speed;
        //�ռ����ܳ�ʼ��
        Player.runtime_ultimate_Skill = Player.ultimate_Skill_Start;
        Player.runtime_ultimate_Skill_Need = Player.ultimate_Skill_Need;
        //λ�ó�ʼ��
        Player.runtime_gird_Position = Player.gird_Position_Start;
        //��С��һ
        Player.runtime_characterFigure = 1;
        //��дīˮ��һ
        Player.runtime_rewrite_ink_NeedRate = Player.rewrite_ink_NeedRate;
        Player.runtime_rewrite_ink_Max = Player.rewrite_ink_Max;
        Player.runtime_rewrite_ink = 0;
        //�Լ���ʹħ�б����
        Player.runtime_myServitors = new List<D_Servitor>();
        //buff���
        Player.runtime_Buff = new List<D_Buff>();
        //�������
        Player.runtime_Tools = new List<int>();

        if (Player is D_LocalPlayer) Player.ownServitorDisplay = 0;
        else Player.ownServitorDisplay = PlayerManager.Instance.OtherPlayers.Count + 1;

    }
    public override Vector3Int GetPlayerStartPos()
    {
        if (cellsForPlayerBorn == null || cellsForPlayerBorn.Count < 1) return Vector3Int.zero;
        return cellsForPlayerBorn[0];
    }
    //���Լ�����Ϊ��ǰ����Ϸģʽ
    public override void SetSelfAsNowaGameMode()
    {
        GameRuntimeManager.Instance.nowaGameMode = this;
    }
    //��Ϸ����ʱ�߼�
    public override void GameRuntimeStart()
    {
        runtimeFeatherPenCount = 0;
        gameRuntimeData.gameLast = 0;
        gameRuntimeData.featherPenSpawnRest = 5;
        //����ʹ��DataSet�ĳ�ʼֵ
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

