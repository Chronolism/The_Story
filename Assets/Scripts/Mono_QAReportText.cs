using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 使用时请？避免未依附于场景
/// </summary>
public class Mono_QAReportText : SingletonMono<Mono_QAReportText>
{
    int _timer;
    Text text;
    void Start()
    {
        text = this.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        _timer = Time.frameCount;
    }
    public void Report(string cantains)
    {
        text.text = cantains +" 于" + _timer + "\n" + text.text;
    }
}
