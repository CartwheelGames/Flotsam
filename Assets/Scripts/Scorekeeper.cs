using System.Collections;
using UnityEngine;
using UnityEngine.UI;
namespace Project
{
	public class Scorekeeper : MonoBehaviour
	{
        [SerializeField]
        [Range(1,10)]
        private int maxScore = 1;
        [SerializeField]
        private float timeOnWinnerScreen = 5f;
        [SerializeField]
        private Text winnerLabel = null;
        [SerializeField]
        private Sprite noScoreSprite = null;
        [SerializeField]
        private Sprite scoreSprite = null;
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
        private bool isWinnerDeclared = false;
		private void Awake()
		{
			GameManager.OnMatchBeginEvent += Enable;
			GameManager.OnMatchEndEvent += Disable;
            playerOneBoat.OnDeathEvent += OnPlayerOneDeath;
            playerTwoBoat.OnDeathEvent += OnPlayerTwoDeath;
        }
        private void Start()
        {
			Disable();
		}
		private void OnPlayerOneDeath()
		{
			playerTwoScore++;
			RefreshTallies();
            if (!isWinnerDeclared && playerTwoScore >= maxScore)
			{
                winnerLabel.enabled = true;
                winnerLabel.text = "Player 2 Wins";
                Invoke("EndMatch", timeOnWinnerScreen);
                isWinnerDeclared = true;
                playerOneBoat.OnWinnerDeclared();
                playerTwoBoat.OnWinnerDeclared();
            }
        }
        private void OnPlayerTwoDeath()
        {
            playerOneScore++;
            RefreshTallies();
            if (!isWinnerDeclared && playerOneScore >= maxScore)
            {
                winnerLabel.enabled = true;
                winnerLabel.text = "Player 1 Wins";
                Invoke("EndMatch", timeOnWinnerScreen);
                isWinnerDeclared = true;
                playerOneBoat.OnWinnerDeclared();
                playerTwoBoat.OnWinnerDeclared();
			}
		}
        private void EndMatch()
        {
            GameManager.EndMatch();
        }
		private void RefreshTallies()
		{
			for (int i = 0; i < playerOneTallies.Length; i++)
			{
                playerOneTallies[i].enabled = i < maxScore;
                playerOneTallies[i].sprite = i < playerOneScore ? scoreSprite : noScoreSprite;
            }
            for (int i = 0; i < playerTwoTallies.Length; i++)
            {
                playerTwoTallies[i].enabled = i < maxScore;
                playerTwoTallies[i].sprite = i < playerTwoScore ? scoreSprite : noScoreSprite;
			}
		}
		private void Enable()
		{
			gameObject.SetActive(true);
		}
		private void Disable()
		{
            winnerLabel.enabled = false;
			gameObject.SetActive(false);
			playerOneScore = 0;
			playerTwoScore = 0;
            isWinnerDeclared = false;
			RefreshTallies();
		}
	}
}
