using System;

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

	public Inventory(int gold, int food, int tools)
	{
		Gold = new ItemSlot(gold);
		Food = new ItemSlot(food);
		Tools = new ItemSlot(tools);
	}
}
