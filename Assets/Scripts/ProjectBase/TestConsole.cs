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
    public string text = "等待输入 ......";
    public string preText = "";
    float timeUse = 0;//显示函数执行时间,存在问题需要维护
    public void FindAndRun(string FunctionName)
    {      
        if (!ConsoleBox.ContainsKey(FunctionName))
        {
            this.WriteLine("找不到名为 " + FunctionName + "的函数");
            return;
        }
        timeUse = Time.realtimeSinceStartup;
        ConsoleBox[FunctionName].Invoke();
        timeUse = Time.realtimeSinceStartup - timeUse;
        text += ("\n" + "已成功调用" + FunctionName +"("+ timeUse.ToString().Substring(0,6) + ")");
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
            //函数列表
            return;
        }
        ConsoleBox[FunctionName] += Function;
    }
    public void AddCommand(string FunctionName, UnityAction Function, string Tips)
    {
        if (!ConsoleTipsBox.ContainsKey(FunctionName))
        {
            ConsoleTipsBox.Add(FunctionName, Tips);
            //可以添加注释
        }
        ConsoleTipsBox[FunctionName] = Tips;
        if (!ConsoleBox.ContainsKey(FunctionName))
        {
            ConsoleBox.Add(FunctionName, Function);
            //函数列表
            return;
        }
        ConsoleBox[FunctionName] += Function;

    }

    public string ProperFunc(string Typing)
    {
        int length = Typing.Length;
        string resultString = "";
        //以下变量可以控制使函数较少的时候显示注释
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
        WriteLine("错误！ " + errorReason);
    }
}
