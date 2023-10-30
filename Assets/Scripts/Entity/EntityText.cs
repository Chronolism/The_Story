using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityText : MonoBehaviour
{
    // Start is called before the first frame update
    float x = 0;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (x < 2)
        {
            x += Time.deltaTime;
            Debug.Log(Mathf.Sin(x * 10 * Mathf.PI) > 0 ? 1 : -1);
        }

    }
}
