using System.Collections.Generic;
using Models;
using System;
using System.Linq;

namespace SmartApplianceLib
{
    public class SmartFridge : ISmartFridge
    {
        private readonly Dictionary<long, ItemType> _itemTypes;
        private readonly Dictionary<long, List<Item>> _inventory = new Dictionary<long, List<Item>>();

        public SmartFridge(Dictionary<long, ItemType> itemTypes)
        {
            _itemTypes = itemTypes;
        }
                
        public delegate void itemRemoved(string itemUUID);
        public event itemRemoved OnItemRemoved;

        public delegate void itemAdded(long itemType, string itemUUID, string name, double fillFactor);
        public event itemAdded OnItemAdded;


        public void AddItem(long type, string itemUUID)
        {
            ItemType itemType = GetItemType(type);
          
            if (!itemType.IsStocked)
            {
                throw new Exception($"Item type {itemType.Name} is not currently being stocked.");
            }

            if (!_inventory.TryGetValue(type, out List<Item> items))
            {
                items = new List<Item>();
                _inventory[type] = items;
            }

            var itemCount = items.Count;
            if (itemCount == itemType.Capacity)
            {
                throw new Exception($"Item type {itemType.Name} is at full capacity, cannot add item {itemUUID}.");
            }

            items.Add(new Item
            {
                Type = type,
                ItemUUID = itemUUID
            });
            itemCount = items.Count;

            var fillFactor = CalculateFillFactor(itemType.Capacity, itemCount);
            OnItemAdded(type, itemUUID, itemType.Name, fillFactor);
        }

        public double GetFillFactor(long type)
        {
            var itemType = GetItemType(type);
            if (!itemType.IsStocked)
            {
                throw new Exception($"Item type {itemType.Name} is not currently being stocked.");
            }
            List<Item> itemTypeItems = (_inventory.ContainsKey(type)) ? _inventory[type] : new List<Item>();
            return CalculateFillFactor(itemType.Capacity, itemTypeItems.Count);
        }

        public IEnumerable<ItemTypeState> GetItems(double fillFactor)
        {
            foreach (var itemList in _inventory)
            {
                var key = itemList.Key;
                var itemType = _itemTypes[key];
                if (itemType.IsStocked)
                {
                    var itemTypeFillFactor = CalculateFillFactor(itemType.Capacity, itemList.Value.Count);
                    if (itemTypeFillFactor <= fillFactor) {
                        yield return new ItemTypeState
                        {
                            Capacity = itemType.Capacity,
                            FillFactor = itemTypeFillFactor,
                            Name = itemType.Name,
                            Type = itemType.Type
                        };
                    }
                }
            }
        }

        public void RemoveItem(long type, string itemUUID)
        {
            if (!_itemTypes.ContainsKey(type)) {
                throw new Exception($"Item type {type} does not exist.");
            }

            if (_inventory.TryGetValue(type, out List<Item> items))
            {
                var item = items.SingleOrDefault(i => i.ItemUUID == itemUUID);
                if (item != null)
                {
                    items.Remove(item);
                }
            }

            OnItemRemoved(itemUUID);
        }

        public void StopStockingItemType(long type)
        {
            if (!_itemTypes.TryGetValue(type, out ItemType itemType))
            {
                throw new Exception($"Item type {type} does not exist.");
            }

            itemType.IsStocked = false;
        }

        private ItemType GetItemType(long type)
        {
            if (!_itemTypes.TryGetValue(type, out ItemType itemType))
            {
                throw new Exception($"Item type {type} does not exist.");
            }            

            return itemType;
        }

        private double CalculateFillFactor(int capacity, int itemCount)
        {
            if (itemCount == 0)
            {
                return 0;
                // or throw error? what to return if item is not currently being stocked?
            }

            // No check for divide by zero, assume capacity should never be zero, even when IsStocked is false.
            return Math.Round( ((double)(itemCount)) / capacity, 2);
        }
    }
}
