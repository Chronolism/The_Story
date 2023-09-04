using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayNetControl : BaseManager<GamePlayNetControl>
{
    public float Local_Net_HP { get { return PlayerManager.Instance.LocalPlayer.runtime_HP; }set { PlayerManager.Instance.LocalPlayer.runtime_HP = value; } }
  
}
