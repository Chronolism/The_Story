using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// ʹ��ʱ�룿����δ�����ڳ���
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
        text.text = cantains +" ��" + _timer + "\n" + text.text;
    }
}
