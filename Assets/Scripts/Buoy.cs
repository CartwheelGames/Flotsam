using System.Collections;
using UnityEngine;
namespace Project
{
	public class Buoy : MonoBehaviour
	{
		private WaveManager waveManager = null;
		private void Start()
		{
			waveManager = FindObjectOfType<WaveManager>();
		}
		private void FixedUpdate()
		{
			float targetYPosition = waveManager.GetWaterHeightAtXPos(0) + 0.1f;
			transform.position = new Vector3(0, targetYPosition);
		}
	}
}
