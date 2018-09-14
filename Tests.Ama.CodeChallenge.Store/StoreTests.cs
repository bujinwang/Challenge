using Ama.CodeChallenge.Store;
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
			var store = new OnlineStore(new Catalog());
			store.CreateShoppingCart("Test");
			store.AddItemToShoppingCart("Test", 1, 10);
			Assert.AreEqual(10, store.GetItemCountInCart("Test", 1));
		}

		[TestMethod]
		public void CheckoutCart_NoOverweight()
		{
			var store = new OnlineStore(new Catalog());
			store.CreateShoppingCart("Test");
			store.AddItemToShoppingCart("Test", 1, 10);
			Assert.AreEqual(10, store.GetItemCountInCart("Test", 1));
			Assert.AreEqual(95, store.CheckoutShoppingCart("Test"));
		}

		[TestMethod]
		public void CheckoutCart_Overweight()
		{
			var store = new OnlineStore(new Catalog());
			store.CreateShoppingCart("Test");
			store.AddItemToShoppingCart("Test", 1, 10);
			Assert.AreEqual(10, store.GetItemCountInCart("Test", 1));
			Assert.AreEqual(95, store.CheckoutShoppingCart("Test"));
		}
	}
}
