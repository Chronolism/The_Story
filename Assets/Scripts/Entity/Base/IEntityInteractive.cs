using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEntityInteractive 
{
    public void Interactive(Entity entity);
    public void OnActive();
    public void OnNotActive();
}
