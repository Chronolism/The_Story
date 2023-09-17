using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConsolePanel : BasePanel
{
    public Text waitText;
    public Text properText;
    public InputField inputText;
    string Temp;
    public override void Init()
    {
        waitText = GetControl<Text>("Prepare");
        properText = GetControl<Text>("Proper");
        inputText = GetControl<InputField>("TestConsole");

        waitText.text = TestConsole.Instance.text;
        if (inputText.GetComponent<CanvasGroup>() == null)
            inputText.gameObject.AddComponent<CanvasGroup>();
        inputText.GetComponent<CanvasGroup>().alpha = 0;

        inputText.onValueChanged.AddListener((s) =>
        {
            if (s.Length < 1) return;
            if (s[0] == '/')
            {
                string properString = TestConsole.Instance.ProperFunc(s.Substring(1));
                properText.text = properString;
                //print("���ܵĺ���:" + properString);

            }

        });
        inputText.onEndEdit.AddListener((s) =>
        {
            if (s.Length < 1)
            {
                return;
            }
            if (s[0] == '/')
            {
                try
                {
                    if (s.Substring(1, 6) == "print ")
                    {
                        Temp = s.Substring(7);
                        TestConsole.Instance.WriteLine(Temp);
                        return;
                    }
                }
                catch { };

                Temp = s.Substring(1);
                TestConsole.Instance.FindAndRun(Temp);
            };
            properText.text = "";
        });

        #region ���Դ���
        TestConsole.Instance.AddCommand("author", () => { print("����Dr3�����ò��"); });
        TestConsole.Instance.AddCommand("about", () => { print("���ܴ���bug������Ŷ~"); });
        #endregion

    }
    protected override void Update()
    {
        base.Update();
        if ((Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.LeftCommand)) && Input.GetKeyDown(KeyCode.T))
        {
            if (inputText.GetComponent<CanvasGroup>().alpha == 0) inputText.GetComponent<CanvasGroup>().alpha = 1;
            else inputText.GetComponent<CanvasGroup>().alpha = 0;
        }
        if (waitText.text != TestConsole.Instance.text) waitText.text = TestConsole.Instance.text;

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            int tempLength = properText.text.IndexOf("\n");
            if (tempLength > 1) 
                TestConsole.Instance.FindAndRun(properText.text.Substring(1, tempLength - 1));
            else
                TestConsole.Instance.WriteLine("�Ҳ������ʺ���");
        }
    }
    

}
