using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

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
        ConfigData configData = new ConfigData();
        configData.Deserialize(content);

        int time = DateTime.Now.Second;

        TimeSpan t = TimeSpan.FromSeconds(time);
        //configData.version = 
        

    }
}
public class ConfigData
{
    public string version;
    public string time;
    public List<string> data = new List<string>();

    public string[] Serialize()
    {
        string[] result = new string[3];
        result[0] = "version:" + version;
        result[1] = "time:" + time;

        string dataString = "";
        if (data.Count > 0)
        {
            dataString +=data[0];
            for (int i = 1; i < data.Count; i++)
            {
                dataString += "," + data[i];
            }
        }
        result[2] = "data:" + dataString;

        return null;
    }

    public void Deserialize(string[] strings)
    {
        foreach(string s in strings)
        {
            string[] temp = s.Split(':');
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