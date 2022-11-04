using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Object = UnityEngine.Object;

namespace UGame_Local
{
    public class MountObject : MonoBehaviour
    {
        [Tooltip("查找以m_开头的Transform")]
        [SerializeField, FormerlySerializedAs("m_Childs")]
        private List<Component> m_m_Childs = null;

        [Tooltip("挂载的其他Object")]
        [Space(15), SerializeField, FormerlySerializedAs("m_Others")]
        private List<Object> m_Others = null;

        private Dictionary<string, Component[]> valuePairs = null;


        public T GetChild<T>(string childName) where T : Component
        {
            if (string.IsNullOrEmpty(childName) || !valuePairs.TryGetValue(childName, out var components))
            {
                throw new ArgumentException($"{nameof(childName)} is invalid");
            }

            foreach (var item in components)
            {
                if (item is T) return item as T;
            }

            return null;
        }


        public Object GetOther(int index)
        {
            if (m_Others == null || m_Others.Count == 0) return null;

            if (index < 0 || index >= m_Others.Count)
            {
                throw new ArgumentException($"{nameof(index)} is invalid");
            }
            return m_Others[index];
        }


        [ContextMenu("查找挂载Child")]
        private void FindChinds()
        {
            m_m_Childs = new List<Component>();
            valuePairs = new Dictionary<string, Component[]>();

            FindChind(transform);

            foreach (Transform item in m_m_Childs)
            {
                if (!valuePairs.ContainsKey(item.name))
                {
                    var coms = item.GetComponents<Component>();
                    valuePairs.Add(item.name, coms);
                }
            }
        }


        private void FindChind(Transform transform)
        {
            foreach (Transform child in transform)
            {
                if (child.name.Substring(0, 2).Equals("m_"))
                {
                    if (!m_m_Childs.Contains(child))
                    {
                        m_m_Childs.Add(child);
                    }
                }

                if (child.GetComponent<MountObject>() != null) continue;

                if (child.childCount > 0)
                {
                    FindChind(child);
                }
            }
        }


        private void OnValidate()
        {
            FindChinds();
        }


    }
}
