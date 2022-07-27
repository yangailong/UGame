using UnityEngine;
/// <summary>单例Mono基类</summary>
public class MonoSingleton<T> where T : UnityEngine.Component
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