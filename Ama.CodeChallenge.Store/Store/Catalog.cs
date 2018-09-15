using System.Collections.Generic;
using System.Linq;
using Ama.CodeChallenge.Store.Product;
using Ama.CodeChallenge.Store.Product.Camping;
using Ama.CodeChallenge.Store.Product.Food;

namespace Ama.CodeChallenge.Store.Store
{
    public class Catalog : ICatalog
    {
        private readonly List<ProductBase> _products;

        public Catalog()
        {
            _products = new List<ProductBase>
            {
                new SleepingBag("Warming sleeping bag", 25M, 3, 0.67M),
                new Tent("Yellow Tent", 50M, 9, 2.5M),
                new Backpack("Amazing backpack", 30.0M, 7, 0.5M),
                new Stove("The wind proof stove", 40, 16, 1),
                new GranolaBar(nameof(GranolaBar), 3M, 30, 0.5M),
                new TrailMix(nameof(TrailMix), 7M, 19, 0.5M),
                new DehydratedMeal(nameof(DehydratedMeal), 12M, 30, 0.7M),
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
            return _products.Single(x => x.ProductType == productType);
        }

        /// <inheritdoc />
        public void ModifyProductInventory(ProductTypeEnum productType, int count)
        {
            foreach (var product in _products)
            {
                if (product.ProductType == productType)
                {
                    if (count < 0)
                    {
                        product.RemoveInventory(count);
                    }
                    else
                    {
                        product.AddInventory(count);
                    }
                }
            }
        }
    }
}