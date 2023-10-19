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
        if (collision.TryGetComponent<PlayerRuntime>(out _collisionPlayer)) 
        {
            if(_collisionPlayer.PlayerData.runtime_rewrite_ink > 0)
            {
                //如果有主人，先解放自己
                if(this.d_servitor.master_runtime_id != 0)
                {
                    PlayerManager.Instance.GetPlayerDataWithRuntime_Id(d_servitor.master_runtime_id).runtime_myServitors.Remove(d_servitor);
                }
                this.d_servitor.master_runtime_id = _collisionPlayer.runtime_id;
                //试图改变自己的样貌
                this.d_servitor.ServitorDisplay = _collisionPlayer.PlayerData.ownServitorDisplay;
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
    }
    private void Update()
    {
        //以下为测试内容
        if (_needChangeDisplay)
        {
            switch (d_servitor.ServitorDisplay)
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
