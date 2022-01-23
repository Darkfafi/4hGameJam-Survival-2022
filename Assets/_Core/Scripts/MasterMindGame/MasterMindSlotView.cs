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

	[SerializeField]
	private GameObject _editingVisuals = null;

	public MasterMindGame.Slot Slot
	{
		get; private set;
	}

	public bool IsInitialized => Slot != null;

	private Color _defaultColor = default;
	private int _guessingNumber = 0;

	public void Initialize(MasterMindGame.Slot slot)
	{
		if (IsInitialized)
		{
			return;
		}

		Slot = slot;
		_defaultColor = _label.color;
		_label.text = "-";
		SetCurrentEditing(false);
	}

	public void SetCurrentEditing(bool isEditing)
	{
		_editingVisuals.SetActive(isEditing);
	}

	public void SetGuess(int guessingNumber)
	{
		_guessingNumber = guessingNumber;
		_label.text = guessingNumber.ToString();
		_label.color = _defaultColor;
	}

	public void SubmitGuess()
	{
		Slot.SetGuess(_guessingNumber);
		_label.text = Slot.Guess.ToString();
		switch(Slot.State)
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
