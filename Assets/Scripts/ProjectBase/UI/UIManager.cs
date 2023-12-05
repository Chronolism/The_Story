using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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
    public RectTransform activeCanvas;
    public BasePanel activePanel;

    //private Transform canvasTrans;
    //获取对象
    public UIManager()
    {
        //创建Canvas 让其过场景的时候 不被移除
        GameObject obj = ResMgr.Instance.Load<GameObject>("UI/Canvas");
        canvas = obj.transform.Find("NotActive") as RectTransform;
        activeCanvas = obj.transform.Find("Active") as RectTransform;
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
    /// <param name="callBack">当面板预设体创建成功后 你想做的事</param>
    public void ShowPanel<T>(UnityAction<T> callBack = null, bool isActive = false) where T : BasePanel //where 接受的范型T要保证T继承于BasePanel
    {
        string panelName = typeof(T).Name;
        if (panelDic.ContainsKey(panelName))
        {
            panelDic[panelName].ShowMe();
            //判断激活panel是否为空，为空无视激活判断，直接激活
            if(activePanel == null)
            {
                activePanel = panelDic[panelName];
                activePanel.isActivePanel = true;
                activePanel.transform.SetParent(activeCanvas);
                activePanel.OnActive();
            }
            else
            {
                //若激活panel为自己无视
                if (isActive && activePanel != panelDic[panelName])
                {
                    //退后现有激活panel
                    activePanel.isActivePanel = false;
                    activePanel.transform.SetParent(canvas);
                    activePanel.OnNotActive();
                    //设置这个panel为激活panel
                    activePanel = panelDic[panelName];
                    activePanel.isActivePanel = true;
                    activePanel.transform.SetParent(activeCanvas);
                    activePanel.OnActive();
                }
            }
            // 处理面板创建完成后的逻辑
            //避免面板重复加载 如果存在该面板 即直接显示 调用回调函数后  直接return 不再处理后面的异步加载逻辑
            if (callBack != null)
                callBack(panelDic[panelName] as T);

            return;
        }

        //通过储存位置寻找UI面板
        ResMgr.Instance.LoadAsync<GameObject>("UI/" + panelName, (obj) =>
        {
            if (GetPanel<T>() != null)
            {
                ShowPanel<T>(callBack, isActive);
                GameObject.Destroy(obj);
                return;
            }
            //得到预设体身上的面板脚本
            T panel = obj.GetComponent<T>();
            //判断激活panel是否为空，为空无视激活判断，直接激活
            if (activePanel == null)
            {
                activePanel = panel;
                panel.isActivePanel = true;
                panel.transform.SetParent(activeCanvas);
                panel.OnActive();
            }
            else
            {
                if (isActive)
                {
                    //退后现有激活panel
                    activePanel.isActivePanel = false;
                    activePanel.transform.SetParent(canvas);
                    activePanel.OnNotActive();
                    //设置这个panel为激活panel
                    activePanel = panel;
                    panel.isActivePanel = true;
                    panel.transform.SetParent(activeCanvas);
                    panel.OnActive();
                }
                else
                {
                    //设置为不激活panel
                    panel.isActivePanel = false;
                    panel.transform.SetParent(canvas);
                    panel.OnNotActive();
                }
            }



            obj.transform.localPosition = Vector3.zero;
            obj.transform.localScale = Vector3.one;
            (obj.transform as RectTransform).offsetMax = Vector2.zero;
            (obj.transform as RectTransform).offsetMin = Vector2.zero;
            obj.name = panelName;


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
    /// 显示子级面板
    /// </summary>
    /// <typeparam name="T">面板脚本类型</typeparam>
    /// <param name="parentPanel">面板父级</param>
    /// <param name="callBack">回调</param>
    public void ShowPanel<T>(BasePanel parentPanel ,UnityAction<T> callBack = null) where T : BasePanel //where 接受的范型T要保证T继承于BasePanel
    {
        string panelName = typeof(T).Name;
        if (parentPanel.childrenPanels.ContainsKey(panelName))
        {
            parentPanel.childrenPanels[panelName].ShowMe();
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
            Transform father = parentPanel.transform;

            //设置父对象  设置相对位置和大小
            obj.transform.SetParent(father);

            obj.transform.localPosition = Vector3.zero;
            obj.transform.localScale = Vector3.one;

            (obj.transform as RectTransform).offsetMax = Vector2.zero;
            (obj.transform as RectTransform).offsetMin = Vector2.zero;

            obj.name = panelName;

            //得到预设体身上的面板脚本
            T panel = obj.GetComponent<T>();
            panel.parentPanel = parentPanel;
            // 处理面板创建完成后的逻辑
            if (callBack != null)
                callBack(panel);
            //panel.uiData = DataMgr.Instance.GetUIStr(panelName);
            panel.ShowMe();

            //把面板存起来
            parentPanel.childrenPanels.Add(panelName, panel);
        });


    }
    /// <summary>
    /// 隐藏面板
    /// </summary>
    /// <typeparam name="T">面板类型</typeparam>
    /// <param name="isFade">是否淡入淡出</param>
    public bool HidePanel<T>(UnityAction callBack = null, bool isFade = true) where T : BasePanel
    {
        //根据名字获取类型
        string panelName = typeof(T).Name;
        if (panelDic.ContainsKey(panelName))
        {
            BasePanel basePanel = panelDic[panelName];
            //删除子面板
            foreach (var panel in basePanel.childrenPanels)
            {
                HidePanel(panel.Value, null, false, false);
            }
            basePanel.childrenPanels.Clear();
            //是否渐变
            if (isFade)
            {
                basePanel.HideMe(() =>
                {
                    //判断是不是激活panel
                    if (activePanel == basePanel)
                    {
                        //找到不激活面板上最后一个，设为激活，找不到就设为null
                        activePanel = canvas.childCount > 0 ? canvas.GetChild(canvas.childCount - 1).GetComponent<BasePanel>() : null;
                        activePanel.isActivePanel = true;
                        activePanel.transform.SetParent(activeCanvas);
                        activePanel.OnActive();
                    }
                    basePanel.OnNotActive();
                    //删除面板
                    GameObject.Destroy(basePanel.gameObject);
                    panelDic.Remove(panelName);
                    callBack?.Invoke();
                });
            }
            else
            {
                //判断是不是激活panel
                if (activePanel == basePanel)
                {
                    //找到不激活面板上最后一个，设为激活，找不到就设为null
                    activePanel = canvas.childCount > 0 ? canvas.GetChild(canvas.childCount - 1).GetComponent<BasePanel>() : null;
                    activePanel.isActivePanel = true;
                    activePanel.transform.SetParent(activeCanvas);
                    activePanel.OnActive();
                }
                basePanel.OnNotActive();
                //删除面板
                GameObject.Destroy(basePanel.gameObject);
                panelDic.Remove(panelName);
                callBack?.Invoke();
            }
            return true;
        }
        else
        {
            return false;
        }
        
    }
    /// <summary>
    /// 删除指定面板
    /// </summary>
    /// <param name="basePanel">指定面板</param>
    /// <param name="callBack">回调</param>
    /// <param name="isFade">是否淡入淡出</param>
    /// <param name="isClearParent">是否删除父面板的子面板（遍历父面板的子面板集时请设为false，然后手动删除）</param>
    public void HidePanel(BasePanel basePanel ,UnityAction callBack = null, bool isFade = true, bool isClearParent = true)
    {
        //根据名字获取类型
        string panelName = basePanel.GetType().Name;
        //判断是不是主面板主面板
        if (panelDic.ContainsKey(panelName))
        {
            //删除子面板
            foreach (var panel in basePanel.childrenPanels)
            {
                HidePanel(panel.Value, null, false, false);
            }
            basePanel.childrenPanels.Clear();
            //是否渐变
            if (isFade)
            {
                basePanel.HideMe(() =>
                {
                    //判断是不是激活panel
                    if (activePanel == basePanel)
                    {
                        //找到不激活面板上最后一个，设为激活，找不到就设为null
                        activePanel = canvas.childCount > 0 ? canvas.GetChild(canvas.childCount - 1).GetComponent<BasePanel>() : null;
                        activePanel.isActivePanel = true;
                        activePanel.transform.SetParent(activeCanvas);
                        activePanel.OnActive();
                    }
                    basePanel.OnNotActive();
                    //删除面板
                    GameObject.Destroy(basePanel.gameObject);
                    panelDic.Remove(panelName);
                    callBack?.Invoke();
                });
            }
            else
            {
                //判断是不是激活panel
                if (activePanel == basePanel)
                {
                    //找到不激活面板上最后一个，设为激活，找不到就设为null
                    activePanel = canvas.childCount > 0 ? canvas.GetChild(canvas.childCount - 1).GetComponent<BasePanel>() : null;
                    activePanel.isActivePanel = true;
                    activePanel.transform.SetParent(activeCanvas);
                    activePanel.OnActive();
                }
                basePanel.OnNotActive();
                //删除面板
                GameObject.Destroy(basePanel.gameObject);
                panelDic.Remove(panelName);
                callBack?.Invoke();
            }
        }
        else
        {
            if (isClearParent && basePanel.parentPanel != null)
            {
                basePanel.parentPanel.childrenPanels.Remove(panelName);
            }
            //隐藏
            if (isFade)
            {
                basePanel.HideMe(() =>
                {
                    //删除面板
                    GameObject.Destroy(basePanel.gameObject);
                    callBack?.Invoke();
                });
            }
            else
            {
                GameObject.Destroy(basePanel.gameObject);
                callBack?.Invoke();
            }
        }
    }
    /// <summary>
    /// 得到某一个已经显示的主面板 方便外部使用
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

    /// <summary>
    /// 清除所有面板
    /// </summary>
    public void ClearAllPanel(bool ifClearActivePanel = false)
    {
        if (!ifClearActivePanel)
        {
            foreach (BasePanel bp in panelDic.Values)
            {
                if (bp == activePanel) continue;
                GameObject.Destroy(bp.gameObject);
            }
            panelDic.Clear();
            panelDic[activePanel.GetType().Name] = activePanel;
        }
        else
        {
            foreach (BasePanel bp in panelDic.Values)
            {
                GameObject.Destroy(bp.gameObject);
            }
            panelDic.Clear();
        }
    }

}
