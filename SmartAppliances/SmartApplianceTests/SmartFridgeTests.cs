using Xunit;
using System;
using System.Collections.Generic;
using System.Linq;
using Models;
using SmartApplianceLib;

namespace SmartApplianceTests
{
    public class SmartFridgeTests
    {
        private SmartFridge _smartFridge;

        List<string> itemAddedEvents = new List<string>();

        public SmartFridgeTests()
        {

            _smartFridge = new SmartFridge(new Dictionary<long, ItemType>
            {
                { 1, new ItemType { Type= 1,
                                    Name = "Milk",
                                    Capacity = 3,
                                    IsStocked = true}
                },
                { 2, new ItemType { Type= 2,
                                    Name = "Eggs",
                                    Capacity = 2,
                                    IsStocked = true}
                },
                { 3, new ItemType { Type= 3,
                                    Name = "Bacon",
                                    Capacity = 2,
                                    IsStocked = true}
                },
                { 4, new ItemType { Type= 4,
                                    Name = "Butter",
                                    Capacity = 3,
                                    IsStocked = true}
                },
                { 5, new ItemType { Type= 5,
                                    Name = "Yogurt",
                                    Capacity = 3,
                                    IsStocked = false}
                }

            });
        }

        [Fact]
        public void WhenAnItemIsAddedAndThereIsCapacityForIt()
        {
            long itemType = 0;
            string itemUUID = string.Empty;
            string itemTypeName = string.Empty;
            double fillFactor = 0;

            _smartFridge.OnItemAdded += delegate (long type, string UUID, string name, double fill)
            {
                itemType = type;
                itemUUID = UUID;
                itemTypeName = name;
                fillFactor = fill;
            };

            var preFillFactor = _smartFridge.GetFillFactor(1);
            Assert.Equal(0, preFillFactor);
            _smartFridge.AddItem(1, "a1");
            var postFillFactor = _smartFridge.GetFillFactor(1);
            Assert.Equal(0.33, postFillFactor);

            Assert.Equal(1, itemType);
            Assert.Equal("a1", itemUUID);
            Assert.Equal("Milk", itemTypeName);
            Assert.Equal(0.33, fillFactor);
        }

        [Fact]
        public void WhenAnItemIsAddedButThereIsNOTCapacityForIt_ItThrowsException()
        {
            _smartFridge.OnItemAdded += delegate (long type, string UUID, string name, double fill) { };

            _smartFridge.AddItem(1, "a1");
            _smartFridge.AddItem(1, "a2");
            _smartFridge.AddItem(1, "a3");
            var exception = Assert.Throws<Exception>(() => _smartFridge.AddItem(1, "a4"));
            Assert.Equal("Item type Milk is at full capacity, cannot add item a4.", exception.Message);
        }

        [Fact]
        public void WhenAnItemIsAddedButTypeIsNotBeingStocked_ItThrowsException()
        {
            _smartFridge.OnItemAdded += delegate (long type, string UUID, string name, double fill) { };

            var exception = Assert.Throws<Exception>(() => _smartFridge.AddItem(5, "y1"));
            Assert.Equal("Item type Yogurt is not currently being stocked.", exception.Message);
        }

        [Fact]
        public void WhenGettingFillFactorButTypeIsNotBeingStocked_ItThrowsException()
        {
            var exception = Assert.Throws<Exception>(() => _smartFridge.GetFillFactor(5));
            Assert.Equal("Item type Yogurt is not currently being stocked.", exception.Message);
        }

        [Fact]
        public void WhenGettingItems_ItOnlyReturnsItemsBelowFillFactorAndCurrentlyBeingStocked()
        {
            _smartFridge.OnItemAdded += delegate (long type, string UUID, string name, double fill) { };
            _smartFridge.AddItem(1, "m1");
            _smartFridge.AddItem(2, "e1");
            _smartFridge.AddItem(3, "ba1");
            _smartFridge.AddItem(4, "bu1");
            _smartFridge.AddItem(4, "bu2");
            var items = _smartFridge.GetItems(0.5).ToList();
            Assert.Equal(3, items.Count());
            Assert.Equal("Milk", items[0].Name);
            Assert.Equal("Eggs", items[1].Name);
            Assert.Equal("Bacon", items[2].Name);
        }

        [Fact]
        public void WhenAnItemIsRemovedButDoesNOTExist_ItThrowsException()
        {
            var exception = Assert.Throws<Exception>(() => _smartFridge.RemoveItem(6, "a1"));
            Assert.Equal("Item type 6 does not exist.", exception.Message);
        }

        [Fact]
        public void WhenAnItemIsRemoved()
        {
            string itemUUID = string.Empty;

            _smartFridge.OnItemAdded += delegate (long type, string UUID, string name, double fill) { };
            _smartFridge.OnItemRemoved += delegate (string UUID) { itemUUID = UUID; };

            _smartFridge.AddItem(1, "a1");
            var fillFactor = _smartFridge.GetFillFactor(1);
            Assert.Equal(0.33, fillFactor);
            _smartFridge.RemoveItem(1, "a1");
            Assert.Equal("a1", itemUUID);
            fillFactor = _smartFridge.GetFillFactor(1);
            Assert.Equal(0, fillFactor);
        }

        [Fact]
        public void WhenAnItemIsNoLongerStocked_CallToGetFillFactorThrowsException()
        {
            _smartFridge.OnItemAdded += delegate (long type, string UUID, string name, double fill) { };
       
            _smartFridge.AddItem(1, "a1");
            var fillFactor = _smartFridge.GetFillFactor(1);
            Assert.Equal(0.33, fillFactor);
            _smartFridge.StopStockingItemType(1);
            var exception = Assert.Throws<Exception>(() => _smartFridge.GetFillFactor(1)); 
            Assert.Equal("Item type Milk is not currently being stocked.", exception.Message);
        }
    }
}
