using UnityEngine;
using System.Collections;
namespace Project
{
	public class Bottle : MonoBehaviour
	{
		[SerializeField]
		private Wave wave;
		private Vector3 lastPosition;
		private Vector3 velocity;
		private void Update()
		{
			RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 100f, Layer.Water.ToMask());
			if (hit.transform == null)
			{
				transform.position = new Vector3(0, wave.GetHeightAtXPos(0));
			}
		}
		private void FixedUpdate()
		{
			//transform.position = new Vector3(0, wave.GetHeightAtXPos(0));
			if (lastPosition != Vector3.zero)
			{
				velocity = transform.position - lastPosition;
			}
			lastPosition = transform.position;
		}
	}
}
