﻿using System;

namespace Ama.CodeChallenge.Store.Product.Food
{
    public abstract class FoodBase : ProductBase
    {
        protected FoodBase(string name, decimal cost, int initialInventory, decimal weight)
            : base(name, cost, initialInventory, weight)
        {
        }

        public bool ContainsAllergens { get; set; }
        public bool RequiresCooking { get; set; }
        public string Ingredients { get; set; }

        /// <summary>
        ///     Returns the name of the Product, whether it contains allergens, whether it requires cooking,
        ///     and the list of ingredients.
        /// </summary>
        /// <returns></returns>
        public override string GetDescription()
        {
            return Name + Environment.NewLine +
                   "Contains Allergens: " + ContainsAllergens + //Bug: use contains allergens in
                   Environment.NewLine +
                   "Requires Cooking: " + RequiresCooking + Environment.NewLine +
                   "Ingredients: " + Ingredients;
        }
    }
}