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
                //미리 만들어진 칸때문에 새로 채워진 곳에 안만들어짐
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
                            Debug.Log("MakeItem");
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
                //여긴 왜또 문제일까
                if (inventoryManger.items[j].GetComponent<InventoryItem>().item
                    == sellList[i].GetComponent<InventoryItem>().item
                    && inventoryManger.items[j].GetComponent<InventoryItem>().count
                    == sellList[i].GetComponent<InventoryItem>().count)
                {
                    Destroy(sellList[i]);
                    Destroy(inventoryManger.items[j]);

                    sellList.Remove(sellList[i]);
                    inventoryManger.items.Remove(inventoryManger.items[j]);
                }
            }
        }

        CheckSellUiItem();
    }
}
