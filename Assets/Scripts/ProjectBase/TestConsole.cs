using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TestConsole
{

    static private TestConsole instance = new TestConsole();
    static public TestConsole Instance => instance;
    Dictionary<string,UnityAction> ConsoleBox = new Dictionary<string, UnityAction>();
    Dictionary<string, string> ConsoleTipsBox = new Dictionary<string, string>();
    public string text = "�ȴ����� ......";
    public string preText = "";
    float timeUse = 0;//��ʾ����ִ��ʱ��,����������Ҫά��
    public void FindAndRun(string FunctionName)
    {      
        if (!ConsoleBox.ContainsKey(FunctionName))
        {
            this.WriteLine("�Ҳ�����Ϊ " + FunctionName + "�ĺ���");
            return;
        }
        timeUse = Time.realtimeSinceStartup;
        ConsoleBox[FunctionName].Invoke();
        timeUse = Time.realtimeSinceStartup - timeUse;
        text += ("\n" + "�ѳɹ�����" + FunctionName +"("+ timeUse.ToString().Substring(0,6) + ")");
        timeUse = 0;
    }
    public void WriteLine(string printContains)
    {
        text += ("\n" + printContains);
    }

    public void AddCommand(string FunctionName,UnityAction Function)
    {
        if (!ConsoleBox.ContainsKey(FunctionName))
        {
            ConsoleBox.Add(FunctionName, Function);
            //�����б�
            return;
        }
        ConsoleBox[FunctionName] += Function;
    }
    public void AddCommand(string FunctionName, UnityAction Function, string Tips)
    {
        if (!ConsoleTipsBox.ContainsKey(FunctionName))
        {
            ConsoleTipsBox.Add(FunctionName, Tips);
            //�������ע��
        }
        ConsoleTipsBox[FunctionName] = Tips;
        if (!ConsoleBox.ContainsKey(FunctionName))
        {
            ConsoleBox.Add(FunctionName, Function);
            //�����б�
            return;
        }
        ConsoleBox[FunctionName] += Function;

    }

    public string ProperFunc(string Typing)
    {
        int length = Typing.Length;
        string resultString = "";
        //���±������Կ���ʹ�������ٵ�ʱ����ʾע��
        int displayFunc = 0;
        foreach(string item in ConsoleBox.Keys)
        {
            try
            {
                if (item.Substring(0, length) == Typing)
                {
                    resultString += ("/" + item + "\n");
                    displayFunc++;
                }                  
            }
            catch { }
        }
        if (displayFunc < 5)
        {
            resultString = "";
            foreach (string item in ConsoleBox.Keys)
            {
                try
                {
                    if (item.Substring(0, length) == Typing)
                    {
                        resultString += ("/" + item + "\n");
                        if (ConsoleTipsBox.ContainsKey(item)) 
                            resultString += (" " + ConsoleTipsBox[item] + "\n");
                    }
                }
                catch { }
            }
        }
        return resultString;
    }

    public void ThrowError(string errorReason)
    {
        UIManager.Instance.ShowPanel<ConsolePanel>();
        if (UIManager.Instance.GetPanel<ConsolePanel>().inputText.GetComponent<CanvasGroup>().alpha == 0) UIManager.Instance.GetPanel<ConsolePanel>().inputText.GetComponent<CanvasGroup>().alpha = 1;   
        WriteLine("���� " + errorReason);
    }
}
