using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ServitorCommon : Base_Servitor
{
    PlayerRuntime _collisionPlayer;
    bool _needChangeDisplay = false;
    //���²���Ϊ��������
    Animator _animator;
    Rigidbody2D _rigidbody;
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
                //����׷��Ŀ��
                this.target = _collisionPlayer.transform;
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
        _animator = GetComponentInChildren<Animator>();
        _rigidbody = GetComponentInChildren<Rigidbody2D>();
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
            _animator.SetInteger("displayType", d_servitor.servitorDisplay + 1);
            /*
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
            */
        }
    }

    
    #region Ѱ·ģ�飨���Դ��룩
    List<AStarNode> path = new List<AStarNode>();
    float time;
    int pathIndex = 0;
    bool havePath;
    Vector2 movement;
    Transform target;
    Transform parent;
    Rigidbody2D rb => _rigidbody;
    float speed => d_servitor.runtime_Speed;
    private void FixedUpdate()
    {
        Movement();
        FindTarget();
        time += Time.fixedDeltaTime;
    }
    private void FindTarget()
    {
        if (time >= 0.5)
        {
            time = 0;
            if (target == null || target == parent)
            {
                float distance = 999;
                foreach (var player in PlayerManager.Instance.GetAllPlayerDataList())
                {
                    if (Vector3.Distance(transform.position, player.runtime_Player.transform.position) < distance)
                    {
                        distance = Vector3.Distance(transform.position, player.runtime_Player.transform.position);
                        target = player.runtime_Player.transform;
                    }
                }
            }

            //AStarMgr.Instance.FindPath(rb.position, target.GetComponent<Rigidbody2D>().position, FindPathCallBack, false);
        }
        else
        {
            time += Time.deltaTime;
        }

    }
    void FindPathCallBack(List<AStarNode> aStarNodes)
    {
        if (aStarNodes == null || aStarNodes.Count == 0)
        {
            havePath = false;
            return;
        }

        path = aStarNodes;
        havePath = true;
        if (path.Count == 1)
        {
            movement = new Vector2(path[0].x, path[0].y);
            return;
        }

        if (Vector2.Distance(rb.position, path[1].pos) > Vector2.Distance(path[0].pos, path[1].pos))
        {
            pathIndex = 0;
            movement = path[0].pos;
        }
        else
        {
            pathIndex = 1;
            movement = path[1].pos;
        }

    }

    private void Movement()
    {

        if (Vector2.Distance(rb.position, movement) > 0.01)
        {
            rb.MovePosition(rb.position + (movement - rb.position).normalized * speed * Time.deltaTime);
        }
        else if (havePath)
        {
            if (pathIndex == path.Count - 1)
            {
                havePath = false;
            }
            else
            {
                pathIndex++;
                movement = path[pathIndex].pos;
                rb.MovePosition(rb.position + (movement - rb.position).normalized * speed * Time.deltaTime);
            }
        }
    }
    #endregion
}
