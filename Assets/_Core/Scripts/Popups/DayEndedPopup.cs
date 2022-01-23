using System;
using TMPro;
using UnityEngine;

public class DayEndedPopup : MonoBehaviour
{
	[SerializeField]
	private TextMeshProUGUI _titleLabel = null;
	[SerializeField]
	private TextMeshProUGUI _amountKeysLabel = null;
	[SerializeField]
	private TextMeshProUGUI _amountGoldLabel = null;

	private Action _callback = null;

	public void OpenPopup(int day, int keyAmount, int goldAmount, Action onCollectPressed)
	{
		gameObject.SetActive(true);
		_titleLabel.text = $"Day: {day} Ended";
		_amountKeysLabel.text = keyAmount.ToString();
		_amountGoldLabel.text = goldAmount.ToString();
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