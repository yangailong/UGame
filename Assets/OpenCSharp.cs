using UnityEngine;
/// <summary> 说明</summary>
public class OpenCSharp : MonoBehaviour
{

    private void Awake()
    {
       string value= PlayerPrefs.GetString(null,null);

        Debug.Log($"{value}");

      
    }

}


