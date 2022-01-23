using System;
using TMPro;
using UnityEngine;

public class OpenedChestPopup : MonoBehaviour
{
	[SerializeField]
	private TextMeshProUGUI _amountLabel = null;

	private Action _callback = null;

	public void OpenPopup(int goldAmount, Action onCollectPressed)
	{
		gameObject.SetActive(true);
		_amountLabel.text = goldAmount.ToString();
		_callback = onCollectPressed;
	}

	public void ClosePopup()
	{
		_callback = null;
		gameObject.SetActive(false);
	}

	public void OnCollectPressed()
	{
		_callback?.Invoke();
		ClosePopup();
	}
}