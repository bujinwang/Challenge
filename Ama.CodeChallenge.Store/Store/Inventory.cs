using System;
using System.Collections.Generic;
using System.Linq;
using Ama.CodeChallenge.Store.Product;
using Ama.CodeChallenge.Store.Product.Camping;
using Ama.CodeChallenge.Store.Product.Food;

namespace Ama.CodeChallenge.Store.Store
{
    public class Inventory : IInventory
    {
        private static List<ProductBase> _products;

        public Inventory()
        {
            _products = new List<ProductBase>
            {
                new SleepingBag("Warming sleeping bag", 25M, 3, 0.67M),
                new Tent("Yellow Tent", 50M, 9, 2.5M),
                new Backpack("Amazing backpack", 30.0M, 7, 0.5M),
                new Stove("The wind proof stove", 40M, 16, 1M),
                new GranolaBar(nameof(GranolaBar), 3M, 30, 0.5M),
                new TrailMix(nameof(TrailMix), 7M, 19, 0.5M),
                new DehydratedMeal(nameof(DehydratedMeal), 5M, 30, 1M),
                new Coffee(nameof(Coffee), 15M, 42, 0.5M)
            };
        }

        /// <inheritdoc />
        public int GetCurrentProductInventory(ProductTypeEnum productType)
        {
            return GetProductByType(productType).GetInventory();
        }

        /// <inheritdoc />
        public ProductBase GetProductByType(ProductTypeEnum productType)
        {
            lock (_products)
            {
                return _products.Single(x => x.ProductType == productType);
            }
        }

        /// <inheritdoc />
        public void ModifyProductInventory(ProductTypeEnum productType, int count)
        {
            lock (_products)
            {
                foreach (var product in _products)
                {
                    if (product.ProductType == productType)
                    {
                        if (count < 0)
                        {
                            product.RemoveInventory(Math.Abs(count));
                        }
                        else
                        {
                            product.AddInventory(Math.Abs(count));
                        }
                    }
                }
            }
        }

        public static int GetTotalProducts()
        {
            lock (_products)
            {
                int total = 0;
                foreach (var product in _products)
                {
                    total += product.GetInventory();
                }

                return total;
            }
        }
    }
}