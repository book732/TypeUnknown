using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
public class Inventory
{


    private int activeIndex;
    public int ActiveIndex => activeIndex;
    public event Action<int> OnActiveInventorySlotchanged;
    public event Action<int, ItemInstance> OnslotChanged;
    private bool isFull;
    public bool IsFull => isFull;
    [SerializeField] private int maxInventorySlot = 5;
    public ItemInstance[] items;



    public Inventory()
    {
        items = new ItemInstance[maxInventorySlot];
    }

    //TODO 지금 아이템먹을시에만 반영되잇음 이벤트구독으로인해 드랍이라던지 사용도 반영해야됨 

    public void UseActiveItem(GameObject user)
    {
        //인벤토리의 active index 에해당하는 아이템이 null 이아니고 아이템액션이잇을경우 

        if (items[activeIndex] != null && items[activeIndex].ItemData.ItemAction != null)
        {
            items[activeIndex].ItemData.ItemAction.Execute(user);

            if (items[activeIndex].ItemData.IsConsumable == true)
            {
                //아이템 없어지고
                //그걸ui에반영하기 
                items[activeIndex].ItemCount--;

                if (items[activeIndex].ItemCount <= 0)
                {
                    // 슬롯 비우기
                    items[activeIndex] = null;
                    OnslotChanged?.Invoke(activeIndex, null);
                }
                else
                {
                    OnslotChanged?.Invoke(activeIndex, items[activeIndex]);
                }


            }
        }


        else
        {



        }
    }
    public bool HasItem(string itemName)
    {
        foreach (var item in items)
        {
            if (item != null && item.ItemData.ItemName == itemName)
                return true;
        }
        return false;
    }

    public void AddItem(ItemInstance itemInstance)
    {


        for (int i = 0; i < items.Length; i++)
        {
            //먹은아이템이 현재인벤토리에같은아이템인경우 
            if (items[i] != null && items[i].ItemData == itemInstance.ItemData)
            {
                items[i].ItemCount++;
                SetActiveInventorySlot(i);
                OnslotChanged?.Invoke(i, items[i]);
                return;
            }

        }
        if (checkisFull())
        {
            return;
        }


        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] == null)
            {
                items[i] = itemInstance;

                SetActiveInventorySlot(i);
                OnslotChanged?.Invoke(i, itemInstance);
                return;
            }
        }



        Debug.Log($"Added item: {itemInstance.ItemData.ItemName}");
    }


    private bool checkisFull()
    {


        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] == null)
            {
                return false;
            }


        }
        return true;


    }

    public void SetActiveInventorySlot(int index)
    {
        activeIndex = index;
        OnActiveInventorySlotchanged?.Invoke(activeIndex);

    }



}
