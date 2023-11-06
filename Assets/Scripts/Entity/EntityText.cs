using Steamworks;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("E");
            foreach(var friend in SteamFriends.GetFriends().ToArray())
            {
                if (friend.GameInfo != null)
                {
                    //Debug.Log(friend.GameInfo.Value.);
                }

            }
            
        }
    }

    private void FixedUpdate()
    {


    }
}
