using UnityEngine;
using System.Collections;
namespace Project
{
	public class Wave : MonoBehaviour
	{

		public SpriteRenderer spriteRenderer { get; private set; }
		private void Awake()
		{
			spriteRenderer = GetComponent<SpriteRenderer>();
		}
		public float GetSlopeAtXPos(float xPos)
		{
			return 0;
		}
	}
}
