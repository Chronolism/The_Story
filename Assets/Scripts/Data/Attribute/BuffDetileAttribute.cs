using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class BuffDetileAttribute : PropertyAttribute
{
#if UNITY_EDITOR
    private static List<BuffData> buffList;

    private static string[] buffName;


    public static string[] AllBuffNames()
    {
        if(buffName == null)
        {
            AllBuff();
        }
        return buffName;
    }

    public static List<BuffData> AllBuff()
    {
        if (buffList == null)
        {
            buffList = JsonMapper.ToObject<List<BuffData>>(File.ReadAllText(Application.streamingAssetsPath + "/" + "BuffData/" + "Chinese" + "BuffData" + ".json"));
            buffName = new string[buffList.Count];
            for(int i = 0; i < buffList.Count; i++)
            {
                buffName[i] = buffList[i].id + "." + buffList[i].name;
            }
        }
        return buffList;
    }
#endif
}
