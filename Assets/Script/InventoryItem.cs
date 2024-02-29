using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour,IBeginDragHandler,IDragHandler,IEndDragHandler
{
    private SellController sellController;

    [Header("UI")]
    public Image image;
    public Text countText;

    public Item item;
    public Transform invenUiTransform;
    [HideInInspector] public int count = 1;
    [HideInInspector] public Transform parentAfterDrag;

    private void Awake()
    {
        sellController = GameObject.FindObjectOfType(typeof(SellController), true).GetComponent<SellController>();
    }

    private void Start()
    {
        image = GetComponent<Image>();
    }

    public void InitialiseItem(Item newItem)
    {
        item = newItem;
        image.sprite = newItem.uiImage;
        invenUiTransform = transform.parent;
        RefreshConunt();
    }

    public void RefreshConunt()
    {
        countText.text = count.ToString();
        bool textActive = count > 1;
        countText.gameObject.SetActive(textActive);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        image.raycastTarget = false;
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        image.raycastTarget = true;
        transform.SetParent(parentAfterDrag);

        if (transform.parent.parent.parent.parent.name == "SellItem")
        {
            for (int i = 0; i < sellController.sellList.Count; i++)
            {
                if (gameObject == sellController.sellList[i])
                {
                    return;
                }
            }

            sellController.sellList.Add(gameObject);
            sellController.sumTime += 10;
        }
        else
        {
            for (int i = 0; i < sellController.sellList.Count; i++)
            {
                if (gameObject == sellController.sellList[i])
                {
                    sellController.sellList.Remove(gameObject);
                }
            }

            sellController.sumTime -= 10;
        }

        if (transform.parent.parent.parent.parent.name == "InvenItem")
        {
            for (int i = 0; i < sellController.invenList.Count; i++)
            {
                if (gameObject == sellController.invenList[i])
                {
                    return;
                }
            }

            sellController.invenList.Add(gameObject);
        }
        else
        {
            for (int i = 0; i < sellController.invenList.Count; i++)
            {
                if (gameObject == sellController.invenList[i])
                {
                    sellController.invenList.Remove(gameObject);
                }
            }
        }

        sellController.timeText.text = sellController.sumTime.ToString(); 
    }
}
