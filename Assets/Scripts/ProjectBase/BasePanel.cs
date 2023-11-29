using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// 面板基类 
/// 帮助我门通过代码快速的找到所有的子控件
/// 方便我们在子类中处理逻辑 
/// 节约找控件的工作量
/// </summary>
public abstract class BasePanel : MonoBehaviour
{
    public UIData uiData;
    //通过里式转换原则 来存储所有的控件
    private Dictionary<string, List<UIBehaviour>> controlDic = new Dictionary<string, List<UIBehaviour>>();

    private CanvasGroup CanvasGroup;

    public bool isActivePanel;
    [HideInInspector] public BasePanel parentPanel;
    [HideInInspector] public Dictionary<string, BasePanel> childrenPanels = new Dictionary<string, BasePanel>();

    [Header("淡入/出速度(单位:0.1s)")]
    public float SpeedIn = 10f;
    public float SpeedOut = 10f;

    private bool isShow;

    private UnityAction hidecallback; //委托or事件 的装载

    // Use this for initialization
    protected virtual void Awake () {
        FindChildrenControl<Button>();
        FindChildrenControl<Image>();
        FindChildrenControl<Text>();
        FindChildrenControl<Toggle>();
        FindChildrenControl<Slider>();
        FindChildrenControl<ScrollRect>();
        FindChildrenControl<InputField>();
        FindChildrenControl<Dropdown>();
        FindChildrenControl<FloatWindow>();
        FindChildrenControl<IntegerTriger>();
        CanvasGroup = this.GetComponent<CanvasGroup>();
        if (CanvasGroup == null)
        {
            CanvasGroup = this.gameObject.AddComponent<CanvasGroup>();
        }
    }

    protected virtual void Start()
    {
        Init();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        //淡入
        if (isShow && CanvasGroup.alpha != 1)
        {
            CanvasGroup.alpha += SpeedIn * Time.unscaledDeltaTime;
            if (CanvasGroup.alpha >= 1)
            {
                CanvasGroup.alpha = 1;
            }
        }
        //淡出
        else if (!isShow && CanvasGroup.alpha != 0)
        {
            CanvasGroup.alpha -= SpeedOut * Time.unscaledDeltaTime;
            if (CanvasGroup.alpha <= 0)
            {
                CanvasGroup.alpha = 0;

                //自己删自己
                hidecallback?.Invoke();
            }
        }
    }

    /// <summary>
    /// 主要用于初始化按钮监听等等的内容
    /// </summary>
    public abstract void Init();


    /// <summary>
    /// 展示自己
    /// </summary>
    public virtual void ShowMe()
    {
        isShow = true;
        CanvasGroup.alpha = 0;//阿尔法通道,即透明值
        LoadUIData();
    }

    /// <summary>
    /// 隐藏自己
    /// </summary>
    /// <param name="callBack">完全隐藏后执行函数</param>
    public virtual void HideMe(UnityAction callBack)
    {
        isShow = false;
        CanvasGroup.alpha = 1;
        //记录淡出函数
        hidecallback = callBack;
    }
    /// <summary>
    /// 显示子面板
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="callBack"></param>
    public void ShowPanel<T>(UnityAction<T> callBack = null) where T : BasePanel
    {
        UIManager.Instance.ShowPanel<T>(this, callBack);
    }
    /// <summary>
    /// 隐藏指定面板
    /// </summary>
    /// <param name="basePanel"></param>
    public void HidePanel(BasePanel basePanel)
    {
        UIManager.Instance.HidePanel(basePanel);
    }

    /// <summary>
    /// 读取UIData内容自动赋值
    /// </summary>
    public virtual void LoadUIData()
    {
        if (uiData.value == null) return;
        Text text;
        foreach (KeyValuePair<string, string> kvp in uiData.value) 
        {
            text = GetControl<Text>(kvp.Key);
            if(text != null)
            {
                text.text = kvp.Value;
            }
        }
    }


    public virtual void OnActive()
    {

    }

    public virtual void OnNotActive()
    {

    }

  
    protected virtual void OnClick(string btnName)
    {

    }

    protected virtual void OnValueChanged(string toggleName, bool value)
    {

    }

    /// <summary>
    /// 得到对应名字的对应控件脚本
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="controlName"></param>
    /// <returns></returns>
    public T GetControl<T>(string controlName) where T : UIBehaviour
    {
        if(controlDic.ContainsKey(controlName))
        {
            for( int i = 0; i <controlDic[controlName].Count; ++i )
            {
                if (controlDic[controlName][i] is T)
                    return controlDic[controlName][i] as T;
            }
        }

        return null;
    }

    /// <summary>
    /// 找到子对象的对应控件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    private void FindChildrenControl<T>() where T:UIBehaviour
    {
        T[] controls = this.GetComponentsInChildren<T>();
        for (int i = 0; i < controls.Length; ++i)
        {
            string objName = controls[i].gameObject.name;
            if (controlDic.ContainsKey(objName))
                controlDic[objName].Add(controls[i]);
            else
                controlDic.Add(objName, new List<UIBehaviour>() { controls[i] });
            //如果是按钮控件
            if(controls[i] is Button)
            {
                (controls[i] as Button).onClick.AddListener(()=>
                {
                    OnClick(objName);
                });
            }
            //如果是单选框或者多选框
            else if(controls[i] is Toggle)
            {
                (controls[i] as Toggle).onValueChanged.AddListener((value) =>
                {
                    OnValueChanged(objName, value);
                });
            }
        }
    }
}
