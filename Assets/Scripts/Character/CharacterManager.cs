using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : BaseManager<CharacterManager>
{
    //�泡�������еĽ�ɫ������
    public Dictionary<string, Base_Character> characterDic = new Dictionary<string, Base_Character>() 
    {
        {"Test",new Character_Test()}
    
    };



}
