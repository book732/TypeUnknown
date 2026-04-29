using NUnit.Framework;
using NUnit.Framework.Interfaces;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.Rendering;
using UnityEngine.UI;


[System.Serializable]
public class Slot
{
    public Image itemIcon;
    public Text itemCount;
    public bool isExpanded = false;
    public ItemData ItemData;
}

//TODO



public class InventoryUI : MonoBehaviour
{
    [SerializeField] private List<Slot> inventorySlots;
    [SerializeField] private Player player;
    private Slot currentSlot;
    private int currentindex;
    [SerializeField] private InputEventSO inputEventSO;
    private bool isfull;
    private Vector3 BasicSlotSize;
    private Inventory inventory;

    public void InitInventory(Inventory playerinventory)
    {
        this.inventory = playerinventory;
        inventory.OnItemAdded += UpdateInventoryUI;
        inventory.OnActiveInventorySlotchanged += ResizeSlot;
    }


    private void Awake()
    {

        InitItemSlots();
        currentSlot = null;
        BasicSlotSize = inventorySlots[0].itemIcon.transform.localScale;



    }


    private void OnEnable()
    {
        inputEventSO.SubscribeChangeItem(SlotChanged);
    }
    private void OnDisable()
    {
        inputEventSO.UnSubscribeChangeItem(SlotChanged);
    }
    private void SlotChanged(Vector2 input)
    {
        int count = inventory.items.Length;
        if (input.y > 0)
        {

            currentindex = (currentindex + 1) % count;
            currentSlot = inventorySlots[currentindex];
            // Debug.Log(currentindex);
            inventory.SetActiveInventorySlot(currentindex);




        }
        else if (input.y < 0)
        {
            currentindex = ((currentindex - 1) + 4) % count;
            currentSlot = inventorySlots[currentindex];

            //s Debug.Log(currentindex);
            inventory.SetActiveInventorySlot(currentindex);
        }

    }



    private void InitItemSlots()
    {
        foreach (var slot in inventorySlots)
        {
            slot.itemIcon.sprite = null;
            slot.itemCount.text = "0";
        }


    }

    private void UpdateInventoryUI(ItemData data)
    {
        for (int i = 0; i < inventorySlots.Count; i++)
        {
            var slot = inventorySlots[i];
            if (slot.itemIcon.sprite == null)
            {
                slot.itemIcon.sprite = data.ItemIcon;
                slot.itemCount.text = "1";

                //아이템먹었을때그아이템 ui활성화 
                currentSlot = slot;
                currentindex = i;



                break;
            }
            else if (slot.itemIcon.sprite == data.ItemIcon)
            {
                var value = inventory.items[i].ItemCount.ToString();
                slot.itemCount.text = value;

                //아이템먹었을때그아이템 ui활성화 
                currentSlot = slot;
                currentindex = i;


                break;
            }




        }




    }


    public void ResizeSlot(int index)
    {


        currentindex = index;
        currentSlot = inventorySlots[index];

        foreach (var slot in inventorySlots)
        {
            slot.itemIcon.transform.localScale = BasicSlotSize;
            slot.isExpanded = false;
        }

        currentSlot.itemIcon.transform.localScale *= 1.5f;
        currentSlot.isExpanded = true;


    }

    public void RestoreSlot()
    {
        currentSlot.itemIcon.transform.localScale = BasicSlotSize;
    }


}
