using UnityEngine;

namespace UGame_Remove
{
    /// <summary>单例Mono基类</summary>
    public class MonoSingleton<T> where T :MonoBehaviour
    {
        protected MonoSingleton() { }

        protected static T instance = null;

        public static T Instance
        {
            get
            {
                if (null == instance)
                {
                    Debug.Log($"0000000:  {typeof(T).Name}");

                    GameObject gameObject = new GameObject("UIManager");

                    gameObject.AddComponent<UIManager>();
                    //instance = new GameObject($"[{typeof(T).Name}]").AddComponent<T>();
                    Debug.Log("11111");
                }
                return instance;
            }
        }
    }
}