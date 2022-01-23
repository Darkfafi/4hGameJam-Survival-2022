using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	// Resources <- Forth
	// Chest <- Third
	// Days <- Second
	// Mini Game <- First (current)

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

	[SerializeField]
	private MasterMindGameView _masterMindView = null;

	private MasterMindGame _masterMindGame = null;

	protected void Awake()
	{
		// Setup Model
		_masterMindGame = new MasterMindGame(5);


		// Setup View
		_masterMindView.Initialize(_masterMindGame);
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
}
