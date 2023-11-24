using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor.Build;
using UnityEditor.Build.Reporting;


public class ConfigBuilder : IPreprocessBuildWithReport
{
    public int callbackOrder => 0;
    public string configPath = Application.streamingAssetsPath + "/config.txt";
    public void OnPreprocessBuild(BuildReport report)
    {
        if (!Directory.Exists(Application.streamingAssetsPath))
            Directory.CreateDirectory(Application.streamingAssetsPath);

        string[] content = default;

        if (File.Exists(configPath))
        {
            content = File.ReadAllLines(configPath);
        }
        else
        {
            Debug.LogError("config.txt文件不存在，已自动生成，请重新生成工程");
        }
        ConfigData configData = new ConfigData();
        configData.Deserialize(content);

        UpdataVersion(ref configData.version);

        configData.time = DateTime.Now.ToString();
        configData.data = new List<string>();

        DirectoryInfo dinfo = Directory.CreateDirectory(Application.streamingAssetsPath);

        ChackFile(dinfo, configData.data);

        File.WriteAllLines(configPath,configData.Serialize());
    }

    void UpdataVersion(ref string version)
    {
        string[] temp;
        if(version == null)
        {
            temp = new string[3];
            temp[0] = DateTime.Now.Year.ToString();
            temp[1] = DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString();
            temp[2] = "a";
        }
        else
        {
            temp = version.Split('.');
            if (ChackTimeIsToday(temp))
            {
                char versionType = temp[2][temp[2].Length - 1];
                versionType++;
                if (versionType > 'z')
                {

                    if (temp[2].Length > 2)
                    {
                        temp[2] = temp[2].Substring(0, temp[2].Length - 2) + "aa";
                    }
                    else
                    {
                        temp[2] = "aa";
                    }
                }
                else
                {
                    if (temp[2].Length > 2)
                    {
                        temp[2] = temp[2].Substring(0, temp[2].Length - 2) + versionType;
                    }
                    else
                    {
                        temp[2] = versionType.ToString();
                    }

                }
            }
            else
            {
                temp = new string[3];
                temp[0] = DateTime.Now.Year.ToString();
                temp[1] = DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString();
                temp[2] = "a";
            }
        }
        

        version = temp[0] + "." + temp[1] + "." + temp[2];

        bool ChackTimeIsToday(string[] times)
        {
            if(times.Length == 0)return false;
            if (times[0]!=DateTime.Now.Year.ToString()) return false;
            if (times[1] != DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString()) return false;
            return true;
        }
    }

    void ChackFile(DirectoryInfo dinfo , List<string> file)
    {
        foreach (FileInfo fileInfo in dinfo.GetFiles())
        {
            string name = fileInfo.FullName.Split(@"StreamingAssets\")[1];
            string[] temp = name.Split('.');
            if (temp[temp.Length - 1] == "meta") continue;

            file.Add(name);
        }
        foreach (DirectoryInfo di in dinfo.GetDirectories())
        {
            ChackFile(di, file);
        }
    }
}
#endif


public class ConfigData
{
    public string version = "";
    public string time = "";
    public List<string> data = new List<string>();

    public string[] Serialize()
    {
        string[] result = new string[3];
        result[0] = "version : " + version;
        result[1] = "time : " + time;

        string dataString = "";
        if (data.Count > 0)
        {
            dataString +=data[0];
            for (int i = 1; i < data.Count; i++)
            {
                dataString += "," + data[i];
            }
        }
        result[2] = "data : " + dataString;

        return result;
    }

    public void Deserialize(string[] strings)
    {
        if(strings == null || strings.Length == 0) return;
        foreach(string s in strings)
        {
            string[] temp = s.Split(" : ");
            switch (temp[0])
            {
                case "version":
                    version = temp[1];
                    break;
                case "time":
                    time = temp[1];
                    break;
                case "data":
                    string[] temp2 = temp[1].Split(',');
                    data = new List<string>();
                    foreach(string s2 in temp2)
                    {
                        if (s2 == "") continue;
                        data.Add(s2);
                    }
                    break;
            }
        }
    }
}