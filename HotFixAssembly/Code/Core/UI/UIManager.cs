using UnityEngine;

namespace UGame_Remove
{
    public class UIManager : MonoBehaviour
    {

        public static void Init()
        {
            UIPanelDic.Clear();
            var uiRootName = "UIRoot";
            UIManager instance = Instantiate(Resources.Load<GameObject>(uiRootName)).AddComponent<UIManager>();
            instance.name = uiRootName;
            DontDestroyOnLoad(instance);
        }


        private static Dictionary<string, UIPanelBase> UIPanelDic = new Dictionary<string, UIPanelBase>();


        public static T? Open<T>(UICallback? callback = null, params object[] param) where T : UIPanelBase
        {
            return Open(typeof(T).Name, callback, param) as T;
        }


        public static UIPanelBase Open(string name, UICallback? callback = null, params object[] param)
        {
            UIPanelBase? panel = null;

            if (!UIPanelDic.TryGetValue(name, out panel))
            {
                panel = CreatUI(name);
            }

            panel.OnUIEnable();

            //TODO...播放动画
            return panel;
        }


        public static void Close(string name, bool isPlayAnim = true, UICallback? callback = null, params object[] param)
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


        public static void Close(UIPanelBase panel, bool isPlayAnim = true, UICallback? callback = null, params object[] param)
        {
            if (isPlayAnim)
            {
                if (callback != null)
                {
                    callback += (p1, p2) => { panel.OnUIDisable(); };
                }
                else
                {
                    callback = (p1, p) => { panel.OnUIDisable(); };
                }

                //TODO...播放动画
            }
            else
            {
                panel.OnUIDisable();
            }
        }


        private static UIPanelBase CreatUI(string name)
        {
            GameObject? go = null;//TODO...加载出来

            UIPanelBase panel = Instantiate(go).GetComponent<UIPanelBase>();
            panel.OnUIAwake();
            UIPanelDic.Add(name, panel);
            return panel;
        }

    }
}
