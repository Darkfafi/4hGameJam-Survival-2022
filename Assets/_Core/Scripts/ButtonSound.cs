using UnityEngine;
using UnityEngine.UI;

public class ButtonSound : MonoBehaviour
{
    [SerializeField]
    private Button _button = null; 

    [SerializeField]
    private AudioClip _sound = null;

	protected void Awake()
	{
		_button.onClick.AddListener(PlaySound);
	}

	protected void OnDestroy()
	{
		_button.onClick.RemoveListener(PlaySound);
	}

	private void PlaySound()
	{
		AudioSource.PlayClipAtPoint(_sound, Camera.main.transform.position, 0.5f);
	}
}
