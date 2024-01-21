using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SellController : MonoBehaviour
{
    private PlayerController player;
    private List<GameObject> sellList;

    public InventoryManger inventoryManger;
    public GameObject inventorySlotPrefeb;
    public GameObject InvenWindow;
    public GameObject sellWindow;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        sellList = new List<GameObject>();
    }

    public void CheckInvenItems()
    {
        if (inventoryManger.items.Count != 0)
        {
            if(InvenWindow.transform.childCount <= inventoryManger.items.Count)
            {
                int num = inventoryManger.items.Count - InvenWindow.transform.childCount;

                for (int i = 0; i < num; i++)
                {
                    Instantiate(inventorySlotPrefeb, InvenWindow.transform);
                    Instantiate(inventorySlotPrefeb, sellWindow.transform);

                    int number = i;

                    while (true)
                    {
                        if(InvenWindow.transform.GetChild(number).childCount == 0)
                        {
                            GameObject newItemGO = Instantiate(inventoryManger.items[number], InvenWindow.transform.GetChild(number));
                            newItemGO.GetComponent<InventoryItem>().InitialiseItem(newItemGO.GetComponent<InventoryItem>().item);
                            sellList.Add(newItemGO);
                            break;
                        }
                        else
                        {
                            number++;
                        }
                    }
                }
            }
        }
    }

    public void CheckSellUiItem()
    {
        for (int i = 0; i < sellWindow.transform.childCount; i++)
        {
            if(sellWindow.transform.GetChild(i).childCount !=0)
            {
                Debug.Log(sellWindow.transform.GetChild(i).GetChild(0).GetComponent<InventoryItem>().count * 10);
            }
        }
    }
}
