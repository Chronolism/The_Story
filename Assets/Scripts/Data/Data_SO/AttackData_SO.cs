using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AttackData_SO",menuName = "Data_SO/AttackData_SO")]
public class AttackData_SO : ScriptableObject
{
    public List<AttackData> attackDatas = new List<AttackData>();
}
