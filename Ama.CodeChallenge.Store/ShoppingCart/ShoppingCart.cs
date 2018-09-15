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
        public int ShippingFees { get; set; }

        public decimal Total
        {
            get { return SubTotal + ShippingFees; }
        }
    }
}