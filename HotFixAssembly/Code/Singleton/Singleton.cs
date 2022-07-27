/// <summary>单例基类</summary>
public class Singleton<T> where T : new()
{
    protected Singleton() { }

    protected static T instance = new T();

    public static T Instance
    {
        get
        {
            if (null == instance)
            { 
                instance = new T();
            }
            return instance;
        }
    }
}

