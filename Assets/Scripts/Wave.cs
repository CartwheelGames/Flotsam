using UnityEngine;
using System.Collections;
namespace Project
{
	public class Wave : MonoBehaviour
	{
		private float scrollPos;
		private SpriteRenderer spriteRenderer;
		private float scrollVel = -1;
		private void Start()
		{
			spriteRenderer = GetComponent<SpriteRenderer>();
		}
		public float GetHeightAtXPos(float xPos)
		{
			RaycastHit2D hit = Physics2D.Raycast(new Vector2(xPos, 75f), Vector2.down, 100f, Layer.Water.ToMask());
			return hit.point.y;
		}
		public float GetSlopeAtXPos(float xPos)
		{
			return 0;
		}
		private void Update()
		{
			scrollPos += Time.deltaTime * scrollVel;
			scrollPos = scrollPos % 10;
			transform.position = new Vector2(scrollPos, 0);
		}
	}
}
