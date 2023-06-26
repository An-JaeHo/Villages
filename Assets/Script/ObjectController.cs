using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectController : MonoBehaviour
{
    [Header("InteractiveObject")]
    public int durability;

    [Header("Item")]
    public Item item;
    public GameObject LootPrefeb;

    // item info => tag, scale, image, actionType
    void Start()
    {
        LootPrefeb = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().lootPrefeb;
        durability = 100;
        transform.localScale = new Vector2(2f, 2f);
        tag = item.type.ToString();
    }
    
    void Update()
    {
        if(durability <=0)
        {
            SpawnItem();
        }
    }

    void SpawnItem()
    {
        GameObject loot = Instantiate(LootPrefeb, transform.position, Quaternion.identity);
        loot.GetComponent<Loot>().Initialize(item);

        Destroy(gameObject);
    }
}
