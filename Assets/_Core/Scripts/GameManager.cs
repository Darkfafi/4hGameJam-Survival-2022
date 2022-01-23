using TMPro;
using UnityEngine;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
	// Game loop
	// Tools			<- Fifth	(done)
	// Chest			<- Forth	(done)
	// Resources		<- Third	(done)
	// Day / Day Parts	<- Second	(done)
	// Mini Game		<- First	(done)

	public readonly KeyCode[] NumberKeyCodes = new KeyCode[]
	{
		KeyCode.Alpha0,
		KeyCode.Alpha1,
		KeyCode.Alpha2,
		KeyCode.Alpha3,
		KeyCode.Alpha4,
		KeyCode.Alpha5,
		KeyCode.Alpha6,
		KeyCode.Alpha7,
		KeyCode.Alpha8,
		KeyCode.Alpha9,
	};

	public const int TotalDayParts = 5;

	[SerializeField]
	private MasterMindGameView _masterMindView = null;

	[SerializeField]
	private InventoryView _inventoryView = null;

	[SerializeField]
	private OpenedChestPopup _openedChestPopup = null;

	[SerializeField]
	private DayEndedPopup _dayEndedPopup = null;

	[SerializeField]
	private SimplePopup _losePopup = null;

	[SerializeField]
	private SimplePopup _keysWarningPopup = null;

	[SerializeField]
	private TextMeshProUGUI _dayLabel = null;

	[SerializeField]
	private RectTransform _chest = null;

	[SerializeField]
	private Transform _dayPartsFill = null;

	private MasterMindGame _masterMindGame = null;

	private int _solvedChests = 0;
	private bool _inputLocked = false;

	public int CurrentDay
	{ 
		get; private set; 
	}

	public int CurrentDayPart
	{
		get; private set;
	}

	public Inventory Inventory
	{
		get; private set;
	}

	protected void Awake()
	{
		// Set Values
		CurrentDay = 1;
		CurrentDayPart = 0;
		Inventory = new Inventory(5, 10, 5, 1, 1);

		// Setup Model
		_masterMindGame = new MasterMindGame(3);

		// Setup View
		_inventoryView.Initialize(Inventory);
		_masterMindView.Initialize(_masterMindGame, Inventory);

		// Events
		_masterMindGame.SubmittedAnswerEvent += OnSubmittedAnswerEvent;

		// Days View
		UpdateDayVisuals();
	}

	protected void Update()
	{
		for (int i = 0; i < NumberKeyCodes.Length; i++)
		{
			if (Input.GetKeyDown(NumberKeyCodes[i]))
			{
				if(_inputLocked)
				{
					return;
				}

				if (Inventory.Tools.Amount == 0)
				{
					ShowKeysWarning();
					return;
				}

				_masterMindView.RegisterInput(i);
			}
		}
	}

	private void OnSubmittedAnswerEvent()
	{
		if (Inventory.Food.TrySpend(1))
		{
			if (Inventory.Tools.TrySpend(1))
			{
				_inputLocked = true;
				_chest.DOKill(true);
				_chest.DOPunchRotation(new Vector3(0f, 0f, 1f), 0.5f).OnComplete(() => 
				{
					_inputLocked = false;

					// Gain Resources (chest)
					if (_masterMindGame.IsSolved())
					{
						_solvedChests++;
						int goldAmount = Random.Range(1, 10) + Random.Range(0, _masterMindGame.Slots.Length + 1);
						_openedChestPopup.OpenPopup(goldAmount, () =>
						{
							Inventory.Gold.Add(goldAmount);
							_masterMindGame.StartNewGame(Mathf.Min(3 + Mathf.FloorToInt(_solvedChests / 5f), 10));
							PassDayPart();
						});
					}
					else
					{
						PassDayPart();
					}
				});
			}
			else
			{
				PassDayPart();
			}
		}
		else
		{
			_losePopup.OpenPopup(()=> 
			{
				UnityEngine.SceneManagement.SceneManager.LoadScene(0);
			});
		}
	}

	private void PassDayPart()
	{
		CurrentDayPart++;
		UpdateDayVisuals();

		if (CurrentDayPart >= TotalDayParts)
		{
			SetNewDay();
		}
		else
		{
			if (Inventory.Tools.Amount == 0)
			{
				ShowKeysWarning();
			}
		}
	}

	private void SetNewDay()
	{
		int toolsToAdd = 2;
		int goldToAdd = 1;
		_dayEndedPopup.OpenPopup(CurrentDay, toolsToAdd, goldToAdd, () => 
		{
			Inventory.Tools.Add(toolsToAdd);
			Inventory.Gold.Add(goldToAdd);
			
			CurrentDay++;
			CurrentDayPart = 0;
			UpdateDayVisuals();

			if (Inventory.Tools.Amount == 0)
			{
				ShowKeysWarning();
			}
		});
	}

	private void UpdateDayVisuals()
	{
		_dayLabel.text = $"Day: {CurrentDay}";
		_dayPartsFill.DOKill();
		_dayPartsFill.DOScaleX((float)CurrentDayPart / TotalDayParts, 0.5f);
	}

	private void ShowKeysWarning()
	{
		CloseKeysWarning();
		_keysWarningPopup.OpenPopup(() => { CloseKeysWarning(); });
	}

	private void CloseKeysWarning()
	{
		_keysWarningPopup.ClosePopup();
	}
}
