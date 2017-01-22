using System.Collections;
using UnityEngine;
namespace Project
{
	public class SoundManager : MonoBehaviour
	{
		private static SoundManager instance;
		[SerializeField]
		private AudioSource source = null;
		[SerializeField]
		private AudioClip deathClip = null;
		[SerializeField]
		private AudioClip fireClip = null;
		[SerializeField]
		private AudioClip hitClip = null;
		[SerializeField]
		private AudioClip splashClip = null;
		public void Awake()
		{
			instance = this;
		}
		public static void PlayDeathClip()
		{
			instance.source.PlayOneShot(instance.deathClip);
		}
		public static void PlayFireClip()
		{
			instance.source.PlayOneShot(instance.fireClip);
		}
		public static void PlayHitClip()
		{
			instance.source.PlayOneShot(instance.hitClip);
		}
		public static void PlaySplashClip()
		{
			instance.source.PlayOneShot(instance.splashClip);
		}
	}
}
