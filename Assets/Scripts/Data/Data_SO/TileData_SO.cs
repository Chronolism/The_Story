using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "TileData_SO",menuName = "Data_SO/TileData_SO")]
public class TileData_SO : ScriptableObject
{
    public List<TileDataList> tileDatas = new List<TileDataList>();
}