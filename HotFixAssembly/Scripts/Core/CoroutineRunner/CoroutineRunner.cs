using System;
using System.Collections;
using UnityEngine;

namespace UGame_Remove
{
    /// <summary>
    /// 延迟执行函数
    /// </summary>
    public class CoroutineRunner : ComponentSingleton<CoroutineRunner>
    {

        /// <summary>
        /// 唤起一个协程
        /// </summary>
        /// <param name="routine">要唤起的协程</param>
        public static void OverStartCoroutine(IEnumerator routine)
        {
            Instance.StartCoroutine(routine);
        }


        /// <summary>
        /// 结束一个协程
        /// </summary>
        /// <param name="routine">要结束的协程</param>
        public static void OverStopCoroutine(IEnumerator routine)
        {
            Instance.StopCoroutine(routine);
        }


        /// <summary>
        /// 等待几秒，执行一个方法
        /// </summary>
        /// <param name="seconds">等待秒数</param>
        /// <param name="action">要执行的方法</param>
        public static void WaitForSeconds(float seconds, Action action)
        {
            Instance.StartCoroutine(Instance.DoWaitForSeconds(seconds, action));
        }



        /// <summary>
        /// 等待几帧，执行一个方法
        /// </summary>
        /// <param name="frames">要等待的帧数</param>
        /// <param name="action">要执行的方法</param>
        public static void WaitForFrames(int frames, Action action)
        {
            Instance.StartCoroutine(Instance.DoWaitForFrames(frames, action));
        }


        /// <summary>
        /// 在自定义时间范围，实时执行一个方法
        /// </summary>
        /// <param name="seconds">自定义时间</param>
        /// <param name="action">要实时执行的方法</param>
        public static void InSecondsUpdate(float seconds, Action<float> action)
        {
            Instance.DoInSecondsUpdate(seconds, action);
        }


        /// <summary>
        /// 等待几秒，执行一个方法
        /// </summary>
        /// <param name="seconds">等待秒数</param>
        /// <param name="action">要执行的方法</param>
        /// <returns></returns>
        private IEnumerator DoWaitForSeconds(float seconds, Action action)
        {
            yield return new WaitForSeconds(seconds);

            action?.Invoke();
        }


        /// <summary>
        /// 等待几帧，执行一个方法
        /// </summary>
        /// <param name="frames">要等待的帧数</param>
        /// <param name="action">要执行的方法</param>
        /// <returns></returns>
        private IEnumerator DoWaitForFrames(int frames, Action action)
        {
            while (frames > 0)
            {
                frames--;
                yield return 0;
            }
            action?.Invoke();
        }


        /// <summary>
        /// 在自定义时间范围，实时执行一个方法
        /// </summary>
        /// <param name="seconds">自定义时间</param>
        /// <param name="action">要实时执行的方法</param>
        private IEnumerator DoInSecondsUpdate(float seconds, Action<float> action)
        {
            float time = 0;

            while (time <= seconds)
            {
                time += Time.deltaTime;

                action?.Invoke(time);

                yield return null;
            }
        }

    }
}
