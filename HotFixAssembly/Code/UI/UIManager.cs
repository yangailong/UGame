using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class UIManager: MonoBehaviour
{
    public static UIManager Instance { get { return MonoSingleton<UIManager>.Instance; } }

    public void Awake()
    {
        Destroy(gameObject);
    }


    public UIBase Open()
    {
        return null;
    }



    public UIBase Close()
    {
        return null;
    }

    public bool IsOpen()
    {
        return false;
    }

    public void CloseAll()
    {
        
    }


    

}

