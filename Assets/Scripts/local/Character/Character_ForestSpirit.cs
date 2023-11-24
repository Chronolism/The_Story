using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_ForestSpirit : Base_Character
{
    public override int character_Code { get => 103; }
    public override float HP_Max { get => 80; }
    public override float Speed { get => 3; }
    public override float atkDamage { get => 1; }
    public override float ultimate_Skill_Need { get => 60; }
    public override float ultimate_Skill_Start { get => 10; }
    public override List<string> special_Tags { get => new List<string>() { "Red" }; }
    public override float rewrite_ink_NeedRate { get => 1; }
    public override float rewrite_ink_Max { get => 100; }
    public override float rewrite_ink_MaxLastTime { get => 10; }
    public override List<int> skill_Index { get => new List<int>(); }
    public override bool Ultimate_Skill(int Choice = 0)
    {
        return false;
    }
    public override bool Passive_Skill(int Choice = 0)
    {
        return false;
    }
}