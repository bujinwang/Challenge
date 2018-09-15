using Ama.CodeChallenge.Store.Product;

namespace Ama.CodeChallenge.Store.Store
{

    public interface IStore 
    {
        /// <summary>
        /// This method will create a new shopping cart for a customer.
        /// </summary>
        /// <param name="customerName"></param>
        void CreateShoppingCart(string customerName);

        /// <summary>
        /// This method will add an item to the customer's shopping cart and return it to the inventory. 
        /// If the shopping cart does not already exist, ana error should be thrown.
        /// </summary>
        /// <param name="customerName"></param>
        /// <param name="productId"></param>
        /// <param name="count"></param>
        void AddItemToShoppingCart(string customerName, ProductTypeEnum productId, int count);

        /// <summary>
        /// This method will remove an item from the customer's shopping cart and return it to the inventory.
        /// If the shopping cart does not already exist, ana error should be thrown.
        /// </summary>
        /// <param name="customerName"></param>
        /// <param name="productId"></param>
        /// <param name="count"></param>
        void RemoveItemFromShoppingCart(string customerName, ProductTypeEnum productId, int count);

        /// <summary>
        /// This method will return the total cost of the customer's order.
        /// If the shopping cart does not already exist, ana error should be thrown.
        /// </summary>
        /// <param name="customerName"></param>
        /// <returns></returns>
        decimal CheckoutShoppingCart(string customerName);

        /// <summary>
        /// Return the number of an item in the cart. If the productId doesn't exist in the 
        /// Catalog, throw an error. If the item is not in the cart, return 0.
        /// If the shopping cart does not already exist, ana error should be thrown.
        /// </summary>
        /// <param name="customerName"></param>
        /// <param name="productType"></param>
        /// <returns></returns>
        int GetItemCountInCart(string customerName, ProductTypeEnum productType);

        // Implement empty cart
    }
}