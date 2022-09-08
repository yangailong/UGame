using UnityEngine;

namespace UGame_Remove
{
    public class RunGame
    {
        public static void StartUp()
        {
            Debug.Log($"UGame_Remove StartUp");

            Init();
        }


        public static void Init()
        {
            //UIManager.Init();
        }

    }

    public class binderTest:UnityEngine.MonoBehaviour
    {

        void Awake()
        {
            Debug.Log("Awake");
        }

        void OnDisable()
        {
            Debug.Log("OnDisable");
        }
    }
}
