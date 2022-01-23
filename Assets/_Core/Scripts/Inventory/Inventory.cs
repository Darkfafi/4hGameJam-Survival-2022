public class Inventory
{
	public ItemSlot Gold
	{
		get; private set;
	}
	
	public ItemSlot Food
	{
		get; private set;
	}

	public ItemSlot Tools
	{
		get; private set;
	}

	public ItemSlot[] InventorySlots = null;

	public Inventory(int gold, int food, int tools, int? foodBuyCost, int? toolsBuyCost)
	{
		InventorySlots = new ItemSlot[]
		{
			Gold = new ItemSlot(gold),
			Food = new ItemSlot(food, foodBuyCost),
			Tools = new ItemSlot(tools, toolsBuyCost),
		};

		for(int i = 0; i < InventorySlots.Length; i++)
		{
			InventorySlots[i].BuyRequestedEvent += OnBuyRequested;
		}
	}

	private void OnBuyRequested(ItemSlot slot)
	{
		if (Gold.TrySpend(slot.BuyCost.Value))
		{
			slot.Add(1);
		}
	}
}
