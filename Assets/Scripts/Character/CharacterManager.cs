using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : BaseManager<CharacterManager>
{
    //�泡�������еĽ�ɫ������
    public Dictionary<int, Base_Character> characterDic = new Dictionary<int, Base_Character>()
    {
        { 405 , new Character_Test()}
    
    };



}
