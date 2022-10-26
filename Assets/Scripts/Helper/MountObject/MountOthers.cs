using System.Collections.Generic;
using UnityEngine.Serialization;
using UnityEngine;
using Object = UnityEngine.Object;
using System;

namespace UGame_Local
{
    public class MountOthers : MountObjectBase
    {
        [Space(15), SerializeField, FormerlySerializedAs("m_Others")]
        private List<Object> m_Others = null;


        public Object GetSubOther(int index)
        {
            if (m_Others != null) return null;

            if (index < 0 || index >= m_Others.Count)
            {
                throw new ArgumentException("index is invalid");
            }

            if (index < m_Others.Count)
            {
                return m_Others[index];
            }

            return null;

        }


    }
}
