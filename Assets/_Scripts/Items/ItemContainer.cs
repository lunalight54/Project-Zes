using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ItemSlot
{
    public Item item;
    public int count;

    public void Copy(ItemSlot slot)
    {
        item = slot.item;
        count = slot.count;
    }
    public void Set(Item item, int count)
    {
        this.item = item;
        this.count = count;
    }

    public void Clear()
    {
        item = null;
        count = 0;
    }
}
[CreateAssetMenu(menuName = "Data/Item Container")]
public class ItemContainer : ScriptableObject
{
    public List<ItemSlot> slots;

    public void Add(Item item, int count)
    {
        
        if (item.stackable == true)
        {
            ItemSlot availableItemSlot = slots.Find(x => x.item == item);
            if (availableItemSlot != null)
            {
                availableItemSlot.count += count;
            }
            else
            {
                availableItemSlot = slots.Find(x => x.item == null);
                if (availableItemSlot != null)
                {
                    availableItemSlot.item = item;
                    availableItemSlot.count = count;
                }
            }


        }
        else
        {
            ItemSlot availableItemSlot = slots.Find(x => x.item == null);
            if (availableItemSlot != null)
            {
                availableItemSlot.item = item;
            }
        }
    }
    public void Remove(Item itemToRemove, int count = 1)
    {
        if (itemToRemove.stackable)
        {
            ItemSlot itemSlot = slots.Find(x => x.item == itemToRemove);
            if (itemSlot == null) { return;  }

            itemSlot.count -= count;
            if (itemSlot.count <= 0)
            {
                itemSlot.Clear();
            }
        }
        else
        {
            while (count > 0)
            {
                count -= 1;
                ItemSlot itemSlot = slots.Find(x => x.item == itemToRemove);
                if (itemSlot == null) { return; }

                itemSlot.Clear();
            }
        }
    }
    internal bool CheckItem(ItemSlot checkingItem)
    {
        ItemSlot itemSlot = slots.Find(x => x.item == checkingItem.item);
        if (itemSlot == null) { return false;  }
        if (checkingItem.item.stackable) { return itemSlot.count >= checkingItem.count; }
        return true;
    }

    internal bool FreeSpace()
    {
        for (int i = 0; i< slots.Count; i++)
        {
            if (slots[i].item == null)
            {
                return true;
            }
        }
        return false;
    }
}
