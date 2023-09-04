using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SeedController : MonoBehaviour
{
    [Header("Item Information")]
    public string itemName;
    [SerializeField] private Sprite[] sprites;

    [Header("Plnat Information")]
    [SerializeField] private int waterPoint;
    [SerializeField] private int glow;

    [SerializeField] private WorldTime worldTime;
    [SerializeField] private int day;
    


    void Start()
    {
        waterPoint = 100;
        worldTime = GameObject.FindGameObjectWithTag("GameController").GetComponent<WorldTime>();
        LoadImage();
        day = worldTime.currentTime.Days;
        glow = 0;
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
