using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem;
using UnityEngine.UI;
/// <summary> 说明</summary>
public class OpenCSharp : MonoBehaviour
{

    private void Awake()
    {
       string value= PlayerPrefs.GetString(null,null);

        Debug.Log($"{value}");

      
    }

}


