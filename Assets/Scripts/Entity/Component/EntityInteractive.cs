using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityInteractive : EntityComponent
{
    public List<IEntityInteractive> builds = new List<IEntityInteractive>();

    IEntityInteractive bb;

    public bool TrigerBuild()
    {
        if (builds.Count == 0) return false;
        builds[0].Interactive(entity);
        return true;
    }


    public void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.TryGetComponent<IEntityInteractive>(out bb))
        {
            builds.Add(bb);
            if (DataMgr.Instance.activePlayer == entity)
            {
                bb.OnActive();
            }
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent<IEntityInteractive>(out bb))
        {
            builds.Remove(bb);
            if (DataMgr.Instance.activePlayer == entity)
            {
                bb.OnNotActive();
            }

        }
    }

    private void OnDisable()
    {
        if (DataMgr.Instance.activePlayer == entity)
        {
            foreach (var build in builds)
            {
                build.OnNotActive();
            }
        }
    }
}
