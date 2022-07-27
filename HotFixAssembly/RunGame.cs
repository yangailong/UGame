
using UnityEngine;

public class RunGame
{
    public static void StartUp()
    {
        Debug.Log($"调用成功了:StartUp");

        Init();
    }


    public static void Init()
    {
        var ui = UIManager.Instance;

    }


}

