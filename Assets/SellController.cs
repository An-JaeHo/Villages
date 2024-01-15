using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SellController : MonoBehaviour
{
    private PlayerController player;

    public InventoryManger inventoryManger;
    public GameObject inventoryPrefeb;
    public GameObject sellWindow;
    public List<GameObject> sellList;
    
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    public void CheckInvenItems()
    {
        if(inventoryManger.items.Count != 0)
        {
            for (int i = 0; i < inventoryManger.items.Count; i++)
            {
                GameObject newItemGO = Instantiate(inventoryPrefeb, sellWindow.transform);
                newItemGO.AddComponent<InventoryItem>();
                sellList.Add(newItemGO);
            }
        }
    }
}
