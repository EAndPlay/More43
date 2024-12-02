using System;
using UnityEngine;
using UnityEngine.UI;

namespace Inventory
{
    public enum ItemId
    {
        HealthPotion,
        RegenerationPotion
    }
    
    [CreateAssetMenu(fileName = "Item", menuName = "Inventory/Item")]
    public class Item : ScriptableObject
    {
        public ItemId Id;
        public string Name; // $name
        public int MaxStack;
        public Sprite Icon;

        public bool IsDroppable;

        public Item Clone()
        {
            var inst = ScriptableObject.CreateInstance<Item>();
            inst.Id = Id;
            inst.Name = Name;
            inst.MaxStack = MaxStack;
            inst.Icon = Icon;
            return inst;
        }
    }
}