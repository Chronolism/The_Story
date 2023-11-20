using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "EffectData_SO",menuName = "Data_SO/EffectData_SO")]
public class EffectData_SO : ScriptableObject
{
    public List<EffectData> effects = new List<EffectData>();
}
