using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpriteData_SO",menuName = "Data_SO/SpriteData_SO")]
public class SpriteData_SO : ScriptableObject
{
    public List<SpriteData> spriteDataSet = new List<SpriteData>();
}
