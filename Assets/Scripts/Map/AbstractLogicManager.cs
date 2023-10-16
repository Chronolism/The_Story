using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ·������&�����߼��������� ���ڸ�ǿ�����ݴ���
/// </summary>
public class AbstractLogicManager : BaseManager<AbstractLogicManager>
{
    //����ʱ���߼���ͼ
    PathNode[,] _runtimeLogicMap;
    public PathNode[,] runtimeLogicMap
    {
        get
        {
            //�״η��ʳ�ʼ��
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
    public V2 logicToDisplayOffset;//�߼���ͼƫ��������������ԭ�㲻��0,0�ĵ�ͼ0.
    //�߼���ͼ��ײ̽��
    [System.Obsolete("������Ĵ���")]
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
    //���Ӷ��������
    public int x;
    public int y;

    //Ѱ·����
    public float f;
    //�����ľ���
    public float g;
    //���յ�ľ���
    public float h;
    //������
    public AStarNode father;

    //���ӵ�����
    public E_Node_Type type;

    /// <summary>
    /// ���캯�� ��������͸�������
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