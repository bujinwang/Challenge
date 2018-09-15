using System.Collections.Generic;
using System.Linq;
using Ama.CodeChallenge.Store.Product;
using Ama.CodeChallenge.Store.ShoppingCart;

namespace Ama.CodeChallenge.Store.Store
{
    public class OnlineStore : IStore
    {
        // can be readonly as it's initialized inside constructor
        private readonly IInventory _inventory;
        private List<ShoppingCart.ShoppingCart> _carts;

        public OnlineStore(IInventory inventory)
        {
            _inventory = inventory;
        }

        /// <inheritdoc />
        public void AddItemToShoppingCart(string customerName, ProductTypeEnum productType, int count)
        {
            var cart = GetCustomerShoppingCart(customerName);

            var shoppingCartItem = GetCustomerShoppingCartItem(productType, cart);

            shoppingCartItem = CreateCustomerShoppingCartItem(productType, shoppingCartItem, cart);

            var product = _inventory.GetProductByType(productType);
            shoppingCartItem.Count = count;
            shoppingCartItem.Cost = count * product.Cost;
            _inventory.ModifyProductInventory(productType, -count);
        }

        /// <inheritdoc />
        public decimal CheckoutShoppingCart(string customerName)
        {
            ShoppingCart.ShoppingCart cart = GetCustomerShoppingCart(customerName);
            var i = -1;
            double total = 0;
            var weight = 0M;


            for (i = 0; i < cart.Items.Count; ++i)
            {
                var item = cart.Items[i];
                if (item.ProductId == (int) ProductTypeEnum.Tent)
                {
                    if (item.Count >= 3)
                        total = total + (double) item.Cost * 0.15;
                    else
                        total = total + (double) item.Cost;
                }
                else
                {
                    total = total + (double) item.Cost;
                }
            }

            for (i = 0; i < cart.Items.Count; ++i)
            {
                var item = cart.Items[i];
                var product = _inventory.GetProductByType((ProductTypeEnum) item.ProductId);
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
            ShoppingCart.ShoppingCart cart = GetCustomerShoppingCart(customerName);

            return cart.Items.Where(x => x.ProductId == (int) productType).First().Count;
        }

        /// <inheritdoc />
        public void RemoveItemFromShoppingCart(string customerName, ProductTypeEnum productType, int count)
        {
            ShoppingCart.ShoppingCart cart = GetCustomerShoppingCart(customerName);

            for (var i = 0; i < cart.Items.Count; ++i)
            {
                var item = cart.Items[i];
                if (item.ProductId == (int) productType)
                {
                    if (item.Count >= count)
                        item.Count = item.Count - count;
                    else
                        item.Count = 0;

                    //                    var productInventory = _inventory.GetCurrentProductInventory((ProductTypeEnum) item.ProductId);
                    // defect fixed, modify takes the changing count, not the total amount
                    _inventory.ModifyProductInventory((ProductTypeEnum) item.ProductId, count);
                }
            }
        }

        private static ShoppingCartItem CreateCustomerShoppingCartItem(ProductTypeEnum productType,
            ShoppingCartItem shoppingCartItem, ShoppingCart.ShoppingCart cart)
        {
            if (shoppingCartItem == null)
            {
                shoppingCartItem = new ShoppingCartItem();
                shoppingCartItem.ProductId = (int) productType;
                cart.Items.Add(shoppingCartItem);
            }

            return shoppingCartItem;
        }

        private static ShoppingCartItem GetCustomerShoppingCartItem(ProductTypeEnum productType,
            ShoppingCart.ShoppingCart cart)
        {
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

            return shoppingCartItem;
        }

        private ShoppingCart.ShoppingCart GetCustomerShoppingCart(string customerName)
        {
            ShoppingCart.ShoppingCart cart = null;
            for (var i = 0; i < _carts.Count; ++i)
                if (_carts[i].CustomerName == customerName)
                {
                    cart = _carts[i];
                    break;
                }

            return cart;
        }

        public int CheckInventory(ProductTypeEnum productTypeEnum)
        {
            return _inventory.GetCurrentProductInventory(productTypeEnum);
        }
    }
}