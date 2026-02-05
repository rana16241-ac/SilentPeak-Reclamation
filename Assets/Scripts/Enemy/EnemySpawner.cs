using UnityEngine;
using System.Collections.Generic;

namespace SilentPeak.Enemy
{
    /// <summary>
    /// Spawns enemies for each mission based on level configuration
    /// Handles enemy placement and distribution
    /// </summary>
    public class EnemySpawner : MonoBehaviour
    {
        [Header("Enemy Prefabs")]
        public GameObject guardSoldierPrefab;
        public GameObject patrolSoldierPrefab;
        public GameObject heavySoldierPrefab;
        public GameObject sniperGuardPrefab;
        public GameObject commanderPrefab;
        public GameObject eliteRebelPrefab;

        [Header("Spawn Points")]
        public Transform[] towerSpawnPoints;
        public Transform[] rooftopSpawnPoints;
        public Transform[] groundSpawnPoints;
        public Transform[] patrolStartPoints;

        [Header("Spawn Settings")]
        public float spawnDelay = 0.5f;

        private List<GameObject> spawnedEnemies = new List<GameObject>();

        /// <summary>
        /// Spawn enemies for current mission
        /// </summary>
        public void SpawnEnemiesForMission(int baseNumber, int subLevel)
        {
            ClearExistingEnemies();

            int enemyCount = GetEnemyCountForLevel(subLevel);
            SpawnEnemies(enemyCount, baseNumber, subLevel);
        }

        /// <summary>
        /// Get enemy count based on sub-level
        /// </summary>
        private int GetEnemyCountForLevel(int subLevel)
        {
            int[] enemyCounts = { 3, 7, 10, 15, 20, 24, 27, 30 };
            return enemyCounts[Mathf.Clamp(subLevel - 1, 0, enemyCounts.Length - 1)];
        }

        /// <summary>
        /// Spawn enemies with appropriate distribution
        /// </summary>
        private void SpawnEnemies(int count, int baseNumber, int subLevel)
        {
            // Calculate enemy type distribution
            int guards = Mathf.CeilToInt(count * 0.4f);
            int patrols = Mathf.CeilToInt(count * 0.3f);
            int heavies = Mathf.CeilToInt(count * 0.15f);
            int snipers = Mathf.CeilToInt(count * 0.1f);
            int commanders = Mathf.Max(1, Mathf.CeilToInt(count * 0.05f));
            int elites = (subLevel >= 6) ? Mathf.CeilToInt(count * 0.05f) : 0;

            // Adjust to match exact count
            int total = guards + patrols + heavies + snipers + commanders + elites;
            if (total > count)
            {
                guards -= (total - count);
            }

            // Spawn each type
            StartCoroutine(SpawnEnemiesCoroutine(guards, patrols, heavies, snipers, commanders, elites));
        }

        /// <summary>
        /// Coroutine to spawn enemies with delay
        /// </summary>
        private System.Collections.IEnumerator SpawnEnemiesCoroutine(int guards, int patrols, int heavies, int snipers, int commanders, int elites)
        {
            // Spawn guards on towers and rooftops
            for (int i = 0; i < guards; i++)
            {
                SpawnEnemy(guardSoldierPrefab, GetRandomSpawnPoint(towerSpawnPoints, rooftopSpawnPoints));
                yield return new WaitForSeconds(spawnDelay);
            }

            // Spawn patrols on ground
            for (int i = 0; i < patrols; i++)
            {
                SpawnPatrolEnemy(patrolSoldierPrefab, GetRandomSpawnPoint(patrolStartPoints, groundSpawnPoints));
                yield return new WaitForSeconds(spawnDelay);
            }

            // Spawn heavies on ground
            for (int i = 0; i < heavies; i++)
            {
                SpawnEnemy(heavySoldierPrefab, GetRandomSpawnPoint(groundSpawnPoints));
                yield return new WaitForSeconds(spawnDelay);
            }

            // Spawn snipers on towers
            for (int i = 0; i < snipers; i++)
            {
                SpawnEnemy(sniperGuardPrefab, GetRandomSpawnPoint(towerSpawnPoints));
                yield return new WaitForSeconds(spawnDelay);
            }

            // Spawn commanders
            for (int i = 0; i < commanders; i++)
            {
                SpawnEnemy(commanderPrefab, GetRandomSpawnPoint(rooftopSpawnPoints, groundSpawnPoints));
                yield return new WaitForSeconds(spawnDelay);
            }

            // Spawn elites (late game only)
            for (int i = 0; i < elites; i++)
            {
                SpawnEnemy(eliteRebelPrefab, GetRandomSpawnPoint(towerSpawnPoints, rooftopSpawnPoints));
                yield return new WaitForSeconds(spawnDelay);
            }
        }

        /// <summary>
        /// Spawn a single enemy
        /// </summary>
        private void SpawnEnemy(GameObject prefab, Vector3 position)
        {
            if (prefab == null) return;

            GameObject enemy = Instantiate(prefab, position, Quaternion.identity);
            spawnedEnemies.Add(enemy);
        }

        /// <summary>
        /// Spawn a patrol enemy with waypoints
        /// </summary>
        private void SpawnPatrolEnemy(GameObject prefab, Vector3 position)
        {
            if (prefab == null) return;

            GameObject enemy = Instantiate(prefab, position, Quaternion.identity);
            spawnedEnemies.Add(enemy);

            // Setup patrol waypoints
            EnemyPatrol patrol = enemy.GetComponent<EnemyPatrol>();
            if (patrol != null && patrolStartPoints != null && patrolStartPoints.Length > 1)
            {
                // Assign random patrol route
                int startIndex = Random.Range(0, patrolStartPoints.Length);
                int endIndex = (startIndex + 1) % patrolStartPoints.Length;
                
                patrol.waypoints = new Transform[] 
                { 
                    patrolStartPoints[startIndex], 
                    patrolStartPoints[endIndex] 
                };
            }
        }

        /// <summary>
        /// Get random spawn point from available arrays
        /// </summary>
        private Vector3 GetRandomSpawnPoint(params Transform[][] spawnPointArrays)
        {
            List<Transform> allPoints = new List<Transform>();
            
            foreach (var array in spawnPointArrays)
            {
                if (array != null && array.Length > 0)
                {
                    allPoints.AddRange(array);
                }
            }

            if (allPoints.Count > 0)
            {
                Transform point = allPoints[Random.Range(0, allPoints.Count)];
                return point.position;
            }

            return Vector3.zero;
        }

        /// <summary>
        /// Clear all spawned enemies
        /// </summary>
        private void ClearExistingEnemies()
        {
            foreach (var enemy in spawnedEnemies)
            {
                if (enemy != null)
                {
                    Destroy(enemy);
                }
            }
            spawnedEnemies.Clear();
        }

        /// <summary>
        /// Get all spawned enemies
        /// </summary>
        public List<GameObject> GetSpawnedEnemies()
        {
            return spawnedEnemies;
        }

        private void OnDrawGizmos()
        {
            // Draw spawn points
            DrawSpawnPoints(towerSpawnPoints, Color.red);
            DrawSpawnPoints(rooftopSpawnPoints, Color.yellow);
            DrawSpawnPoints(groundSpawnPoints, Color.green);
            DrawSpawnPoints(patrolStartPoints, Color.blue);
        }

        private void DrawSpawnPoints(Transform[] points, Color color)
        {
            if (points == null) return;

            Gizmos.color = color;
            foreach (var point in points)
            {
                if (point != null)
                {
                    Gizmos.DrawWireSphere(point.position, 0.5f);
                }
            }
        }
    }
}
