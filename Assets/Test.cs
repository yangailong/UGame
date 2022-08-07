using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        UIMgr.Init();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            UIMgr.Open(typeof(TestA).Name, (p1, p2) => { Debug.Log(p2[0]); }, 1, 2);
        }
        if (Input.GetKeyDown(KeyCode.C))
        {

            UIMgr.Close(typeof(TestA).Name);
        }
    }
}
