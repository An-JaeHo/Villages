using System;
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
    public int sumTime;
    private int itemNumber;

    public InventoryManger inventoryManger;
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

    //생각해야되는것

    public void CheckInvenItems()
    {
        if (inventoryManger.items.Count != 0
            && invenList.Count < inventoryManger.items.Count)
        {
            int num = inventoryManger.items.Count - invenList.Count;

            if (invenList.Count ==0)
            {
                for (int i = 0; i < num; i++)
                {
                    itemNumber = i;

                    if (InvenWindow.transform.GetChild(i).childCount == 0)
                    {
                        GameObject newItemGO = Instantiate(inventoryManger.items[itemNumber], InvenWindow.transform.GetChild(itemNumber));
                        newItemGO.GetComponent<InventoryItem>().InitialiseItem(newItemGO.GetComponent<InventoryItem>().item);
                        invenList.Add(newItemGO);
                    }
                }
            }
            else
            {
                int chek =0;
                int childNum = 0;

                for (int i = 0; i < num; i++)
                {
                    while (chek < 50)
                    {
                        if (InvenWindow.transform.GetChild(childNum).childCount == 0)
                        {
                            GameObject newItemGO = Instantiate(inventoryManger.items[itemNumber], InvenWindow.transform.GetChild(childNum));
                            newItemGO.GetComponent<InventoryItem>().InitialiseItem(newItemGO.GetComponent<InventoryItem>().item);
                            invenList.Add(newItemGO);
                            itemNumber++;
                            break;
                        }
                        else
                        {
                            childNum++;
                        }

                        chek++;
                    }
                }
            }
        }


        timeText.text = sumTime.ToString();
    }

    public void ReturnPosition()
    {
        if(sellList.Count !=0)
        {
            for (int i = 0; i < sellList.Count; i++)
            {
                sellList[i].transform.SetParent(sellList[i].GetComponent<InventoryItem>().invenUiTransform);
            }

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

        player.lifeTime = new System.TimeSpan(player.lifeTime.Hours + sumTime, player.lifeTime.Minutes, 0);

        if (player.lifeTime.Hours > 0.24)
        {
            player.lifeTime = new System.TimeSpan(24, 0, 0);
        }

        player.lifeTimeUi.SetText(player.lifeTime.ToString(@"hh\:mm"));
        sumTime = 0;
        timeText.text = sumTime.ToString();
    }
}
