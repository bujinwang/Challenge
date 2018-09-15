using System;
using Ama.CodeChallenge.Store;
using Ama.CodeChallenge.Store.Product;
using Ama.CodeChallenge.Store.Product.Camping;
using Ama.CodeChallenge.Store.Store;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Ama.CodeChallenge.Store
{
	[TestClass]
	public class CatalogTests
	{
		[TestMethod]
		public void GetProductById()		
		{
			var catalog = new Inventory();
			var product = catalog.GetProductByType(ProductTypeEnum.Tent);
			Assert.IsNotNull(product);
			Assert.AreEqual(1, product.Id);
			Assert.AreEqual(typeof(Tent), product.GetType());
			Assert.AreEqual(50M, product.Cost);
			Assert.AreEqual(2.5M, product.Weight);
		}
            
		[TestMethod]
		public void UpdateProductInventory()
		{
			var catalog = new Inventory();
			var product = catalog.GetProductByType(ProductTypeEnum.Tent);
			Assert.AreEqual(9, product.GetInventory());
			catalog.ModifyProductInventory(ProductTypeEnum.Tent, 80);
			Assert.AreEqual(89, product.GetInventory());
			Assert.AreEqual(89, catalog.GetCurrentProductInventory(ProductTypeEnum.Tent));
		}
	}
}
