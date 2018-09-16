namespace Ama.CodeChallenge.Store.Product.Food
{
    public class GranolaBar : FoodBase
    {
        public GranolaBar(string name, decimal cost, int initialInventory, decimal weight)
            : base(name, cost, initialInventory, weight)
        {
            ProductType = ProductTypeEnum.GranolaBar;
            Ingredients = "Granola, Dried Berries, Honey";
        }
    }
}