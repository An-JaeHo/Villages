using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManger : MonoBehaviour
{
    public static InventoryManger Instance;

    public int maxStackedItems = 4;
    public InventorySlot[] inventorySlots;
    public GameObject inventoryItemPrefab;
    public int selectedSlot = -1;

    public List<GameObject> items;
    public List<GameObject> havingItemSlots;

    private void Awake()
    {
        Instance = this;
        havingItemSlots = new List<GameObject>();
    }

    private void Start()
    {
        ChangeSelectedSlot(0);
    }

    private void Update()
    {
        if(Input.inputString != null)
        {
            bool isNumber = int.TryParse(Input.inputString, out int number);
            if (isNumber && number > 0 && number < 9)
                ChangeSelectedSlot(number - 1);
        }
    }

    public void ChangeSelectedSlot(int newValue)
    {
        if(selectedSlot >= 0)
        {
            inventorySlots[selectedSlot].DeSelet();
        }

        inventorySlots[newValue].Selet();
        selectedSlot = newValue;
    }

    public bool AddItem(Item item)
    {
        // Check smae item
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemSlot = slot.GetComponentInChildren<InventoryItem>();    

            if (itemSlot != null &&
                itemSlot.count < maxStackedItems &&
                itemSlot.item.stackable == true &&
                itemSlot.item.uiImage.name == item.uiImage.name)
            {
                itemSlot.count++;
                itemSlot.RefreshConunt();
                return true;
            }
        }

        //Find any empty slot
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemSlot = slot.GetComponentInChildren<InventoryItem>();

            if(itemSlot == null)
            {
                SpawnNewItem(item, slot);
                return true;
            }
        }

        return false;
    }

    void SpawnNewItem(Item item, InventorySlot slot)
    {
        GameObject newItemGO = Instantiate(inventoryItemPrefab,slot.transform);
        InventoryItem inventoryItem = newItemGO.GetComponent<InventoryItem>();
        inventoryItem.InitialiseItem(item);
        items.Add(newItemGO);
    }

    public Item GetSelectedItem(bool use)
    {
        InventorySlot slot = inventorySlots[selectedSlot];
        InventoryItem itemSlot = slot.GetComponentInChildren<InventoryItem>();

        if(itemSlot != null)
        {
            Item item = itemSlot.item;
            if (use == true)
            {
                itemSlot.count--;
                if (itemSlot.count <= 0)
                {
                    Destroy(itemSlot.gameObject);
                }
                else
                {
                    itemSlot.RefreshConunt();
                }
            }

            return item;
        }

        return null;
    }
}
