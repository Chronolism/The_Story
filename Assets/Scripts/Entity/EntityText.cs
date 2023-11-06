using Steamworks;
using System;
using System.Runtime.InteropServices;
using Steamworks.Data;
using System.Linq;
using UnityEngine;
using System.Text;
using static Steamworks.Friend;

public class EntityText : MonoBehaviour
{
    // Start is called before the first frame update
    float x = 0;
    void Start()
    {
        UIManager.Instance.ShowPanel<SearchRoomPanel>();
    }

    // Update is called once per frame
    void Update()
    {
        

    }

    private void FixedUpdate()
    {


    }
}
