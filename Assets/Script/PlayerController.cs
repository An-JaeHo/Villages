using System;
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
    public float moveHorizontal ;
    public float moveVertical ;
    private Vector2 direction ;
    private List<Vector3> seedVector = new List<Vector3>();
    private bool checkAni;
    private WorldTime worldTime;

    [Header("SerializeField")]
    [SerializeField] private List<GameObject> trees;
    [SerializeField] private List<GameObject> seeds;
    [SerializeField] private List<GameObject> gateringObj;

    [Header("PlayerInfo")]
    public Rigidbody2D rigid;
    public Animator ani;
    public Item item;
    public bool usingTool;
    public bool moveCheck;
    public float stamina;
    public float maxStamina;

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

    [Header("Ui")]
    public Image staminaBar;
    public GameObject sleepUi;

    GameObject temp;

    void Awake()
    {
        usingTool = false;
        checkAni = false;
        moveCheck = true;
        stamina = 100;
        maxStamina = 100;
        rigid = GetComponent<Rigidbody2D>();
        ani = GetComponent<Animator>();
        speed = 1;
        worldTime = GameObject.FindGameObjectWithTag("GameController").GetComponent<WorldTime>();
    }

    void Update()
    {
        staminaBar.fillAmount = stamina / maxStamina;
        moveHorizontal = Input.GetAxis("Horizontal");
        moveVertical = Input.GetAxis("Vertical");
        direction = new Vector2(moveHorizontal * speed, moveVertical * speed);

        // 아직 스테미너에 따라 움직이게 설정 안해놓음
        if (!checkAni
            && moveCheck)
        {
            if (Input.GetKeyDown(KeyCode.Space)
                && moveHorizontal == 0
                && moveVertical == 0
                )
            {
                temp = null;
                if (item != null)
                {
                    switch (item.actionType)
                    {
                        case ActionType.Cuting:
                            CheckObjDistance(trees);
                            break;
                        case ActionType.Plant:
                            CheckObjDistance(seeds);
                            break;
                        default:
                            temp = null;
                            break;
                    }

                    FarmSystem();
                }
                else
                {
                    CheckObjDistance(gateringObj);
                }
            }
            else if (Input.GetKey(KeyCode.Space) && temp != null)
            {
                if (Vector2.Distance(transform.position, temp.transform.position) < 0.5f)
                {
                    ani.SetBool("IsWalking", false);
                    checkAni = true;

                    if (item != null)
                    {
                        if (item.actionType != ActionType.Farming
                            && item.type == ItemType.Tool)
                        {
                            ToolAction();
                        }
                        else
                        {
                            if (temp.tag == "Tree")
                            {
                                if (temp.GetComponent<ObjectController>().glowCheck)
                                {
                                    temp.GetComponent<ObjectController>().SpawnItem();
                                    gateringObj.Remove(temp);
                                }
                            }
                            else
                            {
                                temp.GetComponent<SeedController>().SpawnFruit();
                            }

                            checkAni = false;
                        }
                    }
                    else
                    {
                        if (temp.tag == "Tree")
                        {
                            if(temp.GetComponent<ObjectController>().glowCheck )
                            {
                                temp.GetComponent<ObjectController>().SpawnItem();
                                gateringObj.Remove(temp);
                            }
                        }
                        else
                        {
                            temp.GetComponent<SeedController>().SpawnFruit();
                        }
                        
                        checkAni = false;
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
            else if (Input.GetKeyUp(KeyCode.Space))
            {
                ani.SetBool("IsWalking", false);
            }
            else
            {
                Move();
            }
        }
        else
        {
            
        }
    }

    //Player Movment and direction
    private void Move()
    {
        if (item != null && Input.anyKey && !Input.GetKeyDown(KeyCode.Space))
        {
            if (item.actionType == ActionType.Farming || item.type == ItemType.Seed)
            {
                CheckTile();
            }
        }

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
    private void ToolAction()
    {
        // Player Action
        if (!ani.GetBool("IsWalking")
        && item != null
        && usingTool)
        {
            switch (item.actionType)
            {
                case ActionType.Cuting:
                    ani.SetTrigger("UsingAxe");
                    break;
                case ActionType.Plant:
                    ani.SetTrigger("UsingWater");
                    break;
                default:
                    break;
            }
        }
    }

    private void FarmSystem()
    {
        if (item.actionType == ActionType.Farming)
        {
            ani.SetTrigger("UsingHoe");
            checkAni = true;
            farmMap.SetTile(testPos, farmTile);
            farmMap.RefreshAllTiles();
            sletMap.SetTile(testPos, null);
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
        else
        {
            if (item.type == ItemType.Fruit || item.type == ItemType.Tree)
            {
                CheckObjDistance(gateringObj);
            }
        }
    }

    //각 도구별 오브젝트 정리시에 바꿔줄 예정
    public void CallBackAni()
    {
        if (temp != null
            && checkAni)
        {
            switch (temp.tag)
            {
                case "Tree":
                    //2번으로 나무 베이게 만듬
                    temp.GetComponent<ObjectController>().durability -= 50;

                    if(temp.GetComponent<ObjectController>().durability <=0)
                    {
                        temp.GetComponent<ObjectController>().SpawnItem();
                        
                        temp = null;
                    }
                        break;

                case "Seed":
                    temp.GetComponent<SeedController>().waterPoint += 20;
                    temp.GetComponent<SeedController>().ChageLandColor();
                    break;

                default:
                    break;
            }
        }

        stamina -= 10;
        checkAni = false;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Tree")
        {
            if (collision.GetComponent<ObjectController>().objName != "NormalTree")
            {
                if (collision.GetComponent<ObjectController>().glowCheck)
                {
                    if(!gateringObj.Contains(collision.gameObject))
                    {
                        gateringObj.Add(collision.gameObject);
                    }
                }

            }
            else
            {
                if (!trees.Contains(collision.gameObject))
                {
                    trees.Add(collision.gameObject);
                }
            }
        }
        else if (collision.tag == "Seed")
        {
            if (collision.GetComponent<SeedController>().glow >= 5)
            {
                if (!gateringObj.Contains(collision.gameObject))
                {
                    gateringObj.Add(collision.gameObject);
                }
            }

            if (!seeds.Contains(collision.gameObject))
            {
                seeds.Add(collision.gameObject);
            }
        }

        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "House"
            
            )
        {
            sleepUi.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Tree")
        {
            if(collision.GetComponent<ObjectController>().objName != "NormalTree")
            {
                if(collision.GetComponent<ObjectController>().glowCheck)
                {
                    gateringObj.Remove(collision.gameObject);
                }
            }
            else
            {
                trees.Remove(collision.gameObject);
            }
            
        }
        else if (collision.tag == "Seed")
        {
            if (collision.GetComponent<SeedController>().glow >= 5)
            {
                gateringObj.Remove(collision.gameObject);
            }
            seeds.Remove(collision.gameObject);
        }
    }

    private void CheckObjDistance(List<GameObject> Objs)
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
