using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	// Game loop
	// Chest			<- Forth	(current)
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

	public const int TotalDayParts = 3;

	[SerializeField]
	private MasterMindGameView _masterMindView = null;

	[SerializeField]
	private InventoryView _inventoryView = null;

	[SerializeField]
	private OpenedChestPopup _openedChestPopup = null;

	[SerializeField]
	private TextMeshProUGUI _dayLabel = null;

	[SerializeField]
	private Transform _dayPartsFill = null;

	private MasterMindGame _masterMindGame = null;

	private int _solvedChests = 0;

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
		// Setup Model
		Inventory = new Inventory(5, 10, 5, 1, 1);
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
				_masterMindView.RegisterInput(i);
			}
		}
	}

	private void OnSubmittedAnswerEvent()
	{
		// Resources Consume
		Inventory.Tools.Drain(1);
		Inventory.Food.Drain(1);

		// Gain Resources (chest)
		if(_masterMindGame.IsSolved())
		{
			_solvedChests++;
			int goldAmount = Random.Range(1, 10);
			_openedChestPopup.OpenPopup(goldAmount, ()=> 
			{
				Inventory.Gold.Add(goldAmount);
				_masterMindGame.StartNewGame(Mathf.Min(3 + Mathf.FloorToInt(_solvedChests / 5f), 10));
			});
		}

		// Pass Day
		CurrentDayPart++;
		if(CurrentDayPart >= TotalDayParts)
		{
			SetNewDay();
		}
		else
		{
			UpdateDayVisuals();
		}
	}

	private void SetNewDay()
	{
		CurrentDay++;
		CurrentDayPart = 0;
		UpdateDayVisuals();
	}

	private void UpdateDayVisuals()
	{
		_dayLabel.text = $"Day: {CurrentDay}";

		Vector3 scale = _dayPartsFill.localScale;
		scale.x = (float)CurrentDayPart / TotalDayParts;
		_dayPartsFill.localScale = scale; 
	}
}
