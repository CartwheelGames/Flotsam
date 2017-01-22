using Action = System.Action;
using System.Collections;
using UnityEngine;
namespace Project
{
	public class GameManager : MonoBehaviour
	{
		private static bool isInMatch;
		public static event Action OnMatchBeginEvent;
		public static event Action OnMatchEndEvent;
		public static void BeginMatch()
		{
			if (OnMatchBeginEvent != null)
			{
				OnMatchBeginEvent();
			}
			isInMatch = true;
		}
		public static void EndMatch()
		{
			if (OnMatchEndEvent != null)
			{
				OnMatchEndEvent();
			}
			isInMatch = false;
		}
		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.Escape))
			{
				if (isInMatch)
				{
					EndMatch();
				}
				else
				{
					Application.Quit();
				}
			}
		}
	}
}
