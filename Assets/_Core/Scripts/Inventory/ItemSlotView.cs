using TMPro;
using UnityEngine;

public class ItemSlotView : MonoBehaviour
{
	[SerializeField]
	private TextMeshProUGUI _amountLabel = null;

	public ItemSlot ItemSlot
	{
		get; private set;
	}

	public void SetItemSlot(ItemSlot itemSlot)
	{
		if(ItemSlot != null)
		{
			ItemSlot.ValueChangedEvent -= OnValueChanged;
		}

		ItemSlot = itemSlot;
		ItemSlot.ValueChangedEvent += OnValueChanged;
		OnValueChanged();
	}

	protected void OnDestroy()
	{
		if (ItemSlot != null)
		{
			ItemSlot.ValueChangedEvent -= OnValueChanged;
		}
	}

	private void OnValueChanged()
	{
		_amountLabel.text = ItemSlot.Amount.ToString();
	}
}