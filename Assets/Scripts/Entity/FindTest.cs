using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindTest : MonoBehaviour
{


    private float inputX, inputY;
    public int inputDir;
    public int dir;
    public float maxSpeed = 1;

    private Vector2 movementInput;
    public Vector2 movement;

    private float speedper;

    private bool isMoving;
    private bool inputDisable;

    private Rigidbody2D rb;
    public float InputX;
    public float InputY;

    private void Awake()
    {
        rb = this.transform.parent.GetComponent<Rigidbody2D>();//控制PlayerRuntime的移动而非本体
        this.transform.parent.GetComponent<PlayerRuntime>().inputAndMove = this.gameObject;
    }

    void Start()
    {

        MapManager.Instance.LoadMapCompletelyToScene("400");

        AStarMgr.Instance.InitMapInfo(MapManager.Instance.mapColliderData);

        this.transform.position = new Vector3(0.5f, 0.5f);

        movement = this.transform.position;
    }

    // Update is called once per frame
    private void Update()
    {
        PlayerInput();
    }

    private void FixedUpdate()
    {
        Movement();
    }


    private void PlayerInput()
    {
        inputX = InputX;
        inputY = InputY;       
    }
    private void Movement()
    {
        speedper = Mathf.Sqrt(inputX * inputX + inputY * inputY);
        if (speedper > 1)
        {
            inputX /= speedper;
            inputY /= speedper;
        }
        movementInput = new Vector2(inputX, inputY);

        isMoving = movementInput != Vector2.zero;

        if (isMoving)
        {
            inputDir = Mathf.Abs(inputX) >= Mathf.Abs(inputY) ? inputX > 0 ? 0 : 2 : inputY > 0 ? 1 : 3;
            if (dir != inputDir)
            {
                if ((Mathf.Abs(dir - inputDir)&1) != 1)
                {
                    if (ChackMap(ref movement, inputDir))
                    {
                        dir = inputDir;
                    }
                }
            }
        }

        rb.velocity = Vector2.zero;

        if (Vector2.Distance(rb.position, movement) > 0.1)
        {
            rb.MovePosition(rb.position + (movement - rb.position).normalized * maxSpeed * Time.deltaTime);
        }
        else
        {
            if (ChackMap(ref movement, inputDir))
            {
                dir = inputDir;
                rb.MovePosition(rb.position + (movement - rb.position).normalized * maxSpeed * Time.deltaTime);
            }
            else if(ChackMap(ref movement, dir))
            {
                inputDir = dir;
                rb.MovePosition(rb.position + (movement - rb.position).normalized * maxSpeed * Time.deltaTime);
            }
        }
    }

    bool ChackMap(ref Vector2 v2, int dir)
    {
        switch (dir)
        {
            case 0:
                if (AStarMgr.Instance.ChackType((int)Mathf.Floor( v2.x + 1), (int)Mathf.Floor(v2.y), E_Node_Type.Walk))
                {
                    v2.x += 1;
                    return true;
                }
                break;
            case 1:
                if (AStarMgr.Instance.ChackType((int)Mathf.Floor(v2.x), (int)Mathf.Floor(v2.y + 1), E_Node_Type.Walk))
                {
                    v2.y += 1;
                    return true;
                }
                break;
            case 2:
                if (AStarMgr.Instance.ChackType((int)Mathf.Floor(v2.x - 1), (int)Mathf.Floor(v2.y), E_Node_Type.Walk))
                {
                    v2.x -= 1;
                    return true;
                }
                break;
            case 3:
                if (AStarMgr.Instance.ChackType((int)Mathf.Floor(v2.x), (int)Mathf.Floor(v2.y) - 1, E_Node_Type.Walk))
                {
                    v2.y -= 1;
                    return true;
                }
                break;
        }
        return false;
    }

}
