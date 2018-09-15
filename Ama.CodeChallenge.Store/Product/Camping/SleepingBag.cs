namespace Ama.CodeChallenge.Store.Product.Camping
{
    public class SleepingBag : ProductBase
    {
        public SleepingBag(string name, decimal cost, int initialInventory, decimal weight) 
            : base(name, cost, initialInventory, weight)
        {
            ProductType = ProductTypeEnum.SleepingBag;
           
        }

        public override string GetDescription()
        {
            return Name + ": Soft and fluffy sleeping bag";
        }
    }
}