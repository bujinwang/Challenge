namespace Ama.CodeChallenge.Store.Product.Food
{
    public class Coffee : FoodBase
    {
        public Coffee(string name, decimal cost, int initialInventory, decimal weight)
            : base(name, cost, initialInventory, weight)
        {
            ProductType = ProductTypeEnum.Coffee;
            RequiresCooking = true;
            Ingredients = "Coffee";
        }
    }
}