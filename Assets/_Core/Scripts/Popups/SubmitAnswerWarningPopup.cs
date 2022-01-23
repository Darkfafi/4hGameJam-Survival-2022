using System;
using UnityEngine;

public class SubmitAnswerWarningPopup : MonoBehaviour
{
	private Action<bool> _callback = null;

	public void OpenPopup(Action<bool> submitConfirmedCallback)
	{
		gameObject.SetActive(true);
		_callback = submitConfirmedCallback;
	}

	public void ClosePopup()
	{
		_callback = null;
		gameObject.SetActive(false);
	}

	public void OnYesPressed()
	{
		_callback?.Invoke(true);
		ClosePopup();
	}

	public void OnNoPressed()
	{
		_callback?.Invoke(false);
		ClosePopup();
	}
}