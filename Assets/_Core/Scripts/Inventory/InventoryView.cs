using UnityEngine;

public class InventoryView : MonoBehaviour
{
	[SerializeField]
	private ItemSlotView _goldSlotView = null;

	[SerializeField]
	private ItemSlotView _foodSlotView = null;

	[SerializeField]
	private ItemSlotView _toolsSlotView = null;

	public bool IsInitialized => Inventory != null;

	public Inventory Inventory
	{
		get; private set;
	}

	public void Initialize(Inventory inventory)
	{
		if (IsInitialized)
		{
			return;
		}

		Inventory = inventory;

		_goldSlotView.SetItemSlot(inventory.Gold);
		_foodSlotView.SetItemSlot(inventory.Food);
		_toolsSlotView.SetItemSlot(inventory.Tools);
	}
}
