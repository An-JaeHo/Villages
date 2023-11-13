using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loot : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private BoxCollider2D collider;
    [SerializeField] private float moveSpeed;

    private Item item;

    public void Initialize(Item item)
    {
        this.item = item;
        sprite.sprite = item.uiImage;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            InventoryManger.Instance.AddItem(item);

            StartCoroutine(MoveAndCollect(other.transform));
        }
    }

    private IEnumerator MoveAndCollect(Transform target)
    {
        Destroy(collider);

        while (transform.position != target.position) 
        {
            transform.position = Vector3.MoveTowards(transform.position,target.position, moveSpeed*Time.deltaTime);
            yield return 0;
        }

        Destroy(gameObject);
    }
}
