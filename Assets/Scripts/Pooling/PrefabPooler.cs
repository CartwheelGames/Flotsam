using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace Project.Pooler
{
	public sealed class PrefabPooler : MonoBehaviour
	{
		private static PrefabPooler instance;
		private Dictionary<int, Pool> pools = new Dictionary<int, Pool>();
		private const int defaultPoolCapacity = 16;
		private void Awake()
		{
			instance = this;
		}
		private void RecycleAll()
		{
			foreach (KeyValuePair<int, Pool> kvp in instance.pools)
			{
				kvp.Value.RecycleAll();
			}
		}
		public static void InitializePrefabIntoPool(GameObject prefab, int capacity = defaultPoolCapacity)
		{
			int poolID = prefab.GetHashCode();
			if (!instance.pools.ContainsKey(poolID))
			{
				Pool newPool = new Pool(prefab, poolID, capacity, instance.transform);
				instance.pools.Add(poolID, newPool);
			}
		}
		public static GameObject GetFreeFromPool(GameObject prefab)
		{
			return GetFreeFromPool(prefab, Vector3.zero, Quaternion.identity);
		}
		public static GameObject GetFreeFromPool(GameObject prefab, Vector3 position)
		{
			return GetFreeFromPool(prefab, position, Quaternion.identity);
		}
		public static GameObject GetFreeFromPool(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent = null)
		{
			int poolID = prefab.GetHashCode();
			if (instance.pools.ContainsKey(poolID))
			{
				GameObject objectFromPool = instance.pools[poolID].GetFree();
				if (objectFromPool == null)
				{
					return GetFreeFromPool(prefab);
				}
				objectFromPool.transform.position = position;
				objectFromPool.transform.rotation = rotation;
				objectFromPool.transform.SetParent(parent);
				objectFromPool.SetActive(true);
				return objectFromPool;
			}
			else
			{
				InitializePrefabIntoPool(prefab);
				return GetFreeFromPool(prefab, position, rotation, parent);
			}
		}
		public static void RecycleToPool(int poolID, GameObject item)
		{
			if (item != null)
			{
				if (instance.pools.ContainsKey(poolID))
				{
					instance.pools[poolID].Recycle(item);
				}
				else
				{
					Debug.LogWarningFormat("No pool exists for hash {0}, cannot recycle {1}", poolID, item.name);
				}
			}
			else
			{
				Debug.LogWarning("Could not recycle null item.");
			}
		}
		public static void RemoveFromPool(int poolID, GameObject item)
		{
			if (item != null)
			{
				if (instance.pools.ContainsKey(poolID))
				{
					instance.pools[poolID].Recycle(item);
				}
				else
				{
					Debug.LogWarningFormat("No pool exists for hash {0}, cannot remove {1}.", poolID, item.name);
				}
			}
			else
			{
				Debug.LogWarning("Could not remove null item.");
			}
		}
		public class Pool
		{
			private List<GameObject> recycledInstances;
			private HashSet<GameObject> allInstances;
			private GameObject prefab;
			private int poolID;
			private int capacity;
			private Transform defaultParent;
			private int index = 0;
			public Pool(GameObject prefab, int poolID, int capacity, Transform defaultParent)
			{
				this.prefab = prefab;
				this.poolID = poolID;
				this.capacity = capacity;
				this.defaultParent = defaultParent;
				recycledInstances = new List<GameObject>(capacity);
				allInstances = new HashSet<GameObject>();
			}
			public bool ContainsItem(GameObject item)
			{
				return allInstances.Contains(item);
			}
			private GameObject GenerateInstance()
			{
				GameObject newInstance = Instantiate(prefab, defaultParent) as GameObject;
				newInstance.name = string.Format("{0} {1}", prefab.name, index);
				index++;
				newInstance.SetActive(false);
				allInstances.Add(newInstance);
				PoolableObject poolableObject = newInstance.GetComponent<PoolableObject>();
				if (poolableObject != null)
				{
					poolableObject.Initialize(poolID);
				}
				return newInstance;
			}
			public void Remove(GameObject item)
			{
				if (allInstances.Contains(item))
				{
					allInstances.Remove(item);
				}
				if (recycledInstances.Contains(item))
				{
					recycledInstances.Remove(item);
				}
			}
			public void Recycle(GameObject item)
			{
				if (recycledInstances.Count < capacity)
				{
					if (!recycledInstances.Contains(item))
					{
						item.SetActive(false);
						item.transform.SetParent(defaultParent);
						recycledInstances.Add(item);
					}
				}
				else
				{
					Remove(item);
					Destroy(item);
				}
			}
			public void RecycleAll()
			{
				foreach (GameObject item in allInstances)
				{
					if (!recycledInstances.Contains(item))
					{
						Recycle(item);
					}
				}
			}
			public GameObject GetFree()
			{
				GameObject item = null;
				if (recycledInstances.Count > 0)
				{
					do
					{
						item = recycledInstances[0];
						recycledInstances.RemoveAt(0);
					}
					while (item == null && recycledInstances.Count > 0);
				}
				if (item == null)
				{
					item = GenerateInstance();
				}
				return item;
			}
			public int Count
			{
				get
				{
					return recycledInstances.Count;
				}
			}
			public bool Contains(GameObject target)
			{
				return recycledInstances.Contains(target);
			}
		}
	}
}
