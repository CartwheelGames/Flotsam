using UnityEngine;
using System.Collections;

namespace Project.Pooler
{
	public class PoolableObject : MonoBehaviour
	{
		protected int poolID { get; private set; }
		public void Initialize(int prefabHash)
		{
			this.poolID = prefabHash;
		}
		public void RecycleToPool()
		{
			PrefabPooler.RecycleToPool(poolID, gameObject);
		}
	}
}
