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





public class InventoryUI : MonoBehaviour
{
    [SerializeField] private List<Slot> inventorySlots;
    [SerializeField] private Player player;
    private Slot currentSlot;
    [SerializeField] private InputEventSO inputEventSO;
    private bool isfull;
    private Vector3 BasicSlotSize;
    private Inventory inventory;

    public void InitInventory(Inventory playerinventory)
    {
        this.inventory = playerinventory;
        inventory.OnslotChanged += UpdateInventoryUI;
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
            int candidateIndex = inventory.ActiveIndex;

            candidateIndex = (candidateIndex + 1) % count;
            currentSlot = inventorySlots[candidateIndex];
            // Debug.Log(currentindex);
            inventory.SetActiveInventorySlot(candidateIndex);




        }
        else if (input.y < 0)
        {
            int candidateIndex = inventory.ActiveIndex;

            candidateIndex = ((candidateIndex - 1) + count) % count;
            currentSlot = inventorySlots[candidateIndex];

            //s Debug.Log(currentindex);
            inventory.SetActiveInventorySlot(candidateIndex);
        }

    }



    private void InitItemSlots()
    {
        foreach (var slot in inventorySlots)
        {
            Color itemColor = slot.itemIcon.color;
            Color textColor = slot.itemCount.color;
            slot.itemIcon.sprite = null;
            slot.itemCount.text = "0";
            itemColor.a = 0f;
            textColor.a = 0f;
            slot.itemIcon.color = itemColor;
            slot.itemCount.color = textColor;
        }


    }

    //해결완료
    //이부분문제  UI가지금  데이터를바꾸고잇음이게아니라 Inventory꺼를바꾸고그걸반영해야됨 
    //게다가지금 slot.itemIcon.sprite == data.ItemIcon 이것도이상함 식별은 INVENTORY 에서해야됨
    //게다가 CURRENT SLOT이랑 CURRENT INDEX 가지는것도이상함 

    private void UpdateInventoryUI(int index, ItemInstance itemInstance)
    {
        var slot = inventorySlots[index];
        Color itemColor = slot.itemIcon.color;
        Color textColor = slot.itemCount.color;

        if (itemInstance == null)
        {
            itemColor.a = 0f;
            textColor.a = 0f;

            slot.itemIcon.color = itemColor;
            slot.itemCount.color = textColor;
            slot.itemIcon.sprite = null;
            slot.itemCount.text = "0";
            return;
        }

        slot.itemIcon.sprite = itemInstance.ItemData.ItemIcon;
        slot.itemCount.text = itemInstance.ItemCount.ToString();
        itemColor.a = 1f;
        textColor.a = 1f;
        slot.itemIcon.color = itemColor;
        slot.itemCount.color = textColor;
        currentSlot = inventorySlots[index];
    }

    public void ResizeSlot(int index)
    {




        foreach (var slot in inventorySlots)
        {
            slot.itemIcon.transform.localScale = BasicSlotSize;
            slot.isExpanded = false;
        }

        var currentSlot = inventorySlots[index];
        currentSlot.itemIcon.transform.localScale *= 1.5f;
        currentSlot.isExpanded = true;


    }

    public void RestoreSlot()
    {
        currentSlot.itemIcon.transform.localScale = BasicSlotSize;
    }


}
