using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ama.CodeChallenge.Store
{
	public class OnlineStore : IStore
	{
		private List<ShoppingCart> _carts;
		private ICatalog _catalog;

		public OnlineStore(ICatalog catalog)
		{
			_catalog = catalog;
		}

		/// <inheritdoc />
		public void AddItemToShoppingCart(string customerName, int productId, int count)
		{
			ShoppingCart cart = null;
			for (var i = 0; i < _carts.Count; ++i)
			{
				if (_carts[i].CustomerName == customerName)
				{
					cart = _carts[i];
					break;
				}
			}
			ShoppingCartItem shoppingCartItem = null;
			for (var i = 0; i < cart.Items.Count; ++i)
			{
				var item = cart.Items[i];
				if (item.ProductId == productId)
				{
					shoppingCartItem = item;
					break;
				}

			}

			if (shoppingCartItem == null)
			{
				shoppingCartItem = new ShoppingCartItem();
				shoppingCartItem.ProductId = productId;
				cart.Items.Add(shoppingCartItem);
			}

			var product = _catalog.GetProductById(productId);
			shoppingCartItem.Count = count;
			shoppingCartItem.Cost = count * product.Cost;
			product.RemoveInventory(count);
		}

		/// <inheritdoc />
		public decimal CheckoutShoppingCart(string customerName)
		{
			ShoppingCart cart = null;
			var i = -1;
			double total = 0;
			decimal weight = 0M;
			for (i = 0; i < _carts.Count; ++i)
			{
				if (_carts[i].CustomerName == customerName)
				{
					cart = _carts[i];
					break;
				}
			}
			for (i = 0; i < cart.Items.Count; ++i)
			{
				var item = cart.Items[i];
				if (item.ProductId == ProductIds.Tent)
				{
					if (item.Count >= 3)
					{
						total = total + ((double)item.Cost * 0.15);
					}
					else
					{
						total = total + (double)item.Cost;
					}
				}
				else
				{
					total = total + (double)item.Cost;
				}
			}

			for (i = 0; i < cart.Items.Count; ++i)
			{
				var item = cart.Items[i];
				var product = _catalog.GetProductById(item.ProductId);
				weight += item.Count * product.Weight;
			}

			return (decimal)total + 20; // Subtotal, plus shipping charge
		}

		/// <inheritdoc />
		public void CreateShoppingCart(string customerName)
		{
			_carts = new List<ShoppingCart>();
			_carts.Add(new ShoppingCart { CustomerName = customerName });
		}

		/// <inheritdoc />
		public int GetItemCountInCart(string customerName, int productId)
		{
			ShoppingCart cart = null;
			for (var i = 0; i < _carts.Count; ++i)
			{
				if (_carts[i].CustomerName == customerName)
				{
					cart = _carts[i];
					break;
				}
			}
			return cart.Items.Where(x => x.ProductId == productId).First().Count;
		}

		/// <inheritdoc />
		public void RemoveItemFromShoppingCart(string customerName, int productId, int count)
		{
			ShoppingCart cart = null;
			for (var i = 0; i < _carts.Count; ++i)
			{
				if (_carts[i].CustomerName == customerName)
				{
					cart = _carts[i];
					break;
				}
			}
			for (var i = 0; i < cart.Items.Count; ++i)
			{
				var item = cart.Items[i];
				if (item.ProductId == productId)
				{
					if (item.Count >= count)
					{
						item.Count = item.Count - count;
					}
					else
					{
						item.Count = 0;
					}
					var productInventory = _catalog.GetCurrentProductInventory(item.ProductId);
					_catalog.ModifyProductInventory(item.ProductId, count + productInventory);
				}
			}
		}
	}
}
