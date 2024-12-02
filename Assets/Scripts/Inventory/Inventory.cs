using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Inventory
{
    public class ItemSlot
    {
        public Item Item;
        public int Stack;

        [SerializeField] private TMP_Text _itemName;
        [SerializeField] private TMP_Text _itemCount;
        [SerializeField] private Image _icon;
        
        public void Clear()
        {
            Item = null;
            //_icon = GameGlobals.NoneItemImage;
            _icon.enabled = false;
            _itemName.enabled = false;
            _itemCount.enabled = false;
        }
    }
    
    public class Inventory
    {
        private static readonly string NoEnoughSlots = "No enough slots!";
        
        private List<ItemSlot> _itemSlots;
        public int MaxSlots;

        public Inventory(int maxSlots, IEnumerable<Item> items)
        {
            MaxSlots = maxSlots;

            if (items != null)
                _itemSlots = items.Select(x => new ItemSlot { Item = x.Clone() }).ToList();
            else
                _itemSlots = new();
        }

        public void AddItem(Item item, int count)
        {
            var similarSlots = _itemSlots.Where(x => x.Item.Id == item.Id && x.Item.IsDroppable == item.IsDroppable);
            var itemSlots = similarSlots.ToList();
            var anySlots = itemSlots.Any();
            var possibleSlots = MaxSlots - _itemSlots.Count;

            if (anySlots)
            {
                foreach (var slot in itemSlots)
                {
                    var stack = slot.Item.MaxStack - slot.Stack;
                    if (stack > 0)
                    {
                        if (count > stack)
                        {
                            slot.Stack += stack;
                            count -= stack;
                        }
                        else
                        {
                            slot.Stack += count;
                            break;
                        }
                    }
                }
            }

            if (count != 0 && possibleSlots != 0)
            {
                var slots = count / item.MaxStack;
                var extraCount = count - count * slots;

                var isOverflowed = possibleSlots - slots - (extraCount != 0 ? 1 : 0) < 0;
                if (isOverflowed)
                {
                    PlayerLog.Add(NoEnoughSlots, new Color32(255, 85, 0, 255));
                    for (var i = 0; i < possibleSlots; i++)
                    {
                        _itemSlots.Add(new ItemSlot { Item = item.Clone(), Stack = item.MaxStack });
                    }
                }
                else
                {
                    for (var i = 0; i < slots; i++)
                    {
                        _itemSlots.Add(new ItemSlot { Item = item.Clone(), Stack = item.MaxStack });
                    }

                    if (extraCount != 0)
                    {
                        _itemSlots.Add(new ItemSlot { Item = item.Clone(), Stack = extraCount });
                    }
                }
            }
        }

        public void RemoveItem(Item item, int count)
        {
            var itemCopy = item.Clone();
            var similarSlots = _itemSlots.Where(x => x.Item.Id == item.Id);
            var slotsCount = _itemSlots.Count();

            foreach (var slot in similarSlots)
            {
                if (slot.Stack < count)
                {
                    slot.Stack = 0;
                    slot.Clear();
                    count -= slot.Stack;
                }
                else
                {
                    slot.Stack -= count;
                    break;
                }
            }
            _itemSlots = _itemSlots.Where(x => x.Stack > 0).ToList();
        }

        public bool HasItem(Item item, int count = 1)
        {
            var similarSlots = _itemSlots.Where(x => x.Item.Id == item.Id);
            var itemSlots = similarSlots.ToList();
            if (itemSlots.Any())
            {
                if (count == 1) return true;

                var itemStackSum = itemSlots.Sum(x => x.Stack);

                if (count <= itemStackSum) return true;
            }

            return false;
        }
    }
}