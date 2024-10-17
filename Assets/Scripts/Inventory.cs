using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum ItemId
{
    
}

public class Item : ICloneable
{
    public ItemId Id;
    
    public int Stack;

    public string Name; // $name

    private Item(ItemId id, int stack)
    {
        Id = id;
        Stack = stack;
    }
    
    public object Clone()
    {
        return new Item(Id, Stack);
    }
}

public class ItemSlot
{
    public Item Item;
    public int MaxStack;
}

public class Inventory : MonoBehaviour
{
    [SerializeField] private List<ItemSlot> _itemSlots;
    
    private void Awake()
    {
        _itemSlots = new();
    }
    
    public void CopyInventory(IEnumerable<Item> items)
    {
        _itemSlots = items.Select(x => new ItemSlot { Item = (Item) x.Clone() }).ToList();
    }

    public void AddItem(Item item)
    {
        var itemCopy = (Item)item.Clone();
        var similarSlots = _itemSlots.Where(x => x.Item.Id == item.Id);
        if (!similarSlots.Any())
        {
            _itemSlots.Add(new ItemSlot { Item = itemCopy });
        }
        else
        {
            foreach (var slot in similarSlots)
            {
                var stack = slot.MaxStack - slot.Item.Stack;
                if (stack >= itemCopy.Stack)
                {
                    slot.Item.Stack += itemCopy.Stack;
                    break;
                }
                
                itemCopy.Stack -= stack;
                slot.Item.Stack += stack;
            }
        }
    }

    public void RemoveItem(Item item)
    {
        var itemCopy = (Item)item.Clone();
        var similarSlots = _itemSlots.Where(x => x.Item.Id == item.Id);
        var slotsCount = _itemSlots.Count();

        foreach (var slot in similarSlots)
        {
            if (itemCopy.Stack <= slot.Item.Stack)
            {
                slot.Item.Stack -= itemCopy.Stack;
                break;
            }

            itemCopy.Stack -= slot.Item.Stack;
            slot.Item.Stack = 0;
        }

        var removeOffset = 0;
        for (var i = 0; i < slotsCount - removeOffset; i++)
        {
            var slotItem = _itemSlots[i].Item;
            if (slotItem.Id == item.Id && slotItem.Stack == 0)
            {
                _itemSlots.RemoveAt(i + removeOffset++);
            }
        }
    }

    public bool HasItem(Item item)
    {
        var similarSlots = _itemSlots.Where(x => x.Item.Id == item.Id);
        if (similarSlots.Any())
        {
            if (item.Stack == 0) return true;

            var itemStackSum = similarSlots.Sum(x => x.Item.Stack);

            if (item.Stack <= itemStackSum) return true;
        }

        return false;
    }
}