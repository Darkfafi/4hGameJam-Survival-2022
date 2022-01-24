using System.Collections.Generic;
using UnityEngine;

public class InputBlocker : MonoBehaviour
{
	public static bool IsBlocked => _blockers.Count > 0;

	private static List<string> _blockers = new List<string>();

	public static void AddBlocker(string blocker)
	{
		_blockers.Add(blocker);
	}

	public static void RemoveBlocker(string blocker)
	{
		_blockers.Remove(blocker);
	}

	protected void OnEnable()
	{
		_blockers.Add(name);
	}

	protected void OnDisable()
	{
		_blockers.Remove(name);
	}

	protected void OnDestroy()
	{
		_blockers.Remove(name);
	}
}