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
        /// 异步初始化是否完成  true：完成  false：未完成
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


        /// <summary>
        /// 获取指定得UI层级
        /// </summary>
        /// <param name="layer">要获取得层</param>
        /// <returns></returns>
        public static RectTransform Getlayer(UIPanelLayer layer) => layers[layer];


        /// <summary>
        /// 打开窗口
        /// </summary>
        /// <typeparam name="T">要打开的窗口</typeparam>
        /// <param name="callback">打开窗口后回调</param>
        /// <param name="message">打开窗口所需要参数</param>
        public static void Open<T>(UICallback callback = null, params object[] message) where T : UIPanelBase
        {
            var panelName = typeof(T).Name;

            Action<UIPanelBase> openPanel = panel =>
            {

                panel.SetData(message);

                panel.OnUIEnable();

                UIManager.m_AnimManager.StartEnterAnim(panel, callback);

                panel.transform.SetAsLastSibling();
            };

            if (UIPanelDic.ContainsKey(panelName))
            {
                openPanel.Invoke(UIPanelDic[panelName]);
            }
            else
            {
                UIManager.Clone<T>(panel =>
                {

                    UIPanelDic.Add(typeof(T).Name, panel);

                    panel.OnUIAwake();

                    CoroutineRunner.WaitForFrames(1, panel.OnUIStart);

                    openPanel.Invoke(panel);

                });
            }
        }


        /// <summary>
        /// 关闭窗口
        /// </summary>
        /// <typeparam name="T">要关闭的窗口</typeparam>
        /// <param name="isPlayAnim">是否播放关闭动画</param>
        /// <param name="callback">关闭窗口后的回调</param>
        /// <param name="message">关闭窗口需要的参数</param>
        public static void Close<T>(bool isPlayAnim = true, UICallback callback = null, params object[] message) where T : UIPanelBase
        {
            if (!UIPanelDic.ContainsKey(typeof(T).Name))
            {
                Debug.LogError($"CloseUIWindow Error UI ->{nameof(T)}<-  not Exist!");
            }
            else
            {
                Close(UIPanelDic[typeof(T).Name], isPlayAnim, callback, message);
            }
        }


        /// <summary>
        /// 关闭窗口
        /// </summary>
        /// <param name="panel">要关闭的窗口</param>
        /// <param name="isPlayAnim">是否播放关闭动画</param>
        /// <param name="callback">关闭窗口后的回调</param>
        /// <param name="message">关闭窗口需要的参数</param>
        public static void Close(UIPanelBase panel, bool isPlayAnim = true, UICallback callback = null, params object[] message)
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

                UIManager.m_AnimManager.StartExitAnim(panel, callback, message);

            }
            else
            {
                panel.OnUIDisable();
            }
        }


        /// <summary>
        /// 关闭所有窗口
        /// </summary>
        /// <param name="isPlayerAnim">是否播放关闭动画</param>
        public static void CloseAll(bool isPlayerAnim = false)
        {
            foreach (var item in UIPanelDic.Values)
            {
                UIManager.Close(item, isPlayerAnim);
            }
        }


        /// <summary>
        /// 删除窗口
        /// </summary>
        /// <typeparam name="T">要删除的窗口</typeparam>
        public static void Destroy<T>(bool isPlayerAnim = false, UICallback callback = null, params object[] message) where T : UIPanelBase
        {
            if (UIPanelDic.TryGetValue(typeof(T).Name, out var panel))
            {
                UIManager.Destroy(panel, isPlayerAnim, callback, message);
            }
        }


        /// <summary>
        /// 删除窗口
        /// </summary>
        /// <param name="panel">要删除的窗口</param>
        public static void Destroy(UIPanelBase panel, bool isPlayerAnim = false, UICallback callback = null, params object[] message)
        {
            if (isPlayerAnim)
            {
                if (callback != null)
                {
                    callback += (u, p) =>
                    {
                        panel.OnUIDisable();
                        panel.OnUIDestroy();
                    };
                }
                else
                {
                    callback = (u, p) =>
                    {
                        panel.OnUIDisable();
                        panel.OnUIDestroy();
                    };
                }

                UIManager.m_AnimManager.StartExitAnim(panel, callback, message);
            }
            else
            {
                panel.OnUIDisable();
                panel.OnUIDestroy();
            }

            if (UIPanelDic.ContainsKey(panel.name))
            {
                UIPanelDic.Remove(panel.name);
            }
        }


        /// <summary>
        /// 删除所有窗口
        /// </summary>
        public static void DestroyAll()
        {
            var arr = UIPanelDic.Values.ToArray();

            for (int i = 0; i < arr.Length; i++)
            {
                UIManager.Destroy(arr[i]);
            }
        }


        /// <summary>
        /// 获取已被创建的窗口
        /// </summary>
        /// <typeparam name="T">要获取的窗口</typeparam>
        /// <returns></returns>
        public static T Get<T>() where T : UIPanelBase
        {
            if (UIPanelDic.TryGetValue(typeof(T).Name, out var panel))
            {
                return panel as T;
            }
            return null;
        }


        private static void Clone<T>(Action<UIPanelBase> callback) where T : UIPanelBase
        {
            ResourceManager.LoadAssetAsync<GameObject>(typeof(T).Name, o =>
            {
                if (o == null)
                {
                    throw new ApplicationException($"load {typeof(T).Name} panel fail");
                }

                //默认Normal层
                Transform parent = UIManager.Getlayer(UIPanelLayer.Normal);

                var objects = typeof(T).GetCustomAttributes(typeof(UILayerAttribute), true);

                if (objects?.Length > 0)
                {
                    var layerAttr = objects[0] as UILayerAttribute;

                    parent = UIManager.Getlayer(layerAttr.layer);
                }

                var newPanel = GameObject.Instantiate(o, parent).AddComponent<T>();

                newPanel.name = o.name;

                callback.Invoke(newPanel);
            });
        }


    }
}
