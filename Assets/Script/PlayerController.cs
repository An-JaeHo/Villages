using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rigid;
    public Animator ani;
    public bool usingTool;
    public Image staminaBar;
    public Item item;
    private int speed;
    private float moveHorizontal ;
    private float moveVertical ;
    private Vector2 direction ;
    [SerializeField] private float stamina;
    [SerializeField] private float maxStamina;
    [SerializeField] private RaycastHit2D hit;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private Transform hitObj;

    [Header("LootPrfebs")]
    public GameObject lootPrefeb;

    [Header("FarmTile")]
    public Tilemap farmMap;
    public TileBase checkTile;
    public Vector3Int testPos;

    void Awake()
    {
        usingTool = false;
        stamina = 100;
        maxStamina = 100;
        rigid = GetComponent<Rigidbody2D>();
        ani = GetComponent<Animator>();
        speed = 2;
        layerMask = LayerMask.GetMask("Collision");
    }

    void Update()
    {
        staminaBar.fillAmount = stamina/ maxStamina;

        moveHorizontal = Input.GetAxis("Horizontal");
        moveVertical = Input.GetAxis("Vertical");
        direction = new Vector2(moveHorizontal * speed, moveVertical * speed);

        if (Input.GetKeyDown(KeyCode.Space)&& usingTool)
        {
            Action();
            stamina -= 10;
        }
        else
        {
            Move();
        }

        if (usingTool && item.actionType == ActionType.Farming)
        {
            CheckHoeTile();
        }
    }

    //Player Movment and direction
    private void Move()
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            ani.SetBool("IsWalking", false);
            rigid.velocity = Vector2.zero;
        }
        else
        {
            ani.SetBool("IsWalking", true);
            rigid.velocity = new Vector2(moveHorizontal * speed, moveVertical * speed);
        }

        if (moveHorizontal != 0 || moveVertical != 0)
        {
            ani.SetFloat("X", moveHorizontal);
            ani.SetFloat("Y", moveVertical);
            //CheckHoeTile();
        }
        else
        {
            ani.SetBool("IsWalking", false);
        }

        hit = Physics2D.Raycast(transform.position, direction, 0.3f, layerMask);
        Debug.DrawRay(transform.position, direction * 0.3f, Color.red);
        
        if (hit)
        {
            hitObj = hit.transform;
        }
    }

    private void CheckHoeTile()
    {
        farmMap.SetTile(testPos, null);
        
        if (moveHorizontal == 0)
        {
            if (moveVertical > 0)
            {
                testPos = new Vector3Int((int)(transform.position.x), (int)(transform.position.y + 1), 0);

                if (transform.position.y < -1)
                {
                    testPos = new Vector3Int((int)(transform.position.x), (int)(transform.position.y), 0);
                }
            }
            else if (moveVertical < 0)
            {
                testPos = new Vector3Int((int)(transform.position.x), (int)(transform.position.y - 1), 0);

                if (transform.position.y > -1 && transform.position.y < 1)
                {
                    testPos = new Vector3Int((int)(transform.position.x), (int)(transform.position.y - 2f), 0);
                }

                if (transform.position.y < -1)
                {
                    testPos = new Vector3Int((int)(transform.position.x), (int)(transform.position.y - 2f), 0);
                }
            }

            //if (transform.position.x < 0)
            //{
            //    testPos = new Vector3Int((int)(transform.position.x - 1f), (int)(transform.position.y), 0);
            //}
        }

        //if (moveVertical == 0)
        //{
        //    if (moveHorizontal > 0)
        //    {
        //        testPos = new Vector3Int((int)(transform.position.x + 1f), (int)(transform.position.y), 0);

        //        if (transform.position.x < -1)
        //        {
        //            testPos = new Vector3Int((int)(transform.position.x ), (int)(transform.position.y), 0);
        //        }

        //        if (transform.position.y < 0)
        //        {
        //            testPos = new Vector3Int((int)(testPos.x), (int)(testPos.y - 1), 0);
        //        }
        //    }
        //    else if (moveHorizontal < 0)
        //    {
        //        testPos = new Vector3Int((int)(transform.position.x - 1f), (int)(transform.position.y), 0);

        //        if (transform.position.x > -1 && transform.position.x < 1)
        //        {
        //            testPos = new Vector3Int((int)(transform.position.x - 2f), (int)(transform.position.y), 0);
        //        }

        //        if (transform.position.x < -1)
        //        {
        //            testPos = new Vector3Int((int)(transform.position.x - 2f), (int)(transform.position.y), 0);
        //        }

        //        if (transform.position.y < 0)
        //        {
        //            testPos = new Vector3Int((int)(testPos.x), (int)(testPos.y - 1), 0);
        //        }
        //    }
        //}

        farmMap.SetTile(testPos, checkTile);
        farmMap.RefreshAllTiles();
    }

    //Player Action
    private void Action()
    {
        if(!ani.GetBool("IsWalking"))
        {
            ani.SetTrigger("UsingTool");
            
        }

        if(hitObj != null)
        {
            hitObj.GetComponent<ObjectController>().durability -= 50;
        }
    }
}
