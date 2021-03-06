﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Ama.CodeChallenge.Store.Product;
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
			var sleepingBag = new SleepingBag("Nice sleeping bad", 20M,2, 1M);
			Assert.AreEqual(sleepingBag.Name +": Soft and fluffy sleeping bag", sleepingBag.GetDescription());

			sleepingBag.Name = "Outdoor Research Kids Sleeping Bag";
			Assert.AreEqual("Outdoor Research Kids Sleeping Bag: Soft and fluffy sleeping bag", sleepingBag.GetDescription());
		}

		

		[TestMethod]
		public void GetDescriptionOfTrailMix()
		{
		    var trailMix = new TrailMix("Han-D Pack Trail Mix", 20M, 2, 1.2M);
		    Assert.AreEqual(true, trailMix.ContainsAllergens);
			Assert.AreEqual(false, trailMix.RequiresCooking);
			Assert.AreEqual("Han-D Pack Trail Mix" + Environment.NewLine +
				"Contains Allergens: True" + Environment.NewLine +
				"Requires Cooking: False" + Environment.NewLine +
				"Ingredients: Nuts, Oats, Honey, Dried Berries", trailMix.GetDescription());
		}

	    [TestMethod]
	    public void SleepingBag_ToString()
	    {
	        var sleepingBag = new SleepingBag("Wonderful Sleeping bag", 20M, 2, 1.2M);
	        Console.WriteLine(sleepingBag);
	        Assert.AreEqual(sleepingBag.ToString(), sleepingBag.GetDescription());
	    }
	    [TestMethod]
	    public void Description_Test()
	    {
	        var coffee = new Coffee(nameof(Coffee), 15M, 42, 0.5M);
	        Console.WriteLine(coffee);

            Assert.AreEqual(coffee.ToString(), coffee.GetDescription());
	    }

	    [TestMethod]
	    public void GetIdOfSleepingBag()
	    {
	        var sleepingBag = new SleepingBag("Nice sleeping bad", 20M, 1, 1M);
	        Assert.AreEqual(sleepingBag.Id, (int)ProductTypeEnum.SleepingBag);
	    }

	    [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
	    public void Product_Requires_Name_etc()
	    {
	        var sleepingBag = new SleepingBag(string.Empty, 20M, 0, 1M);
	        Assert.AreEqual(sleepingBag.Id, (int)ProductTypeEnum.SleepingBag);
	    }

	    [TestMethod]
	    [ExpectedException(typeof(ArgumentException))]
	    public void Product_Requires_Cost_etc()
	    {
	        new SleepingBag("Sleeping Bad", -1, 0, 1M);
	    }

	    [TestMethod]
	    [ExpectedException(typeof(ArgumentException))]
	    public void Product_Requires_Inventory_etc()
	    {
	         new SleepingBag("Sleeping Bad", 1.0M, 0, 1M);
	    }

	    [TestMethod]
	    [ExpectedException(typeof(ArgumentException))]
	    public void Product_Requires_Weight_etc()
	    {
	         new SleepingBag("Sleeping Bad", 1.0M, 1, 0);
	    }
    }
}
