using Excel;
using LitJson;
using System.Collections.Generic;
using System.Data;
using System.IO;
using UnityEditor;
using UnityEngine;

public class MapDataTool 
{
    private static string MAP_INFO_PATH = Application.dataPath + "/Editor/MapDataTool/Data/";


    [MenuItem("DataTool/生成MapData")]
    public static void CreatMapData_SO()
    {
        DirectoryInfo dinfo = Directory.CreateDirectory(MAP_INFO_PATH);
        FileInfo[] fileInfos = dinfo.GetFiles();
        DataTableCollection result;
        (int x, int y) mapSize = (0, 0);

        string basepath = Application.streamingAssetsPath + "/MapData/";

        if (!Directory.Exists(basepath))
        {
            Directory.CreateDirectory(basepath);
        }
        else
        {
            Directory.Delete(basepath, true);
            Directory.CreateDirectory(basepath);
        }

        //mapDetile_SO = new MapDetile_SO();
        for (int i = 0; i < fileInfos.Length; i++)
        {
            if (fileInfos[i].Extension != ".xlsx" && fileInfos[i].Extension != ".xls") continue;
            using(FileStream fs = fileInfos[i].OpenRead())
            {
                IExcelDataReader excelDataReader = ExcelReaderFactory.CreateOpenXmlReader(fs);
                result = excelDataReader.AsDataSet().Tables;
                fs.Close();
            }
            MapDetile mapDetile = new MapDetile();
            int id = 0;
            int type = 0;
            foreach (DataTable dt in result)
            {
                switch (dt.TableName)
                {
                    case "MapData":
                        for(int j = 0; j < dt.Rows.Count; j++)
                        {
                            if(dt.Rows[j][0].ToString() != "")
                            {
                                switch (dt.Rows[j][0])
                                {
                                    case "MapSize":
                                        mapSize = (int.Parse(dt.Rows[j][1].ToString()), int.Parse(dt.Rows[j][2].ToString()));
                                        mapDetile.Size = new V2() { x = mapSize.x, y = mapSize.y };
                                        mapDetile.MapTileDetiles = new MapTileDetile[mapSize.x ,mapSize.y];
                                        for( int a = 0; a<mapSize.x; a++)
                                        {
                                            for(int b = 0; b < mapSize.y; b++)
                                            {
                                                mapDetile.MapTileDetiles[a, b] = new MapTileDetile();
                                            }
                                        }
                                        break;
                                    case "id":
                                        id = int.Parse(dt.Rows[j][1].ToString());
                                        break;
                                    case "type":
                                        type = int.Parse(dt.Rows[j][1].ToString());
                                        break;
                                    case "BeansMax":
                                        mapDetile.beansMax = int.Parse(dt.Rows[j][1].ToString());
                                        break;
                                    case "BirthTime":
                                        mapDetile.birthTime = int.Parse(dt.Rows[j][1].ToString());
                                        break;
                                }
                            }

                        }
                        break;
                    case "Map":
                        if (dt.Rows[0].ItemArray.Length > mapSize.x || dt.Rows.Count > mapSize.y)
                        {
                            Debug.Log("MapData中MapSize数值不对\nmapSize=(" + mapSize.x + "," + mapSize.y + ")\n"
                                        + "Map=(" + dt.Rows[0].ItemArray.Length + "," + dt.Rows.Count + ")");
                            return;
                        }
                        for (int j = 0; j < dt.Rows.Count; j++)
                        {
                            for (int k = 0; k < dt.Rows[j].ItemArray.Length; k++)
                            {
                                if (dt.Rows[j][k].ToString() != "")
                                {
                                    MapTileDetile md;
                                    if (mapDetile.MapTileDetiles[k, j] == null)
                                    {
                                        md = mapDetile.MapTileDetiles[k, j] = new MapTileDetile();
                                    }
                                    else
                                    {
                                        md = mapDetile.MapTileDetiles[k, j];
                                    }
                                    
                                    md.tile.Add("Map1", int.Parse(dt.Rows[j][k].ToString()));
                                    md.x = k;
                                    md.y = j;
                                    md.Tile_x = k + 1;
                                    md.Tile_y = mapSize.y - j;
                                    md.world_x = md.Tile_x + 0.5f;
                                    md.world_y = md.Tile_y + 0.5f;
                                }

                            }
                        }
                        
                        break;
                    case "MapCollider":
                        if (dt.Rows[0].ItemArray.Length > mapSize.x || dt.Rows.Count > mapSize.y)
                        {
                            Debug.Log("MapData中MapSize数值不对\nmapSize=(" + mapSize.x + "," + mapSize.y + ")\n"
                                        + "MapCollider=(" + dt.Rows[0].ItemArray.Length + "," + dt.Rows.Count + ")");
                            return;
                        }
                        for (int j = 0; j < dt.Rows.Count; j++)
                        {
                            for (int k = 0; k < dt.Rows[j].ItemArray.Length; k++)
                            {
                                if (dt.Rows[j][k].ToString() != "")
                                {
                                    MapTileDetile md;
                                    if (mapDetile.MapTileDetiles[k, j] == null)
                                    {
                                        md = mapDetile.MapTileDetiles[k, j] = new MapTileDetile();
                                    }
                                    else
                                    {
                                        md = mapDetile.MapTileDetiles[k, j];
                                    }
                                    try
                                    {
                                        md.tile.Add("MapCollider", int.Parse(dt.Rows[j][k].ToString()));
                                    }
                                    catch
                                    {
                                        Debug.Log("MapName:" + id + "-" + type + "中的" + j + "-" + k + "不为数字和空\n是" + dt.Rows[j][k].ToString());
                                    }
                                    
                                    md.x = k;
                                    md.y = j;
                                    md.Tile_x = k + 1;
                                    md.Tile_y = mapSize.y - j;
                                    md.world_x = md.Tile_x + 0.5f;
                                    md.world_y = md.Tile_y + 0.5f;
                                }

                            }
                        }
                        break;
                    case "Instance":
                        if (dt.Rows[0].ItemArray.Length > mapSize.x || dt.Rows.Count > mapSize.y)
                        {
                            Debug.Log("MapData中MapSize数值不对\nmapSize=(" + mapSize.x + "," + mapSize.y + ")\n"
                                        + "Instance=(" + dt.Rows[0].ItemArray.Length + "," + dt.Rows.Count + ")");
                            return;
                        }
                        for (int j = 0; j < dt.Rows.Count; j++)
                        {
                            for (int k = 0; k < dt.Rows[j].ItemArray.Length; k++)
                            {
                                if (dt.Rows[j][k].ToString() != "")
                                {
                                    MapTileDetile md;
                                    if (mapDetile.MapTileDetiles[k, j] == null)
                                    {
                                        md = mapDetile.MapTileDetiles[k, j] = new MapTileDetile();
                                    }
                                    else
                                    {
                                        md = mapDetile.MapTileDetiles[k, j];
                                    }
                                    md.tile.Add("Instance", int.Parse(dt.Rows[j][k].ToString()));
                                    md.x = k;
                                    md.y = j;
                                    md.Tile_x = k + 1;
                                    md.Tile_y = mapSize.y - j;
                                    md.world_x = md.Tile_x + 0.5f;
                                    md.world_y = md.Tile_y + 0.5f;
                                }

                            }
                        }
                        break;
                    case "EnemyData":
                        for (int j = 2; j < dt.Rows.Count; j++)
                        {
                            EnemyDetile ed = new EnemyDetile();
                            try
                            {
                                ed.id = int.Parse(dt.Rows[j][0].ToString());
                            }
                            catch
                            {
                                Debug.Log(j + "--" + dt.Rows[j][0].ToString() + "--");
                                continue;
                            }
                            ed.actionType =int.Parse(dt.Rows[j][1].ToString());
                            if (dt.Rows[j][2].ToString() != "")
                            {
                                string[] strs = dt.Rows[j][2].ToString().Split(",");
                                foreach(string str in strs)
                                {
                                    ed.Limit_one.Add(int.Parse(str));
                                }
                            }
                            for (int k = 5; k < dt.Rows[j].ItemArray.Length; k++)
                            {
                                if (dt.Rows[j][k].ToString() != "")
                                {
                                    int value = int.Parse(dt.Rows[j][k].ToString());
                                    if ((k - 1) % 2 == 0) 
                                    {
                                        ed.targets.Add(new V2() { x = value }) ;
                                    }
                                    else
                                    {
                                        ed.targets[(k - 6) / 2] = new V2() { x = ed.targets[(k - 6) / 2].x, y = mapSize.y - value + 1 };
                                    }
                                }
                                else
                                {
                                    break;
                                }

                            }
                            mapDetile.enemyDetiles.Add(ed);
                        }
                        break;
                    case "PlayerPoint":
                        for (int j = 2; j < dt.Rows.Count; j++)
                        {
                            mapDetile.start.Add(new V2() { x = int.Parse(dt.Rows[j][0].ToString()), y = mapSize.y - int.Parse(dt.Rows[j][1].ToString()) + 1 }) ;
                            mapDetile.end.Add(new V2() { x = int.Parse(dt.Rows[j][2].ToString()), y = mapSize.y - int.Parse(dt.Rows[j][3].ToString()) + 1 }) ;
                        }
                        break;

                }

            }
            string MapName = type.ToString() + "-" + id.ToString();
            Debug.Log(MapName);
            Debug.Log(Application.persistentDataPath);
            mapDetile.id = id;
            mapDetile.type = type;
            mapDetile.name = fileInfos[i].Name.Split(".")[0];
            Debug.Log(mapDetile.name);

            string path = basepath + MapName + ".json";
            string jsonStr = "";

            List<MapTileDetile> mapDetileList = new List<MapTileDetile>();
            foreach (MapTileDetile md in mapDetile.MapTileDetiles)
            {
                mapDetileList.Add(md);
            }

            MapDetileToJson mapDetileToJson = new MapDetileToJson()
            {
                Size = mapDetile.Size,
                type = mapDetile.type,
                id = mapDetile.id,
                name = mapDetile.name,
                beansMax = mapDetile.beansMax,
                birthTime = mapDetile.birthTime,
                MapTileDetiles = mapDetileList,
                enemyDetiles = mapDetile.enemyDetiles,
                start = mapDetile.start,
                end = mapDetile.end
            };
            
            jsonStr = JsonMapper.ToJson(mapDetileToJson);
            File.WriteAllText(path, jsonStr);

        }
    }

}
