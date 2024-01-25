using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class SellController : MonoBehaviour
{
    private PlayerController player;
    public List<GameObject> sellList;
    private int sumTime;

    public InventoryManger inventoryManger;
    public GameObject inventorySlotPrefeb;
    public GameObject InvenWindow;
    public GameObject sellWindow;
    public TMP_Text timeText;

    private void Awake()
    {
        sumTime = 0;
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
            if (sellWindow.transform.GetChild(i).childCount != 0)
            {
                sumTime += sellWindow.transform.GetChild(i).GetChild(0).GetComponent<InventoryItem>().count * 10;
            }
        }

        timeText.text = sumTime.ToString();
        sumTime = 0;
    }   

    public void ReturnPosition()
    {
        if(sellList.Count !=0)
        {
            for (int i = 0; i < sellList.Count; i++)
            {
                sellList[i].transform.SetParent(sellList[i].GetComponent<InventoryItem>().invenUiTransform);
            }
            sumTime = 0;
            timeText.text = sumTime.ToString();
        }
    }

    public void SellButton()
    {
        for (int i = 0; i < sellList.Count; i++)
        {
            for (int j = 0; j < inventoryManger.items.Count; j++)
            {
                if (inventoryManger.items[j].GetComponent<InventoryItem>().item
                    == sellList[i].GetComponent<InventoryItem>().item)
                {
                    //Debug.Log(sellList[i].GetComponent<InventoryItem>().item);
                    Destroy(sellList[i]);
                    Destroy(inventoryManger.items[j]);

                    break;
                }
            }
        }

        //Debug.Log(inventoryManger.items.Count);
        //Debug.Log(sellList.Count);
    }
}
