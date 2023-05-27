using UnityEngine;

public class ItemObject : MonoBehaviour, IInteractable
{
    [SerializeField] private ItemData itemData;

    private SpriteRenderer _renderer;

    public static void SpawnItemObject(Vector3 position, ItemData itemData)
    {
        GameObject item = new GameObject(itemData.name,typeof(ItemObject),typeof(SpriteRenderer));
        item.transform.position = position;
        item.GetComponent<ItemObject>().SetItemData(itemData);
        Debug.Log("Spawned item" + itemData.name);
    }
    public string Prompt { get; set; }
    public bool InteractionEnabled { get; set; }

    private void Start()
    {
        InteractionEnabled = true;
        _renderer = GetComponent<SpriteRenderer>();
        _renderer.sortingLayerName = "Object";
    }

    public void SetItemData(ItemData itemData_)
    {
        if (itemData_ == null) return;
        itemData = itemData_;
        _renderer = GetComponent<SpriteRenderer>();
        Prompt = "Pick up: " + itemData.displayName;
        _renderer.sprite = itemData.sprite;
    }
/*
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (itemData == null) return;
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
*/
    public void ReceiveInteraction(GameObject interactor)
    {
        Inventory inventory = interactor.GetComponent<Inventory>();
        if (inventory != null)
        {
            inventory.AddItem(itemData);
            Destroy(gameObject);
            Debug.Log("Player picked up" + itemData.displayName);
        }
    }
}
