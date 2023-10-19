using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServitorCommon : Base_Servitor
{
    PlayerRuntime _collisionPlayer;
    bool _needChangeDisplay = false;
    //以下参数为测试内容
    SpriteRenderer _spriteRenderer;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<PlayerRuntime>(out _collisionPlayer) && _collisionPlayer.PlayerData != null) 
        {
            if(_collisionPlayer.PlayerData.runtime_rewrite_ink > 0 && d_servitor.master_runtime_id != _collisionPlayer.PlayerData.runtime_id )
            {
                //如果有主人，先解放自己
                if(this.d_servitor.master_runtime_id != 0)
                {
                    PlayerManager.Instance.GetPlayerDataWithRuntime_Id(d_servitor.master_runtime_id).runtime_myServitors.Remove(d_servitor);
                }
                this.d_servitor.master_runtime_id = _collisionPlayer.runtime_id;
                //试图改变自己的样貌
                this.d_servitor.servitorDisplay = _collisionPlayer.PlayerData.ownServitorDisplay;
                _needChangeDisplay = true;
                //添加自己的信息到玩家局内信息中
                _collisionPlayer.PlayerData.runtime_myServitors.Add(d_servitor);
                //消耗墨水
                _collisionPlayer.PlayerData.runtime_rewrite_ink -= d_servitor.rewrite_ink_Need * _collisionPlayer.PlayerData.runtime_rewrite_ink_NeedRate;
                if (_collisionPlayer.PlayerData.runtime_rewrite_ink < 0) _collisionPlayer.PlayerData.runtime_rewrite_ink = 0;
                //玩家获得能量
                _collisionPlayer.PlayerData.runtime_ultimate_Skill += d_servitor.rewrite_given_ultimate_Need;
                if (_collisionPlayer.PlayerData.runtime_ultimate_Skill > _collisionPlayer.PlayerData.runtime_ultimate_Skill_Need) _collisionPlayer.PlayerData.runtime_ultimate_Skill = _collisionPlayer.PlayerData.runtime_ultimate_Skill_Need;
            }
        }
    }
    private void Awake()
    {
        InitServitor();
        //以下为测试内容
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }
    public override void InitServitor()
    {
        base.InitServitor();
        //主人的id(0指完全中立),由游戏模式管理赋予显示的使魔式样(-1指完全中立)
        //生命
        d_servitor.HP_Max = 100;
        d_servitor.runtime_HP_Max = 100;
        d_servitor.runtime_HP = 100;
        //速度
        d_servitor.Speed = 1;
        d_servitor.runtime_Speed = 1;
        d_servitor.runtime_Speed_Max = 1.5f;
        //攻击
        d_servitor.atkDamage = 1;
        //被改写消耗墨水量
        d_servitor.rewrite_ink_Need = 25;
        //被改写给予的能量
        d_servitor.rewrite_given_ultimate_Need = 10;
    }
    private void Update()
    {
        //以下为测试内容
        if (_needChangeDisplay)
        {
            switch (d_servitor.servitorDisplay)
            {
                case -1:
                    _spriteRenderer.color = Color.white;
                    break;
                case 0:
                    _spriteRenderer.color = Color.red;
                    break;
                case 1:
                    _spriteRenderer.color = Color.blue;
                    break;
            }
        }
    }
}
