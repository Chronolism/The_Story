using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public class BuffData
{
    public int id;
    public int spriteIndex;
    public Sprite img;
    public string name;
    public string description;
    public float cd;
    public int energy;
    public int maxEnergy;
    public int times;
    public List<int> black = new List<int>();
    public List<int> white = new List<int>();
    public float attribute;
}
