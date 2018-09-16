namespace Ama.CodeChallenge.Store.ShoppingCart
{
    /// <summary>
    ///     This class cannot change.
    /// </summary>
    public class ShoppingCartItem
    {
        public int ProductId { get; set; }
        public int Count { get; set; }
        public decimal Cost { get; set; }
    }
}