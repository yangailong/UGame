using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

/// <summary>启动场景</summary>
public class StartUp : MonoBehaviour
{
    private Slider m_Slider = null;

    private void Awake()
    {
        m_Slider = GetComponentInChildren<Slider>();
    }




    private IEnumerator Start()
    {


        //TODO...从服务器加载dll

        UnityWebRequest request = UnityWebRequest.Head("");

        yield return request.SendWebRequest();



    }


}

