using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoScript : MonoBehaviour
{
    public InventoryManger inventoryManger;
    public Item[] itemToPickup;

    public void PickItem(int id)
    {
        bool result = inventoryManger.AddItem(itemToPickup[id]);
        inventoryManger.ChangeSelectedSlot(inventoryManger.selectedSlot);
    }

    public void GetSelectedItem()
    {
        Item receviedItem = inventoryManger.GetSelectedItem(false);
        if (receviedItem != null)
            Debug.Log("receviedItem : " + receviedItem);
        else
            Debug.Log("No recevied!");
    }

    public void UseSelectedItem()
    {
        Item receviedItem = inventoryManger.GetSelectedItem(true);
        if (receviedItem != null)
            Debug.Log("receviedItem : " + receviedItem);
        else
            Debug.Log("No recevied!");
    }
}
