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
		private Transform barrelEnd = null;
		[SerializeField]
		private SpriteRenderer spriteRenderer = null;
		[SerializeField]
		private SpriteRenderer barrelRenderer = null;
		[SerializeField]
		private SpriteRenderer sailFaceRenderer = null;
		[SerializeField]
		private SpriteRenderer sailRenderer = null;
		[SerializeField]
		private ParticleSystem damageParticles = null;
		[SerializeField]
		private ParticleSystem frontParticles = null;
		[SerializeField]
		private CircleCollider2D localCollider = null;
		[SerializeField]
		private Rigidbody2D localRigidbody = null;
		[SerializeField]
		private string horizontalAxis = "Horizontal";
		[SerializeField]
		private string fireButton = "Fire";
		[SerializeField]
		private int maxHealthPoints = 2;
		[SerializeField]
		private Color deathColor = Color.gray;
		[SerializeField]
		private float deathGravity = 0.075f;
		[SerializeField]
		private Sprite sailHealthy = null;
		[SerializeField]
		private Sprite sailDamaged = null;
		[SerializeField]
		private Sprite sailDead = null;
		[SerializeField]
		private Sprite sailNearDeath = null;
		//Local Fields
		private WaveManager waveManager = null;
		private int healthPoints = 2;
		private float fireCooldownProgress = 0f;
		private float xPosition = 0f;
		private float horizontalInput = 0f;
		private bool isInFireCooldown = false;
		private bool isDead = false;
		private void Start()
		{
			xPosition = transform.position.x;
			healthPoints = maxHealthPoints;
			if (damageParticles.isPlaying)
			{
				damageParticles.Stop();
			}
			waveManager = FindObjectOfType<WaveManager>();
		}
		private void Update()
		{
			if (!isDead)
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
				Collider2D frontPoint = Physics2D.OverlapCircle(frontParticles.transform.position, 0.02f, Layer.Water.ToMask());
				if (frontPoint != null)
				{
					frontParticles.EnableEmission();
				}
				else
				{
					frontParticles.DisableEmission();
				}
			}
		}
		private void FixedUpdate()
		{
			if (!isDead)
			{
				xPosition += horizontalInput * movementSpeed;
				float targetYPosition = waveManager.GetWaterHeightAtXPos(xPosition) + 0.1f;
				transform.position = new Vector3(xPosition, targetYPosition);
			}
		}
		private void Fire()
		{
			PrefabPooler.GetFreeFromPool(projectilePrefab, barrelEnd.position, barrelEnd.rotation);
			fireCooldownProgress = 0f;
			isInFireCooldown = true;
		}
		public void OnHit()
		{
			healthPoints--;
			if (healthPoints <= 0)
			{
				OnDeath();
			}
			else
			{
				if (healthPoints == 1)
				{
					sailFaceRenderer.sprite = sailNearDeath;
				}
				else
				{
					sailFaceRenderer.sprite = sailDamaged;
				}
				if (!damageParticles.isPlaying)
				{
					damageParticles.Play();
				}
				damageParticles.EnableEmission();
			}
		}
		private void OnDeath()
		{
			if (!isDead)
			{
				isDead = true;
				sailFaceRenderer.sprite = sailDead;
				sailRenderer.color = deathColor;
				spriteRenderer.color = deathColor;
				barrelRenderer.color = deathColor;
				localRigidbody.bodyType = RigidbodyType2D.Dynamic;
				localRigidbody.gravityScale = deathGravity;
				localCollider.enabled = false;
				damageParticles.DisableEmission();
				frontParticles.DisableEmission();
			}
		}
	}
}
