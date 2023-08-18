using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TestConsole
{

    static private TestConsole instance = new TestConsole();
    static public TestConsole Instance => instance;
    Dictionary<string,UnityAction> ConsoleBox = new Dictionary<string, UnityAction>();
    public string text = "�ȴ����� ......";
    public string preText = "";
    public void FindAndRun(string FunctionName)
    {

        if (!ConsoleBox.ContainsKey(FunctionName))
        {
            this.WriteLine("�Ҳ�����Ϊ " + FunctionName + "�ĺ���");
            return;
        }

        ConsoleBox[FunctionName].Invoke();
        text += ("\n" + "�ѳɹ�����" + FunctionName);
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

    public string ProperFunc(string Typing)
    {
        int length = Typing.Length;
        string resultString = "";
        foreach(string item in ConsoleBox.Keys)
        {
            try
            {
                if (item.Substring(0, length) == Typing) resultString += ("/" + item + "\n" );
            }
            catch { }
        }
        return resultString;
    }
}
