using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mono_CameraContoller : MonoBehaviour
{
    Camera targetCamera;
    Vector3 targetPos;
    void Start()
    {
        GameManager.Instance.camera = this.GetComponent<Camera>();
        targetCamera = this.GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if(PlayerManager.Instance.LocalPlayer != null && PlayerManager.Instance.LocalPlayer.runtime_Player != null)
            this.transform.position = new Vector3(PlayerManager.Instance.LocalPlayer.runtime_Player.transform.position.x, PlayerManager.Instance.LocalPlayer.runtime_Player.transform.position.y,-10);
    }
    
}
