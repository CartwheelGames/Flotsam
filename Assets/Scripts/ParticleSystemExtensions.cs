using UnityEngine;
using System.Collections;
using Particle = UnityEngine.ParticleSystem.Particle;
public static class ParticleSystemExtensions
{
    public static void EnableEmission(this ParticleSystem particles)
    {
        ParticleSystem.EmissionModule emissionModule = particles.emission;
        if (!emissionModule.enabled)
        {
            emissionModule.enabled = true;
        }
    }
    public static void DisableEmission(this ParticleSystem particles)
    {
        ParticleSystem.EmissionModule emissionModule = particles.emission;
        if (emissionModule.enabled)
        {
            emissionModule.enabled = false;
        }
    }
    public static void SetEmissionRate(this ParticleSystem particles, float rate)
    {
		if (particles.emission.rateOverDistance.constant != rate)
		{
			ParticleSystem.EmissionModule emissionModule = particles.emission;
			ParticleSystem.MinMaxCurve constantCurve = new ParticleSystem.MinMaxCurve(rate);
			emissionModule.rateOverDistance = constantCurve;
		}
    }
    public static float GetEmissionRate(this ParticleSystem particles)
    {
        ParticleSystem.EmissionModule emissionModule = particles.emission;
        return emissionModule.rateOverDistance.constantMax;
    }
	public static float GetStartLifetime(this ParticleSystem particles)
	{
		ParticleSystem.MainModule main = particles.main;
		return main.startLifetime.constant;
	}
	public static void SetStartLifetime(this ParticleSystem particles, float value)
	{
		ParticleSystem.MainModule main = particles.main;
		main.startLifetime = new ParticleSystem.MinMaxCurve(value);
	}
	public static int GetMaxParticles(this ParticleSystem particles)
	{
		ParticleSystem.MainModule main = particles.main;
		return main.maxParticles;
	}
	public static void SetMaxParticles(this ParticleSystem particles, int value)
	{
		ParticleSystem.MainModule main = particles.main;
		main.maxParticles = value;
	}
	public static float GetStartSpeed(this ParticleSystem particles)
	{
		return particles.main.startSpeed.constant;
	}
	public static void SetStartSpeed(this ParticleSystem particles, float value)
	{
		ParticleSystem.MainModule main = particles.main;
		main.startSpeed = value;
	}
}
