using System;
using System.Collections.Generic;
using System.Linq;
using Ama.CodeChallenge.Store.Product;
using Ama.CodeChallenge.Store.ShoppingCart;

namespace Ama.CodeChallenge.Store.Store
{
    public class OnlineStore : IStore
    {
        protected internal const int OverWeightShippingCharge = 25;
        protected internal const double FiveTentsDiscountAmount = 0.85D;
        protected internal const int ShippingChargeThreshHold = 200;
        protected internal const int OverWeightThreadHold = 10;

        // can be readonly as it's initialized inside constructor
        private readonly IInventory _inventory;
        private List<ShoppingCart.ShoppingCart> _carts;
        protected internal const int DefaultShippingCharge = 20;

        public OnlineStore(IInventory inventory)
        {
            _inventory = inventory;
        }

        /// <inheritdoc />
        public void AddItemToShoppingCart(string customerName, ProductTypeEnum productType, int count)
        {
            var cart = GetCustomerShoppingCart(customerName);

            var shoppingCartItem = GetCustomerShoppingCartItem(productType, cart);

            if (shoppingCartItem == null)
            {
                shoppingCartItem = CreateCustomerShoppingCartItem(productType, cart);
            }

            var product = _inventory.GetProductByType(productType);
            shoppingCartItem.Count += count;
            shoppingCartItem.Cost = shoppingCartItem.Count * product.Cost;
            _inventory.ModifyProductInventory(productType, -count);
        }

        /// <inheritdoc />
        public decimal CheckoutShoppingCart(string customerName)
        {
            ShoppingCart.ShoppingCart cart = GetCustomerShoppingCart(customerName);
            double total = 0;
            var weight = 0M;
            var fiveTentsDiscount = false;

            foreach (var item in cart.Items)
            {
                fiveTentsDiscount =
                    fiveTentsDiscount || item.ProductId == (int) ProductTypeEnum.Tent && item.Count >= 5;
                bool threeTentsDiscount =
                    item.ProductId == (int) ProductTypeEnum.Tent && item.Count >= 3 && item.Count < 5;

                total += threeTentsDiscount ? (double) item.Cost * 0.85 : (double) item.Cost;

                // calculate weight
                var product = _inventory.GetProductByType((ProductTypeEnum) item.ProductId);
                weight += item.Count * product.Weight;
            }

            // apply 5 tent discount, applies to all items
            total *= fiveTentsDiscount ? FiveTentsDiscountAmount : 1.0D;

            //$20 shipping charge is < $200
            total += total < ShippingChargeThreshHold ? DefaultShippingCharge : 0;

            //10kg $25 overweight shipping charge
            total += weight > OverWeightThreadHold ? OverWeightShippingCharge : 0;

            return (decimal) total; // Subtotal
        }


        public decimal CheckoutShoppingCart(string customerName, bool detialed)
        {
            return 0;
        }

        /// <inheritdoc />
        public void CreateShoppingCart(string customerName)
        {
            _carts = new List<ShoppingCart.ShoppingCart> {new ShoppingCart.ShoppingCart {CustomerName = customerName}};
        }

        /// <inheritdoc />
        public int GetItemCountInCart(string customerName, ProductTypeEnum productType)
        {
            var count = 0;
            var item = GetItemInCart(customerName, productType);
            if (item != null)
                count = item.Count;
            return count;
        }

        public ShoppingCartItem GetItemInCart(string customerName, ProductTypeEnum productType)
        {
            ShoppingCartItem ret = null;

            var cart = GetCustomerShoppingCart(customerName);
            if (cart != null)
                ret = cart.Items.FirstOrDefault(x => x.ProductId == (int) productType);
            return ret;
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
                    {
                        item.Count = item.Count - count;
                        // defect fixed, modify takes the changing count, not the total amount
                        _inventory.ModifyProductInventory((ProductTypeEnum) item.ProductId, count);
                    }
                    else
                    {
                        throw new InvalidOperationException(
                            $"Remove count{count} is more than the existing count {item.Count} from shopping cart");
                    }
                }
            }
        }

        private static ShoppingCartItem CreateCustomerShoppingCartItem(ProductTypeEnum productType,
            ShoppingCart.ShoppingCart cart)
        {
            ShoppingCartItem shoppingCartItem = new ShoppingCartItem();
            shoppingCartItem.ProductId = (int) productType;
            cart.Items.Add(shoppingCartItem);

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
            {
                if (_carts[i].CustomerName == customerName)
                {
                    cart = _carts[i];
                    break;
                }
            }

            return cart;
        }

        public int CheckInventory(ProductTypeEnum productTypeEnum)
        {
            return _inventory.GetCurrentProductInventory(productTypeEnum);
        }

        public void DropShoppingCart(string customerName)
        {
            var cart = GetCustomerShoppingCart(customerName);
            foreach (var shoppingCartItem in cart.Items)
            {
                var productType = (ProductTypeEnum) shoppingCartItem.ProductId;
                int count = shoppingCartItem.Count;
                shoppingCartItem.Count -= count;
                shoppingCartItem.Cost = 0.0M;
                _inventory.ModifyProductInventory(productType, count);
            }

            cart.Items.Clear();
        }
    }
}