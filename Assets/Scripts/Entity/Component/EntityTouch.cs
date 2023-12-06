using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityTouch : EntityComponent
{

    [Tooltip("攻击间隔时间，单位毫秒")]
    public int atkTime;
    List<IEntityTouch> trigerEntities = new List<IEntityTouch>();

    IEntityTouch tager;

    public override void Init(Entity entity)
    {
        base.Init(entity);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isServer)
        {
            tager = collision.GetComponent<IEntityTouch>();
            if (tager != null)
            {

                trigerEntities.Add(tager);
                tager.Touch(entity);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (isServer)
        {
            tager = collision.GetComponent<IEntityTouch>();
            if (tager != null)
            {
                trigerEntities.Remove(tager);
            }
        }
    }

    public virtual void FixedUpdate()
    {
        if (!isServer) return;
        if (!entity.ifPause)
        {

        }
    }

    public static Quaternion LookAt2D(Vector3 start, Vector3 end)
    {
        return Quaternion.AngleAxis((Vector3.Cross(start.normalized, end.normalized).z > 0 ? 1 : -1) * Mathf.Acos(Vector3.Dot(start.normalized, end.normalized)) * Mathf.Rad2Deg, new Vector3(0, 0, 1));
    }
}
