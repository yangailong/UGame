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

        private static Canvas canvas = null;

        private static UIManager uiRoot = null;

        private static UIAnimManager animManager = null;

        private static EventSystem eventSystem = null;

        private static Camera camera = null;


        public static void Init()
        {
            UIPanelDic = new Dictionary<string, UIPanelBase>();

            layers = new Dictionary<UIPanelLayer, RectTransform>();

            var uiRootName = "UIRoot";

            ResourceManager.LoadAssetAsync<GameObject>(uiRootName, o =>
            {
                if (o == null) return;

                uiRoot = Instantiate(o).AddComponent<UIManager>();

                animManager = uiRoot.gameObject.AddComponent<UIAnimManager>();

                eventSystem = uiRoot.transform.GetComponentInChildren<EventSystem>();

                uiRoot.name = uiRootName;

                canvas = uiRoot.transform.GetComponentInChildren<Canvas>();

                camera = uiRoot.transform.GetComponentInChildren<Camera>();

                foreach (UIPanelLayer layer in Enum.GetValues(typeof(UIPanelLayer)))
                {
                    layers.Add(layer, canvas.transform.Find(layer.ToString()) as RectTransform);
                }

                DontDestroyOnLoad(uiRoot);
            });
        }


        public static UIManager UIRoot => uiRoot;

        public static Canvas Canvas => canvas;

        public static Camera Camera => camera;


        public static EventSystem EventSystem => eventSystem;


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

                UIManager.animManager.StartEnterAnim(panel, callback, param);
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

                UIManager.animManager.StartExitAnim(panel, callback, param);

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

                var att = Attribute.GetCustomAttribute(typeof(UIPanelBase), typeof(UILayerAttribute)) as UILayerAttribute;

                var panel = GameObject.Instantiate(o, UIManager.Getlayer(att.layer)).GetComponent<UIPanelBase>();

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
