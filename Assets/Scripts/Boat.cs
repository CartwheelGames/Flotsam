using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Project.Pooler;
namespace Project
{
	public class Boat : MonoBehaviour
	{
		//Serialized Fields
		[SerializeField]
		private float fireCooldownDuration = 0.75f;
		[SerializeField]
		private float movementSpeed = 0.01f;
		[SerializeField]
		private float rotationCorrectionSpeed = 10f;
		[SerializeField]
		GameObject projectilePrefab = null;
		[SerializeField]
		private WaveManager waveManager = null;
		[SerializeField]
		private Transform barrelEnd = null;
		[SerializeField]
		private SpriteRenderer spriteRenderer = null;
		[SerializeField]
		private string horizontalAxis = "Horizontal";
		[SerializeField]
		private string fireButton = "Fire";
		//Local Fields
		private float fireCooldownProgress = 0f;
		private float xPosition = 0f;
		private float horizontalInput = 0f;
		private bool isInFireCooldown = false;
		private void Start()
		{
			xPosition = transform.position.x;
		}
		private void Update()
		{
			horizontalInput = Input.GetAxis(horizontalAxis);
			if (isInFireCooldown)
			{
				fireCooldownProgress += Time.deltaTime;
				if (fireCooldownProgress >= fireCooldownDuration)
				{
					isInFireCooldown = false;
				}
			}
			else if (Input.GetButtonDown(fireButton))
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
			transform.position = new Vector3(xPosition, waveManager.GetWaterHeightAtXPos(xPosition) + 0.1f);
		}
		private void Fire()
		{
			PrefabPooler.GetFreeFromPool(projectilePrefab, barrelEnd.position, barrelEnd.rotation);
			fireCooldownProgress = 0f;
			isInFireCooldown = true;
		}
		public void OnHit()
		{

		}
	}
}