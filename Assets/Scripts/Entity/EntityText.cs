using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityText : MonoBehaviour
{
    // Start is called before the first frame update
    float x = 0;
    void Start()
    {
        List<int> list = new List<int>() { 0,1,2,3,4,5,6,7,8,9};
        int j = 20;
        for (int i = 0; i < list.Count && j > 0; j--) 
        {
            list.Remove(i);
            i++;
        }
        foreach(int i in list)
        {
            Debug.Log(i);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {


    }
}
