using UnityEngine;

public class SceneLoader : MonoBehaviour
{
	public void LoadScene(int index)
	{
		UnityEngine.SceneManagement.SceneManager.LoadScene(index);
	}
}
