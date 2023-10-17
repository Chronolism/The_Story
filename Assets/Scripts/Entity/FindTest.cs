using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FindTest : MonoBehaviour
{
    // Start is called before the first frame update
    public Tile[] tiles;
    Vector2 start;
    Vector2 end;

    Tilemap tilemap;

    void Start()
    {
        MapManager.Instance.LoadMapCompletelyToScene("400");

        //tilemap = GetComponentInChildren<Tilemap>();
        //Dictionary<Vector3Int, bool> map = new Dictionary<Vector3Int, bool>();
        //for(int i = -10; i < 10; i++)
        //{
        //    for(int j = -10; j < 10; j++)
        //    {
        //        Vector3Int v3 = new Vector3Int(j, i);
        //        bool b = Random.Range(0, 1f) > 0.4;
        //        map.Add(v3,b ? true : false);
        //        tilemap.SetTile(v3, b ? tiles[1] : tiles[0]);
        //    }
        //}
        //AStarMgr.Instance.InitMapInfo(map);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            end = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            AStarMgr.Instance.FindPath(start, end, FindCallBack);
        }
        else if (Input.GetMouseButtonUp(1))
        {
            start = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
    }

    public void FindCallBack(List<AStarNode> nodes)
    {
        int deviationW = AStarMgr.Instance.deviationW;
        int deviationH = AStarMgr.Instance.deviationH;
        foreach(AStarNode node in nodes)
        {
            tilemap.SetTile(new Vector3Int(node.x + deviationW, node.y + deviationH, 0), null);
        }
    }
}
