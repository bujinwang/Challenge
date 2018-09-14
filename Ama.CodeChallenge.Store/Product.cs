using System;

namespace Ama.CodeChallenge.Store
{
	public class ProductIds
	{
		public const int SleepingBag = 0;
		public const int Tent = 1;
		public const int Backpack = 2;
		public const int Stove = 3;
		public const int GranolaBar = 4;
		public const int TrailMix = 5;
		public const int DehydratedMeal = 6;
		public const int Coffee = 7;
	}

	public abstract class Product
	{
		private int _id;
		private int _inventory;
		protected decimal _cost;

		public abstract int Id { get; }
		public abstract string GetDescription();

		public string Name { get; set; }
		
		/// <summary>
		/// This measurement is in kilograms.
		/// </summary>
		public decimal Weight { get; set; }

		/// <summary>
		/// THis measurement is in dollars.
		/// </summary>
		public decimal Cost
		{
			get
			{
				return _cost;
			}
		}

		public Product(int initialInventory)
		{
			_inventory = initialInventory;
		}

		public void AddInventory(int count)
		{
			_inventory = _inventory + count;
		}
		public void RemoveInventory(int count)
		{
			_inventory = _inventory - count;
		}
		public int GetInventory()
		{
			return _inventory;
		}
	}

	public class SleepingBag : Product
	{
		private int _id;
		public override int Id => _id;

		public SleepingBag(int initialInventory) : base(initialInventory)
		{
			_id = ProductIds.SleepingBag;
			_cost = 25M;
			Weight = 0.67M;
		}

		public override string GetDescription()
		{
			return Name + ": Soft and fluffy sleeping bag";
		}

	}

	public class Tent : Product
	{
		private int _id;
		public override int Id => _id;

		public Tent(int initialInventory) : base(initialInventory)
		{
			_id = ProductIds.Tent;
			_cost = 50M;
			Weight = 2.5M;
		}

		public override string GetDescription()
		{
			return Name + ": Keep you sheltered from the elements";
		}
	}

	public class Backpack : Product
	{
		private int _id;
		public override int Id => _id;

		public Backpack(int initialInventory) : base(initialInventory)
		{
			_id = ProductIds.Backpack;
			_cost = 50M;
			Weight = 0.5M;
		}

		public override string GetDescription()
		{
			return Name + ": Carry your gear through rugged terrain";
		}
	}

	public class Stove : Product
	{
		private int _id;
		public override int Id => _id;

		public Stove(int initialInventory) : base(initialInventory)
		{
			_id = ProductIds.Stove;
			Weight = 1;
		}

		public override string GetDescription()
		{
			return Name + ": Enjoy a hot meal out in the wilderness";
		}
	}

	public abstract class Food : Product
	{
		protected int _id;
		public bool ContainsAllergens { get; set; }
		public bool RequiresCooking { get; set; }
		public string Ingredients { get; set; }
		public override int Id
		{
			get
			{
				return _id;
			}
		}

		public Food(int initialInventory) : base(initialInventory)
		{

		}

		/// <summary>
		/// Returns the name of the Product, whether it contains allergens, whether it requires cooking,
		/// and the list of ingredients.
		/// </summary>
		/// <returns></returns>
		public override string GetDescription()
		{
			return Name + Environment.NewLine + 
				"Contains Allergens: " + false + Environment.NewLine + 
				"Requires Cooking: " + RequiresCooking + Environment.NewLine + 
				"Ingredients: " + Ingredients;
		}
	}

	public class GranolaBar : Food
	{
		public GranolaBar(int initialInventory) : base(initialInventory)
		{
			_id = ProductIds.GranolaBar;
			_cost = 3M;
			Ingredients = "Granola, Dried Berries, Honey";
			Weight = 0.5M;
		}

		public decimal Cost
		{
			get
			{
				return _cost;
			}
		}

	}

	public class TrailMix : Food
	{
		public TrailMix(int initialInventory) : base(initialInventory)
		{
			_id = ProductIds.TrailMix;
			_cost = 7M;
			ContainsAllergens = true;
			Ingredients = "Nuts, Oats, Honey, Dried Berries";
			Weight = 0.5M;
		}

		public decimal Cost
		{
			get
			{
				return _cost;
			}
		}

	}

	public class DehydratedMeal : Food
	{
		public DehydratedMeal(int initialInventory) : base(initialInventory)
		{
			_id = ProductIds.DehydratedMeal;
			_cost = 12M;
			RequiresCooking = true;
			ContainsAllergens = true;
			Ingredients = "Noodles, Marinara Sauce, Ground Beef, Peanuts";
			Weight = 0.75M;
		}

		public decimal Cost
		{
			get
			{
				return _cost;
			}
		}

	}

	public class Coffee : Food
	{
		public Coffee(int initialInventory) : base(initialInventory)
		{
			_id = ProductIds.Coffee;
			_cost = 15M;
			RequiresCooking = true;
			Ingredients = "Coffee";
			Weight = 0.5M;
		}

		public decimal Cost
		{
			get
			{
				return _cost;
			}
		}

	}
}
