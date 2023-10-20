using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Base_Character 
{
    public abstract int character_Code { get ; }
    public abstract float HP_Max { get ; }
    public abstract float Speed { get ; }
    public abstract float atkDamage { get; }
    public abstract float ultimate_Skill_Need { get ; }
    public abstract float ultimate_Skill_Start { get ; }
    public abstract List<string> special_Tags { get ; }
    public abstract float rewrite_ink_NeedRate { get ; }
    public abstract float rewrite_ink_Max { get; }
    public abstract float rewrite_ink_MaxLastTime { get; }
    public abstract List<int> skill_Index { get ; }

    public virtual void InitLocalPlayCharacter()
    {
        PlayerManager.Instance.character_Code = character_Code;
        PlayerManager.Instance.HP_Max = HP_Max;
        PlayerManager.Instance.Speed = Speed;
        PlayerManager.Instance.atkDamage = atkDamage;
        PlayerManager.Instance.ultimate_Skill_Need = ultimate_Skill_Need;
        PlayerManager.Instance.ultimate_Skill_Start= ultimate_Skill_Start;
        PlayerManager.Instance.special_Tags = special_Tags;
        PlayerManager.Instance.rewrite_ink_NeedRate= rewrite_ink_NeedRate;
        PlayerManager.Instance.rewrite_ink_Max = rewrite_ink_Max;
        PlayerManager.Instance.rewrite_ink_MaxLastTime = rewrite_ink_MaxLastTime;
        PlayerManager.Instance.skill_Index = skill_Index;
    }
    public virtual bool Ultimate_Skill()
    {
        return false;
    }
    public virtual bool Passive_Skill()
    {
        return false;
    }
}
