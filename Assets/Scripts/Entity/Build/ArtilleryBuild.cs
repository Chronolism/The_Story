using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;

public class ArtilleryBuild : BaseBuild, IEntityInteractive
{
    public float initAngle;
    public float maxAngle;
    public float maxDistance;
    public float minDistance;

    private float leftAngle;
    private float rightAngle;

    [SyncVar]
    public float angle;
    [SyncVar]
    public float distance;

    public GameObject E;

    public GameObject body;
    public GameObject pos;
    public GameObject target;

    public float getEnergy = 20;
    private float ratationSpeed = 45;
    private float distanceSpeed = 2;

    private bool isShot = false;

    public void Init(List<float> floats)
    {
        initAngle = floats[0];
        body.transform.rotation *= Quaternion.AngleAxis(initAngle, Vector3.forward);
        maxAngle = floats[1];
        leftAngle = initAngle + maxAngle;
        rightAngle = initAngle - maxAngle;
        maxDistance = floats[2];
        minDistance = floats[3];

        angle = initAngle;
        distance = minDistance;
    }

    private void FixedUpdate()
    {
        if (isServer)
        {
            if (time > 0)
            {
                time -= Time.deltaTime;
            }
        }
        E.SetActive(time <= 0);
    }

    private void Update()
    {
        if (isServer && entityParent != null)
        {
            PlayerContrl();
        }
        body.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        pos.transform.localPosition = new Vector3(distance, 0, 0);
    }

    private void Start()
    {
        ReSetBuild();
    }

    public void Interactive(Entity entity)
    {
        if (entityParent == null && entity is Player player && time <= 0) 
        {
            Working(player);
        }
    }

    private void Working(Player player)
    {
        time = timeMax;
        entityParent = player;
        player.Inhibit(true);
        player.HideEntity(true);
        WorkingRpc(player.netId);
    }
    [ClientRpc]
    private void WorkingRpc(uint netid)
    {
        if (isServer) return;
        Player player = Mirror.Utils.GetSpawnedInServerOrClient(netId).GetComponent<Player>();
        player.HideEntity(true);
    }

    private void PlayerContrl()
    {
        if (isShot) return;

        if (entityParent.inputX != 0)
        {
            angle += entityParent.inputX * ratationSpeed * Time.deltaTime;
            angle = Mathf.Clamp(angle, rightAngle, leftAngle);
        }
        if (entityParent.inputY != 0)
        {
            distance += entityParent.inputY * distanceSpeed * Time.deltaTime;
            distance = Mathf.Clamp(distance, minDistance, maxDistance);
        }
        if (entityParent.fire1 != 0)
        {
            StartCoroutine(IStartShot());
        }
    }

    float flySpeed = 3;

    IEnumerator IStartShot()
    {
        isShot = true;

        entityParent.transform.position = transform.position;
        target.transform.position = transform.position;

        entityParent.transform.SetParent(target.transform);
        entityParent.HideEntity(false);
        StartShotRpc(entityParent.netId);

        AStarNode aStar = AStarMgr.Instance.FindNearNode(pos.transform.position, entityParent.mapColliderType);

        Vector3 v3 = new Vector3(aStar.pos.x - transform.position.x, aStar.pos.y - transform.position.y, 0);

        float flyTime = v3.magnitude / flySpeed;

        while(flyTime > 0)
        {
            flyTime -= Time.deltaTime;
            target.transform.Translate(v3 * Time.deltaTime);
            yield return null;
        }
        entityParent.transform.parent = null;
        isShot = false;
        entityParent.Inhibit(false);
        entityParent.AddEnergy(new InkData(0, getEnergy, true));
        ReSetBuild();
    }
    [ClientRpc]
    public void StartShotRpc(uint netid)
    {
        if (isServer) return;
        Player player = Mirror.Utils.GetSpawnedInServerOrClient(netId).GetComponent<Player>();
        player.HideEntity(false);
    }

    public void OnActive()
    {
        
    }

    public void OnNotActive()
    {
        
    }

}
