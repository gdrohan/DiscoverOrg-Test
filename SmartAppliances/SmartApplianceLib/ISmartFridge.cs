using System.Collections.Generic;
using Models;

namespace SmartApplianceLib
{
    public interface ISmartFridge
    {
        event SmartFridge.itemAdded OnItemAdded;
        event SmartFridge.itemRemoved OnItemRemoved;

        void AddItem(long type, string itemUUID);
        double GetFillFactor(long type);
        IEnumerable<ItemTypeState> GetItems(double fillFactor);
        void RemoveItem(long type, string itemUUID);
        void StopStockingItemType(long type);
    }
}