using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    [SerializeField] public ItemData itemData;


    private void Awake()
    {

    }

    public void EatedItem()
    {

        Destroy(gameObject);
    }
}

