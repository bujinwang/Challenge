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
            Assert.AreEqual(store.CheckInventory(ProductTypeEnum.Tent), 9);
            Assert.AreEqual(store.CheckInventory(ProductTypeEnum.SleepingBag), 3);
            Assert.AreEqual(store.CheckInventory(ProductTypeEnum.Backpack), 7);
            Assert.AreEqual(store.CheckInventory(ProductTypeEnum.Stove), 16);
            Assert.AreEqual(store.CheckInventory(ProductTypeEnum.GranolaBar), 30);
            Assert.AreEqual(store.CheckInventory(ProductTypeEnum.TrailMix), 19);
            Assert.AreEqual(store.CheckInventory(ProductTypeEnum.DehydratedMeal), 30);
            Assert.AreEqual(store.CheckInventory(ProductTypeEnum.Coffee), 42);
        }

        [TestMethod]
        public void Add_StoreInventory()
        {
            var store = new OnlineStore(new Inventory());
            Assert.AreEqual(store.CheckInventory(ProductTypeEnum.Tent), 9);
            Assert.AreEqual(store.CheckInventory(ProductTypeEnum.SleepingBag), 3);
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
            Assert.AreEqual(tentInInventory, 4);
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
            const string customerName = "Test";
            store.CreateShoppingCart(customerName);
            store.AddItemToShoppingCart(customerName, ProductTypeEnum.Tent, 10); // only have 9 tents in the inventory
        }


        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void RemoveMoreItemsFromCart_Expecting_Exception()
        {
            var store = new OnlineStore(new Inventory());
            const string customerName = "Test";
            store.CreateShoppingCart(customerName);
            store.AddItemToShoppingCart(customerName, ProductTypeEnum.Tent, 8);
            store.RemoveItemFromShoppingCart(customerName, ProductTypeEnum.Tent, 10);
        }

        [TestMethod]
        public void CheckoutCart_Over200_Overweight()
        {
            var store = new OnlineStore(new Inventory());
            store.CreateShoppingCart("Test");
            // 11*1 = 11kg overweight, $25 overweight charge applies
            store.AddItemToShoppingCart("Test", ProductTypeEnum.Stove, 11);

            Assert.AreEqual(11, store.GetItemCountInCart("Test", ProductTypeEnum.Stove));
            Assert.AreEqual(465M, store.CheckoutShoppingCart("Test")); // price = 11*40 + 25
        }

        [TestMethod]
        public void CheckoutCart_LessThan200_Overweight()
        {
            var store = new OnlineStore(new Inventory());
            store.CreateShoppingCart("Test");
            // 11*1 = 11kg overweight, $25 overweight charge applies, price is $55, less than $200
            store.AddItemToShoppingCart("Test", ProductTypeEnum.DehydratedMeal, 11);

            Assert.AreEqual(11, store.GetItemCountInCart("Test", ProductTypeEnum.DehydratedMeal));
            Assert.AreEqual(55M + 25M + 20M,
                store.CheckoutShoppingCart("Test")); // price = 11*5 + $25 OW+ $20 Default Charge
        }

        [TestMethod]
        public void CheckoutCart_LessThan200_DefaultShippingCost()
        {
            var store = new OnlineStore(new Inventory());
            store.CreateShoppingCart("Test");
            // 11*1 = 11kg overweight, $25 overweight charge applies
            store.AddItemToShoppingCart("Test", ProductTypeEnum.Stove, 3); //$120

            Assert.AreEqual(3, store.GetItemCountInCart("Test", ProductTypeEnum.Stove));
            Assert.AreEqual(120M + 20M, store.CheckoutShoppingCart("Test")); // price = $120+$20
        }


        [TestMethod]
        public void CheckoutCart_Over200_WaiveShippingCost_OverweightNotApply()
        {
            var store = new OnlineStore(new Inventory());
            store.CreateShoppingCart("Test");
            // 6*1 = 6kg not overweight, $25 overweight charge not appluy
            store.AddItemToShoppingCart("Test", ProductTypeEnum.Stove, 6); //$240

            Assert.AreEqual(6, store.GetItemCountInCart("Test", ProductTypeEnum.Stove));
            Assert.AreEqual(240M, store.CheckoutShoppingCart("Test")); // price = $240
        }

        [TestMethod]
        public void CheckoutCart_Over3Tents_15PercentDiscount()
        {
            var store = new OnlineStore(new Inventory());
            const string customerName = "Test";
            store.CreateShoppingCart(customerName);

            store.AddItemToShoppingCart(customerName, ProductTypeEnum.Tent, 5); //$5*$50
            store.AddItemToShoppingCart(customerName, ProductTypeEnum.DehydratedMeal, 10); //10*5

            Assert.AreEqual(5, store.GetItemCountInCart(customerName, ProductTypeEnum.Tent));
            Assert.AreEqual(10, store.GetItemCountInCart(customerName, ProductTypeEnum.DehydratedMeal));
            // price = 5*$50 + $25 + $5*10
            // 5*2.5 = 12.5kg 'overweight, $25 overweight charge apply
            Assert.AreEqual((5 * 50M + 10 * 5M) * .85M + 25M, store.CheckoutShoppingCart(customerName));
        }
    }
}