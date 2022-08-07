using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
[RequireComponent(typeof(UILayerMgr))]
[RequireComponent(typeof(UIAnimMgr))]
public class UIMgr : MonoBehaviour
{
    #region 初始化
    private static bool isInit;
    public static void Init()
    {
        if (!isInit)
        {
            isInit = true;
            UIDic.Clear();
            UIMgr instance = GameObject.FindObjectOfType(typeof(UIMgr)) as UIMgr;

            if (instance == null)
            {
                //TODO...资源加载
                instance = new GameObject(typeof(UIMgr).Name).AddComponent<UIMgr>();
            }

            s_UILayerManager = instance.GetComponent<UILayerMgr>();
            s_UIAnimManager = instance.GetComponent<UIAnimMgr>();
            s_EventSystem = instance.GetComponentInChildren<EventSystem>();

            if (Application.isPlaying)
            {
                DontDestroyOnLoad(instance);
            }
        }
    }
    #endregion

    private static UILayerMgr s_UILayerManager; //UI层级管理器
    private static UIAnimMgr s_UIAnimManager;   //UI动画管理器
    private static EventSystem s_EventSystem;//UI事件管理器

    public static UILayerMgr layerMgr { get => s_UILayerManager; }
    public static UIAnimMgr AnimMgr { get => s_UIAnimManager; }
    public static EventSystem EventSystem { get => s_EventSystem; }

    private static Dictionary<string, UIBase> UIDic = new Dictionary<string, UIBase>();


    private static string uiPath = "Prefabs/UI/";
    private static UIBase CreatUI(string name)
    {
        var go = Resources.Load<UIBase>($"{uiPath}{name}/{name}");
        UIBase ui = GameObject.Instantiate(go);

        UIDic.Add(name, ui);
        ui.OnUIStart();
        layerMgr.SetLayer(ui);
        return ui;
    }

    public static T GetUI<T>(string name) where T : UIBase
    {
        if (UIDic.ContainsKey(name))
        {
            return UIDic[name] as T;
        }
        else
        {
            Debug.LogError($"不存在{name}");
            return null;
        }
    }


    public static T Open<T>(UICallBack callback = null, params object[] objs) where T : UIBase
    {
        return (T)Open(typeof(T).Name, callback, objs);
    }

    public static UIBase Open(string name, UICallBack callback = null, params object[] objs)
    {
        UIBase ui = null;
        if (UIDic.ContainsKey(name))
        {
            ui = UIDic[name];
        }
        else
        {
            ui = CreatUI(name);
        }

        ui.OnUIEnable();

        AnimMgr.StartEnterAnim(ui, callback, objs);

        return ui;
    }

    public static void Close(string name, bool isPlayAnim = true, UICallBack callback = null, params object[] objs)
    {
        if (!UIDic.ContainsKey(name))
        {
            Debug.LogError($"CloseUIWindow Error UI ->{name}<-  not Exist!");
        }
        else
        {
            Close(UIDic[name], isPlayAnim, callback, objs);
        }
    }

    public static void Close(UIBase ui, bool isPlayAnim = true, UICallBack callback = null, params object[] objs)
    {
        if (isPlayAnim)
        {
            if (callback != null)
            {
                callback += (p1, p2) => { ui.OnUIDisable(); };
            }
            else
            {
                callback = (p1, p) => { ui.OnUIDisable(); };
            }
            AnimMgr.StartExitAnim(ui, callback, objs);
        }
        else
        {
            ui.OnUIDisable();
        }
    }

    public static void CloseAll(bool isPlayerAnim = false)
    {
        foreach (UIBase ui in UIDic.Values)
        {
            if (ui.isActiveAndEnabled)
            {
                Close(ui, isPlayerAnim);
            }
        }
    }

    public static void Destroy(UIBase ui)
    {
        Debug.Log($"UIManager DestroyUI {ui.name}");

        if (ui != null)
        {
            UIDic.Remove(ui.name);
            ui.OnUIDestroy();
            GameObject.Destroy(ui.gameObject);
        }
    }

    public static void DestroyAllHide()
    {
        foreach (UIBase ui in UIDic.Values)
        {
            ui.OnUIDisable();
            ui.OnUIDestroy();
        }

        UIDic.Clear();
    }

    public static void SetEventSystemEnable(bool enable)
    {
        EventSystem.enabled = enable;
    }

}


