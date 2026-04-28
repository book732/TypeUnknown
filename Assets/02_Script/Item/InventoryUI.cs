using NUnit.Framework;
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
}




public class InventoryUI : MonoBehaviour
{
    [SerializeField] private List<Slot> inventorySlots;
    [SerializeField] private Player player;
    private Slot currentSlot;
    private int currentindex;
    [SerializeField] private InputEventSO inputEventSO;
    private bool isfull;
    private Vector3 BasicSlotSize;
    private GameObject HoldingItem;

    private void Awake()
    {
        player.OnInventoryChanged += UpdateInventoryUI;
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

        if (input.y > 0)
        {
            currentindex = (currentindex + 1) % 4;
            currentSlot = inventorySlots[currentindex];
            ResizeSlot();
        }
        else if (input.y < 0)
        {
            currentindex = ((currentindex - 1) + 4) % 4;
            currentSlot = inventorySlots[currentindex];
            ResizeSlot();
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

                ResizeSlot();

                break;
            }
            else if (slot.itemIcon.sprite == data.ItemIcon)
            {
                float value = float.Parse(slot.itemCount.text);
                value++;
                slot.itemCount.text = value.ToString();

                //아이템먹었을때그아이템 ui활성화 
                currentSlot = slot;
                currentindex = i;
                ResizeSlot();
                break;
            }




        }




    }
    public void ActiveSlotChanged(ItemData data)
    {

    }
    public void ResizeSlot()
    {
        foreach (var slot in inventorySlots)
        {
            slot.itemIcon.transform.localScale = BasicSlotSize;
            slot.isExpanded = false;
        }


        if (currentSlot != null && currentSlot.isExpanded != true)
        {
            currentSlot.itemIcon.transform.localScale = currentSlot.itemIcon.transform.localScale * 1.5f;
            currentSlot.isExpanded = true;
            return;
        }



    }

    public void RestoreSlot()
    {
        currentSlot.itemIcon.transform.localScale = BasicSlotSize;
    }


}
