using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
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
        private void Start()
        {
            startButton.onClick.AddListener(StartGame);
            soundButton.onClick.AddListener(ChangeSound);
            quitButton.onClick.AddListener(Application.Quit);
            RefreshSoundLabel();
        }
        private void StartGame()
        {
            SceneManager.LoadScene(1);
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
	}
}
