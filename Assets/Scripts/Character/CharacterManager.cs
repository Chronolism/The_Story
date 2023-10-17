using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : BaseManager<CharacterManager>
{
    //存场景上所有的角色的索引
    public Dictionary<string, Base_Character> characterDic = new Dictionary<string, Base_Character>() 
    {
        {"Test",new Character_Test()}
    
    };



}
