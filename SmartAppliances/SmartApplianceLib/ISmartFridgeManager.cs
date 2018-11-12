using Models;
using System.Collections.Generic;

namespace SmartApplianceLib
{
    public interface ISmartFridgeManager
    {
        /**
         * Event Handlers - These are methods invoked by the SmartFridge hardware to send notification of items that have
         * been added and/or removed from the fridge. Every time an item is removed by the fridge user, it will emit a
         * handleItemRemoved() event to this class, every time a new item is added or a previously removed item is re-inserted,
         * the fridge will emit a handleItemAdded() event with its updated fillFactor.
         */

        /**
         * This method is called every time an item is removed from the fridge
         *
         * @param itemUUID
         */
        void HandleItemRemoved(string itemUUID);
        /**
         * This method is called every time an item is stored in the fridge
         *
         * @param itemType
         * @param itemUUID
         * @param name
         * @param fillFactor
         */
        void HandleItemAdded(long itemType, string itemUUID, string name, double fillFactor);

        /**
         * These are the query methods for the fridge to be able to display alerts and create shopping
         * lists for the fridge user.
         */

        /**
         * Returns a list of items based on their fill factor. This method is used by the
         * fridge to display items that are running low and need to be replenished.
         *
         * i.e.
         *      getItems( 0.5 ) - will return any items that are 50% or less full, including
         *                        items that are depleted. Unless all available containers are
         *                        empty, this method should only consider the non-empty containers
         *                        when calculating the overall fillFactor for a given item.
         *
         * @return an IEnumerable of ItemTypeState objects.
         */
        IEnumerable<ItemTypeState> GetItems(double fillFactor);

        /**
         * Returns the fill factor for a given item type to be displayed to the owner. Unless all available containers are
         * empty, this method should only consider the non-empty containers
         * when calculating the overall fillFactor for a given item.
         *
         * @param itemType
         *
         * @return a double representing the average fill factor for the item type
         */
        double GetFillFactor(long itemType);

        /**
         * Stop stocking a given item. This method is used by the fridge to signal that its
         * owner will no longer stock this item and thus should not be returned from #getItems()
         *
         * @param itemType
         */
        void ForgetItem(long itemType);
    }
}