using UnityEngine;
using System.Collections;
namespace Project
{
	public enum Layer
	{
		Default = 0,
		TransparentFX = 1,
		IgnoreRaycast = 2,
		Water = 4,
		UI = 4,
		Boat = 8,
		Projectile = 9,
		Environment = 10
	}
	internal static class LayerExensions
	{
		public static int ToIndex(this Layer layer)
		{
			return (int)layer;
		}
		public static int ToMask(this Layer layer)
		{
			return 1 << (int)layer;
		}
	}
}
