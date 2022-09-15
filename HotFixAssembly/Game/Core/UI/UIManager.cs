using System;
using System.Collections.Generic;
using UnityEngine;

namespace UGame_Remove
{
    public class UIManager : MonoBehaviour
    {
        private static Dictionary<string, UIPanelBase> UIPanelDic = null;

        private static Dictionary<UIPanelLayer, RectTransform> layers = null;

        private static Canvas canvas = null;


        public static Canvas Canvas => canvas;


        public static RectTransform Getlayer(UIPanelLayer layer) => layers[layer];


        public static void Init()
        {
            UIPanelDic = new Dictionary<string, UIPanelBase>();

            layers = new Dictionary<UIPanelLayer, RectTransform>();

            var uiRootName = "UIRoot";

            ResourceManager.LoadAssetAsync<GameObject>(uiRootName, o =>
            {
                if (o == null) return;

                UIManager go = Instantiate(o).AddComponent<UIManager>();

                go.name = uiRootName;

                canvas = go.transform.GetComponentInChildren<Canvas>();

                foreach (UIPanelLayer layer in Enum.GetValues(typeof(UIPanelLayer)))
                {
                    layers.Add(layer, canvas.transform.Find(layer.ToString()) as RectTransform);
                }

                DontDestroyOnLoad(go);
            });
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

                //TODO...播放动画
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

                //TODO...播放动画
            }
            else
            {
                panel.OnUIDisable();
            }
        }


        private static void CreatUI(string name, Action<UIPanelBase> callback)
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


    }
}
