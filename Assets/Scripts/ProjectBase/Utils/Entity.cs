using System;
using UnityEngine;
using System.Reflection;
using System.Collections.Generic;
using System.Text;
using System.Collections;
//Player，NPC，Monster等实体的父对象类
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(AudioSource))]



public class Entity : NetworkBehaviour
{
    [Header("Entity Property")]


    public Animator animator;
    new public Collider collider;



    //原始数据
    float text = 2048;
    //同步属性
    //定义公共属性并标记特性
    //允许类型
    //string
    //byte
    //bool
    //short
    //int
    //long
    //float
    //Array
    //Class
    [NetWork]
    public int Text
    {
        get { return (int)text; }
        set { text = value; }
    }
    //测试list
    public List<int> list = new List<int>();

    [NetWork]
    public List<int> List
    {
        get { return list; }
        set { list = value; }
    }
    //测试数组
    public int[] ints = new int[2];

    [NetWork]
    public int[] Ints
    {
        get { return ints; }
        set { ints = value; }
    }
}