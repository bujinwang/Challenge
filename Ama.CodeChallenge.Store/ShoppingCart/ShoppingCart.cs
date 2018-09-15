using System.Collections.Generic;

namespace Ama.CodeChallenge.Store.ShoppingCart
{
    public class ShoppingCart
    {
        public ShoppingCart()
        {
            Items = new List<ShoppingCartItem>();
        }

        public string CustomerName { get; set; }
        public List<ShoppingCartItem> Items { get; set; }
        public decimal SubTotal { get; set; }
        public decimal ShippingFees { get; set; }

        public decimal Total
        {
            get => SubTotal + ShippingFees;
            set => throw new System.NotImplementedException();
        }

        public decimal Discounts { get; set; }
    }
}