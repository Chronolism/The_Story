using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //UIManager.Instance.ShowPanel<MainMenuPanel>();
        //long time = DateTime.Now.Ticks/100000;

        //TimeSpan t = TimeSpan.FromSeconds(time);
        //Debug.Log(time);
        InvokeRepeating("test", 1, 1);
    }

    void test()
    {
        long time = DateTime.Now.Ticks / 10000000;
        //TimeSpan t = TimeSpan.FromSeconds(time);
        Debug.Log(time);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
