using Ama.CodeChallenge.Store.Product;
using Ama.CodeChallenge.Store.Store;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Ama.CodeChallenge.Store
{
	[TestClass]
	public class StoreTests
	{
		[TestMethod]
		public void AddItemToCart()
		{
			var catalog = new Catalog();
			var store = new OnlineStore(new Catalog());
			store.CreateShoppingCart("Test");
			store.AddItemToShoppingCart("Test", ProductTypeEnum.Tent, 10);
			Assert.AreEqual(10, store.GetItemCountInCart("Test", ProductTypeEnum.Tent));
		}

		[TestMethod]
		public void CheckoutCart_NoOverweight()
		{
			var catalog = new Catalog();
			var store = new OnlineStore(new Catalog());
			store.CreateShoppingCart("Test");
			store.AddItemToShoppingCart("Test", ProductTypeEnum.Tent, 10);
			Assert.AreEqual(10, store.GetItemCountInCart("Test", ProductTypeEnum.Tent));
			Assert.AreEqual(95, store.CheckoutShoppingCart("Test"));
		}

		[TestMethod]
		public void CheckoutCart_Overweight()
		{
			var catalog = new Catalog();
			var store = new OnlineStore(new Catalog());
			store.CreateShoppingCart("Test");
			store.AddItemToShoppingCart("Test", ProductTypeEnum.Tent, 10);
			Assert.AreEqual(10, store.GetItemCountInCart("Test", ProductTypeEnum.Tent));
			Assert.AreEqual(95, store.CheckoutShoppingCart("Test"));
		}
	}
}
