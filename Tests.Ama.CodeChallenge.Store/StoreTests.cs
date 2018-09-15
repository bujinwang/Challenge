using System;
using Ama.CodeChallenge.Store.Product;
using Ama.CodeChallenge.Store.Store;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Ama.CodeChallenge.Store
{
	[TestClass]
	public class StoreTests
	{
	    [TestMethod]
	    public void Initialize_StoreInventory()
	    {
	        var store = new OnlineStore(new Inventory());
            Assert.AreEqual(store.CheckInventory(ProductTypeEnum.Tent),9);
            Assert.AreEqual(store.CheckInventory(ProductTypeEnum.SleepingBag),3);
            Assert.AreEqual(store.CheckInventory(ProductTypeEnum.Backpack),7);
            Assert.AreEqual(store.CheckInventory(ProductTypeEnum.Stove),16);
            Assert.AreEqual(store.CheckInventory(ProductTypeEnum.GranolaBar),30);
            Assert.AreEqual(store.CheckInventory(ProductTypeEnum.TrailMix),19);
            Assert.AreEqual(store.CheckInventory(ProductTypeEnum.DehydratedMeal),30);
            Assert.AreEqual(store.CheckInventory(ProductTypeEnum.Coffee),42);
        }

	    [TestMethod]
		public void AddItemToCart_TakeoutFromInventory()
		{
			var store = new OnlineStore(new Inventory());
		    var customerName = "Test";
		    store.CreateShoppingCart(customerName);
		    bool hasInventory = store.CheckInventory(ProductTypeEnum.Tent) >= 10;
            Assert.IsFalse(hasInventory);
		    hasInventory = store.CheckInventory(ProductTypeEnum.Tent) >= 5;
		    Assert.IsTrue(hasInventory);
            store.AddItemToShoppingCart(customerName, ProductTypeEnum.Tent, 5);
		    int tentInInventory = store.CheckInventory(ProductTypeEnum.Tent);
            Assert.AreEqual(tentInInventory,4);
			Assert.AreEqual(5, store.GetItemCountInCart(customerName, ProductTypeEnum.Tent));

		}

	    [TestMethod]
	    public void RemoveItemFromCart_ReturnsToInventory()
	    {
	        var store = new OnlineStore(new Inventory());
	        var customerName = "Test";
	        store.CreateShoppingCart(customerName);
	        bool hasInventory = store.CheckInventory(ProductTypeEnum.Tent) >= 5;
	        Assert.IsTrue(hasInventory);
	        store.AddItemToShoppingCart(customerName, ProductTypeEnum.Tent, 5);
	        int tentInInventory = store.CheckInventory(ProductTypeEnum.Tent);
	        Assert.AreEqual(tentInInventory, 4);

	        Assert.AreEqual(5, store.GetItemCountInCart(customerName, ProductTypeEnum.Tent));
            // now take one item from cart
            store.RemoveItemFromShoppingCart(customerName, ProductTypeEnum.Tent, 1);
            // show be 4 left in cart
	        Assert.AreEqual(4, store.GetItemCountInCart(customerName, ProductTypeEnum.Tent));
            // inventory should have 5
	        tentInInventory = store.CheckInventory(ProductTypeEnum.Tent);
	        Assert.AreEqual(tentInInventory, 5);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddMoreItemsThanInventoryToCart_Expecting_Exception()
		{
			var store = new OnlineStore(new Inventory());
			store.CreateShoppingCart("Test");
			store.AddItemToShoppingCart("Test", ProductTypeEnum.Tent, 10); // only have 9 tents in the inventory
		}

		[TestMethod]
		public void CheckoutCart_Overweight()
		{
			var store = new OnlineStore(new Inventory());
			store.CreateShoppingCart("Test");
			store.AddItemToShoppingCart("Test", ProductTypeEnum.Tent, 10);
			Assert.AreEqual(10, store.GetItemCountInCart("Test", ProductTypeEnum.Tent));
			Assert.AreEqual(95, store.CheckoutShoppingCart("Test"));
		}
	}
}
