using System;
using UnityEngine;

public class ItemSlot
{
	public event Action<ItemSlot> BuyRequestedEvent;
	public event Action ValueChangedEvent;

	public int Amount
	{
		get; private set;
	}

	public int? BuyCost
	{
		get; private set;
	}

	public ItemSlot(int amount, int? buyCost = null)
	{
		Amount = amount;
		BuyCost = buyCost;
	}

	public void Drain(int delta)
	{
		Amount = Mathf.Max(Amount - delta, 0);
		ValueChangedEvent?.Invoke();
	}

	public void Add(int delta)
	{
		Amount += delta;
		ValueChangedEvent?.Invoke();
	}

	public bool CanAfford(int amount)
	{
		return Amount >= amount;
	}

	public bool TrySpend(int amount)
	{
		if(CanAfford(amount))
		{
			Drain(amount);
			return true;
		}
		return false;
	}

	public void TryRequestBuy()
	{
		if (BuyCost.HasValue)
		{
			BuyRequestedEvent?.Invoke(this);
		}
	}
}