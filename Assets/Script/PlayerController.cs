using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using UnityEngine.WSA;

public class PlayerController : MonoBehaviour
{
    private int speed;
    private float moveHorizontal ;
    private float moveVertical ;
    private Vector2 direction ;
    private List<Vector3> seedVector = new List<Vector3>();

    [Header("SerializeField")]
    [SerializeField] private float stamina;
    [SerializeField] private float maxStamina;
    [SerializeField] private RaycastHit2D hit;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private Transform hitObj;

    [Header("PlayerInfo")]
    public Rigidbody2D rigid;
    public Animator ani;
    public bool usingTool;
    public Image staminaBar;
    public Item item;

    [Header("Prfebs")]
    public GameObject lootPrefeb;
    public GameObject seedPrefeb;

    [Header("FarmTile")]
    public Tilemap baseTileMap;
    public Tilemap sletMap;
    public Tilemap farmMap;
    public TileBase checkTile;
    public TileBase farmTile;
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

        if (item != null && Input.anyKey && !Input.GetKeyDown(KeyCode.Space))
        {
            if(item.actionType == ActionType.Farming || item.type == ItemType.Seed)
            {
                CheckTile();
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(stamina > 10)
            {
                //Action();
            }

            if (usingTool)
            {
                Action();
                stamina -= 10;
            }
            else if(item != null
                && item.type == ItemType.Seed
                && sletMap.GetTile(testPos))
            {
                Vector3 seedPos = new Vector3(testPos.x + 0.5f, testPos.y + 0.5f);
                if(seedVector != null)
                {
                    foreach (var vector in seedVector)
                    {
                        if (vector == seedPos)
                        {
                            return;
                        }
                    }
                }
                
                GameObject seed = Instantiate(seedPrefeb, seedPos, Quaternion.identity);
                seed.GetComponent<SeedController>().farmTile = farmMap;
                seed.GetComponent<SeedController>().myItemInfo = item;
                stamina -= 10;
                seedVector.Add(seedPos);
            }

            if (hitObj != null
            && hitObj.tag == "Seed")
            {
                if (hitObj.GetComponent<SeedController>().glow == 5)
                {
                    hitObj.GetComponent<SeedController>().SpawnFruit();
                }
            }
        }
        else
        {
            Move();
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

    //HoeTile Check
    private void CheckTile()
    {
        sletMap.SetTile(testPos, null);

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

            if (transform.position.x < 0)
            {
                testPos = new Vector3Int((int)(transform.position.x - 1f), (int)(testPos.y), 0);
            }
        }

        if (moveVertical == 0)
        {
            if (moveHorizontal > 0)
            {
                testPos = new Vector3Int((int)(transform.position.x), (int)(transform.position.y), 0);

                if (transform.position.x > 0)
                {
                    testPos = new Vector3Int((int)(transform.position.x + 1), (int)(transform.position.y), 0);
                }

                if (transform.position.y < 0)
                {
                    testPos = new Vector3Int((int)(testPos.x), (int)(transform.position.y - 1), 0);
                }
            }
            else if (moveHorizontal < 0)
            {
                testPos = new Vector3Int((int)(transform.position.x - 1f), (int)(transform.position.y), 0);

                if (transform.position.x > -1 && transform.position.x < 1)
                {
                    testPos = new Vector3Int((int)(transform.position.x - 2f), (int)(transform.position.y), 0);
                }
                else if (transform.position.x < -1)
                {
                    testPos = new Vector3Int((int)(transform.position.x - 2f), (int)(transform.position.y), 0);
                }

                if (transform.position.y < 0)
                {
                    testPos = new Vector3Int((int)(testPos.x), (int)(testPos.y - 1), 0);
                }
            }
        }


        if (item.actionType == ActionType.Farming
            && farmTile != baseTileMap.GetTile(testPos)
            && farmTile != farmMap.GetTile(testPos))
        {
            sletMap.SetTile(testPos, checkTile);
        }
        else if (item.type == ItemType.Seed
            && farmTile == farmMap.GetTile(testPos))
        {
            sletMap.SetTile(testPos, checkTile);
        }
        else
        {
            sletMap.SetTile(testPos, null);
        }

        sletMap.RefreshAllTiles();

    }

    //Player Action and Tool Durability
    private void Action()
    {
        // Player Action
        if (!ani.GetBool("IsWalking") && item != null)
        {
            switch (item.actionType)
            {
                case ActionType.Using:
                    ani.SetTrigger("UsingAxe");
                    if (hitObj != null && hitObj.tag == "Tree")
                    {
                        hitObj.GetComponent<ObjectController>().durability -= 50;
                    }
                    break;

                case ActionType.Gather:

                    break;

                case ActionType.Farming:
                    ani.SetTrigger("UsingHoe");
                    farmMap.SetTile(testPos, farmTile);
                    farmMap.RefreshAllTiles();
                    //sletMap.SetTile(testPos, null);
                    break;

                case ActionType.Plant:
                    break;

                default:
                    break;
            }


        }

        
    }
}
