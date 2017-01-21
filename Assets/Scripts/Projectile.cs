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
		}
		private void OnDisable()
		{
			CleanUp();
		}
		private void Update()
		{
			if ((hasHit || !spriteRenderer.isVisible) && particles.particleCount == 0)
			{
				RecycleToPool();
			}
		}
		private void OnCollisionEnter2D(Collision2D col)
		{
			if (!hasEnteredWater && col.gameObject.layer == Layer.Water.ToIndex())
			{
				localRigidbody.velocity *= 0.1f;
				localRigidbody.angularVelocity *= 0.1f;
				localRigidbody.gravityScale = 0.075f;
				circleCollider.enabled = false;
				particles.DisableEmission();
				spriteRenderer.color = Color.gray;
				hasEnteredWater = true;
			}
		}
		private void CleanUp()
		{
			localRigidbody.velocity = Vector3.zero;
			localRigidbody.angularVelocity = 0;
			localRigidbody.gravityScale = originalGravity;
			circleCollider.enabled = true;
			particles.EnableEmission();
			spriteRenderer.color = Color.white;
			hasHit = false;
			hasEnteredWater = false;
		}
	}
}
