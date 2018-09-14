using Ama.CodeChallenge.Store;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
			store.AddItemToShoppingCart("Test", 1, 10);
			Assert.AreEqual(10, store.GetItemCountInCart("Test", 1));
		}

		[TestMethod]
		public void CheckoutCart_NoOverweight()
		{
			var catalog = new Catalog();
			var store = new OnlineStore(new Catalog());
			store.CreateShoppingCart("Test");
			store.AddItemToShoppingCart("Test", 1, 10);
			Assert.AreEqual(10, store.GetItemCountInCart("Test", 1));
			Assert.AreEqual(95, store.CheckoutShoppingCart("Test"));
		}

		[TestMethod]
		public void CheckoutCart_Overweight()
		{
			var catalog = new Catalog();
			var store = new OnlineStore(new Catalog());
			store.CreateShoppingCart("Test");
			store.AddItemToShoppingCart("Test", 1, 10);
			Assert.AreEqual(10, store.GetItemCountInCart("Test", 1));
			Assert.AreEqual(95, store.CheckoutShoppingCart("Test"));
		}
	}
}
