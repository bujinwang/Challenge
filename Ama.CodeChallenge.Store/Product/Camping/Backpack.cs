namespace Ama.CodeChallenge.Store.Product.Camping
{
    public class Backpack : ProductBase
    {
        public Backpack(string name, decimal cost, int initialInventory, decimal weight)
            : base(name, cost, initialInventory, weight)
        {
            ProductType = ProductTypeEnum.Backpack;
        }

        public override string GetDescription()
        {
            return Name + ": Carry your gear through rugged terrain";
        }
    }
}