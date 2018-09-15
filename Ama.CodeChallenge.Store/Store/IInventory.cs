using Ama.CodeChallenge.Store.Product;

namespace Ama.CodeChallenge.Store.Store
{
    public interface IInventory
    {
        /// <summary>
        /// Return a Product from the Catalog.
        /// </summary>
        /// <param name="productType"></param>
        /// <returns></returns>
        ProductBase GetProductByType(ProductTypeEnum productType);

        /// <summary>
        /// Adjust the inventory of a product in the catalog.
        /// </summary>
        /// <param name="productType">the product type enum</param>
        /// <param name="count">A negative number will remove inventory. A positive number will add inventory.</param>
        void ModifyProductInventory(ProductTypeEnum productType, int count);

        /// <summary>
        /// Return the current Inventory of a Product from the Catalog.
        /// </summary>
        /// <param name="productType">the product type enum</param>
        /// <returns></returns>
        int GetCurrentProductInventory(ProductTypeEnum productType);
    }
}