using System.Collections;
using UnityEngine;
namespace Project
{
	public class CloudManager : MonoBehaviour
	{
		[SerializeField]
		private float minStartX = 2f;
		[SerializeField]
		private float maxStartX = 3f;
		[SerializeField]
		private float cloudSpeed = 0.5f;
		[SerializeField]
		private SpriteRenderer[] clouds;
		private void Update()
		{
			foreach (SpriteRenderer cloud in clouds)
			{
				if (cloud.transform.position.x < -0f && !cloud.isVisible)
				{
					cloud.transform.position = new Vector3(Random.Range(minStartX, maxStartX), cloud.transform.position.y, 0f);
				}
				else
				{
					cloud.transform.Translate(Vector3.left * cloudSpeed * Time.deltaTime);
				}
			}
		}
	}
}
