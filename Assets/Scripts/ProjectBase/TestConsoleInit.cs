using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestConsoleInit : MonoBehaviour
{

    public Text waitText;
    public Text properText;
    string Temp;

    // Start is called before the first frame update
    void Awake()
    {
        waitText.text = TestConsole.Instance.text;
        if (this.GetComponent<CanvasGroup>() == null) 
            this.gameObject.AddComponent<CanvasGroup>();
        this.GetComponent<CanvasGroup>().alpha = 0;
        this.GetComponent<InputField>().onValueChanged.AddListener((s) =>
        {
            if (s.Length < 1) return;
            if (s[0] == '/')
            {
                string properString = TestConsole.Instance.ProperFunc(s.Substring(1));
                properText.text = properString;
                //print("可能的函数:" + properString);

            }
                
        });
        this.GetComponent<InputField>().onEndEdit.AddListener((s) =>
        {
            if (s.Length < 1)
            {
                return;
            }
            if (s[0] == '/')
            {
                try
                {
                    if(s.Substring(1, 6) == "print ")
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
        
        #region 测试代码
        TestConsole.Instance.AddCommand("author", () => { print("来自Dr3的自用插件"); });
        TestConsole.Instance.AddCommand("about", () => { print("可能存在bug请多包含哦~"); });
        #endregion

    }
    void Update()
    {
        if ((Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.LeftCommand)) && Input.GetKeyDown(KeyCode.T))
        {
            if (this.GetComponent<CanvasGroup>().alpha == 0) this.GetComponent<CanvasGroup>().alpha = 1;
            else this.GetComponent<CanvasGroup>().alpha = 0;
        }
        if (waitText.text != TestConsole.Instance.text) waitText.text = TestConsole.Instance.text;

    }
}
