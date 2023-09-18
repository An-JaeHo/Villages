using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using static UnityEditor.Progress;

public class SeedController : MonoBehaviour
{
    [Header("Item Information")]
    public string itemName;
    public Tilemap farmTile;
    public Item myItemInfo;
    [SerializeField] private Vector3Int myPosition;
    [SerializeField] private Sprite[] sprites;
    [SerializeField] private WorldTime worldTime;
    [SerializeField] private int day;

    [Header("Plnat Information")]
    public int waterPoint;
    public int glow;
    public GameObject fruitPrefeb;

    
    private Color paleBrown = new Color32(255 , 255 , 255, 255);
    private Color brown = new Color32(255 , 192 , 192, 255);
    private Color darkBrown = new Color32(172, 90, 90, 255);



    void Start()
    {
        waterPoint = 100;
        worldTime = GameObject.FindGameObjectWithTag("GameController").GetComponent<WorldTime>();
        LoadImage();
        day = worldTime.currentTime.Days-1;
        glow = 1;
        myPosition = Vector3Int.FloorToInt(transform.position);
        farmTile.SetTileFlags(myPosition, TileFlags.None);
    }

    void Update()
    {
        if (day < worldTime.currentTime.Days)
        {
            waterPoint -= 20;
            day = worldTime.currentTime.Days;
            glow++;

            if(glow >5)
            {
                glow = 5;
            }

            GetComponent<SpriteRenderer>().sprite = sprites[glow];

            if (waterPoint > 60)
            {
                farmTile.SetColor(myPosition, darkBrown);
            }
            else if (waterPoint > 30 && waterPoint <= 60)
            {
                farmTile.SetColor(myPosition, brown);
            }
            else if (waterPoint <= 30)
            {
                farmTile.SetColor(myPosition, paleBrown);
            }
        }
    }

    void LoadImage()
    {
        object[] loadedItem = Resources.LoadAll(itemName, typeof(Sprite));

        sprites = new Sprite[loadedItem.Length];

        for (int i = 0; i < loadedItem.Length; i++)
        {
            sprites[i] = (Sprite)loadedItem[i];
        }
    }

    public void SpawnFruit()
    {
        GameObject loot = Instantiate(fruitPrefeb, transform.position, Quaternion.identity);
        loot.GetComponent<Loot>().Initialize(myItemInfo);


        Destroy(gameObject);
    }
}
