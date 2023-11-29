﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// A星寻路管理器 单例模式
/// </summary>
public class AStarMgr : BaseManager<AStarMgr>
{
    //地图的宽高
    public int mapW;
    public int mapH;

    public int deviationW;
    public int deviationH;


    //地图相关所有的格子对象容器
    public AStarNode[,] nodes;
    //开启列表
    private List<AStarNode> openList = new List<AStarNode>();
    //关闭列表
    private List<AStarNode> closeList = new List<AStarNode>();

    /// <summary>
    /// 初始化地图信息
    /// </summary>
    /// <param name="w"></param>
    /// <param name="h"></param>
    public void InitMapInfo(int w, int h, AStarNode[,] aStarNodes )
    {
        //根据宽高 创建格子 阻挡的问题 我们可以随机阻挡 
        //因为我们现在没有地图相关的数据

        //记录宽高 范围
        this.mapW = w;
        this.mapH = h;

        //申明容器可以装多少个格子
        nodes = aStarNodes;

    }


    public void InitMapInfo(Dictionary<Vector3Int , bool> map)
    {
        var kvpf = map.First();
        deviationW = kvpf.Key.x;
        deviationH = kvpf.Key.y;
        foreach (var kvp in map)
        {
            if (kvp.Key.x < deviationW) deviationW = kvp.Key.x;
            else if (kvp.Key.x >= mapW + deviationW) mapW = kvp.Key.x - deviationW + 1;
            if (kvp.Key.y < deviationH) deviationH = kvp.Key.y;
            else if (kvp.Key.y >= mapH + deviationH) mapH = kvp.Key.y - deviationH + 1;
        }
        nodes = new AStarNode[mapW, mapH];
        foreach (var kvp in map)
        {
            nodes[kvp.Key.x - deviationW, kvp.Key.y - deviationH] = new AStarNode(kvp.Key.x - deviationW, kvp.Key.y - deviationH, kvp.Value ? E_Node_Type.Stop : E_Node_Type.Walk);
        }
    }

    public void InitMapInfo(Dictionary<V2,MapColliderType> colliders)
    {
        var kvpf = colliders.First();
        deviationW = kvpf.Key.x;
        deviationH = kvpf.Key.y;
        foreach (var kvp in colliders)
        {
            if (kvp.Key.x < deviationW) deviationW = kvp.Key.x;
            else if (kvp.Key.x >= mapW + deviationW) mapW = kvp.Key.x - deviationW + 1;
            if (kvp.Key.y < deviationH) deviationH = kvp.Key.y;
            else if (kvp.Key.y >= mapH + deviationH) mapH = kvp.Key.y - deviationH + 1;
        }
        nodes = new AStarNode[mapW, mapH];
        for (int i = 0; i < mapW; i++)
            for (int j = 0; j < mapH; j++)
                nodes[i, j] = new AStarNode(i, j, 0);
        foreach (var kvp in colliders)
        {
            nodes[kvp.Key.x - deviationW, kvp.Key.y - deviationH].type = kvp.Value == MapColliderType.Wall ? E_Node_Type.Stop : E_Node_Type.Walk;
        }
    }

