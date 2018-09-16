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
        protected internal const int DefaultShippingCharge = 20;
        protected internal const double ThreeTentsDiscountPercentage = 0.85;

        // can be readonly as it's initialized inside constructor
        private readonly ICatalog _catalog;
        private List<ShoppingCart.ShoppingCart> _carts;

        public OnlineStore(ICatalog catalog)
        {
            _catalog = catalog;
        }

        /// <inheritdoc />
        public void AddItemToShoppingCart(string customerName, ProductTypeEnum productType, int count)
        {
            var cart = GetCustomerShoppingCart(customerName);

            var shoppingCartItem = GetCustomerShoppingCartItem(productType, cart);

            shoppingCartItem = shoppingCartItem ?? CreateCustomerShoppingCartItem(productType, cart);

            var product = _catalog.GetProductByType(productType);
            shoppingCartItem.Count += count;
            shoppingCartItem.Cost = shoppingCartItem.Count * product.Cost;
            _catalog.ModifyProductInventory(productType, -count);
        }

        /// <inheritdoc />
        public decimal CheckoutShoppingCart(string customerName)
        {
            var cart = GetCustomerShoppingCart(customerName);
            double total = 0;
            var weight = 0M;
            var fiveTentsDiscount = false;

            foreach (var item in cart.Items)
            {
                fiveTentsDiscount =
                    fiveTentsDiscount || item.ProductId == (int) ProductTypeEnum.Tent && item.Count >= 5;
                var threeTentsDiscount =
                    item.ProductId == (int) ProductTypeEnum.Tent && item.Count >= 3 && item.Count < 5;

                total += threeTentsDiscount ? (double) item.Cost * ThreeTentsDiscountPercentage : (double) item.Cost;

                // calculate weight
                var product = _catalog.GetProductByType((ProductTypeEnum) item.ProductId);
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

        /// <inheritdoc />
        public void RemoveItemFromShoppingCart(string customerName, ProductTypeEnum productType, int count)
        {
            var cart = GetCustomerShoppingCart(customerName);

            for (var i = 0; i < cart.Items.Count; ++i)
            {
                var item = cart.Items[i];
                if (item.ProductId == (int) productType)
                {
                    if (item.Count >= count)
                    {
                        item.Count = item.Count - count;
                        // defect fixed, modify takes the changing count, not the total amount
                        _catalog.ModifyProductInventory((ProductTypeEnum) item.ProductId, count);
                    }
                    else
                    {
                        throw new InvalidOperationException(
                            $"Remove count{count} is more than the existing count {item.Count} from shopping cart");
                    }
                }
            }
        }

        public void DropShoppingCart(string customerName)
        {
            var cart = GetCustomerShoppingCart(customerName);
            foreach (var shoppingCartItem in cart.Items)
            {
                var productType = (ProductTypeEnum) shoppingCartItem.ProductId;
                var count = shoppingCartItem.Count;
                shoppingCartItem.Count -= count;
                shoppingCartItem.Cost = 0.0M;
                _catalog.ModifyProductInventory(productType, count);
            }

            cart.Items.Clear();
        }


        public ShoppingCart.ShoppingCart CheckoutShoppingCart(string customerName, bool detialed)
        {
            var cart = GetCustomerShoppingCart(customerName);
            var weight = 0M;
            var fiveTentsDiscount = false;

            var shoppingCart = new ShoppingCart.ShoppingCart {Items = cart.Items};

            foreach (var item in shoppingCart.Items)
            {
                fiveTentsDiscount =
                    fiveTentsDiscount || item.ProductId == (int) ProductTypeEnum.Tent && item.Count >= 5;
                var threeTentsDiscount =
                    item.ProductId == (int) ProductTypeEnum.Tent && item.Count >= 3 && item.Count < 5;

                shoppingCart.SubTotal += (decimal) (threeTentsDiscount
                    ? (double) item.Cost * ThreeTentsDiscountPercentage
                    : (double) item.Cost);
                shoppingCart.Discounts += threeTentsDiscount ? item.Cost * (decimal) 0.15 : 0;

                // calculate weight
                var product = _catalog.GetProductByType((ProductTypeEnum) item.ProductId);
                weight += item.Count * product.Weight;
            }

            // apply 5 tent discount, applies to all items
            shoppingCart.Discounts +=
                shoppingCart.SubTotal * (decimal) (fiveTentsDiscount ? 1 - FiveTentsDiscountAmount : 0D);

            shoppingCart.SubTotal *= (decimal) (fiveTentsDiscount ? FiveTentsDiscountAmount : 1.0D);

            //$20 shipping charge is < $200
            shoppingCart.ShippingFees += shoppingCart.SubTotal < ShippingChargeThreshHold ? DefaultShippingCharge : 0;

            //10kg $25 overweight shipping charge
            shoppingCart.ShippingFees += weight > OverWeightThreadHold ? OverWeightShippingCharge : 0;

            return shoppingCart;
        }

        public ShoppingCartItem GetItemInCart(string customerName, ProductTypeEnum productType)
        {
            ShoppingCartItem ret = null;

            var cart = GetCustomerShoppingCart(customerName);
            if (cart != null)
                ret = cart.Items.FirstOrDefault(x => x.ProductId == (int) productType);
            return ret;
        }

        private static ShoppingCartItem CreateCustomerShoppingCartItem(ProductTypeEnum productType,
            ShoppingCart.ShoppingCart cart)
        {
            var shoppingCartItem = new ShoppingCartItem();
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
                if (_carts[i].CustomerName == customerName)
                {
                    cart = _carts[i];
                    break;
                }

            return cart;
        }

        public int CheckInventory(ProductTypeEnum productTypeEnum)
        {
            return _catalog.GetCurrentProductInventory(productTypeEnum);
        }
    }
}