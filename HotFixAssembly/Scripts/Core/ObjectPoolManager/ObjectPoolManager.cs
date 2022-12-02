using UnityEngine;
using UnityEngine.Pool;

namespace UGame_Remove
{
    public class ObjectPoolManager : MonoBehaviour
    {

        private static ObjectPool<Component> pool;

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="collectionCheck"></param>
        /// <param name="defaultCapacity"></param>
        /// <param name="maxSize"></param>
        public static void Init(bool collectionCheck = true, int defaultCapacity = 10, int maxSize = 10000)
        {
            pool = new ObjectPool<Component>(Create, OnGet, OnRelease, OnDestroy, collectionCheck, defaultCapacity, maxSize);
        }


        /// <summary>
        /// 活动和非活动对象的总数
        /// </summary>
        public int CountAll => pool.CountAll;


        /// <summary>
        /// 池已创建但当前正在使用且尚未返回的对象数。
        /// </summary>
        public int CountActive => pool.CountActive;


        /// <summary>
        /// 池中当前可用的对象数
        /// </summary>
        public int CountInactive => pool.CountInactive;


        /// <summary>
        /// 从池中获取实例。如果池为空，则将创建一个新实例。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Get<T>() where T : Component
        {
            return pool.Get() as T;
        }


        /// <summary>
        /// 将实例返回到池中
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="element"></param>
        public static void Release<T>(T element) where T : Component
        {

        }


        /// <summary>
        /// 清除对象池，如果池包含销毁回调，则将为池中的每个项目调用它。
        /// </summary>
        public static void Clear()
        {
            pool.Clear();
        }


        /// <summary>
        /// 清除对象池，如果池包含销毁回调，则将为池中的每个项目调用它。
        /// </summary>
        public static void Dispose()
        {
            pool.Dispose();
        }


        /// <summary>
        /// 创建一个对象
        /// </summary>
        /// <returns></returns>
        private static Component Create()
        {
            return null;
        }


        /// <summary>
        /// 当调用对象池的Get方法是要执行的操作
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="element"></param>
        private static void OnGet<T>(T element)
        {

        }


        /// <summary>
        /// 当调用对象池的OnRelease方法是要执行的操作
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="element"></param>
        private static void OnRelease<T>(T element)
        {

        }


        /// <summary>
        /// 当调用对象池的Clear或者Dispose或者当数量超过最大，对象值自动清理时，调用这个方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="element"></param>
        private static void OnDestroy<T>(T element)
        {

        }

    }
}


