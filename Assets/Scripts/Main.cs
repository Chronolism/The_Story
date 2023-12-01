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
            o.SetCurrent("���ݸ�����");
        }, true);
    }

    IEnumerator LoadDataForAndroid()
    {
        Debug.Log(Application.persistentDataPath);
        Debug.Log("���汾");
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
            Debug.Log("�汾��ͬ��ʼ����:" + streamConfig.version + "  " + persistentConfig.version);
            foreach (string dataPath in streamConfig.data)
            {
                Debug.Log("���أ�" + dataPath);
                webRequest = UnityWebRequest.Get(new Uri(Application.streamingAssetsPath + "/" + dataPath));
                yield return webRequest.SendWebRequest();
                Debug.Log("������ɣ�" + dataPath);
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
                    Debug.Log("������ɣ�" + dataPath);
                }
                else
                {
                    Debug.Log("����ʧ�ܣ�" + dataPath);
                }
            }
            UIManager.Instance.ShowPanel<TipPanel>((o) =>
            {
                o.SetCurrent("�汾���³ɹ�", true);
            }, true);
        }
        else
        {
            UIManager.Instance.ShowPanel<TipPanel>((o) =>
            {
                o.SetCurrent("�汾�ޱ仯", true);
            }, true);
            Debug.Log("�汾�ޱ仯");
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
