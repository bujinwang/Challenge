using System;
using System.Collections.Generic;
using System.Linq;

namespace Ama.CodeChallenge.Store
{
	public interface ICatalog
	{
		/// <summary>
		/// Return a Product from the Catalog.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		Product GetProductById(int id);

		/// <summary>
		/// Adjust the inventory of a product in the catalog.
		/// </summary>
		/// <param name="id"></param>
		/// <param name="count">A negative number will remove inventory. A positive number will add inventory.</param>
		void ModifyProductInventory(int id, int count);

		/// <summary>
		/// Return the current Inventory of a Product from the Catalog.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		int GetCurrentProductInventory(int id);
	}

	public class Catalog : ICatalog
	{
		private List<Product> _products;
		public Catalog()
		{
			_products = new List<Product>();
			_products.Add(new SleepingBag(7));
			_products.Add(new Tent(9));
			_products.Add(new Backpack(7));
			_products.Add(new Stove(16));
			_products.Add(new GranolaBar(3));
			_products.Add(new TrailMix(19));
			_products.Add(new DehydratedMeal(30));
			_products.Add(new Coffee(42));
		}

		/// <inheritdoc />
		public int GetCurrentProductInventory(int id)
		{
			return GetProductById(id).GetInventory();
		}

		/// <inheritdoc />
		public Product GetProductById(int id)
		{
			return _products.Single(x => x.Id == 1);
		}

		/// <inheritdoc />
		public void ModifyProductInventory(int id, int count)
		{
			foreach (var product in _products)
			{
				if (product.Id == id)
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
