using UnityEngine;

public class ItemInstance
{
    public ItemData ItemData { get; private set; }
    public int ItemCount;


    public ItemInstance(ItemData data)
    {
        ItemData = data;
        ItemCount = 1;
    }
}
