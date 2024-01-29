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
    public List<GameObject> invenList;
    private int sumTime;
    private int itemNumber;

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
        invenList = new List<GameObject>();
    }

    public void CheckInvenItems()
    {
        if (inventoryManger.items.Count != 0)
        {
            if (invenList.Count < inventoryManger.items.Count)
            {
                Debug.Log("New Slot");
                int num = inventoryManger.items.Count - invenList.Count;

                for (int i = 0; i < num; i++)
                {
                    Instantiate(inventorySlotPrefeb, InvenWindow.transform);
                    Instantiate(inventorySlotPrefeb, sellWindow.transform);

                    itemNumber = i;

                    while (true)
                    {
                        if (InvenWindow.transform.GetChild(itemNumber).childCount == 0)
                        {
                            GameObject newItemGO = Instantiate(inventoryManger.items[itemNumber], InvenWindow.transform.GetChild(itemNumber));
                            newItemGO.GetComponent<InventoryItem>().InitialiseItem(newItemGO.GetComponent<InventoryItem>().item);
                            invenList.Add(newItemGO);
                            break;
                        }
                        else
                        {
                            itemNumber++;
                        }
                    }
                }


            }
            else
            {
                //int num = InvenWindow.transform.childCount - inventoryManger.items.Count;
                //
                //for (int i = 0; i < num; i++)
                //{
                //    if(InvenWindow.transform.GetChild(i).childCount == 0)
                //    {
                //        GameObject newItemGO = Instantiate(inventoryManger.items[itemNumber], InvenWindow.transform.GetChild(i));
                //        newItemGO.GetComponent<InventoryItem>().InitialiseItem(newItemGO.GetComponent<InventoryItem>().item);
                //        itemNumber++;
                //    }
                //}
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
        for (int j = 0; j < inventoryManger.items.Count; j++)
        {
            for (int i = 0; i < sellList.Count; i++)
            {
                if (inventoryManger.items[j].GetComponent<InventoryItem>().item
                    == sellList[i].GetComponent<InventoryItem>().item
                    && inventoryManger.items[j].GetComponent<InventoryItem>().count
                    == sellList[i].GetComponent<InventoryItem>().count)
                {
                    Destroy(sellList[i]);
                    Destroy(inventoryManger.items[j]);

                    inventoryManger.items.Remove(inventoryManger.items[j]);
                    sellList.Remove(sellList[i]);
                    itemNumber = inventoryManger.items.Count;
                }
            }
        }

        CheckSellUiItem();
        timeText.text = sumTime.ToString();
    }
}
