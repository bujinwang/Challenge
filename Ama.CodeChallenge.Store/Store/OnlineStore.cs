using System.Collections.Generic;
using System.Linq;
using Ama.CodeChallenge.Store.Product;
using Ama.CodeChallenge.Store.ShoppingCart;

namespace Ama.CodeChallenge.Store.Store
{
    public class OnlineStore : IStore
    {
        private List<ShoppingCart.ShoppingCart> _carts;
        // can be readonly as it's initialized inside constructor
        private readonly ICatalog _catalog;

        public OnlineStore(ICatalog catalog)
        {
            _catalog = catalog;
        }

        /// <inheritdoc />
        public void AddItemToShoppingCart(string customerName, ProductTypeEnum productType, int count)
        {
            ShoppingCart.ShoppingCart cart = null;
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
                if (item.ProductId == (int) productType)
                {
                    shoppingCartItem = item;
                    break;
                }
            }

            if (shoppingCartItem == null)
            {
                shoppingCartItem = new ShoppingCartItem();
                shoppingCartItem.ProductId = (int) productType;
                cart.Items.Add(shoppingCartItem);
            }

            var product = _catalog.GetProductByType(productType);
            shoppingCartItem.Count = count;
            shoppingCartItem.Cost = count * product.Cost;
            product.RemoveInventory(count);
        }

        /// <inheritdoc />
        public decimal CheckoutShoppingCart(string customerName)
        {
            ShoppingCart.ShoppingCart cart = null;
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
                if (item.ProductId == (int) ProductTypeEnum.Tent)
                {
                    if (item.Count >= 3)
                    {
                        total = total + ((double) item.Cost * 0.15);
                    }
                    else
                    {
                        total = total + (double) item.Cost;
                    }
                }
                else
                {
                    total = total + (double) item.Cost;
                }
            }

            for (i = 0; i < cart.Items.Count; ++i)
            {
                var item = cart.Items[i];
                var product = _catalog.GetProductByType((ProductTypeEnum) item.ProductId);
                weight += item.Count * product.Weight;
            }

            return (decimal) total + 20; // Subtotal, plus shipping charge
        }

        /// <inheritdoc />
        public void CreateShoppingCart(string customerName)
        {
            _carts = new List<ShoppingCart.ShoppingCart> {new ShoppingCart.ShoppingCart {CustomerName = customerName}};
        }

        /// <inheritdoc />
        public int GetItemCountInCart(string customerName, ProductTypeEnum productType)
        {
            ShoppingCart.ShoppingCart cart = null;
            for (var i = 0; i < _carts.Count; ++i)
            {
                if (_carts[i].CustomerName == customerName)
                {
                    cart = _carts[i];
                    break;
                }
            }

            return cart.Items.Where(x => x.ProductId == (int) productType).First().Count;
        }

        /// <inheritdoc />
        public void RemoveItemFromShoppingCart(string customerName, ProductTypeEnum productType, int count)
        {
            ShoppingCart.ShoppingCart cart = null;
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
                if (item.ProductId == (int) productType)
                {
                    if (item.Count >= count)
                    {
                        item.Count = item.Count - count;
                    }
                    else
                    {
                        item.Count = 0;
                    }

                    var productInventory = _catalog.GetCurrentProductInventory((ProductTypeEnum) item.ProductId);
                    _catalog.ModifyProductInventory((ProductTypeEnum) item.ProductId, count + productInventory);
                }
            }
        }
    }
}