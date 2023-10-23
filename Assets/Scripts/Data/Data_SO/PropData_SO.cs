using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PropData_SO",menuName = "Data_SO/PropData_SO")]
public class PropData_SO : ScriptableObject
{
    public List<PropData> propDatas = new List<PropData>();
}
