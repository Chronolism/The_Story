using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public class BuffData
{
    public int id;
    public string img;
    public string name;
    public string description;
    public int quality;
    public float weight;
    public int type;
    public int times;
    public List<int> black = new List<int>();
    public List<int> white = new List<int>();
    public float attribute;
}
