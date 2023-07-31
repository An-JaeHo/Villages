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
    private int speed;

    [SerializeField] private float stamina;
    [SerializeField] private float maxStamina;
    [SerializeField] private RaycastHit2D hit;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private Transform hitObj;

    [Header("LootPrfebs")]
    public GameObject lootPrefeb;

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

        if (Input.GetKeyDown(KeyCode.Space)&& usingTool)
        {
            Action();
            stamina -= 10;
        }
        else
        {
            Move();
        }
    }

    //Player Movment and direction
    private void Move()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        Vector2 direction = new Vector2(moveHorizontal * speed, moveVertical * speed);

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
            Debug.Log(hit.transform.position);
            hitObj = hit.transform;
        }
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
