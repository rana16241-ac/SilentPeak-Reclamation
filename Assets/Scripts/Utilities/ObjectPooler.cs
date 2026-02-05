using UnityEngine;
using System.Collections.Generic;

namespace SilentPeak.Utilities
{
    /// <summary>
    /// Object pooling system for performance optimization
    /// Reuses objects instead of instantiating/destroying
    /// </summary>
    public class ObjectPooler : MonoBehaviour
    {
        public static ObjectPooler Instance { get; private set; }

        [System.Serializable]
        public class Pool
        {
            public string tag;
            public GameObject prefab;
            public int size;
        }

        [Header("Pools")]
        public List<Pool> pools;

        private Dictionary<string, Queue<GameObject>> poolDictionary;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
                return;
            }

            InitializePools();
        }

        /// <summary>
        /// Initialize all object pools
        /// </summary>
        private void InitializePools()
        {
            poolDictionary = new Dictionary<string, Queue<GameObject>>();

            foreach (Pool pool in pools)
            {
                Queue<GameObject> objectPool = new Queue<GameObject>();

                for (int i = 0; i < pool.size; i++)
                {
                    GameObject obj = Instantiate(pool.prefab);
                    obj.SetActive(false);
                    obj.transform.SetParent(transform);
                    objectPool.Enqueue(obj);
                }

                poolDictionary.Add(pool.tag, objectPool);
            }
        }

        /// <summary>
        /// Spawn object from pool
        /// </summary>
        public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
        {
            if (!poolDictionary.ContainsKey(tag))
            {
                Debug.LogWarning($"Pool with tag {tag} doesn't exist.");
                return null;
            }

            GameObject objectToSpawn = poolDictionary[tag].Dequeue();

            objectToSpawn.SetActive(true);
            objectToSpawn.transform.position = position;
            objectToSpawn.transform.rotation = rotation;

            poolDictionary[tag].Enqueue(objectToSpawn);

            return objectToSpawn;
        }

        /// <summary>
        /// Return object to pool
        /// </summary>
        public void ReturnToPool(GameObject obj)
        {
            obj.SetActive(false);
            obj.transform.SetParent(transform);
        }

        /// <summary>
        /// Clear all pools
        /// </summary>
        public void ClearPools()
        {
            foreach (var pool in poolDictionary.Values)
            {
                foreach (var obj in pool)
                {
                    if (obj != null)
                    {
                        Destroy(obj);
                    }
                }
                pool.Clear();
            }
            poolDictionary.Clear();
        }
    }
}
