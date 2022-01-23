using UnityEngine;
using UnityEngine.UI;

public class MasterMindGameView : MonoBehaviour
{
	[SerializeField]
	private RectTransform _slotsContainer = null;

	[SerializeField]
	private MasterMindSlotView _slotPrefab = null;

	[SerializeField]
	private Button _submitButton = null;

	private MasterMindGame _masterMindGame = null;

	private MasterMindSlotView[] _slotViews = null;

	private int _currentGuessingIndex = 0;

	public bool IsInitialized => _masterMindGame != null;

	private bool ReadyToSubmit => _currentGuessingIndex >= _slotViews.Length;

	protected void Awake()
	{
		_slotPrefab.gameObject.SetActive(false);
	}

	protected void OnDestroy()
	{
		_submitButton.onClick.RemoveListener(SubmitGuesses);
	}

	public void Initialize(MasterMindGame game)
	{
		if(IsInitialized)
		{
			return;
		}

		_masterMindGame = game;

		_slotViews = new MasterMindSlotView[_masterMindGame.Slots.Length];

		for (int i = 0; i < _slotViews.Length; i++)
		{
			MasterMindGame.Slot slot = _masterMindGame.Slots[i];
			MasterMindSlotView slotView = Instantiate(_slotPrefab, _slotsContainer);
			slotView.Initialize(slot);
			_slotViews[i] = slotView;
			slotView.gameObject.SetActive(true);
		}

		_submitButton.onClick.AddListener(SubmitGuesses);

		RefreshCurrentGuessingIndex();
	}

	public void RegisterInput(int number)
	{
		SetGuessCurrentSlot(number);
	}

	public void SetGuessCurrentSlot(int guess)
	{
		if(ReadyToSubmit)
		{
			return;
		}

		_slotViews[_currentGuessingIndex].SetGuess(guess);

		do
		{
			_slotViews[_currentGuessingIndex].SetCurrentEditing(false);

			_currentGuessingIndex++;

			if (!ReadyToSubmit)
			{
				_slotViews[_currentGuessingIndex].SetCurrentEditing(true);
			}
		} while (!ReadyToSubmit && _slotViews[_currentGuessingIndex].Slot.State == MasterMindGame.ResultState.Correct);

		_submitButton.interactable = ReadyToSubmit;
	}

	public void SubmitGuesses()
	{
		if (!ReadyToSubmit)
		{
			return;
		}

		for (int i = 0; i < _slotViews.Length; i++)
		{
			_slotViews[i].SubmitGuess();
		}

		RefreshCurrentGuessingIndex();
		_masterMindGame.SubmittedAnswer();
	}

	private void RefreshCurrentGuessingIndex()
	{
		_currentGuessingIndex = -1;
		do
		{
			if (_currentGuessingIndex >= 0)
			{
				_slotViews[_currentGuessingIndex].SetCurrentEditing(false);
			}

			_currentGuessingIndex++;

			if (!ReadyToSubmit)
			{
				_slotViews[_currentGuessingIndex].SetCurrentEditing(true);
			}

		} while (!ReadyToSubmit && _slotViews[_currentGuessingIndex].Slot.State == MasterMindGame.ResultState.Correct);


		_submitButton.interactable = ReadyToSubmit;
	}
}
