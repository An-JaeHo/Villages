using System.Collections;
using System.Collections.Generic;
//using System.Diagnostics;
using UnityEngine;

public class ObjectController : MonoBehaviour
{
    [Header("InteractiveObject")]
    public int durability;

    [Header("Item")]
    public Item itemInfo;
    public GameObject LootPrefeb;
    public bool glowCheck;
    public string objName;
    
    private WorldTime worldTime;
    private Sprite[] itemSprite;
    public Sprite[] objSprites;
    private string[] treeNames;
    private SpriteRenderer spriteRenderer;
    private int growDay;

    private void Awake()
    {
        treeNames = new string[5] { "GreenTree", "NormalTree", "OrangeTree", "PinkTree", "RedTree" };
        tag = "Tree";
        glowCheck = false;
        itemInfo = new Item();
    }

    // item info => tag, scale, image, actionType
    void Start()
    {
        LootPrefeb = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().lootPrefeb;
        worldTime = GameObject.FindGameObjectWithTag("GameController").GetComponent<WorldTime>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        durability = 100;
        transform.localScale = new Vector2(1f, 1f);
        growDay = worldTime.currentTime.Days;
        objName = treeNames[Random.Range(0, treeNames.Length)];
        LoadItemInfo(objName);

    }

    void Update()
    {
        if (objName == "NormalTree")
        {
            switch (worldTime.season)
            {
                case 1:
                    spriteRenderer.sprite = itemSprite[1];
                    break;
                case 2:
                    spriteRenderer.sprite = itemSprite[2];
                    break;
                case 3:
                    spriteRenderer.sprite = itemSprite[0];
                    break;
                case 4:
                    spriteRenderer.sprite = itemSprite[3];
                    break;
                default:
                    break;
            }
        }
        else
        {
            if (growDay + 1 <= worldTime.currentTime.Days)
            {
                glowCheck = true;
                spriteRenderer.sprite = itemSprite[1];
            }
            else
            {
                spriteRenderer.sprite = itemSprite[0];
            }
        }
    }

    public void SpawnItem()
    {
        GameObject loot = Instantiate(LootPrefeb, transform.position, Quaternion.identity);

        for (int i = 0; i < objSprites.Length; i++)
        {
            if (objSprites[i].name == objName)
            {
                itemInfo.uiImage = objSprites[i];
            }
        }

        loot.GetComponent<Loot>().Initialize(itemInfo);

        if (objName != "NormalTree")
        {
            spriteRenderer.sprite = itemSprite[0];
            glowCheck = false;
            growDay = worldTime.currentTime.Days;
        }
        else
        {
            Destroy(gameObject);
        }

        
    }

    void LoadItemInfo(string name)
    {
        object[] loadedItem = Resources.LoadAll("Land/Tree/" + name, typeof(Sprite));
        itemSprite = new Sprite[loadedItem.Length];
        for (int i = 0; i < loadedItem.Length; i++)
        {
            itemSprite[i] = (Sprite)loadedItem[i];
        }


        object[] loadedObj = Resources.LoadAll("Item/ItemImage/Item", typeof(Sprite));
        objSprites = new Sprite[loadedObj.Length];

        for (int i = 0; i < loadedObj.Length; i++)
        {
            objSprites[i] = (Sprite)loadedObj[i];
        }

        if(objName == "NormalTree")
        {
            itemInfo.type = ItemType.Tree;
            itemInfo.actionType = ActionType.Gathering;
        }
        else
        {
            itemInfo.type = ItemType.Fruit;
            itemInfo.actionType = ActionType.Gathering;
        }

    }
}
