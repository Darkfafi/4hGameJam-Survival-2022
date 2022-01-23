using System;
using UnityEngine;

public class ItemSlot
{
	public event Action ValueChangedEvent;

	public int Amount
	{
		get; private set;
	}

	public ItemSlot(int amount)
	{
		Amount = amount;
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
}