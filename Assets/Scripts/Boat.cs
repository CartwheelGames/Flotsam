using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
namespace Project
{
	public class Boat : MonoBehaviour
	{
		//Serialized Fields
		[SerializeField]
		private float movementSpeed = 0.01f;
		[SerializeField]
		private float rotationCorrectionSpeed = 10f;
		[SerializeField]
		private Wave wave = null;
		[SerializeField]
		private Transform barrelEnd = null;
		[SerializeField]
		private string horizontalAxis = "Horizontal";
		[SerializeField]
		private string verticalAxis = "Vertical";
		[SerializeField]
		private string fireButton = "Fire";
		//Local Fields
		private float xPosition = 0f;
		private float horizontalInput = 0f;
		private void Start()
		{
			xPosition = transform.position.x;
		}
		private void LateUpdate()
		{
			horizontalInput = Input.GetAxis(horizontalAxis);
			if (Input.GetButtonDown(fireButton))
			{
				Fire();
			}
			RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 4f, Layer.Water.ToMask());
			if (hit.transform != null)
			{
				Quaternion surfaceRotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
				transform.rotation = Quaternion.Slerp(transform.rotation, surfaceRotation, rotationCorrectionSpeed * Time.deltaTime);
			}
		}
		private void FixedUpdate()
		{
			xPosition += horizontalInput * movementSpeed;
			transform.position = new Vector3(xPosition, wave.GetHeightAtXPos(xPosition) + 0.1f);
		}
		private void Fire()
		{
		}
	}
}