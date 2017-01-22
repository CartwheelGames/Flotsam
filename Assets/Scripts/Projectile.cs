using System.Collections;
using UnityEngine;
using Project.Pooler;
namespace Project
{
	public class Projectile : PoolableObject
	{
		[SerializeField]
		private float power = 3f;
		[SerializeField]
		private SpriteRenderer spriteRenderer = null;
		[SerializeField]
		private CircleCollider2D circleCollider = null;
		[SerializeField]
		private Rigidbody2D localRigidbody = null;
		[SerializeField]
		private ParticleSystem particles = null;
		[SerializeField]
		private Color underwaterColor = Color.gray;
		[SerializeField]
		private GameObject explosionPrefab = null;
		[SerializeField]
		private GameObject splashPrefab = null;
		private const float armingTime = 0.1f;
		private float timeToArm = 0f;
		private float originalGravity;
		private bool hasHit = false;
		private bool hasEnteredWater = false;
		private void Awake()
		{
			originalGravity = localRigidbody.gravityScale;
		}
		private void OnEnable()
		{
			localRigidbody.AddForce(transform.right * power);
			timeToArm = Time.time + armingTime;
		}
		private void OnDisable()
		{
			CleanUp();
		}
		private void Update()
		{
            if (((hasHit || !spriteRenderer.isVisible) && particles.particleCount == 0) || transform.position.y < -10f)
			{
				RecycleToPool();
			}
		}
		private void OnTriggerEnter2D(Collider2D col)
		{
			if (!hasEnteredWater && !hasHit)
			{
				if (col.gameObject.layer == Layer.Water.ToIndex())
				{
					OnEnterWater();
				}
				else if (col.gameObject.layer == Layer.Boat.ToIndex())
				{
					if (Time.time > timeToArm)
					{
						Boat boat = col.gameObject.GetComponent<Boat>();
						if (boat != null)
						{
							boat.OnHit();
						}
						OnHit();
					}
				}
				else if (col.gameObject.layer == Layer.Projectile.ToIndex() || col.gameObject.layer == Layer.Environment.ToIndex())
				{
					OnHit();
				}
			}
		}
		private void OnEnterWater()
		{
			localRigidbody.gravityScale = 0.075f;
			circleCollider.enabled = false;
			particles.DisableEmission();
			spriteRenderer.color = underwaterColor;
			hasEnteredWater = true;
			Vector3 splashPosition = new Vector3(transform.position.x, transform.position.y, -0.8f);
			PrefabPooler.GetFreeFromPool(splashPrefab, splashPosition, transform.rotation);
			SoundManager.PlaySplashClip();
		}
		private void OnHit()
		{
			circleCollider.enabled = false;
			particles.DisableEmission();
			spriteRenderer.enabled = false;
			hasHit = true;
			Vector3 explosionPosition = new Vector3(transform.position.x, transform.position.y, -0.8f);
			PrefabPooler.GetFreeFromPool(explosionPrefab, explosionPosition, transform.rotation);
			SoundManager.PlayHitClip();
		}
		private void CleanUp()
		{
			localRigidbody.velocity = Vector3.zero;
			localRigidbody.angularVelocity = 0;
			localRigidbody.gravityScale = originalGravity;
			circleCollider.enabled = true;
			particles.EnableEmission();
			spriteRenderer.enabled = true;
			spriteRenderer.color = Color.white;
			hasHit = false;
			hasEnteredWater = false;
			timeToArm = 0f;
		}
	}
}
