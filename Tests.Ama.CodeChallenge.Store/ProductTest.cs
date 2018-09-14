using Ama.CodeChallenge.Store;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Ama.CodeChallenge.Store.Product.Camping;
using Ama.CodeChallenge.Store.Product.Food;

namespace Tests.Ama.CodeChallenge.Store
{
	[TestClass]
	public class ProductTest
	{
		[TestMethod]
		public void GetDescriptionOfSleepingBag()
		{
			var sleepingBag = new SleepingBag(0);
			Assert.AreEqual(": Soft and fluffy sleeping bag", sleepingBag.GetDescription());

			sleepingBag.Name = "Outdoor Research Kids Sleeping Bag";
			Assert.AreEqual("Outdoor Research Kids Sleeping Bag: Soft and fluffy sleeping bag", sleepingBag.GetDescription());
		}

		

		[TestMethod]
		public void GetDescriptionOfTrailMix()
		{
			var trailMix = new TrailMix(0);
			trailMix.Name = "Han-D Pack Trail Mix";
			Assert.AreEqual(true, trailMix.ContainsAllergens);
			Assert.AreEqual(false, trailMix.RequiresCooking);
			Assert.AreEqual("Han-D Pack Trail Mix" + Environment.NewLine +
				"Contains Allergens: False" + Environment.NewLine +
				"Requires Cooking: False" + Environment.NewLine +
				"Ingredients: Nuts, Oats, Honey, Dried Berries", trailMix.GetDescription());
		}
	}
}
