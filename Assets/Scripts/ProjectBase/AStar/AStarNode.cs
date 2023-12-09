using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 格子类型
/// </summary>
public enum E_Node_Type
{
    //可以走的地方
    Walk,
    //不能走的阻挡
    Stop,
    Special,
}

/// <summary>
/// A星格子类
/// </summary>
public class AStarNode
{
    //格子对象的坐标
    public int x;
    public int y;
    //世界坐标
    public Vector2 pos;

    //寻路消耗
    public float f;
    //离起点的距离
    public float g;
    //离终点的距离
    public float h;
    //父对象
    public AStarNode father;

    //格子的类型
    public MapColliderType type;

    /// <summary>
    /// 构造函数 传入坐标和格子类型
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="type"></param>
    public AStarNode( int x, int y, MapColliderType type )
    {
        this.x = x;
        this.y = y;
        this.type = type;
    }

    public AStarNode(int x, int y, MapColliderType type ,int dx,int dy)
    {
        this.x = x;
        this.y = y;
        this.type = type;
        pos = new Vector2(x + dx + 0.5f, y + dy + 0.5f);
    }

    public bool ChackType(MapColliderType mapColliderType)
    {
        return (type & mapColliderType) == type;
    }
}
