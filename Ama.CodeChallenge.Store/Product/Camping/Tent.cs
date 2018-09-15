namespace Ama.CodeChallenge.Store.Product.Camping
{
    public class Tent : ProductBase
    {
        public Tent(string name, decimal cost, int initialInventory, decimal weight) 
            : base(name, cost, initialInventory, weight)
        {
            ProductType = ProductTypeEnum.Tent;
        }

        public override string GetDescription()
        {
            return Name + ": Keep you sheltered from the elements";
        }
    }
}