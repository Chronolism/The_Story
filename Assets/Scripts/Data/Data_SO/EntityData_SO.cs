using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EntityData_SO",menuName = "Data_SO/EntityData_SO")]
public class EntityData_SO : ScriptableObject
{
    public List<EntityData> entities = new List<EntityData>();
}
