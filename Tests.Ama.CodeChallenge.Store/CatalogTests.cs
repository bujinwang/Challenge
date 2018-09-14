using Ama.CodeChallenge.Store;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Ama.CodeChallenge.Store
{
	[TestClass]
	public class CatalogTests
	{
		[TestMethod]
		public void GetProductById()		
		{
			var catalog = new Catalog();
			var product = catalog.GetProductById(1);
			Assert.IsNotNull(product);
			Assert.AreEqual(1, product.Id);
			Assert.AreEqual(typeof(Tent), product.GetType());
			Assert.AreEqual(50M, product.Cost);
			Assert.AreEqual(2.5M, product.Weight);
		}

		[TestMethod]
		public void UpdateProductInventory()
		{
			var catalog = new Catalog();
			var product = catalog.GetProductById(1);
			Assert.AreEqual(9, product.GetInventory());
			catalog.ModifyProductInventory(1, 80);
			Assert.AreEqual(89, product.GetInventory());
			Assert.AreEqual(89, catalog.GetCurrentProductInventory(1));
		}
	}
}
