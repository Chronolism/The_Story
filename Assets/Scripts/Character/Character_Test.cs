using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_Test : Base_Character
{
    public int character_Code { get => 402; }
    public float HP_Max { get => 100; }
    public float Speed { get => 1; }
    public float atkDamage { get => 1; }
    public float ultimate_Skill_Need { get => 100; }
    public float ultimate_Skill_Start { get => 10; }
    public List<string> special_Tags{ get => new List<string>() { "Test" }; }
    public float rewrite_ink_NeedRate { get => 1; }
    public float rewrite_ink_Max { get => 100; }
    public float rewrite_ink_MaxLastTime { get => 10; }
    public List<int> skill_Index { get => new List<int>() { 403 };}


}
