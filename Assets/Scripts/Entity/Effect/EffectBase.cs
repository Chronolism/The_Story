using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectBase : MonoBehaviour
{
    Animator animator;
    Entity entity;
    public int type;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public virtual void Start()
    {
        entity = transform.parent.GetComponent<Entity>();
        if(entity!=null&&entity is Servitor)
        {
            type = 1;
        }else if(entity!=null&&entity is Player)
        {
            type = 2;
        }
    }

    public virtual void Update()
    {
        if (type == 1)
        {
            animator.SetFloat("x", entity.dir);
        }
        else if(type == 2)
        {
            animator.SetFloat("dir", entity.dir);
        }
    }
}
