
using UnityEngine;
namespace UGame_Remove
{
    public class RunGame
    {
        public static void StartUp()
        {
            Debug.Log($"---调用成功了:StartUp");

            GameObject gameObject = new GameObject("哈哈哈哈");

            Init();
        }


        public static void Init()
        {
            var ui = UIManager.Instance;

        }


    }
}
