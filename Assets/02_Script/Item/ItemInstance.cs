using UnityEngine;

public class ItemInstance
{
    public ItemData ItemData { get; private set; }
    public int ItemCount { get; private set; }


    public ItemInstance(ItemData data)
    {
        ItemData = data;

    }
}
