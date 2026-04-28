using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
public class Inventory
{
    private List<ItemInstance> items = new List<ItemInstance>();

    public void AddItem(ItemInstance itemInstance)
    {
        items.Add(itemInstance);
        Debug.Log($"Added item: {itemInstance.ItemData.ItemName}");
    }
}
