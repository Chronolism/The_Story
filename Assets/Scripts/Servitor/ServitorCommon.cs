using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServitorCommon : Base_Servitor
{
    PlayerRuntime _collisionPlayer;
    bool _needChangeDisplay = false;
    //���²���Ϊ��������
    SpriteRenderer _spriteRenderer;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<PlayerRuntime>(out _collisionPlayer) && _collisionPlayer.PlayerData != null) 
        {
            if(_collisionPlayer.PlayerData.runtime_rewrite_ink > 0 && d_servitor.master_runtime_id != _collisionPlayer.PlayerData.runtime_id )
            {
                //��������ˣ��Ƚ���Լ�
                if(this.d_servitor.master_runtime_id != 0)
                {
                    PlayerManager.Instance.GetPlayerDataWithRuntime_Id(d_servitor.master_runtime_id).runtime_myServitors.Remove(d_servitor);
                }
                this.d_servitor.master_runtime_id = _collisionPlayer.runtime_id;
                //��ͼ�ı��Լ�����ò
                this.d_servitor.servitorDisplay = _collisionPlayer.PlayerData.ownServitorDisplay;
                _needChangeDisplay = true;
                //����Լ�����Ϣ����Ҿ�����Ϣ��
                _collisionPlayer.PlayerData.runtime_myServitors.Add(d_servitor);
                //����īˮ
                _collisionPlayer.PlayerData.runtime_rewrite_ink -= d_servitor.rewrite_ink_Need * _collisionPlayer.PlayerData.runtime_rewrite_ink_NeedRate;
                if (_collisionPlayer.PlayerData.runtime_rewrite_ink < 0) _collisionPlayer.PlayerData.runtime_rewrite_ink = 0;
                //��һ������
                _collisionPlayer.PlayerData.runtime_ultimate_Skill += d_servitor.rewrite_given_ultimate_Need;
                if (_collisionPlayer.PlayerData.runtime_ultimate_Skill > _collisionPlayer.PlayerData.runtime_ultimate_Skill_Need) _collisionPlayer.PlayerData.runtime_ultimate_Skill = _collisionPlayer.PlayerData.runtime_ultimate_Skill_Need;
            }
        }
    }
    private void Awake()
    {
        InitServitor();
        //����Ϊ��������
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }
    public override void InitServitor()
    {
        base.InitServitor();
        //���˵�id(0ָ��ȫ����),����Ϸģʽ��������ʾ��ʹħʽ��(-1ָ��ȫ����)
        //����
        d_servitor.HP_Max = 100;
        d_servitor.runtime_HP_Max = 100;
        d_servitor.runtime_HP = 100;
        //�ٶ�
        d_servitor.Speed = 1;
        d_servitor.runtime_Speed = 1;
        d_servitor.runtime_Speed_Max = 1.5f;
        //����
        d_servitor.atkDamage = 1;
        //����д����īˮ��
        d_servitor.rewrite_ink_Need = 25;
        //����д���������
        d_servitor.rewrite_given_ultimate_Need = 10;
    }
    private void Update()
    {
        //����Ϊ��������
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
