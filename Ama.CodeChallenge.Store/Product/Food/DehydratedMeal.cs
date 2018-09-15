namespace Ama.CodeChallenge.Store.Product.Food
{
    public class DehydratedMeal : FoodBase
    {
        public DehydratedMeal(string name, decimal cost, int initialInventory, decimal weight) 
            : base(name, cost, initialInventory, weight)
        {
            ProductType = ProductTypeEnum.DehydratedMeal;
            Cost = 12M;
            ContainsAllergens = true;
            Ingredients = "Noodles, Marinara Sauce, Ground Beef, Peanuts";
        }
    }
}