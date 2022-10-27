using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UGame_Local;
using UnityEngine;
using UnityEngine.EventSystems;
namespace UGame_Remove
{
    public class UIManager : MonoBehaviour
    {
        private static Dictionary<string, UIPanelBase> UIPanelDic = null;

        private static Dictionary<UIPanelLayer, RectTransform> layers = null;

        private static Canvas m_Canvas = null;

        private static UIManager m_UIRoot = null;

        private static UIAnimManager m_AnimManager = null;

        private static EventSystem m_EventSystem = null;

        private static Camera m_Camera = null;



        /// <summary>
        /// 异步初始化是否完成
        /// true：完成
        /// false：未完成
        /// </summary>
        public static bool AsyncInitComplete { get; set; } = false;


        public static void AsyncInit()
        {
            UIPanelDic = new Dictionary<string, UIPanelBase>();

            layers = new Dictionary<UIPanelLayer, RectTransform>();

            var uiRootName = "UIRoot";

            ResourceManager.LoadAssetAsync<GameObject>(uiRootName, o =>
            {
                if (o == null) return;

                m_UIRoot = Instantiate(o).AddComponent<UIManager>();

                m_AnimManager = m_UIRoot.gameObject.AddComponent<UIAnimManager>();

                m_EventSystem = m_UIRoot.transform.GetComponentInChildren<EventSystem>();

                m_UIRoot.name = uiRootName;

                m_Canvas = m_UIRoot.transform.GetComponentInChildren<Canvas>();

                m_Camera = m_UIRoot.transform.GetComponentInChildren<Camera>();

                foreach (UIPanelLayer layer in Enum.GetValues(typeof(UIPanelLayer)))
                {
                    layers.Add(layer, m_Canvas.transform.Find(layer.ToString()) as RectTransform);
                }

                DontDestroyOnLoad(m_UIRoot);

                AsyncInitComplete = true;

                Debug.Log($"{nameof(UIManager)} Async Init Complete ");

            });
        }


        public static UIManager UIRoot => m_UIRoot;

        public static Canvas Canvas => m_Canvas;

        public static Camera Camera => m_Camera;


        public static EventSystem EventSystem => m_EventSystem;


        public static RectTransform Getlayer(UIPanelLayer layer) => layers[layer];


        public static UIPanelBase GetHasPanel<T>()
        {
            return UIManager.GetHasPanel(typeof(T).Name);
        }


        public static UIPanelBase GetHasPanel(string name)
        {
            if (!UIPanelDic.TryGetValue(name, out var panel))
            {
                Debug.LogError($"The {name} panel does not exist");
            }

            return panel;
        }


        public static void Open<T>(UICallback callback = null, params object[] param)
        {
            UIManager.Open(typeof(T).Name, callback, param);
        }


        public static void Open(string name, UICallback callback = null, params object[] param)
        {
            Action<UIPanelBase> openPanel = panel =>
            {
                panel.OnUIEnable();

                UIManager.m_AnimManager.StartEnterAnim(panel, callback, param);
            };

            if (UIPanelDic.ContainsKey(name))
            {
                openPanel.Invoke(UIPanelDic[name]);
            }
            else
            {
                UIManager.CreatUI(name, creatPanel =>
                {
                    creatPanel.OnUIAwake();

                    UIPanelDic.Add(name, creatPanel);

                    openPanel.Invoke(creatPanel);
                });
            }


        }


        public static void Close(string name, bool isPlayAnim = true, UICallback callback = null, params object[] param)
        {
            if (!UIPanelDic.ContainsKey(name))
            {
                Debug.LogError($"CloseUIWindow Error UI ->{name}<-  not Exist!");
            }
            else
            {
                Close(UIPanelDic[name], isPlayAnim, callback, param);
            }
        }


        public static void Close(UIPanelBase panel, bool isPlayAnim = true, UICallback callback = null, params object[] param)
        {
            if (isPlayAnim)
            {
                if (callback != null)
                {
                    callback += (u, p) => { panel.OnUIDisable(); };
                }
                else
                {
                    callback = (u, p) => { panel.OnUIDisable(); };
                }

                UIManager.m_AnimManager.StartExitAnim(panel, callback, param);

            }
            else
            {
                panel.OnUIDisable();
            }
        }


        public static void CloseAllUI(bool isPlayerAnim = false)
        {
            foreach (var item in UIPanelDic.Values)
            {
                UIManager.Close(item, isPlayerAnim);
            }
        }


        public static void CreatUI<T>(Action<UIPanelBase> callback)
        {
            UIManager.CreatUI(typeof(T).Name, callback);
        }


        public static void CreatUI(string name, Action<UIPanelBase> callback)
        {
            ResourceManager.LoadAssetAsync<GameObject>(name, o =>
            {
                if (o == null)
                {
                    Debug.LogError($"no {name} panel exists");
                    return;
                }


                Type type = typeof(UIPanelBase);

                var panel = GameObject.Instantiate(o).GetComponent<UIPanelBase>();

                var att = Attribute.GetCustomAttribute(panel.GetType(), typeof(UILayerAttribute)) as UILayerAttribute;

                Debug.LogError($"Names:{att}");

                panel.transform.SetParent(UIManager.Getlayer(att.layer));

                Debug.LogError($"layer:{UIManager.Getlayer(att.layer)}");

                callback.Invoke(panel);
            });
        }


        public static void Destroy<T>()
        {
            UIManager.Destroy(typeof(T).Name);
        }


        public static void Destroy(string name)
        {
            if (UIPanelDic.TryGetValue(name, out var panel))
            {
                panel.OnUIDestroy();
                UIPanelDic.Remove(name);
            }
        }


        public static void DestroyAll()
        {
            var arr = UIPanelDic.Values.ToArray();

            for (int i = 0; i < arr.Length; i++)
            {
                Destroy(arr[i]);
            }
        }


    }
}
