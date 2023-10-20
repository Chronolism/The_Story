using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

/// <summary>
/// UI管理器
/// 1.管理所有显示的面板
/// 2.提供给外部 显示和隐藏等等接口
/// </summary>
public class UIManager : BaseManager<UIManager>
{
    //private static UIManager instance = new UIManager();
    //public static UIManager Instance => instance;

    //储存面板的Dic
    public Dictionary<string, BasePanel> panelDic = new Dictionary<string, BasePanel>();
   
    //记录我们UI的Canvas父对象 方便以后外部可能会使用它
    public RectTransform canvas;

    //private Transform canvasTrans;
    //获取对象
    public UIManager()
    {
        //创建Canvas 让其过场景的时候 不被移除
        GameObject obj = ResMgr.Instance.Load<GameObject>("UI/Canvas");
        canvas = obj.transform as RectTransform;
        //通过 动态创建 动态删除 来 显示/隐藏 面板
        GameObject.DontDestroyOnLoad(obj);

        //创建EventSystem 让其过场景的时候 不被移除
        //obj = ResMgr.Instance.Load<GameObject>("UI/EventSystem");
        //GameObject.DontDestroyOnLoad(obj);
    }

    /// <summary>
    /// 显示面板
    /// </summary>
    /// <typeparam name="T">面板脚本类型</typeparam>
    /// <param name="panelName">面板名</param>
    /// <param name="layer">显示在哪一层</param>
    /// <param name="callBack">当面板预设体创建成功后 你想做的事</param>
    public void ShowPanel<T>( UnityAction<T> callBack = null) where T:BasePanel //where 接受的范型T要保证T继承于BasePanel
    {
        string panelName = typeof(T).Name;

        if (panelDic.ContainsKey(panelName))
        {
            panelDic[panelName].ShowMe();


            // 处理面板创建完成后的逻辑
            //避免面板重复加载 如果存在该面板 即直接显示 调用回调函数后  直接return 不再处理后面的异步加载逻辑
            if (callBack != null)
                callBack(panelDic[panelName] as T);
            
            return;
        }

        //通过储存位置寻找UI面板
        ResMgr.Instance.LoadAsync<GameObject>("UI/" + panelName, (obj) =>
        {
            //把他作为 Canvas的子对象
            Transform father = canvas;

            //设置父对象  设置相对位置和大小
            obj.transform.SetParent(father);

            obj.transform.localPosition = Vector3.zero;
            obj.transform.localScale = Vector3.one;

            (obj.transform as RectTransform).offsetMax = Vector2.zero;
            (obj.transform as RectTransform).offsetMin = Vector2.zero;

            obj.name=panelName;

            //得到预设体身上的面板脚本
            T panel = obj.GetComponent<T>();
            // 处理面板创建完成后的逻辑
            if (callBack != null)
                callBack(panel);
            //panel.uiData = DataMgr.Instance.GetUIStr(panelName);
            panel.ShowMe();

            //把面板存起来
            panelDic.Add(panelName, panel);
        });
    }


    /// <summary>
    /// 隐藏面板
    /// </summary>
    /// <typeparam name="T">面板类型</typeparam>
    /// <param name="isFade">是否淡入淡出</param>
    public void HidePanel<T>(UnityAction callBack = null, bool isFade = true) where T : BasePanel
    {
        //根据名字获取类型
        string panelName = typeof(T).Name;
        if (panelDic.ContainsKey(panelName))
        {
            if (isFade)
            {
                panelDic[panelName].HideMe(() =>
                {
                    //删除面板
                    GameObject.Destroy(panelDic[panelName].gameObject);
                    panelDic.Remove(panelName);
                    callBack?.Invoke();
                });
            }
            else
            {
                GameObject.Destroy(panelDic[panelName]);
                panelDic.Remove(panelName);
                callBack?.Invoke();
            }
        }
    }

    /// <summary>
    /// 得到某一个已经显示的面板 方便外部使用
    /// </summary>
    public T GetPanel<T>() where T:BasePanel
    {
        string name = typeof(T).Name;
        if (panelDic.ContainsKey(name))
            return panelDic[name] as T;
        return null;
    }

    /// <summary>
    /// 给控件添加自定义事件监听
    /// </summary>
    /// <param name="control">控件对象</param>
    /// <param name="type">事件类型</param>
    /// <param name="callBack">事件的响应函数</param>
    public static void AddCustomEventListener(UIBehaviour control, EventTriggerType type, UnityAction<BaseEventData> callBack)
    {
        EventTrigger trigger = control.GetComponent<EventTrigger>();
        if (trigger == null)
            trigger = control.gameObject.AddComponent<EventTrigger>();

        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = type;
        entry.callback.AddListener(callBack);

        trigger.triggers.Add(entry);
    }

}
