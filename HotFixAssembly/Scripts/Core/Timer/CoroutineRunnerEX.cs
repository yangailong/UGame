using System;
using System.Collections;
using UnityEngine;

namespace UGame_Remove
{
    public class CoroutineRunnerEX : MonoBehaviour
    {

        public static void Start(IEnumerator routine)
        {
            CoroutineRunnerEX ins = instance;
            if (ins == null) { return; }
            ins.StartCoroutine(routine);
        }


        public static void DelayFrameCall(int Frame, Action action)
        {
            CoroutineRunnerEX ins = instance;
            if (ins == null) { return; }
            ins.StartCoroutine(ins.WaitForFrame(Frame, action));
        }


        private IEnumerator WaitForFrame(int frame, Action action)
        {
            while (frame > 0)
            {
                frame--;
                yield return 0;
            }
            if (action != null)
            {
                action();
            }
        }


        public static void DelayCall(float delay, Action action)
        {
            CoroutineRunnerEX ins = instance;
            if (ins == null) { return; }
            ins.StartCoroutine(ins.WaitForSecond(delay, action));
        }

        private IEnumerator WaitForSecond(float delay, Action action)
        {
            yield return new WaitForSeconds(delay);
            if (action != null)
            {
                action();
            }
        }


        private static CoroutineRunnerEX s_instance;
        public static CoroutineRunnerEX instance
        {
            get
            {
#if UNITY_EDITOR
				if (s_instance == null && Application.isPlaying)
#else
                if (s_instance == null)
#endif
                {
                    GameObject go = new GameObject("CoroutineRunner");
                    DontDestroyOnLoad(go);
                    s_instance = go.AddComponent<CoroutineRunnerEX>();
                    s_instance.enabled = false;
                }
                return s_instance;
            }
        }



    }
}