    /// <summary>
    /// 寻路方法 提供给外部使用
    /// </summary>
    /// <param name="startPos"></param>
    /// <param name="endPos"></param>
    /// <returns></returns>
    public async void FindPath(Vector2 startPos, Vector2 endPos , UnityAction<List<AStarNode>> callback , bool removeFirst = true)
    {
        await Task.Yield();
        //实际项目中 传入的点往往是 坐标系中的位置
        //我们这里省略换算的步骤  直接认为它是传进来的格子坐标
        startPos.x = (int)(startPos.x - deviationW);
        startPos.y = (int)(startPos.y - deviationH);
        endPos.x = (int)(endPos.x - deviationW);
        endPos.y = (int)(endPos.y - deviationH);

        //首先判断 传入的两个点 是否合法
        //如果不合法 应该直接 返回null 意味着不能寻路
        //1.首先 要在地图范围内
        if (startPos.x < 0 || startPos.x >= mapW ||
            startPos.y < 0 || startPos.y >= mapH ||
            endPos.x < 0 || endPos.x >= mapW ||
            endPos.y < 0 || endPos.y >= mapH)
        {
            Debug.Log("开始或者结束点在地图格子范围外");
            callback(null);
            return;
        }

        //2.要不是阻挡
        //应该得到起点和终点 对应的格子
        AStarNode start = nodes[(int)startPos.x, (int)startPos.y];
        AStarNode end = nodes[(int)endPos.x, (int)endPos.y];
        if (start.type == E_Node_Type.Stop ||
            end.type == E_Node_Type.Stop)
        {
            //Debug.Log("开始或者结束点是阻挡");
            callback(null);
            return;
        }

        if (start.x == end.x && start.y == end.y)
        {
            //Debug.Log("开始为结束点");
            callback(null);
            return;
        }

        //清空上一次相关的数据 避免他们影响 这一次的寻路计算

        //清空关闭和开启列表
        closeList.Clear();
        openList.Clear();

        //把开始点放入关闭列表中
        start.father = null;
        start.f = 0;
        start.g = 0;
        start.h = 0;
        closeList.Add(start);

        while(true)
        {
            //从起点开始 找周围的点 并放入开启列表中
            //左上 x - 1  y - 1
            //FindNearlyNodeToOpenList(start.x - 1, start.y - 1, 1.4f, start, end);
            //上 x  y -1
            FindNearlyNodeToOpenList(start.x, start.y - 1, 1, start, end);
            //右上 x + 1 y - 1
            //FindNearlyNodeToOpenList(start.x + 1, start.y - 1, 1.4f, start, end);
            //左 x - 1 y
            FindNearlyNodeToOpenList(start.x - 1, start.y, 1, start, end);
            //右 x + 1 y
            FindNearlyNodeToOpenList(start.x + 1, start.y, 1, start, end);
            //左下 x-1 y +1
            //FindNearlyNodeToOpenList(start.x - 1, start.y + 1, 1.4f, start, end);
            //下 x y + 1
            FindNearlyNodeToOpenList(start.x, start.y + 1, 1, start, end);
            //右下 x+1 y+1
            //FindNearlyNodeToOpenList(start.x + 1, start.y + 1, 1.4f, start, end);
            
            //思路判断 开启列表为空 都还没有找到终点 就认为是思路
            if (openList.Count == 0)
            {
                Debug.Log("死路");
                callback(null);
                return;
            }

            //选出开启列表中 寻路消耗最小的点
            openList.Sort(SortOpenList);
            //for ( int i = 0; i < openList.Count; ++i )
            //{
            //    AStarNode node = openList[i];
            //    //Debug.Log("点" + node.x + "," + node.y + ":g=" + node.g + "h=" + node.h + "f=" + node.f);
            //}

            //放入关闭列表中 然后再从开启列表中移除
            closeList.Add(openList[0]);
            //找得这个点 又编程新的起点 进行下一次寻路计算了
            start = openList[0];
            openList.RemoveAt(0);

            //如果这个点已经是终点了 那么得到最终结果返回出去
            //如果这个点 不是终点 那么继续寻路
            if (start == end)
            {
                //找完了 找到路径了
                List<AStarNode> path = new List<AStarNode>();
                end.pos = new Vector2(end.x + deviationW + 0.5f, end.y + deviationH + 0.5f);
                path.Add(end);
                while(end.father != null)
                {
                    path.Add(end.father);
                    end = end.father;
                    end.pos = new Vector2(end.x + deviationW + 0.5f, end.y + deviationH + 0.5f);
                }
                //列表翻转的API
                path.Reverse();
                if (removeFirst)
                {
                    path.Remove(path.First());
                }
                callback(path);
                return;
            }
        }
    }

    /// <summary>
    /// 排序函数
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    private int SortOpenList(AStarNode a, AStarNode b)
    {
        if (a.f > b.f)
            return 1;
        else if (a.f == b.f)
            return 1;
        else
            return -1;
    }

    /// <summary>
    /// 把临近的点放入开启列表中的函数
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    private void FindNearlyNodeToOpenList(int x, int y, float g, AStarNode father, AStarNode end)
    {
        //边界判断
        if (x < 0 || x >= mapW ||
            y < 0 || y >= mapH)
            return;
        //在范围内 再去取点
        AStarNode node = nodes[x, y];

        //判断这些点 是否是边界 是否是阻挡  是否在开启或者关闭列表 如果都不是 才放入开启列表
        if (node == null||
            node.type == E_Node_Type.Stop ||
            closeList.Contains(node) ||
            openList.Contains(node) )
            return;
        //计算f值
        //f = g + h
        //记录父对象
        node.father = father;
        //计算g  我离起点的距离 就是我父亲离起点的距离 + 我离我父亲的距离
        node.g = father.g + g;
        node.h = Mathf.Abs(end.x - node.x) + Mathf.Abs(end.y - node.y);
        node.f = node.g + node.h;

        //Debug.Log("点" + node.x + "," + node.y + ":g=" + node.g + "h=" + node.h);

        //如果通过了上面的合法验证 就存到开启列表中
        openList.Add(node);
    }

    public bool ChackType(float x,float y,E_Node_Type type)
    {
        x -= deviationW;
        y -= deviationH;
        if(x < 0 || x >= mapW ||
            y< 0 || y >= mapH)
        {
            return false;
        }
        if (nodes[(int)x,(int)y].type == type)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

}
