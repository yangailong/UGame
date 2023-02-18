﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.EventSystems;
using UnityEngine.ResourceManagement.AsyncOperations;

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
        public static bool InitAsyncComplete { get; set; } = false;

        public static async void InitAsync()
        {
            UIPanelDic = new Dictionary<string, UIPanelBase>();

            layers = new Dictionary<UIPanelLayer, RectTransform>();

            var uiRootName = "UIRoot";

            AsyncOperationHandle<GameObject> handle = Addressables.LoadAssetAsync<GameObject>(AssetsMapper.LoadPath(uiRootName));
            await handle.Task;

            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                if (handle.Result == null) return;

                m_UIRoot = Instantiate(handle.Result).AddComponent<UIManager>();

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

                InitAsyncComplete = true;

                Debug.Log($"{nameof(UIManager)}  Init Async Complete ");
            }
        }



        public static UIManager UIRoot => m_UIRoot;

        public static Canvas Canvas => m_Canvas;

        public static Camera Camera => m_Camera;

        public static EventSystem EventSystem => m_EventSystem;


        /// <summary>
        /// 获取指定层级
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
        public static void Open<T>(params object[] message) where T : UIPanelBase
        {
            var panelName = typeof(T).Name;

            Action<UIPanelBase> openPanel = panel =>
            {
                //为了防止其他panel在OnUIEnable打开其他窗口,故此代码SetAsLastSibling执行优先级最高
                panel.transform.SetAsLastSibling();

                panel.SetData(message);

                panel.OnUIEnable();

                UIManager.m_AnimManager.StartEnterAnim(panel);
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
        public static void Close<T>(bool isPlayAnim = true) where T : UIPanelBase
        {
            if (!UIPanelDic.ContainsKey(typeof(T).Name))
            {
                Debug.LogError($"CloseUIWindow Error UI ->{nameof(T)}<-  not Exist!");
            }
            else
            {
                Close(UIPanelDic[typeof(T).Name], isPlayAnim);
            }
        }


        /// <summary>
        /// 关闭窗口
        /// </summary>
        /// <param name="panel">要关闭的窗口</param>
        /// <param name="isPlayAnim">是否播放关闭动画</param>
        /// <param name="callback">关闭窗口后的回调</param>
        /// <param name="message">关闭窗口需要的参数</param>
        public static void Close(UIPanelBase panel, bool isPlayAnim = true)
        {
            if (isPlayAnim)
            {
                Action action = () =>
                {
                    panel.OnUIDisable();
                };

                UIManager.m_AnimManager.StartExitAnim(panel, action);
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
        public static void Destroy<T>(bool isPlayerAnim = false) where T : UIPanelBase
        {
            if (UIPanelDic.TryGetValue(typeof(T).Name, out var panel))
            {
                UIManager.Destroy(panel, isPlayerAnim);
            }
        }


        /// <summary>
        /// 删除窗口
        /// </summary>
        /// <param name="panel">要删除的窗口</param>
        public static void Destroy(UIPanelBase panel, bool isPlayerAnim = false)
        {
            if (isPlayerAnim)
            {
                Action action = () =>
                {
                    panel.OnUIDisable();
                    panel.OnUIDestroy();
                };

                UIManager.m_AnimManager.StartExitAnim(panel, action);
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
            var tmp = new Dictionary<string, UIPanelBase>();

            foreach (var item in UIPanelDic)
            {
                tmp.Add(item.Key, item.Value);
            }

            foreach (var item in tmp.Values)
            {
                UIManager.Destroy(item);
            }

            UIPanelDic.Clear();
            tmp.Clear();
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


        /// <summary>
        /// 克隆panel
        /// </summary>
        /// <typeparam name="T">要克隆的窗口</typeparam>
        /// <param name="callback">克隆完成后回调</param>
        /// <exception cref="ApplicationException">加载未成功</exception>
        private static async void Clone<T>(Action<UIPanelBase> callback) where T : UIPanelBase
        {
            AsyncOperationHandle<GameObject> handle = Addressables.LoadAssetAsync<GameObject>(AssetsMapper.LoadPath(typeof(T).Name));
            await handle.Task;

            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                //默认Normal层
                Transform parent = UIManager.Getlayer(UIPanelLayer.Normal);

                var objects = typeof(T).GetCustomAttributes(typeof(UILayerAttribute), true);

                if (objects?.Length > 0)
                {
                    var layerAttr = objects[0] as UILayerAttribute;

                    parent = UIManager.Getlayer(layerAttr.layer);
                }

                var newPanel = GameObject.Instantiate(handle.Result, parent).AddComponent<T>();

                newPanel.name = handle.Result.name;

                callback.Invoke(newPanel);
            }
            else
            {
                throw new ApplicationException($"load {typeof(T).Name} panel fail");
            }

        }


    }
}
