using System;
using Ama.CodeChallenge.Store.Product;
using Ama.CodeChallenge.Store.Product.Camping;
using Ama.CodeChallenge.Store.Store;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Ama.CodeChallenge.Store
{
    [TestClass]
    public class InventoryTests
    {
        [TestMethod]
        public void GetProductById()
        {
            var inventory = new Catalog();
            var product = inventory.GetProductByType(ProductTypeEnum.Tent);
            Assert.IsNotNull(product);
            Assert.AreEqual(ProductTypeEnum.Tent, product.ProductType);
            Assert.AreEqual(typeof(Tent), product.GetType());
            Assert.AreEqual(50M, product.Cost);
            Assert.AreEqual(2.5M, product.Weight);
        }

        [TestMethod]
        public void UpdateProductInventory()
        {
            var inventory = new Catalog();
            var product = inventory.GetProductByType(ProductTypeEnum.Tent);
            Assert.AreEqual(9, product.GetInventory());
            inventory.ModifyProductInventory(ProductTypeEnum.Tent, 80);
            Assert.AreEqual(89, product.GetInventory());
            Assert.AreEqual(89, inventory.GetCurrentProductInventory(ProductTypeEnum.Tent));
        }

        [TestMethod]
        public void InventoryTotalProduct()
        {
            var inventory = new Catalog();
            Assert.AreEqual(Catalog.GetTotalProducts(), 156);
            inventory.ModifyProductInventory(ProductTypeEnum.Tent, 80);
            Assert.AreEqual(Catalog.GetTotalProducts(), 156 + 80);

            inventory.ModifyProductInventory(ProductTypeEnum.Tent, -10);
            Assert.AreEqual(Catalog.GetTotalProducts(), 156 + 80 - 10);
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void InventoryModify_Out_of_Range()
        {
            var inventory = new Catalog();
            Assert.AreEqual(Catalog.GetTotalProducts(), 156);
            inventory.ModifyProductInventory(ProductTypeEnum.Tent, -180);
        }
    }
}