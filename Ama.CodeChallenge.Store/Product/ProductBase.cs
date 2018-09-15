using System;

namespace Ama.CodeChallenge.Store.Product
{
    public abstract class ProductBase
    {
        private readonly object _inventoryLock = new object();
        private int _id;
        private ProductTypeEnum _productType;
        private int _inventory; // don't use auto property as it does not support lock for multiple thread

        /// <summary>
        /// I would like to retire using the int ID as the product type
        /// </summary>
        
        public int Id
        {
            get => _id;
            set => _id = value;
        }

        public ProductTypeEnum ProductType
        {
            get => _productType;

            set
            {
                _productType = value;

                _id = (int) value;
            }
        }

        public abstract string GetDescription();

        public string Name { get; set; }

        /// <summary>
        /// This measurement is in kilograms.
        /// </summary>
        public decimal Weight { get; set; }

        /// <summary>
        /// THis measurement is in dollars.
        /// </summary>
        public decimal Cost { get; protected set; }

        /// <summary>
        /// Every product has a name, cost, inventory and weight. Enforce the product creation with these required fields
        /// make the class much more reusable
        /// </summary>
        /// <param name="name">The name of the product</param>
        /// <param name="cost">the required cost</param>
        /// <param name="initialInventory">the inventory</param>
        /// <param name="weight">the weight</param>
        public ProductBase(string name, decimal cost, int initialInventory, decimal weight)
        {
            //name can't be null or empty
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("can't be null or empty", nameof(name));
            }

            // product cost has to be > 0
            if (cost <= 0)
            {
                throw new ArgumentException("can't be less than 0", nameof(cost));
            }

            // weight has to > 0
            if (weight <= 0)
            {
                throw new ArgumentException("can't be less than 0", nameof(weight));
            }

            if (initialInventory < 0)
            {
                throw new ArgumentException("can't be less than 0", nameof(initialInventory));
            }

            Name = name;
            Cost = cost;
            _inventory = initialInventory;
            Weight = weight;
        }

        public void AddInventory(int count)
        {
            lock (_inventoryLock)
            {
                _inventory = _inventory + count;
            }
        }

        public void RemoveInventory(int count)
        {
            lock (_inventoryLock)
            {
                _inventory = _inventory - count;
            }
        }

        public int GetInventory()
        {
            return _inventory;
        }
    }
}