using System.Collections;
using UnityEngine;
using UnityEngine.UI;
namespace Project
{
	public class Scorekeeper : MonoBehaviour
	{
		[SerializeField]
		private Boat playerOneBoat = null;
		[SerializeField]
		private Boat playerTwoBoat = null;
		[SerializeField]
		private Image[] playerOneTallies = null;
		[SerializeField]
		private Image[] playerTwoTallies = null;
		private int playerOneScore = 0;
		private int playerTwoScore = 0;
		private void Awake()
		{
			GameManager.OnMatchBeginEvent += Enable;
			GameManager.OnMatchEndEvent += Disable;
		}
		private void Start()
		{
			playerOneBoat.OnDeathEvent += OnPlayerOneDeath;
			playerTwoBoat.OnDeathEvent += OnPlayerTwoDeath;
			Disable();
		}
		private void OnPlayerOneDeath()
		{
			playerTwoScore++;
			RefreshTallies();
			if (playerTwoScore >= playerTwoTallies.Length)
			{
				GameManager.EndMatch();
			}
		}
		private void OnPlayerTwoDeath()
		{
			playerOneScore++;
			RefreshTallies();
			if (playerOneScore >= playerOneTallies.Length)
			{
				GameManager.EndMatch();
			}
		}
		private void RefreshTallies()
		{
			for (int i = 0; i < playerOneTallies.Length; i++)
			{
				playerOneTallies[i].enabled = i < playerOneScore;
			}
			for (int i = 0; i < playerTwoTallies.Length; i++)
			{
				playerTwoTallies[i].enabled = i < playerTwoScore;
			}
		}
		private void Enable()
		{
			gameObject.SetActive(true);
		}
		private void Disable()
		{
			gameObject.SetActive(false);
			playerOneScore = 0;
			playerTwoScore = 0;
			RefreshTallies();
		}
	}
}
