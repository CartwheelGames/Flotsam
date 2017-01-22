using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
namespace Project
{
	public class Menu : MonoBehaviour
	{
        [SerializeField]
        private Button startButton = null;
        [SerializeField]
        private Button soundButton = null;
        [SerializeField]
        private Text soundLabel = null;
        [SerializeField]
        private Button quitButton = null;
		private void Awake()
		{
			GameManager.OnMatchBeginEvent += Disable;
			GameManager.OnMatchEndEvent += Enable;
		}
        private void Start()
        {
            startButton.onClick.AddListener(BeginGame);
            soundButton.onClick.AddListener(ChangeSound);
            quitButton.onClick.AddListener(Application.Quit);
            RefreshSoundLabel();
        }
        private void BeginGame()
        {
			GameManager.BeginMatch();
        }
        private void ChangeSound()
        {
            AudioListener.pause = !AudioListener.pause;
            RefreshSoundLabel();
        }
        private void RefreshSoundLabel()
        {
            soundLabel.text = AudioListener.pause ? "sound Off" : "sound On";
        }
		private void Enable()
		{
			gameObject.SetActive(true);
		}
		private void Disable()
		{
			gameObject.SetActive(false);
		}
	}
}
