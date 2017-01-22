using System.Collections;
using UnityEngine;
namespace Project
{
	public class WaveManager : MonoBehaviour
	{
        [SerializeField]
        private Boat playerOneBoat = null;
        [SerializeField]
        private Boat playerTwoBoat = null;
        [SerializeField]
        private float maxAbsSpeed = 0.5f;
        [SerializeField]
        private float speedChangeRate = 0.3f;
        [SerializeField]
        private Wave[] waves;
        private float targetSpeed = -1f;
        private float currentSpeed = 1f;
        private void Awake()
        {
            playerOneBoat.OnDeathEvent += OnPlayerDeath;
            playerTwoBoat.OnDeathEvent += OnPlayerDeath;
        }
		private void Start()
		{
            currentSpeed = targetSpeed;
			ResetWavePositions();
		}
		private void Update()
		{
			float waveWidth = waves[0].spriteRenderer.bounds.extents.x * 2f;
            if (currentSpeed < 0f && waves[1].transform.localPosition.x < -waveWidth)
			{
				ResetWavePositions();
			}
            else if (currentSpeed > 0f && waves[1].transform.localPosition.x > waveWidth)
            {
                ResetWavePositions();
            }
            if (currentSpeed != targetSpeed)
            {
                currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, Time.deltaTime * speedChangeRate);
            }
			for (int i = 0; i < waves.Length; i++)
			{
                waves[i].transform.Translate(new Vector3(currentSpeed * maxAbsSpeed * Time.deltaTime, 0f));
			}
		}
		private void ResetWavePositions()
		{
			float waveWidth = waves[0].spriteRenderer.bounds.extents.x * 2f;
			for (int i = 0; i < waves.Length; i++)
			{
                waves[i].transform.localPosition = new Vector3(waveWidth * (i - 1), 0f);
			}
		}
		private void SetWaveHeight(float height)
		{
			for (int i = 0; i < waves.Length; i++)
			{
				waves[i].transform.localScale = new Vector3(waves[i].transform.localScale.x, height, waves[i].transform.localScale.z);
			}
		}
		public float GetWaterHeightAtXPos(float xPos)
		{
			RaycastHit2D hit = Physics2D.Raycast(new Vector2(xPos, 75f), Vector2.down, 100f, Layer.Water.ToMask());
			return hit.point.y;
		}
        private void OnPlayerDeath()
        {
            targetSpeed = -targetSpeed;
        }
	}
}
