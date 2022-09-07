using UnityEngine;

namespace UGame_Remove
{
    public class GlobalEvent
    {
        public delegate void EventHandler(params object[] sender);

        private static Dictionary<Enum, EventHandler> m_EnumEventDic = new Dictionary<Enum, EventHandler>();


        /// <summary>
        /// 添加事件
        /// </summary>
        /// <param name="key">枚举类型</param>
        /// <param name="handler">回调方法</param>
        public static void AddEvent(Enum key, EventHandler handler)
        {
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
        /// 移除某类事件的中的一个
        /// </summary>
        /// <param name="key">事件类型</param>
        /// <param name="handler">回调方法</param>
        public static void RemoveEvent(Enum key, EventHandler handler)
        {
            if (!m_EnumEventDic.TryGetValue(key, out EventHandler? eventHandler))
            {
                eventHandler -= handler;
            }
        }


        /// <summary>
        /// 移除某类事件
        /// </summary>
        /// <param name="key">事件类型</param>
        public static void RemoveEvent(Enum key)
        {
            if (m_EnumEventDic.ContainsKey(key))
            {
                m_EnumEventDic.Remove(key);
            }
        }


        /// <summary>
        /// 触发事件
        /// </summary>
        /// <param name="key">事件类型</param>
        /// <param name="sender">事件参数</param>
        public static void DispatchEvent(Enum key, params object[] sender)
        {
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

