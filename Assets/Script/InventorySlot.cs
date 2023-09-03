using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using static UnityEditor.Progress;
using Unity.VisualScripting;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    public Image image;
    public Color seletedColor, notSeletedColor;
    public GameObject player;

    private void Awake()
    {
        DeSelet();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public void Selet()
    {
        image.color = seletedColor;
        player.GetComponent<PlayerController>().sletMap.SetTile(player.GetComponent<PlayerController>().testPos, null);

        if (transform.childCount != 0)
        {
            if(transform.GetChild(0).GetComponent<InventoryItem>().item.type == ItemType.Tool)
            {
                player.GetComponent<PlayerController>().usingTool = true;
            }
            else
            {
                player.GetComponent<PlayerController>().usingTool = false;
            }

            player.GetComponent<PlayerController>().item = transform.GetChild(0).GetComponent<InventoryItem>().item;
        }
        else
        {
            player.GetComponent<PlayerController>().usingTool = false;
            player.GetComponent<PlayerController>().item = null;
        }
    }

    public void DeSelet()
    {
        image.color = notSeletedColor;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if(transform.childCount == 0) {
            InventoryItem inventoryItem = eventData.pointerDrag.GetComponent<InventoryItem>();
            inventoryItem.parentAfterDrag = transform;
        }
    }
}
