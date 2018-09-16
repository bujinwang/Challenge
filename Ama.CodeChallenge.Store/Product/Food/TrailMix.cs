namespace Ama.CodeChallenge.Store.Product.Food
{
    public class TrailMix : FoodBase
    {
        public TrailMix(string name, decimal cost, int initialInventory, decimal weight)
            : base(name, cost, initialInventory, weight)
        {
            ProductType = ProductTypeEnum.TrailMix;
            ContainsAllergens = true;
            Ingredients = "Nuts, Oats, Honey, Dried Berries";
        }
    }
}