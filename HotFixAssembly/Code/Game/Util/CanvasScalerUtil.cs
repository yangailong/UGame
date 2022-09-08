using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace UGame_Remove
{
    public class CanvasScalerUtil : MonoBehaviour
    {
        //检测频率
        private float m_Frequency = 0.1f;

        private Vector2 ReferenceResolution = new Vector2(Screen.width, Screen.height);

        private Vector2 oldScreenRect = new Vector2(Screen.width, Screen.height);

        private CanvasScaler? canvasScaler = null;

        void Awake()
        {
            canvasScaler = GetComponent<CanvasScaler>();
            ReferenceResolution = canvasScaler.referenceResolution;
            oldScreenRect = ReferenceResolution;
        }

        //IEnumerator Start()
        //{
        //    while (true)
        //    {
        //        yield return new WaitForSeconds(m_Frequency);
        //        Refresh();
        //    }

        //}


        


        private void Refresh()
        {
            if (Screen.width != oldScreenRect.x || Screen.height != oldScreenRect.y)
            {
                oldScreenRect.x = Screen.width;
                oldScreenRect.y = Screen.height;
                ReferenceResolution = canvasScaler.referenceResolution;

                float ratio = (float)Screen.height / (float)Screen.width;

                if (ratio > ReferenceResolution.y / ReferenceResolution.x)
                {
                    ReferenceResolution.y = Screen.height * ReferenceResolution.x / Screen.width;
                    canvasScaler.matchWidthOrHeight = 0;
                }
                else
                {
                    canvasScaler.matchWidthOrHeight = 1;
                    ReferenceResolution.x = Screen.width * ReferenceResolution.y / Screen.height;
                }

            }
        }


    }
}

