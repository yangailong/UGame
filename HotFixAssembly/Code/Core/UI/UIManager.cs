using System;
using System.Collections.Generic;
using UnityEngine;

namespace UGame_Remove
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance { get { return MonoSingleton<UIManager>.Instance; } }

        public void Awake()
        {
            Debug.Log("455641651456");
            Destroy(gameObject);
        }

        void Start()
        {
            Debug.Log("Start");
        }

        void Update()
        {

        }

        public UIBase Open()
        {
            return null;
        }



        public UIBase Close()
        {
            return null;
        }

        public bool IsOpen()
        {
            return false;
        }

        public void CloseAll()
        {

        }




    }
}
