using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTouch : MonoBehaviour
{
    Player player;
    private void Awake()
    {
        player = transform.parent.GetComponent<Player>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (player.isServer)
        {
            if (collision.CompareTag("Servitor"))
            {
                Servitor servitor = collision.GetComponent<Servitor>();
                if (servitor != null)
                {
                    player.AddServitor(servitor);
                }
            }
        }
    }
}
