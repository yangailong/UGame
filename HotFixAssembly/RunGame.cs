
using UnityEngine;
namespace UGame_Remove
{
    public class RunGame
    {
        public static void StartUp()
        {
            Debug.Log($"---调用成功了:StartUp");

            //GameObject gameObject = new GameObject("UIManager");

            //gameObject.AddComponent<Manager>();

            //var mgr = gameObject.GetComponent<Manager>();
            //mgr.Test();

            Init();


        }


        public static void Init()
        {
            var ui = UIManager.Instance;

        }




    }

    public class Manager : MonoBehaviour
    {
        void Awake()
        {
            Debug.Log("Awake");
        }

        void Start()
        {
            Debug.Log("Start");

        }

        //void Update()
        //{
        //    Debug.Log("---");
        //}

        public void Test()
        {
            Debug.Log("Test");
        }

    }
}
