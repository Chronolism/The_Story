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
    public GameObject NetworkManager;

    // Start is called before the first frame update
    void Awake()
    {
        Application.targetFrameRate = -1;
        StartCoroutine(LoadDataForAndroid());
        UIManager.Instance.ShowPanel<TipPanel>((o) =>
        {
            o.SetCurrent("数据更新中");
        }, true);
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
        if (File.Exists(Application.persistentDataPath + "/config.txt"))
        {
            persistentConfig.Deserialize(File.ReadAllLines(Application.persistentDataPath + "/config.txt"));
        }
        if (!streamConfig.version.Equals(persistentConfig.version))
        {
            Debug.Log("版本不同开始加载:" + streamConfig.version + "  " + persistentConfig.version);
            foreach (string dataPath in streamConfig.data)
            {
                Debug.Log("加载：" + dataPath);
                webRequest = UnityWebRequest.Get(new Uri(Application.streamingAssetsPath + "/" + dataPath));
                yield return webRequest.SendWebRequest();
                Debug.Log("加载完成：" + dataPath);
                if (webRequest.error == null)
                {
                    string path = Application.persistentDataPath + "/";
                    string[] temp = dataPath.Split('\\');
                    for (int i = 0; i < temp.Length - 1; i++)
                    {
                        path += temp[i] + "/";
                    }
                    if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                    File.WriteAllBytes(Application.persistentDataPath + "/" + dataPath, webRequest.downloadHandler.data);
                }
                webRequest.Dispose();

                if (File.Exists(Application.persistentDataPath + "/" + dataPath))
                {
                    Debug.Log("储存完成：" + dataPath);
                }
                else
                {
                    Debug.Log("储存失败：" + dataPath);
                }
            }
            UIManager.Instance.ShowPanel<TipPanel>((o) =>
            {
                o.SetCurrent("版本更新成功", true);
            }, true);
        }
        else
        {
            UIManager.Instance.ShowPanel<TipPanel>((o) =>
            {
                o.SetCurrent("版本无变化", true);
            }, true);
            Debug.Log("版本无变化");
        }
        Debug.Log("finish");
        DataMgr.Instance.DataLoad();
        NetworkManager.SetActive(true);
        UIManager.Instance.ShowPanel<MainMenuPanel>();
        //UIManager.Instance.ShowPanel<MapEditorPanel>();

    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
