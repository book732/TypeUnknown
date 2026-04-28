using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    [SerializeField] public ItemData itemData;

    public void EatedItem()
    {
        Destroy(gameObject);
    }
}

