using System;
using Ama.CodeChallenge.Store.Product;
using Ama.CodeChallenge.Store.ShoppingCart;
using Ama.CodeChallenge.Store.Store;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Ama.CodeChallenge.Store
{
    [TestClass]
    public class StoreTests
    {
        protected internal const string CustomerName = "Test";

        [TestMethod]
        public void Initialize_StoreInventory()
        {
            var store = new OnlineStore(new Catalog());
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
            var store = new OnlineStore(new Catalog());
            Assert.AreEqual(store.CheckInventory(ProductTypeEnum.Tent), 9);
            Assert.AreEqual(store.CheckInventory(ProductTypeEnum.SleepingBag), 3);
        }

        [TestMethod]
        public void AddItemToCart_TakeoutFromInventory()
        {
            var store = new OnlineStore(new Catalog());
            var customerName = CustomerName;
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
        public void AddItemToCart_MutipleCalls_Should_Total()
        {
            var store = new OnlineStore(new Catalog());
            var customerName = CustomerName;
            store.CreateShoppingCart(customerName);
            store.AddItemToShoppingCart(customerName, ProductTypeEnum.Tent, 3);
            store.AddItemToShoppingCart(customerName, ProductTypeEnum.Tent, 2);
            Assert.AreEqual(5, store.GetItemCountInCart(customerName, ProductTypeEnum.Tent));
            Assert.AreEqual(5* 50M, store.GetItemInCart(customerName, ProductTypeEnum.Tent).Cost);

        }

        [TestMethod]
        public void RemoveItemFromCart_ReturnsToInventory()
        {
            var store = new OnlineStore(new Catalog());
            var customerName = CustomerName;
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
            var store = new OnlineStore(new Catalog());
            const string customerName = CustomerName;
            store.CreateShoppingCart(customerName);
            store.AddItemToShoppingCart(customerName, ProductTypeEnum.Tent, 10); // only have 9 tents in the inventory
        }


        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void RemoveMoreItemsFromCart_Expecting_Exception()
        {
            var store = new OnlineStore(new Catalog());
            const string customerName = CustomerName;
            store.CreateShoppingCart(customerName);
            store.AddItemToShoppingCart(customerName, ProductTypeEnum.Tent, 8);
            store.RemoveItemFromShoppingCart(customerName, ProductTypeEnum.Tent, 10);
        }

        [TestMethod]
        public void CheckoutCart_Over200_Overweight()
        {
            var store = new OnlineStore(new Catalog());
            store.CreateShoppingCart(CustomerName);
            // 11*1 = 11kg overweight, $25 overweight charge applies
            store.AddItemToShoppingCart(CustomerName, ProductTypeEnum.Stove, 11);

            Assert.AreEqual(11, store.GetItemCountInCart(CustomerName, ProductTypeEnum.Stove));
            Assert.AreEqual(465M, store.CheckoutShoppingCart(CustomerName)); // price = 11*40 + 25
        }

        [TestMethod]
        public void CheckoutCart_LessThan200_Overweight()
        {
            var store = new OnlineStore(new Catalog());
            store.CreateShoppingCart(CustomerName);
            // 11*1 = 11kg overweight, $25 overweight charge applies, price is $55, less than $200
            store.AddItemToShoppingCart(CustomerName, ProductTypeEnum.DehydratedMeal, 11);

            Assert.AreEqual(11, store.GetItemCountInCart(CustomerName, ProductTypeEnum.DehydratedMeal));
            Assert.AreEqual(55M + 25M + 20M,
                store.CheckoutShoppingCart(CustomerName)); // price = 11*5 + $25 OW+ $20 Default Charge
        }

        [TestMethod]
        public void CheckoutCart_LessThan200_DefaultShippingCost()
        {
            var store = new OnlineStore(new Catalog());
            store.CreateShoppingCart(CustomerName);
            // 11*1 = 11kg overweight, $25 overweight charge applies
            store.AddItemToShoppingCart(CustomerName, ProductTypeEnum.Stove, 3); //$120

            Assert.AreEqual(3, store.GetItemCountInCart(CustomerName, ProductTypeEnum.Stove));
            Assert.AreEqual(120M + 20M, store.CheckoutShoppingCart(CustomerName)); // price = $120+$20
        }


        [TestMethod]
        public void CheckoutCart_Over200_WaiveShippingCost_OverweightNotApply()
        {
            var store = new OnlineStore(new Catalog());
            const string customerName = CustomerName;
            store.CreateShoppingCart(customerName);
            // 6*1 = 6kg not overweight, $25 overweight charge not appluy
            store.AddItemToShoppingCart(customerName, ProductTypeEnum.Stove, 6); //$240

            Assert.AreEqual(6, store.GetItemCountInCart(customerName, ProductTypeEnum.Stove));
            Assert.AreEqual(240M, store.CheckoutShoppingCart(customerName)); // price = $240
        }

        [TestMethod]
        public void CheckoutCart_Over3Tents_15PercentDiscount()
        {
            var store = new OnlineStore(new Catalog());
            const string customerName = CustomerName;
            store.CreateShoppingCart(customerName);

            store.AddItemToShoppingCart(customerName, ProductTypeEnum.Tent, 5); //$5*$50
            store.AddItemToShoppingCart(customerName, ProductTypeEnum.DehydratedMeal, 10); //10*5

            Assert.AreEqual(5, store.GetItemCountInCart(customerName, ProductTypeEnum.Tent));
            Assert.AreEqual(10, store.GetItemCountInCart(customerName, ProductTypeEnum.DehydratedMeal));
            // price = 5*$50 + $25 + $5*10
            // 5*2.5 = 12.5kg 'overweight, $25 overweight charge apply
            Assert.AreEqual((5 * 50M + 10 * 5M) * .85M + 25M, store.CheckoutShoppingCart(customerName));
        }

        [TestMethod]
        public void DropShoppingCart_Should_ZeroOutCart_RestoreInventory()
        {
            var store = new OnlineStore(new Catalog());
            var customerName = CustomerName;
            store.CreateShoppingCart(customerName);
            var originalTotalInventory = Catalog.GetTotalProducts();
            store.AddItemToShoppingCart(customerName, ProductTypeEnum.Tent, 3);
            
            Assert.AreEqual(originalTotalInventory-3, Catalog.GetTotalProducts());
            store.AddItemToShoppingCart(customerName, ProductTypeEnum.Backpack, 2);
            Assert.AreEqual(originalTotalInventory - 3 -2 , Catalog.GetTotalProducts());

            Assert.AreEqual(3, store.GetItemCountInCart(customerName, ProductTypeEnum.Tent));
            Assert.AreEqual(2, store.GetItemCountInCart(customerName, ProductTypeEnum.Backpack));

            store.DropShoppingCart(customerName);
            Assert.AreEqual(store.GetItemCountInCart(customerName, ProductTypeEnum.Tent), 0);
            Assert.AreEqual(store.GetItemCountInCart(customerName, ProductTypeEnum.Backpack), 0);
            // total inventory changed back
            Assert.AreEqual(originalTotalInventory, Catalog.GetTotalProducts());
        }


        [TestMethod]
        public void CheckoutCart_Detailed()
        {
            var store = new OnlineStore(new Catalog());
            const string customerName = CustomerName;
            store.CreateShoppingCart(customerName);
            // 11*1 = 11kg overweight, $25 overweight charge applies
            store.AddItemToShoppingCart(customerName, ProductTypeEnum.Stove, 3); //$120

            Assert.AreEqual(3, store.GetItemCountInCart(customerName, ProductTypeEnum.Stove));
            var shoppingCart = store.CheckoutShoppingCart(customerName, true);

            Assert.AreEqual(120, shoppingCart.SubTotal); 
            Assert.AreEqual(20, shoppingCart.ShippingFees);
            Assert.AreEqual(140, shoppingCart.Total);
            Assert.AreEqual(0, shoppingCart.Discounts);
                  
        }

        [TestMethod]
        public void NewCheckoutCart_Over5Tents_15PercentDiscount()
        {
            var store = new OnlineStore(new Catalog());
            const string customerName = CustomerName;
            store.CreateShoppingCart(customerName);

            store.AddItemToShoppingCart(customerName, ProductTypeEnum.Tent, 5); //$5*$50
            store.AddItemToShoppingCart(customerName, ProductTypeEnum.DehydratedMeal, 10); //10*5

            Assert.AreEqual(5, store.GetItemCountInCart(customerName, ProductTypeEnum.Tent));
            Assert.AreEqual(10, store.GetItemCountInCart(customerName, ProductTypeEnum.DehydratedMeal));
            // price = 5*$50 + $25 + $5*10
            // 5*2.5 = 12.5kg 'overweight, $25 overweight charge apply
            ShoppingCart checkoutShoppingCart = store.CheckoutShoppingCart(customerName, true);
            Assert.AreEqual((5 * 50M + 10 * 5M) * .85M + 25M, checkoutShoppingCart.Total);
            Assert.AreEqual((5 * 50M + 10 * 5M) * .15M, checkoutShoppingCart.Discounts);
            Assert.AreEqual(25M, checkoutShoppingCart.ShippingFees); // default shipping charge waived
            Assert.AreEqual(255, checkoutShoppingCart.SubTotal);
        }

    }
}