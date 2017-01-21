using System.Collections;
using UnityEngine;
namespace Project
{
	public class WaveManager : MonoBehaviour
	{
		[SerializeField]
		private float scrollVel = -1f;
		[SerializeField]
		private Wave[] waves;
		private void Start()
		{
			ResetWavePositions();
		}
		private void Update()
		{
			float waveWidth = waves[0].spriteRenderer.bounds.extents.x * 2f;
			if (waves[0].transform.localPosition.x < -waveWidth)
			{
				ResetWavePositions();
			}
			for (int i = 0; i < waves.Length; i++)
			{
				waves[i].transform.Translate(new Vector3(scrollVel * Time.deltaTime, 0f));
			}
		}
		private void ResetWavePositions()
		{
			float waveWidth = waves[0].spriteRenderer.bounds.extents.x * 2f;
			for (int i = 0; i < waves.Length; i++)
			{
				waves[i].transform.localPosition = new Vector3(waveWidth * i, 0f);
			}
		}
		public float GetWaterHeightAtXPos(float xPos)
		{
			RaycastHit2D hit = Physics2D.Raycast(new Vector2(xPos, 75f), Vector2.down, 100f, Layer.Water.ToMask());
			return hit.point.y;
		}
	}
}
