using UnityEngine;
using System.Collections.Generic;

public class EntityText : MonoBehaviour
{
    // Start is called before the first frame update
    float x = 0;
    void Start()
    {
        UIManager.Instance.ShowPanel<StartPanel>();
    }

    // Update is called once per frame
    void Update()
    {
        

    }

    private void FixedUpdate()
    {


    }
}
