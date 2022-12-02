using System;
using System.Collections.Generic;
using UnityEngine;

namespace UGame_Remove
{
    public class GlobalEvent
    {
        public delegate void EventHandler(params object[] sender);

        private static Dictionary<Enum, EventHandler> m_EnumEventDic = null;


        public static void Init()
        {
            m_EnumEventDic = new Dictionary<Enum, EventHandler>();
        }


        /// <summary>
        ///  添加事件
        /// </summary>
        /// <param name="key">要添加的事件</param>
        /// <param name="handler">要添加的事件参数</param>
        /// <exception cref="ArgumentNullException">无效参数</exception>
        public static void AddEvent(Enum key, EventHandler handler)
        {
            if (key == null)
            {
                throw new ArgumentNullException($"{nameof(key)} is invalid");
            }

            if (handler == null)
            {
                throw new ArgumentNullException($"{nameof(handler)} is invalid");
            }

            if (m_EnumEventDic.ContainsKey(key))
            {
                m_EnumEventDic[key] += handler;
            }
            else
            {
                m_EnumEventDic.Add(key, handler);
            }
        }


        /// <summary>
        /// 移除某类事件的中的一个方法
        /// </summary>
        /// <param name="key">要移除事件key</param>
        /// <param name="handler">要移除的方法</param>
        /// <exception cref="ArgumentNullException">无效参数</exception>
        public static void RemoveEvent(Enum key, EventHandler handler)
        {
            if (key == null)
            {
                throw new ArgumentNullException($"{nameof(key)} is invalid");
            }

            if (handler == null)
            {
                throw new ArgumentNullException($"{nameof(handler)} is invalid");
            }

            if (m_EnumEventDic.ContainsKey(key))
            {
                m_EnumEventDic[key] -= handler;
            }
        }


        /// <summary>
        ///  移除某类事件
        /// </summary>
        /// <param name="key">要移除事件key</param>
        /// <exception cref="ArgumentNullException">无效参数</exception>
        public static void RemoveEvent(Enum key)
        {
            if (key == null)
            {
                throw new ArgumentNullException($"{nameof(key)} is invalid");
            }

            if (m_EnumEventDic.ContainsKey(key))
            {
                m_EnumEventDic.Remove(key);
            }
        }


        /// <summary>
        /// 触发事件
        /// </summary>
        /// <param name="key">要触发事件key</param>
        /// <param name="sender">事件参数</param>
        /// <exception cref="ArgumentNullException">无效参数</exception>
        public static void DispatchEvent(Enum key, params object[] sender)
        {
            if (key == null)
            {
                throw new ArgumentNullException($"{nameof(key)} is invalid");
            }

            if (m_EnumEventDic.ContainsKey(key))
            {
                try
                {
                    m_EnumEventDic[key]?.Invoke(sender);
                }
                catch (Exception e)
                {
                    Debug.LogError(e.ToString());
                }
            }
        }


        /// <summary>
        /// 移除所有事件
        /// </summary>
        public static void RemoveAllEvent()
        {
            m_EnumEventDic.Clear();
        }


    }
}

