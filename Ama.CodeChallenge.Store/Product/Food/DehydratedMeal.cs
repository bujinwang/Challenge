namespace Ama.CodeChallenge.Store.Product.Food
{
    public class DehydratedMeal : FoodBase
    {
        public DehydratedMeal(string name, decimal cost, int initialInventory, decimal weight) 
            : base(name, cost, initialInventory, weight)
        {
            ProductType = ProductTypeEnum.DehydratedMeal;
            ContainsAllergens = true;
            Ingredients = "Noodles, Marinara Sauce, Ground Beef, Peanuts";
        }
    }
}