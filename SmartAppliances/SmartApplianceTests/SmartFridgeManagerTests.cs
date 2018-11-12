using Xunit;
using System.Collections.Generic;
using System.Linq;
using Models;
using SmartApplianceLib;
using Moq;

namespace SmartApplianceTests
{
    public class SmartFridgeManagerTests
    {
        private Mock<ISmartFridge> _smartFridge;
        private SmartFridgeManager _smartFridgeManager;
        public SmartFridgeManagerTests() {

            _smartFridge = new Mock<ISmartFridge>();
            _smartFridgeManager = new SmartFridgeManager(_smartFridge.Object);
        }

        [Fact]
        public void WhenItemIsAddedToFridgeItIsHandledByManager()
        {
            _smartFridge.Setup(s => s.AddItem(1,"a1")).Raises(s => s.OnItemAdded += null, 1, "a1", "Milk", 0.2);
            _smartFridge.Object.AddItem(1, "a1");
            var message = _smartFridgeManager.Messages.Dequeue();
            Assert.Equal($"Added ItemUUID: a1 for Type: 1, Name: Milk. Current FillFactor is 0.2 of total capacity", message);
        }

        [Fact]
        public void WhenItemIsRemovedFromFridgeItIsHandledByManager()
        {
            _smartFridge.Setup(s => s.RemoveItem(1, "a1")).Raises(s => s.OnItemRemoved += null, "a1");
            _smartFridge.Object.RemoveItem(1, "a1");
            var message = _smartFridgeManager.Messages.Dequeue();
            Assert.Equal($"ItemUUID: a1 was removed.", message);
        }

        [Fact]
        public void WhenGetFillFactorCorrectValueIsReturned()
        {
            _smartFridge.Setup(s => s.GetFillFactor(2)).Returns(0.4);
            var fillFactor = _smartFridgeManager.GetFillFactor(2);
            Assert.Equal(0.4, fillFactor);
        }

        [Fact]
        public void WhenForgetItemInvokedThenSmartFridgeStopStockingItemIsCalled()
        {
            _smartFridgeManager.ForgetItem(3);
            _smartFridge.Verify(s => s.StopStockingItemType(3));
        }

        [Fact]
        public void WhenGetItemsIsInvokedThenReturnsResultOfFridgeGetItems()
        {
            var expectedItemTypeState = new ItemTypeState { Name = "Soda", Type = 10, Capacity = 100, FillFactor = 40 };
            _smartFridge.Setup(s => s.GetItems(0.5)).Returns(new List<ItemTypeState>() { expectedItemTypeState });
            var itemTypes = _smartFridgeManager.GetItems(0.5).ToList();
            Assert.Single(itemTypes);
            var itemTypeState = itemTypes.First();
            Assert.Equal(expectedItemTypeState, itemTypeState);
        }       
    }
}
