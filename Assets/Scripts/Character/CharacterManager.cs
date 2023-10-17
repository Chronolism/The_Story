using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : BaseManager<CharacterManager>
{
    //存场景上所有的角色的索引
    public Dictionary<int, Base_Character> characterDic = new Dictionary<int, Base_Character>()
    {
        { 405 , new Character_Test()}
    
    };



}
