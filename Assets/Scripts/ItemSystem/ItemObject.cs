using System;
using UnityEngine;
using UnityEngine.Serialization;

public class ItemObject : MonoBehaviour
{
    [SerializeField] private ItemData itemData;

    private SpriteRenderer _renderer;

    private void Start()
    {
        _renderer = GetComponent<SpriteRenderer>();
        SetItemData(itemData);
    }

    public void SetItemData(ItemData itemData)
    {
        if (itemData == null) return;
        _renderer.sprite = this.itemData.sprite;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("Col: " + col.gameObject.name);
        Inventory inventory = col.gameObject.GetComponent<Inventory>();
        if (inventory != null)
        {
            inventory.AddItem(itemData);
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        Debug.Log("Col: " + col.gameObject.name);
        Inventory inventory = col.gameObject.GetComponent<Inventory>();
        if (inventory != null)
        {
            inventory.AddItem(itemData);
            Destroy(gameObject);
        }
    }
}
