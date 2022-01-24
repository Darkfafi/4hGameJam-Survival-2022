using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;

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

	[Header("Audio")]
	[SerializeField]
	private AudioSource _source = null;
	[SerializeField]
	private AudioClip _inputSound = null;

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
		InputBlocker.RemoveBlocker(name);
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
		if (InputBlocker.IsBlocked)
		{
			return;
		}

		if (!ReadyToSubmit)
		{
			return;
		}

		Action submitAction = () =>
		{
			float duration = 0.5f;
			float durationPer = duration / _slotViews.Length;
			float pitchIncrease = 0.5f / _slotViews.Length;

			InputBlocker.AddBlocker(name);
			for (int i = 0; i < _slotViews.Length; i++)
			{
				float pitch = 1f + pitchIncrease * i;
				MasterMindSlotView slotView = _slotViews[i];
				slotView.Container.DOKill(true);
				slotView.Container.DOPunchPosition(Vector3.up * 5, durationPer).SetDelay(i * durationPer)
				.OnStart(() =>
				{
					_source.pitch = pitch;
					_source.clip = _inputSound;
					_source.Play();
				}).OnComplete(() =>
				{
					slotView.SubmitGuess();
				});
			}

			DoMethodAfterDelay(() =>
			{
				RefreshCurrentGuessingIndex();
				_masterMindGame.SubmittedAnswer();
				InputBlocker.RemoveBlocker(name);
			}, duration);
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

	public void DoMethodAfterDelay(Action method, float delay)
	{
		StartCoroutine(DoMethodAfterDelayRoutine(method, delay));
	}

	private IEnumerator DoMethodAfterDelayRoutine(Action method, float delay)
	{
		yield return new WaitForSeconds(delay);
		method();
	}
}
