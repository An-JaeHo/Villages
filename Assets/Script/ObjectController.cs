using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class ObjectController : MonoBehaviour
{
    [Header("InteractiveObject")]
    public int durability;

    [Header("Item")]
    public Item item;
    public GameObject LootPrefeb;

    [SerializeField]
    private WorldTime worldTime;

    [SerializeField]
    private Sprite[] sprite;

    private SpriteRenderer spriteRenderer;

    // item info => tag, scale, image, actionType
    void Start()
    {
        LootPrefeb = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().lootPrefeb;
        worldTime = GameObject.FindGameObjectWithTag("GameController").GetComponent<WorldTime>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        durability = 100;
        transform.localScale = new Vector2(1f, 1f);

        LoadSprite();
    }
    
    void Update()
    {
        if(durability <=0)
        {
            SpawnItem();
        }

        switch (worldTime.season)
        {
            case 1:
                spriteRenderer.sprite = sprite[1];
                break;
            case 2:
                spriteRenderer.sprite = sprite[2];
                break;
            case 3:
                spriteRenderer.sprite = sprite[0];
                break;
            case 4:
                spriteRenderer.sprite = sprite[3];
                break;
            default:
                break;
        }

    }

    void SpawnItem()
    {
        GameObject loot = Instantiate(LootPrefeb, transform.position, Quaternion.identity);
        loot.GetComponent<Loot>().Initialize(item);

        Destroy(gameObject);
    }

    void LoadSprite()
    {
        object[] loadedItem = Resources.LoadAll("Land/Tree/NormalTree", typeof(Sprite));
        
        sprite = new Sprite[loadedItem.Length];

        for (int i = 0; i < loadedItem.Length; i++)
        {
            sprite[i] = (Sprite)loadedItem[i];
        }
    }
}
