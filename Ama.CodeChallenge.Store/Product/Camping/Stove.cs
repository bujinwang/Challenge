namespace Ama.CodeChallenge.Store.Product.Camping
{
    public class Stove : ProductBase
    {
        public Stove(string name, decimal cost, int initialInventory, decimal weight) 
            : base(name, cost, initialInventory, weight)
        {
            ProductType = ProductTypeEnum.Stove;
        }

        public override string GetDescription()
        {
            return Name + ": Enjoy a hot meal out in the wilderness";
        }
    }
}