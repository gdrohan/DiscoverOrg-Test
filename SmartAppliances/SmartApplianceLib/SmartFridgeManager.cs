using Models;
using System.Collections.Generic;

namespace SmartApplianceLib
{
    public class SmartFridgeManager : ISmartFridgeManager
    {
        private readonly ISmartFridge _fridge;
        public Queue<string> Messages = new Queue<string>();
        
        public SmartFridgeManager(ISmartFridge fridge)
        {
            _fridge = fridge;
            _fridge.OnItemAdded += HandleItemAdded;
            _fridge.OnItemRemoved += HandleItemRemoved;
        }

        public void ForgetItem(long itemType)
        {
            _fridge.StopStockingItemType(itemType);
        }

        public double GetFillFactor(long itemType)
        {
            return _fridge.GetFillFactor(itemType);
        }

        public IEnumerable<ItemTypeState> GetItems(double fillFactor)
        {
            return _fridge.GetItems(fillFactor);
        }

        public void HandleItemAdded(long itemType, string itemUUID, string name, double fillFactor)
        {
            Messages.Enqueue($"Added ItemUUID: {itemUUID} for Type: {itemType}, Name: {name}. Current FillFactor is {fillFactor} of total capacity");
        }

        public void HandleItemRemoved(string itemUUID)
        {
            Messages.Enqueue($"ItemUUID: {itemUUID} was removed.");
        }
    }
}
