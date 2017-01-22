using Action = System.Action;
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
        private bool isRightBoat = false;
		[SerializeField]
		private float fireCooldownDuration = 0.75f;
		[SerializeField]
		private float movementSpeed = 0.01f;
		[SerializeField]
		private float rotationCorrectionSpeed = 10f;
		[SerializeField]
		private GameObject projectilePrefab = null;
		[SerializeField]
		private GameObject splashFXPrefab = null;
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
		private ParticleSystem deathParticles = null;
		[SerializeField]
		private ParticleSystem frontParticles = null;
        [SerializeField]
        private ParticleSystem rearParticles = null;
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
		private float nearDeathParticleRate = 32f;
		[SerializeField]
		private float respawnDelay = 3f;
		[SerializeField]
		private float invulnerableDuration = 2f;
		[SerializeField]
		private float invulnerableFlashRate = 6f;
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
		private float damagedParticleRate = 4f;
		private int healthPoints = 2;
		private float fireCooldownProgress = 0f;
		private float originalXPosition = 0f;
		private float xPosition = 0f;
		private float horizontalInput = 0f;
		private bool isInFireCooldown = false;
		private float timeToRespawn = 0f;
		private float timeToVulnerable = 0f;
		private bool isInvulnerable = false;
		private bool isDead = false;
		public event Action OnDeathEvent;
		private void Awake()
		{
			GameManager.OnMatchBeginEvent += Enable;
			GameManager.OnMatchEndEvent += Disable;
		}
		private void Start()
		{
			xPosition = transform.position.x;
			originalXPosition = xPosition;
			healthPoints = maxHealthPoints;
			if (damageParticles.isPlaying)
			{
				damageParticles.Stop();
			}
			damagedParticleRate = damageParticles.GetEmissionRate();
			waveManager = FindObjectOfType<WaveManager>();
			localRigidbody.gravityScale = deathGravity;
			Disable();
		}
		private void Update()
		{
			if (!isDead)
			{
				if (isInvulnerable)
				{
					if (Time.time > timeToVulnerable)
					{
						isInvulnerable = false;
						ShowGeometry();
					}
					else
					{
						if (Mathf.Round(Time.time * invulnerableFlashRate) % 2 > 0)
						{
							ShowGeometry();
						}
						else
						{
							HideGeometry();
						}
					}
				}
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
                if (horizontalInput > 0.1f)
                {
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
                else
                {
                    frontParticles.DisableEmission();
                }
                if (horizontalInput < -0.1f)
                {
                    Collider2D rearPoint = Physics2D.OverlapCircle(rearParticles.transform.position, 0.02f, Layer.Water.ToMask());
                    if (rearPoint != null)
                    {
                        rearParticles.EnableEmission();
                    }
                    else
                    {
                        rearParticles.DisableEmission();
                    }
                }
                else
                {
                    rearParticles.DisableEmission();
                }
			}
			else if (Time.time > timeToRespawn)
			{
				Respawn();
			}
		}
		private void FixedUpdate()
		{
			if (!isDead)
			{
				xPosition += horizontalInput * movementSpeed;

                float leftBorder = isRightBoat ? 0.15f : Camera.main.ViewportToWorldPoint(Vector3.zero).x;
                float rightBorder = !isRightBoat ? -0.15f : Camera.main.ViewportToWorldPoint(Vector3.right).x;
				xPosition = Mathf.Clamp(xPosition, leftBorder, rightBorder);

				float targetYPosition = waveManager.GetWaterHeightAtXPos(xPosition) + 0.1f;

				transform.position = new Vector3(xPosition, targetYPosition);
			}
		}
		private void Fire()
		{
			PrefabPooler.GetFreeFromPool(projectilePrefab, barrelEnd.position, barrelEnd.rotation);
			fireCooldownProgress = 0f;
			isInFireCooldown = true;
			SoundManager.PlayFireClip();
		}
		public void OnHit()
		{
			if (!isInvulnerable)
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
						damageParticles.SetEmissionRate(nearDeathParticleRate);
						sailFaceRenderer.sprite = sailNearDeath;
					}
					else
					{
						damageParticles.SetEmissionRate(damagedParticleRate);
						sailFaceRenderer.sprite = sailDamaged;
					}
					if (!damageParticles.isPlaying)
					{
						damageParticles.Play();
					}
					damageParticles.EnableEmission();
				}
			}
		}
		private void HideGeometry()
		{
			foreach (Transform t in transform)
			{
				t.gameObject.SetActive(false);
			}
		}
		private void ShowGeometry()
		{
			foreach (Transform t in transform)
			{
				t.gameObject.SetActive(true);
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
				localCollider.enabled = false;
				deathParticles.Emit(32);
				damageParticles.DisableEmission();
                frontParticles.DisableEmission();
                rearParticles.DisableEmission();
				timeToRespawn = Time.time + respawnDelay;
				PrefabPooler.GetFreeFromPool(splashFXPrefab, transform.position, transform.rotation);
				SoundManager.PlayDeathClip();
				if (OnDeathEvent != null)
				{
					OnDeathEvent();
				}
			}
		}
		private void Respawn()
		{
			isInFireCooldown = false;
			isDead = false;
			xPosition = originalXPosition;
			healthPoints = maxHealthPoints;
			sailFaceRenderer.sprite = sailHealthy;
			sailRenderer.color = Color.white;
			spriteRenderer.color = Color.white;
			barrelRenderer.color = Color.white;
			localRigidbody.velocity = Vector2.zero;
			localRigidbody.angularVelocity = 0f;
			localRigidbody.bodyType = RigidbodyType2D.Kinematic;
			localCollider.enabled = true;
			damageParticles.DisableEmission();
			damageParticles.SetEmissionRate(damagedParticleRate);
			damageParticles.Stop();
            frontParticles.EnableEmission();
            rearParticles.EnableEmission();
			isInvulnerable = true;
			timeToVulnerable = invulnerableDuration + Time.time;
		}
		private void Enable()
		{
			gameObject.SetActive(true);
		}
		private void Disable()
		{
			gameObject.SetActive(false);
		}
	}
}
