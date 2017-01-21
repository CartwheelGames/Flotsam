using UnityEngine;
using System.Collections;
using System.Linq;
namespace Project.Pooler
{
	public class SinglePoolableFX : PoolableObject
	{
		[SerializeField]
		private ParticleSystem particle;
		private bool hasStartedPlaying = false;
		public float defaultEmissionRate { get; private set; }
		public float defaultStartSpeed { get; private set; }
		public float defaultStartLifetime { get; private set; }
		public int defaultMaxParticles { get; private set; }
		private bool isLooping;
		public float EmissionRate
		{
			get { return particle.GetEmissionRate(); }
			set
			{
				particle.SetEmissionRate(value);
			}
		}
		public float StartSpeed
		{
			get { return particle.startSpeed; }
			set
			{
				particle.startSpeed = value;
			}
		}
		public float StartLifetime
		{
			get { return particle.startLifetime; }
			set
			{
				particle.startLifetime = value;
			}
		}
		public int MaxParticles
		{
			get { return particle.maxParticles; }
			set
			{
				particle.maxParticles = value;
			}
		}
		private void Awake()
		{
			defaultEmissionRate = EmissionRate;
			defaultStartSpeed = StartSpeed;
			defaultMaxParticles = MaxParticles;
			defaultStartLifetime = StartLifetime;
		}
		private void OnEnable()
		{
			hasStartedPlaying = false;
		}
		private void OnDisable()
		{
			particle.Clear();
		}
		private void OnDestroy()
		{
			PrefabPooler.RemoveFromPool(poolID, gameObject);
		}
		public void EnableEmission()
		{
			particle.EnableEmission();
		}
		public void DisableEmission()
		{
			particle.DisableEmission();
		}
		public void Emit(int count)
		{
			particle.Emit(count);
		}
		private void Update()	//TODO iterative fx manager calls a refresh function, not update
		{
			if (!hasStartedPlaying)
			{
				if (particle.isPlaying && particle.IsAlive() && particle.particleCount > 0)
				{
					hasStartedPlaying = true;
				}
			}
			else
			{
				if (!particle.isPlaying || !particle.IsAlive() || particle.particleCount == 0)
				{
					RecycleToPool();
				}
			}
		}
	}
}
