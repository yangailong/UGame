
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
            UIManager.Init();
        }
    }

    public class Demo : MonoBehaviour
    {
        //[SerializeField]
        private string tip = "kanwoya ";
        void Awake()
        {
            Debug.Log("Awake");
        }

        void Start()
        {
            Debug.Log("Start");
        }
    }
}