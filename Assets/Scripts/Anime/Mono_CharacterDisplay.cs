using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mono_CharacterDisplay : MonoBehaviour
{
    public Animator animator;
    Vector3 _lastPos;
    float _towardsX = 1;
    float _towardsY = 0;
    public bool allowAnimeRun = true;
    private void Awake()
    {
        if (this.TryGetComponent<Animator>(out animator)) allowAnimeRun = !GameManager.ThrowError(501);
    }
    void Start()
    {
        _lastPos = this.transform.position;
    }


    void Update()
    {
        if (allowAnimeRun)
        {
            if (animator == null) this.TryGetComponent<Animator>(out animator);
            else
            {
                if (this.transform.position != _lastPos)
                {
                    Vector3 moveOffset = this.transform.position - _lastPos;
                    float nowTowardsX = (moveOffset.x > 0) ? 1 : -1;
                    float nowTowardsY = (moveOffset.y > 0) ? 1 : -1;
                    if (nowTowardsX != _towardsX) //ֻ�иı䳯��Ž�״̬��//����AD
                    {
                        animator.SetFloat("AD", nowTowardsX);
                        animator.SetFloat("WS", 0);
                    }
                    else if (nowTowardsY != _towardsY)
                    {
                        animator.SetFloat("WS", nowTowardsY);
                        animator.SetFloat("AD", 0);
                    }
                    _towardsX = nowTowardsX;
                    _towardsY = nowTowardsY;
                    animator.SetFloat("isMove", 1);
                }
                else
                {
                    animator.SetFloat("isMove", 0);
                }
            }
        }
        _lastPos = this.transform.position;
    }

}
