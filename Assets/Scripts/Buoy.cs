using System.Collections;
using UnityEngine;
namespace Project
{
	public class Buoy : MonoBehaviour
	{
		private WaveManager waveManager = null;
		private void Awake()
		{
			GameManager.OnMatchBeginEvent += Enable;
			GameManager.OnMatchEndEvent += Disable;
		}
		private void Start()
		{
			waveManager = FindObjectOfType<WaveManager>();
			Disable();
		}
		private void FixedUpdate()
		{
			float targetYPosition = waveManager.GetWaterHeightAtXPos(0) + 0.1f;
			transform.position = new Vector3(0, targetYPosition);
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
