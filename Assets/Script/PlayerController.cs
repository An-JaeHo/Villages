using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private int speed;
    private float moveHorizontal ;
    private float moveVertical ;
    private Vector2 direction ;
    private List<Vector3> seedVector = new List<Vector3>();
    public List<GameObject> trees;
    public List<GameObject> seeds;
    private bool checkAni;

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

    GameObject temp;

    void Awake()
    {
        usingTool = false;
        checkAni = false;
        stamina = 100;
        maxStamina = 100;
        rigid = GetComponent<Rigidbody2D>();
        ani = GetComponent<Animator>();
        speed = 1;
        layerMask = LayerMask.GetMask("Collision");
    }

    void Update()
    {
        staminaBar.fillAmount = stamina / maxStamina;
        moveHorizontal = Input.GetAxis("Horizontal");
        moveVertical = Input.GetAxis("Vertical");
        direction = new Vector2(moveHorizontal * speed, moveVertical * speed);

        if (!checkAni)
        {
            if (item != null && Input.anyKey && !Input.GetKeyDown(KeyCode.Space))
            {
                if (item.actionType == ActionType.Farming || item.type == ItemType.Seed)
                {
                    CheckTile();
                }
            }

            if (Input.GetKeyDown(KeyCode.Space)
                && moveHorizontal == 0 
                && moveVertical == 0)
            {
                //&& stamina > 10
                ChackObjDistance(trees);

                //Action();

                if (hitObj != null
                && hitObj.tag == "Seed")
                {
                    if (hitObj.GetComponent<SeedController>().glow == 5)
                    {
                        hitObj.GetComponent<SeedController>().SpawnFruit();
                    }
                }
            }
            else if(Input.GetKey(KeyCode.Space))
            {
                if(temp != null )
                {
                    if(Vector2.Distance(transform.position, temp.transform.position) <= 1)
                    {
                        ani.SetBool("IsWalking", false);

                        switch (item.actionType)
                        {
                            case ActionType.Using:
                                ani.SetTrigger("UsingAxe");
                                Debug.Log("s");
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

                                if (hitObj != null
                                    && hitObj.tag == "Seed")
                                {
                                    hitObj.GetComponent<SeedController>().waterPoint += 20;
                                    hitObj.GetComponent<SeedController>().ChageLandColor();
                                    ani.SetTrigger("UsingWater");
                                }
                                else
                                {
                                    return;
                                }

                                break;

                            default:
                                break;
                        }
                    }
                    else
                    {
                        transform.position = Vector3.MoveTowards(transform.position, temp.transform.position, Time.deltaTime * 1f);

                        Vector3 direction = temp.transform.position - transform.position;
                        Debug.DrawRay(transform.position, direction, Color.black);

                        ani.SetBool("IsWalking", true);
                        ani.SetFloat("X", direction.normalized.x);
                        ani.SetFloat("Y", direction.normalized.y);
                    }
                }
            }
            else if(Input.GetKeyUp(KeyCode.Space))
            {
                ani.SetBool("IsWalking", false);
            }
            else
            {
                Move();
            }
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
        if (!ani.GetBool("IsWalking")
            && item != null
            && usingTool)
        {
            checkAni = true;

            switch (item.actionType)
            {
                case ActionType.Using:
                    ani.SetTrigger("UsingAxe");
                    
                    if (hitObj != null && hitObj.tag == "Tree")
                    {
                        //hitObj.GetComponent<ObjectController>().durability -= 50;
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

                    if(hitObj!= null
                        && hitObj.tag =="Seed")
                    {
                        hitObj.GetComponent<SeedController>().waterPoint += 20;
                        hitObj.GetComponent<SeedController>().ChageLandColor();
                        ani.SetTrigger("UsingWater");
                    }
                    else
                    {
                        return;
                    }

                    break;

                default:
                    break;
            }

            stamina -= 10;
        }
        else if (item != null
                 && item.type == ItemType.Seed
                 && sletMap.GetTile(testPos))
        {
            Vector3 seedPos = new Vector3(testPos.x + 0.5f, testPos.y + 0.5f);

            if (seedVector != null)
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
    }

    public void CallBackAni()
    {
        if (temp != null && temp.tag == "Tree")
        {
            temp.GetComponent<ObjectController>().durability -= 50;
        }

        checkAni = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag =="Tree")
        {
            trees.Add(collision.gameObject);
        }
        else if(collision.tag =="Seed")
        {
            seeds.Add(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Tree")
        {
            trees.Remove(collision.gameObject);
        }
        else if (collision.tag == "Seed")
        {
            seeds.Remove(collision.gameObject);
        }


    }

    private void ChackObjDistance(List<GameObject> Objs)
    {
        if (Objs.Count > 1)
        {
            temp = Objs[0];
            for (int i = 1; i < Objs.Count; i++)
            {
                float firstDistance = Vector3.Distance(transform.position, temp.transform.position);
                float secondDistance = Vector3.Distance(transform.position, Objs[i].transform.position);
                
                if (firstDistance > secondDistance)
                {
                    temp = Objs[i];
                }
            }
        }
        else if(Objs.Count ==1)
        {
            temp = Objs[0];
        }
        else
        {
            temp = null;
        }
    }

    
}
