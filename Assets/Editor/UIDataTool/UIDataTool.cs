using Excel;
using LitJson;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Xml;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;

public class UIDataTool
{
    private static string UI_INFO_PATH = Application.dataPath + "/Editor/UIDataTool/Data/";

    [MenuItem("DataTool/Éú³ÉUIData")]
    public static void CreatUIData()
    {

        string uiName = "";

        Debug.Log(Application.streamingAssetsPath);

        DirectoryInfo dinfo = Directory.CreateDirectory(UI_INFO_PATH);
        FileInfo[] fileInfos = dinfo.GetFiles();
        DataTableCollection result;

        string basepath = Application.streamingAssetsPath + "/UIData/";

        if (!Directory.Exists(basepath))
        {
            Directory.CreateDirectory(basepath);
        }
        else
        {
            Directory.Delete(basepath, true);
            Directory.CreateDirectory(basepath);
        }

        for (int i = 0; i < fileInfos.Length; i++)
        {
            if (fileInfos[i].Extension != ".xlsx" && fileInfos[i].Extension != ".xls") continue;
            using (FileStream fs = fileInfos[i].OpenRead())
            {
                IExcelDataReader excelDataReader = ExcelReaderFactory.CreateOpenXmlReader(fs);
                result = excelDataReader.AsDataSet().Tables;
                fs.Close();
            }

            foreach (DataTable dt in result)
            {
                uiName = dt.TableName;

                List<UIData> uiDataList = new List<UIData>();

                for (int k = 2; k < dt.Rows[0].ItemArray.Length; k++)
                {
                    if (dt.Rows[0][k].ToString() != "")
                    {
                        uiDataList.Add(new UIData() { value = new Dictionary<string, string>() });
                    }
                }

                for (int j = 1; j < dt.Rows.Count; j++)
                {
                    if (dt.Rows[j][0].ToString() != "")
                    {
                        for (int k = 2; k < dt.Rows[j].ItemArray.Length; k++)
                        {
                            uiDataList[k - 2].value.Add(dt.Rows[j][0].ToString(), dt.Rows[j][k].ToString());
                        }
                    }
                }

                for (int k = 2; k < dt.Rows[0].ItemArray.Length; k++)
                {
                    if (dt.Rows[0][k].ToString() != "")
                    {
                        string pathStr = basepath + dt.Rows[0][k].ToString() + "/";
                        if (!Directory.Exists(pathStr))
                        {
                            Directory.CreateDirectory(pathStr);
                        }
                        File.WriteAllText(pathStr + uiName + ".json", JsonMapper.ToJson(uiDataList[k - 2]));
                    }
                }

            }

        }
    }

}
