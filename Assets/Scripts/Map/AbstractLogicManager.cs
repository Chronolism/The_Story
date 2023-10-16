using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 路径管理&抽象逻辑运算中枢 用于高强度数据处理
/// </summary>
public class AbstractLogicManager : BaseManager<AbstractLogicManager>
{
    //运行时的逻辑地图
    PathNode[,] _runtimeLogicMap;
    public PathNode[,] runtimeLogicMap
    {
        get
        {
            //首次访问初始化
            if (_runtimeLogicMap == null)
            {
                _runtimeLogicMap = new PathNode[logicMapSize.x, logicMapSize.y];
                for (int i = 0; i < _runtimeLogicMap.GetLength(0); i++)
                {
                    for (int j = 0; j < _runtimeLogicMap.GetLength(1); j++)
                    {
                        _runtimeLogicMap[i, j] = new PathNode(i, j, E_Node_Type.Walk);
                    }
                }
            }
            return _runtimeLogicMap;
        }
    }
    public V2 logicMapSize;
    public V2 logicToDisplayOffset;//逻辑地图偏移量，用于适配原点不在0,0的地图0.
    //逻辑地图碰撞探针
    [System.Obsolete("有问题的代码")]
    public void CellProbe(ref int UpDown, ref int RightLeft, V2 Pos)
    {
        bool UpLimit = false;
        bool DownLimit = false;
        bool RightLimit = false;
        bool LeftLimit = false;
        int tempX = Pos.x - logicToDisplayOffset.x;
        int tempY = Pos.y - logicToDisplayOffset.y;
        Pos = new V2(tempX , tempY);
        bool OverLimit = (Pos.y + 1 > _runtimeLogicMap.GetLength(1)) || (Pos.x + 1 > _runtimeLogicMap.GetLength(0)) || Pos.y - 1 < 0 || Pos.x - 1 < 0;
        if (OverLimit || _runtimeLogicMap[Pos.x, Pos.y + 1].type == E_Node_Type.Stop) UpLimit = true;
        if (OverLimit || _runtimeLogicMap[Pos.x, Pos.y - 1].type == E_Node_Type.Stop) DownLimit = true;
        if (OverLimit || _runtimeLogicMap[Pos.x + 1, Pos.y].type == E_Node_Type.Stop) RightLimit = true; 
        if (OverLimit || _runtimeLogicMap[Pos.x - 1, Pos.y].type == E_Node_Type.Stop) LeftLimit = true;
        if (UpDown == 1 && UpLimit) UpDown = 0;
        if (UpDown == -1 && DownLimit) UpDown = 0;
        if (RightLeft == 1 && RightLimit) RightLeft = 0;
        if (RightLeft == -1 && LeftLimit) RightLeft = 0;
        if (UpLimit || RightLimit || DownLimit || LeftLimit) Debug.LogWarning("CellProbe:True in" + Pos.x + "," + Pos.y);
    }

}

public class PathNode
{
    //格子对象的坐标
    public int x;
    public int y;

    //寻路消耗
    public float f;
    //离起点的距离
    public float g;
    //离终点的距离
    public float h;
    //父对象
    public AStarNode father;

    //格子的类型
    public E_Node_Type type;

    /// <summary>
    /// 构造函数 传入坐标和格子类型
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="type"></param>
    public PathNode(int x, int y, E_Node_Type type)
    {
        this.x = x;
        this.y = y;
        this.type = type;
    }
}