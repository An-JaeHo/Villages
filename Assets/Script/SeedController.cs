using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SeedController : MonoBehaviour
{
    [Header("Item Information")]
    public string itemName;
    public Tilemap fruitTile;
    [SerializeField] private Vector3Int myPosition;
    [SerializeField] private Sprite[] sprites;

    [Header("Plnat Information")]
    [SerializeField] private int waterPoint;
    [SerializeField] private int glow;

    [SerializeField] private WorldTime worldTime;
    [SerializeField] private int day;
    
    private Color paleBrown = new Color32(255 , 255 , 255, 255);
    private Color brown = new Color32(255 , 192 , 192, 255);
    private Color darkBrown = new Color32(172, 90, 90, 255);



    void Start()
    {
        waterPoint = 100;
        worldTime = GameObject.FindGameObjectWithTag("GameController").GetComponent<WorldTime>();
        LoadImage();
        day = worldTime.currentTime.Days;
        glow = 0;
        myPosition = Vector3Int.FloorToInt(transform.position);
        fruitTile.SetTileFlags(myPosition, TileFlags.None);
        
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

            if(waterPoint > 60)
            {
                fruitTile.SetColor(myPosition, darkBrown);
            }
            else if (waterPoint > 30 && waterPoint <= 60)
            {
                fruitTile.SetColor(myPosition, brown);
            }
            else if (waterPoint <= 30)
            {
                fruitTile.SetColor(myPosition, paleBrown);
            }
        }
    }

    private void LoadImage()
    {
        object[] loadedItem = Resources.LoadAll(itemName, typeof(Sprite));

        sprites = new Sprite[loadedItem.Length];

        for (int i = 0; i < loadedItem.Length; i++)
        {
            sprites[i] = (Sprite)loadedItem[i];
        }
    }
}
