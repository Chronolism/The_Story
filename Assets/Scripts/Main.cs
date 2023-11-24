using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;

public class Main : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        UIManager.Instance.ShowPanel<MainMenuPanel>();
#if UNITY_ANDROID
    
#endif
        UIManager.Instance.ShowPanel<TipPanel>((o) =>
        {
            o.SetCurrent("数据加载中");
        }, true);
        StartCoroutine(LoadDataForAndroid());
        //TimeSpan t = TimeSpan.FromSeconds(time);
        //Debug.Log(time);
        //InvokeRepeating("test", 1, 1);
    }

    IEnumerator LoadDataForAndroid()
    {
        Debug.Log(Application.persistentDataPath);
        Debug.Log("检查版本");
        ConfigData streamConfig = new ConfigData();
        ConfigData persistentConfig = new ConfigData();
        UnityWebRequest webRequest = UnityWebRequest.Get(new Uri(Application.streamingAssetsPath + "/config.txt"));
        yield return webRequest.SendWebRequest();
        streamConfig.Deserialize(webRequest.downloadHandler.text.Split("\r\n"));
        webRequest.Dispose();
        if(File.Exists(Application.persistentDataPath + "/config.txt"))
        {
            persistentConfig.Deserialize(File.ReadAllLines(Application.persistentDataPath + "/config.txt"));
        }
        if (!streamConfig.version.Equals(persistentConfig.version)) 
        {
            Debug.Log("版本不同开始加载:" + streamConfig.version + "  " + persistentConfig.version);
            foreach (string dataPath in streamConfig.data)
            {
                webRequest = UnityWebRequest.Get(new Uri(Application.streamingAssetsPath + "/" + dataPath));
                yield return webRequest.SendWebRequest();
                Debug.Log("加载：" + dataPath);
                if (webRequest.error == null)
                {
                    string path = Application.persistentDataPath + "/";
                    string[] temp = dataPath.Split('\\');
                    for(int i = 0; i < temp.Length-1; i++)
                    {
                        path += temp[i] + "/";
                    }
                    if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                    File.WriteAllBytes(Application.persistentDataPath + "/" + dataPath, webRequest.downloadHandler.data);
                }
                webRequest.Dispose();
            }
        }
        else
        {
            Debug.Log("版本无变化");
        }
        Debug.Log("finish");
        UIManager.Instance.ShowPanel<TipPanel>((o) =>
        {
            o.SetCurrent("数据加载成功", true);
        }, true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
