using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterData_SO",menuName = "Data_SO/CharacterData_SO")]
public class CharacterData_SO :ScriptableObject
{
    public List<CharacterData> characterDatas = new List<CharacterData>();
}
