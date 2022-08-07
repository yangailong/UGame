using UnityEngine;

namespace UGame_Remove
{
    /// <summary>单例Mono基类</summary>
    public class MonoSingleton<T> where T : MonoBehaviour
    {
        protected MonoSingleton() { }

        protected static T instance = null;

        public static T Instance
        {
            get
            {
                if (null == instance)
                {
                    instance = new GameObject($"[{typeof(T).Name}]").AddComponent<T>();
                }
                return instance;
            }
        }
    }
}