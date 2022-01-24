using UnityEngine;
using UnityEngine.UI;

public class SubmitButton : MonoBehaviour
{
	[SerializeField]
	private Button _button = null;

	public static bool IsSubmitPressed()
	{
		return Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space);
	}

	protected void Update()
	{
		if(IsSubmitPressed() && 
			_button.interactable && 
			_button.enabled && 
			_button.gameObject.activeInHierarchy)
		{
			_button.onClick.Invoke();
		}
	}
}
