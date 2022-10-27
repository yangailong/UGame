using System;
using System.Collections.Generic;
using System.Linq;
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
            if (!UIPanelDic.TryGetValue(nameof(T), out var panel))
            {
                Debug.LogError($"The {nameof(T)} panel does not exist");
            }

            return panel;
        }


        public static void Open<T>(UICallback callback = null, params object[] param) where T : UIPanelBase
        {
            var panelName = nameof(T);

            Action<UIPanelBase> openPanel = panel =>
            {
                panel.OnUIEnable();

                UIManager.m_AnimManager.StartEnterAnim(panel, callback, param);
            };

            if (UIPanelDic.ContainsKey(panelName))
            {
                openPanel.Invoke(UIPanelDic[panelName]);
            }
            else
            {
                UIManager.CreatUI<T>(creatPanel =>
                {
                    creatPanel.OnUIAwake();

                    UIPanelDic.Add(panelName, creatPanel);

                    openPanel.Invoke(creatPanel);
                });
            }
        }


        public static void Close<T>(bool isPlayAnim = true, UICallback callback = null, params object[] param) where T : UIPanelBase
        {
            if (!UIPanelDic.ContainsKey(nameof(T)))
            {
                Debug.LogError($"CloseUIWindow Error UI ->{nameof(T)}<-  not Exist!");
            }
            else
            {
                Close(UIPanelDic[nameof(T)], isPlayAnim, callback, param);
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


        public static void CreatUI<T>(Action<UIPanelBase> callback) where T : UIPanelBase
        {
            ResourceManager.LoadAssetAsync<GameObject>(typeof(T).Name, o =>
            {
                if (o == null)
                {
                    Debug.LogError($"no {typeof(T).Name} panel exists");
                    return;
                }

                var panel = GameObject.Instantiate(o).AddComponent<T>();


                //var att = Attribute.GetCustomAttribute(typeof(T), typeof(UILayerAttribute)) as UILayerAttribute;

                //Debug.LogError($"Names:{att}");

                //panel.transform.SetParent(UIManager.Getlayer(att.layer));

                //Debug.LogError($"layer:{UIManager.Getlayer(att.layer)}");

                panel.transform.SetParent(Canvas.transform.Find("Normal"));

                callback.Invoke(panel);
            });
        }


        public static void Destroy<T>() where T : UIPanelBase
        {
            if (UIPanelDic.TryGetValue(nameof(T), out var panel))
            {
                panel.OnUIDestroy();
                UIPanelDic.Remove(nameof(T));
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
