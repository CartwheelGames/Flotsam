using UnityEngine;
using System.Collections;
using System.Linq;
namespace Project.Pooler
{
	public class MultiplePoolableFX : PoolableObject
	{
		[SerializeField]
		private ParticleSystem[] particleSystems;
		private bool isCheckingToRecycle = false;
		private bool hasStartedPlaying = false;
		public float EmissionRate
		{
			get { return particleSystems[0].GetEmissionRate(); }
			set
			{
				foreach (ParticleSystem system in particleSystems)
				{
					system.SetEmissionRate(value);
				}
			}
		}
		public float StartSpeed
		{
			get { return particleSystems[0].startSpeed; }
			set
			{
				foreach (ParticleSystem system in particleSystems)
				{
					system.startSpeed = value;
				}
			}
		}
		public float StartLifetime
		{
			get { return particleSystems[0].startLifetime; }
			set
			{
				foreach (ParticleSystem system in particleSystems)
				{
					system.startLifetime = value;
				}
			}
		}
		public int MaxParticles
		{
			get { return particleSystems[0].maxParticles; }
			set
			{
				foreach (ParticleSystem system in particleSystems)
				{
					system.maxParticles = value;
				}
			}
		}
		private void OnEnable()
		{
			hasStartedPlaying = false;
		}
		private void OnDestroy()
		{
			PrefabPooler.RemoveFromPool(poolID, gameObject);
		}
		public void EnableEmission()
		{
			foreach (ParticleSystem system in particleSystems)
			{
				system.EnableEmission();
			}
		}
		public void DisableEmission()
		{
			foreach (ParticleSystem system in particleSystems)
			{
				system.DisableEmission();
			}
		}
		public void Emit(int count)
		{
			foreach (ParticleSystem system in particleSystems)
			{
				system.Emit(count);
			}
		}
		private void Update()	//TODO iterative fx manager calls a refresh function, not update
		{
			if (!hasStartedPlaying)
			{
				if (particleSystems.Any(p => p.isPlaying) && particleSystems[0].particleCount > 0)
				{
					hasStartedPlaying = true;
				}
			}
			else
			{
				if (!particleSystems[0].IsAlive(true) || particleSystems[0].particleCount == 0)
				{
					RecycleToPool();
				}
			}
		}
	}
}
