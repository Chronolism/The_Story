using System;
using UnityEngine;
using System.Reflection;
using System.Collections.Generic;
using System.Text;
using System.Collections;
//Player��NPC��Monster��ʵ��ĸ�������
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(AudioSource))]



public class Entity : NetworkBehaviour
{
    [Header("Entity Property")]


    public Animator animator;
    new public Collider collider;



    //ԭʼ����
    float text = 2048;
    //ͬ������
    //���幫�����Բ��������
    //��������
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
    //����list
    public List<int> list = new List<int>();

    [NetWork]
    public List<int> List
    {
        get { return list; }
        set { list = value; }
    }
    //��������
    public int[] ints = new int[2];

    [NetWork]
    public int[] Ints
    {
        get { return ints; }
        set { ints = value; }
    }
}