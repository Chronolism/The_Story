using Mirror;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

/// <summary>
/// 资源加载模块
/// 1.异步加载
/// 2.委托和 lambda表达式
/// 3.协程
/// 4.泛型
/// </summary>
public class ResMgr : BaseManager<ResMgr>
{
    BinaryFormatter binaryFormatter = new BinaryFormatter();
    NetworkWriter writer = new NetworkWriter();
    NetworkReader reader = new NetworkReader(null);
    /// <summary>
    /// 加载通用Resources资源
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="name"></param>
    /// <returns></returns>
    public T Load<T>(string name ) where T:Object
    {
        T res = Resources.Load<T>(name);
        //如果对象是一个GameObject类型的 我把他实例化后 再返回出去 外部 直接使用即可
        if (res is GameObject)
            return GameObject.Instantiate(res);
        else//TextAsset AudioClip
            return res;
    }
    /// <summary>
    /// 加载json
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="name"></param>
    /// <returns></returns>
    public T LoadJson<T>(string name) where T : new()
    {
        return JsonMgr.Instance.LoadData<T>(name);
    }
    /// <summary>
    /// 加载二进制
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="name"></param>
    /// <returns></returns>
    public T LoadBinary<T>(string name) where T : class
    {
        string path = Application.streamingAssetsPath + "/" + name;
        if (!File.Exists(path)) path = Application.persistentDataPath + "/" + name;
        if (!File.Exists(path))
        {
            Debug.Log("不存在文件：" + name);
            return null;
        }
        using (FileStream fileStream = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
        {
            return binaryFormatter.Deserialize(fileStream) as T;
        }
    }
    /// <summary>
    /// 通过mirror加载二进制（mirror注册的数据类）
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="name"></param>
    /// <returns></returns>
    public T LoadBinaryWithMirror<T>(string name) where T : new()
    {
        string path = Application.streamingAssetsPath + "/" + name;
        if (!File.Exists(path)) path = Application.persistentDataPath + "/" + name;
        if (!File.Exists(path))
        {
            Debug.Log("不存在文件：" + name);
            return default;
        }
        reader.SetBuffer(File.ReadAllBytes(path));
        return reader.Read<T>();
    }
    /// <summary>
    /// 保存json
    /// </summary>
    /// <param name="data"></param>
    /// <param name="name"></param>
    public void SaveJson(object data,string name)
    {
        JsonMgr.Instance.SaveData(data,name);
    }
    /// <summary>
    /// 保存二进制（数据应为一般类型）
    /// </summary>
    /// <param name="data"></param>
    /// <param name="name"></param>
    public void SaveBinary(object data , string name)
    {
        using(FileStream fileStream = File.Create(Application.streamingAssetsPath + "/" + name))
        {
            binaryFormatter.Serialize(fileStream,data);
        }
    }
    /// <summary>
    /// 通过mirror保存二进制（mirror注册的数据类）
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="data"></param>
    /// <param name="name"></param>
    public void SaveBinaryWithMirror<T>(T data, string name) where T : new()
    {
        writer.Reset();
        writer.Write(data);
        using (FileStream fileStream = File.Create(Application.streamingAssetsPath + "/" + name))
        {
            fileStream.Write(writer.ToArraySegment());
        }
    }

    //异步加载资源
    public void LoadAsync<T>(string name, UnityAction<T> callback) where T:Object
    {
        //开启异步加载的协程
        MonoMgr.Instance.StartCoroutine(ReallyLoadAsync(name, callback));
    }

    //真正的协同程序函数  用于 开启异步加载对应的资源
    private IEnumerator ReallyLoadAsync<T>(string name, UnityAction<T> callback) where T : Object
    {
        ResourceRequest r = Resources.LoadAsync<T>(name);
        
        yield return r;

        //if (r.asset == null)
        //{
        //    UnityWebRequest req = new UnityWebRequest(DataManager.Instance.Web, UnityWebRequest.kHttpVerbGET);
        //    req.downloadHandler = new DownloadHandlerFile(Application.persistentDataPath + name);
        //    yield return req.SendWebRequest();
        //}

        if (r.asset is GameObject)
            callback(GameObject.Instantiate(r.asset) as T);
        else
            callback(r.asset as T);
    }


}

public enum ResType
{
    normal,
    json,
    binary
}