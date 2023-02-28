using System;
using System.Collections.Generic;
using UnityEngine.Events;

namespace UGame_Remove
{
    public class GlobalEvent
    {

        private static Dictionary<Enum, UnityEvent<object>> eventDictionary = null;

        public static void Init()
        {
            if (eventDictionary == null)
            {
                eventDictionary = new Dictionary<Enum, UnityEvent<object>>();
            }
        }


        /// <summary>
        /// 添加事件
        /// </summary>
        /// <param name="eventName">事件名</param>
        /// <param name="listener">要添加的事件</param>
        public static void AddListener(Enum eventName, UnityAction<object> listener)
        {
            if (eventDictionary.TryGetValue(eventName, out UnityEvent<object> thisEvent))
            {
                thisEvent.AddListener(listener);
            }
            else
            {
                thisEvent = new UnityEvent<object>();
                thisEvent.AddListener(listener);
                eventDictionary.Add(eventName, thisEvent);
            }
        }


        /// <summary>
        /// 移出事件
        /// </summary>
        /// <param name="eventName">事件名</param>
        /// <param name="listener">要移出的事件</param>
        public static void RemoveListener(Enum eventName, UnityAction<object> listener)
        {
            if (eventDictionary.TryGetValue(eventName, out UnityEvent<object> thisEvent))
            {
                thisEvent.RemoveListener(listener);
            }
        }


        /// <summary>
        /// 触发事件
        /// </summary>
        /// <param name="eventName">事件名</param>
        /// <param name="eventData">要触发的事件参数</param>
        public static void TriggerEvent(Enum eventName, object eventData = null)
        {
            if (eventDictionary.TryGetValue(eventName, out UnityEvent<object> thisEvent))
            {
                thisEvent.Invoke(eventData);
            }
        }


        /// <summary>
        /// 移除所有事件
        /// </summary>
        public static void RemoveAll()
        {
            foreach (var item in eventDictionary)
            {
                item.Value.RemoveAllListeners();
            }
            eventDictionary.Clear();
        }


    }
}

