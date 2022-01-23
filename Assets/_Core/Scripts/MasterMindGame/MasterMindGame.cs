using UnityEngine;

public class MasterMindGame
{
	public Slot[] Slots
	{
		get; private set;
	}

	public MasterMindGame(int slotsAmount)
	{
		Slots = new Slot[slotsAmount];
		for(int i = 0; i < Slots.Length; i++)
		{
			Slots[i] = new Slot(Random.Range(0, 10));
		}
	}

	public bool IsSolved()
	{
		for (int i = 0; i < Slots.Length; i++)
		{
			Slot slot = Slots[i];
			if(slot.State != ResultState.Correct)
			{
				return false;
			}
		}

		return true;
	}

	public class Slot
	{
		public int Guess
		{
			get; private set;
		}

		public int Answer
		{
			get; private set;
		}

		public ResultState State
		{
			get; private set;
		}

		public Slot(int answer)
		{
			Answer = answer;
			Guess = 0;
			State = ResultState.None;
		}

		public void SetGuess(int number)
		{
			Guess = number;
			if(Guess < Answer)
			{
				State = ResultState.TooLow;
			}
			else if(Guess > Answer)
			{
				State = ResultState.TooHigh;
			}
			else
			{
				State = ResultState.Correct;
			}
		}
	}

	public enum ResultState
	{
		None	= 0,
		Correct = 1,
		TooLow	= 2,
		TooHigh	= 3
	}
}
