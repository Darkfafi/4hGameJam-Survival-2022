using System;
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

	[SerializeField]
	private SubmitAnswerWarningPopup _submitAnswerWarningPopup = null;

	private MasterMindGame _masterMindGame = null;
	private Inventory _inventory = null;

	private MasterMindSlotView[] _slotViews = null;

	private int _currentGuessingIndex = 0;

	public bool IsInitialized => _masterMindGame != null;

	private bool ReadyToSubmit => _inventory.Tools.Amount == 0 || _currentGuessingIndex >= _slotViews.Length;

	protected void Awake()
	{
		_slotPrefab.gameObject.SetActive(false);
	}

	protected void OnDestroy()
	{
		_inventory.Tools.ValueChangedEvent -= OnToolsValueChanged;
		_masterMindGame.NewGameStartedEvent -= OnNewGameStarted;
		_submitButton.onClick.RemoveListener(SubmitGuesses);
	}

	public void Initialize(MasterMindGame game, Inventory inventory)
	{
		if (IsInitialized)
		{
			return;
		}

		_inventory = inventory;
		_masterMindGame = game;
		_masterMindGame.NewGameStartedEvent += OnNewGameStarted;
		_inventory.Tools.ValueChangedEvent += OnToolsValueChanged;

		_submitButton.onClick.AddListener(SubmitGuesses);

		OnNewGameStarted();
	}

	private void OnNewGameStarted()
	{
		if (_slotViews != null)
		{
			for (int i = 0; i < _slotViews.Length; i++)
			{
				Destroy(_slotViews[i].gameObject);
			}
			_slotViews = null;
		}

		_slotViews = new MasterMindSlotView[_masterMindGame.Slots.Length];

		for (int i = 0; i < _slotViews.Length; i++)
		{
			MasterMindGame.Slot slot = _masterMindGame.Slots[i];
			MasterMindSlotView slotView = Instantiate(_slotPrefab, _slotsContainer);
			slotView.Initialize(slot);
			_slotViews[i] = slotView;
			slotView.gameObject.SetActive(true);
		}

		RefreshCurrentGuessingIndex();
	}

	public void RegisterInput(int number)
	{
		SetGuessCurrentSlot(number);
	}

	public void SetGuessCurrentSlot(int guess)
	{
		if (ReadyToSubmit)
		{
			return;
		}

		_slotViews[_currentGuessingIndex].SetGuess(guess);

		do
		{
			_slotViews[_currentGuessingIndex].SetCurrentEditing(false);

			_currentGuessingIndex++;

			if (_currentGuessingIndex < _slotViews.Length)
			{
				_slotViews[_currentGuessingIndex].SetCurrentEditing(true);
			}
		} while (_currentGuessingIndex < _slotViews.Length && _slotViews[_currentGuessingIndex].Slot.State == MasterMindGame.ResultState.Correct);

		_submitButton.interactable = ReadyToSubmit;
	}

	public void SubmitGuesses()
	{
		if (!ReadyToSubmit)
		{
			return;
		}

		Action submitAction = () =>
		{
			for (int i = 0; i < _slotViews.Length; i++)
			{
				_slotViews[i].SubmitGuess();
			}

			RefreshCurrentGuessingIndex();
			_masterMindGame.SubmittedAnswer();
		};

		if (_inventory.Food.Amount == 0)
		{
			_submitAnswerWarningPopup.OpenPopup((confirmedSubmit) =>
			{
				if (confirmedSubmit)
				{
					submitAction.Invoke();
				}
			});
		}
		else
		{
			submitAction();
		}
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

			if (_currentGuessingIndex < _slotViews.Length)
			{
				_slotViews[_currentGuessingIndex].SetCurrentEditing(true);
			}

		} while (_currentGuessingIndex < _slotViews.Length && _slotViews[_currentGuessingIndex].Slot.State == MasterMindGame.ResultState.Correct);


		_submitButton.interactable = ReadyToSubmit;
	}

	private void OnToolsValueChanged()
	{
		_submitButton.interactable = ReadyToSubmit;
	}
}
