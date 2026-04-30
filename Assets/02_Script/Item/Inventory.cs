using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
public class Inventory
{

    private int ActiveInventorySlot;
    public event Action<int> OnActiveInventorySlotchanged;
    public event Action<ItemData> OnItemAdded;
    private bool isFull;
    [SerializeField] private int maxInventorySlot = 4;
    public ItemInstance[] items;



    public Inventory()
    {
        items = new ItemInstance[maxInventorySlot];
    }
    public void AddItem(ItemInstance itemInstance)
    {
        if (checkisFull())
        {
            return;
        }

        for (int i = 0; i < items.Length; i++)
        {
            //먹은아이템이 현재인벤토리에같은아이템인경우 
            if (items[i] != null && items[i].ItemData == itemInstance.ItemData)
            {
                items[i].ItemCount++;
                SetActiveInventorySlot(i);
                OnItemAdded?.Invoke(itemInstance.ItemData);
                return;
            }

        }



        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] == null)
            {
                items[i] = itemInstance;

                SetActiveInventorySlot(i);
                OnItemAdded?.Invoke(itemInstance.ItemData);
                return;
            }
        }



        Debug.Log($"Added item: {itemInstance.ItemData.ItemName}");
    }


    private bool checkisFull()
    {
        return false;
        //Todo처리할것
    }

    public void SetActiveInventorySlot(int index)
    {
        ActiveInventorySlot = index;
        OnActiveInventorySlotchanged?.Invoke(ActiveInventorySlot);

    }



}
