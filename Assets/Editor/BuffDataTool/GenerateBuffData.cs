using Excel;
using LitJson;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using UnityEditor;
using UnityEngine;

public class GenerateBuffData 
{
    private static string SAVE_PATH = Application.dataPath + "/Scripts/Buff/BaseBuff/";
    private static string BUff_INFO_PATH = Application.dataPath + "/Editor/BuffDataTool/Data/";

    [MenuItem("DataTool/生成BuffData")]
    public static void CreatBuffData()
    {
        DirectoryInfo dinfo = Directory.CreateDirectory(BUff_INFO_PATH);
        FileInfo[] fileInfos = dinfo.GetFiles();
        DataTableCollection result;
        

        for (int i = 0; i < fileInfos.Length; i++)
        {
            if (fileInfos[i].Extension != ".xlsx" && fileInfos[i].Extension != ".xls") continue;
            using (FileStream fs = fileInfos[i].OpenRead())
            {
                IExcelDataReader excelDataReader = ExcelReaderFactory.CreateOpenXmlReader(fs);
                result = excelDataReader.AsDataSet().Tables;
                fs.Close();
            }
            string registerStr = "";
            //Buffdata_SO buffdata_SO = ResMgr.Instance.Load<Buffdata_SO>()
            foreach (DataTable dt in result)
            {
                registerStr = "";
                List<BuffData> buffdata = new List<BuffData>(); 
                for (int j = 2; j < dt.Rows.Count; j++)
                {
                    int id = int.Parse(dt.Rows[j][0].ToString());
                    BuffData bd = new BuffData();
                    bd.id = id;
                    bd.name = dt.Rows[j][1].ToString();
                    bd.description = dt.Rows[j][9].ToString();
                    bd.quality = int.Parse(dt.Rows[j][2].ToString());
                    bd.type = int.Parse(dt.Rows[j][4].ToString());
                    bd.weight = int.Parse(dt.Rows[j][5].ToString());
                    bd.times = int.Parse(dt.Rows[j][6].ToString());
                    string[] idStr;
                    if (dt.Rows[j][7].ToString() != "")
                    {
                        idStr = dt.Rows[j][7].ToString().Split(",");
                        foreach (string str in idStr)
                        {
                            bd.white.Add(int.Parse(str));
                        }
                    }
                    if (dt.Rows[j][8].ToString() != "")
                    {
                        idStr = dt.Rows[j][8].ToString().Split(",");
                        foreach (string str in idStr)
                        {
                            bd.black.Add(int.Parse(str));
                        }
                    }
                    buffdata.Add(bd);
                    registerStr += $"\t\tRegister({id}, typeof(Buff_{id}));\r\n";
                    if (!Directory.Exists(SAVE_PATH + "Buff" + "/"))
                        Directory.CreateDirectory(SAVE_PATH + "Buff" + "/");
                    if (File.Exists(SAVE_PATH + "Buff" + "/" + "Buff_" + id + ".cs"))
                        continue;
                    string buffStr =
                                $"public class Buff_{id} : BuffBase\r\n" +
                                "{\r\n" +
                                    "\tpublic override void OnStart(Entity entity,float Value)\r\n" +
                                    "\t{\r\n" +
                                    "\t}\r\n" +
                                    "\tpublic override void OnEnd(Entity entity,float Value)\r\n" +
                                    "\t{\r\n" +
                                    "\t}\r\n" +
                                    "\tpublic override void OnAdd(Entity entity,float Value)\r\n" +
                                    "\t{\r\n" +
                                    "\t}\r\n" +
                                    "\tpublic override void OnRemove(Entity entity,float Value)\r\n" +
                                    "\t{\r\n" +
                                    "\t}\r\n" +
                                "}\r\n";
                    File.WriteAllText(SAVE_PATH + "Buff" + "/" + "Buff_" + id + ".cs", buffStr);
                }
                string jsonpath = Application.streamingAssetsPath + "/" + "BuffData/";
                if (!Directory.Exists(jsonpath))
                    Directory.CreateDirectory(jsonpath);
                File.WriteAllText(jsonpath + dt.TableName + "BuffData" + ".json", JsonMapper.ToJson(buffdata));
                Debug.Log(jsonpath);
            }

            string buffPoolStr = "using System;\r\n" +
                            "using System.Collections.Generic;\r\n" +
                            "public class BuffPool\r\n" +
                            "{\r\n" +
                                "\tprivate Dictionary<int, Type> buffs = new Dictionary<int, Type>();\r\n" +
                                "\tpublic BuffPool()\r\n" +
                                "\t{\r\n" +
                                    registerStr +
                                "\t}\r\n" +
                                "\tprivate void Register(int id, Type buffType)\r\n" +
                                "\t{\r\n" +
                                    "\t\tbuffs.Add(id, buffType);\r\n" +
                                "\t}\r\n" +
                                "\tpublic BuffBase GetBuff(int id)\r\n" +
                                "\t{\r\n" +
                                    "\t\tif (!buffs.ContainsKey(id))\r\n" +
                                    "\t\t\treturn null;\r\n" +
                                    "\t\treturn Activator.CreateInstance(buffs[id]) as BuffBase;\r\n" +
                                "\t}\r\n" +
                            "}\r\n";
            string path = SAVE_PATH + "/Pool/";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            //保存到本地
            File.WriteAllText(path + "BuffPool.cs", buffPoolStr);

            Debug.Log("Buff池生成结束");
        }
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        UnityEditor.Compilation.CompilationPipeline.RequestScriptCompilation();
    }
}
