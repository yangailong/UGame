using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace UGame_Local
{
    public class MountChind : MountObjectBase
    {
        [SerializeField, FormerlySerializedAs("m_Childs")]
        private List<Component> m_m_Childs = null;


        public T GetSubChild<T>(string subChildName) where T : Component
        {
            if (string.IsNullOrEmpty(subChildName))
            {
                throw new ArgumentException("subChildName is invalid");
            }

            foreach (var item in m_m_Childs)
            {
                if (item.name.Equals(subChildName))
                {
                    var result = item.GetComponent<T>();

                    if (result != null) return result;
                }
            }

            return null;
        }



        [ContextMenu("查找挂载Child")]
        private void FindChinds()
        {
            m_m_Childs = new List<Component>();
            FindChind(transform);
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

                if (child.GetComponent<MountChind>() != null) continue;

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
