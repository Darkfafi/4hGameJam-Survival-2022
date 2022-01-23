using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimplePopup : MonoBehaviour
{
	private Action _callback = null;

	public void OpenPopup(Action onCloseCallback)
	{
		gameObject.SetActive(true);
		_callback = onCloseCallback;
	}

	public void ClosePopup()
	{
		Action callback = _callback;
		_callback = null;
		gameObject.SetActive(false);
		callback?.Invoke();
	}
}