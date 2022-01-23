using TMPro;
using UnityEngine;

public class MasterMindSlotView : MonoBehaviour
{
	[SerializeField]
	private TextMeshProUGUI _label = null;

	[SerializeField]
	private Color _correctColor = default;

	[SerializeField]
	private Color _tooHighColor = default;

	[SerializeField]
	private Color _tooLowColor = default;

	private MasterMindGame.Slot _slot = null;

	public bool IsInitialized => _slot != null;

	private Color _defaultColor = default;
	private int _guessingNumber = 0;

	public void Initialize(MasterMindGame.Slot slot)
	{
		if (IsInitialized)
		{
			return;
		}

		_slot = slot;
		_defaultColor = _label.color;
		_label.text = "-";
	}

	public void SetGuess(int guessingNumber)
	{
		_guessingNumber = guessingNumber;
		_label.text = guessingNumber.ToString();
		_label.color = _defaultColor;
	}

	public void SubmitGuess()
	{
		_slot.SetGuess(_guessingNumber);
		_label.text = _slot.Guess.ToString();
		switch(_slot.State)
		{
			case MasterMindGame.ResultState.None:
				_label.color = _defaultColor;
				break;
			case MasterMindGame.ResultState.Correct:
				_label.color = _correctColor;
				break;
			case MasterMindGame.ResultState.TooHigh:
				_label.color = _tooHighColor;
				break;
			case MasterMindGame.ResultState.TooLow:
				_label.color = _tooLowColor;
				break;
		}
	}
}
