using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mono_CameraContoller : MonoBehaviour
{
    public Camera targetCamera;
    public bool AllowCameraFollow = true;
    Vector3 targetPos;
    void Start()
    {
        GameManager.Instance.camera = this.GetComponent<Camera>();
        targetCamera = this.GetComponent<Camera>();
        targetPos = targetCamera.transform.position;
        DontDestroyOnLoad(this);
    }

    // Update is called once per frame
    void Update()
    {
        if(PlayerManager.Instance.LocalPlayer != null && PlayerManager.Instance.LocalPlayer.runtime_Player != null)
            this.transform.position = new Vector3(PlayerManager.Instance.LocalPlayer.runtime_Player.transform.position.x, PlayerManager.Instance.LocalPlayer.runtime_Player.transform.position.y,-10);
    }
    private void FixedUpdate()
    {
        if(AllowCameraFollow && DataMgr.Instance.activePlayer != null)
        {
            targetPos = DataMgr.Instance.activePlayer.transform.position;
            targetPos = new Vector3(targetPos.x, targetPos.y, -10);
            if (Vector3.Distance(targetCamera.transform.position, targetPos) < 0.01f)
                targetCamera.transform.position = targetPos;
            else
            {
                targetCamera.transform.position = Vector3.Slerp(targetCamera.transform.position, targetPos, Time.deltaTime * 2f);
            }
        }      
    }
}
