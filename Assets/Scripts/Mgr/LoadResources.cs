using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

public class LoadResources : BaseManager<LoadResources>
{
    public int callbackOrder => throw new System.NotImplementedException();

    public void Load()
    {
        MonoMgr.Instance.StartCoroutine(ILoad());
    }


    IEnumerator ILoad()
    {
        yield return null;
    }

    public void OnPostprocessBuild(BuildReport report)
    {
        
    }

    public void OnPreprocessBuild(BuildReport report)
    {
        if (!Directory.Exists(Application.streamingAssetsPath))
            Directory.CreateDirectory(Application.streamingAssetsPath);
    }

}


public class AutoRegisterResources{

}