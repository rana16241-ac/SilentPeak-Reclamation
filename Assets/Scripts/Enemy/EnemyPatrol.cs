using UnityEngine;

namespace SilentPeak.Enemy
{
    /// <summary>
    /// Patrol enemy that moves between waypoints
    /// Inherits from EnemyBase
    /// </summary>
    public class EnemyPatrol : EnemyBase
    {
        [Header("Patrol Settings")]
        public Transform[] waypoints;
        public float moveSpeed = 2f;
        public float waypointReachDistance = 0.5f;
        public float waitTimeAtWaypoint = 2f;

        private int currentWaypointIndex = 0;
        private float waitTimer = 0f;
        private bool isWaiting = false;

        protected override void Start()
        {
            base.Start();
            enemyType = EnemyType.PatrolSoldier;

            // If no waypoints assigned, create simple back-and-forth patrol
            if (waypoints == null || waypoints.Length == 0)
            {
                CreateDefaultWaypoints();
            }
        }

        private void Update()
        {
            if (isDead || isAlerted) return;

            if (isWaiting)
            {
                waitTimer -= Time.deltaTime;
                if (waitTimer <= 0)
                {
                    isWaiting = false;
                    MoveToNextWaypoint();
                }
            }
            else
            {
                Patrol();
            }
        }

        /// <summary>
        /// Patrol between waypoints
        /// </summary>
        private void Patrol()
        {
            if (waypoints == null || waypoints.Length == 0) return;

            Transform targetWaypoint = waypoints[currentWaypointIndex];
            
            // Move towards waypoint
            Vector3 direction = (targetWaypoint.position - transform.position).normalized;
            transform.position += direction * moveSpeed * Time.deltaTime;

            // Rotate to face movement direction
            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
            }

            // Check if reached waypoint
            float distance = Vector3.Distance(transform.position, targetWaypoint.position);
            if (distance < waypointReachDistance)
            {
                isWaiting = true;
                waitTimer = waitTimeAtWaypoint;
            }

            // Update animation
            if (animator != null)
            {
                animator.SetBool("IsWalking", true);
            }
        }

        /// <summary>
        /// Move to next waypoint in sequence
        /// </summary>
        private void MoveToNextWaypoint()
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        }

        /// <summary>
        /// Create default waypoints if none assigned
        /// </summary>
        private void CreateDefaultWaypoints()
        {
            GameObject waypointsParent = new GameObject($"{gameObject.name}_Waypoints");
            waypointsParent.transform.position = transform.position;

            waypoints = new Transform[2];
            
            // Create two waypoints for simple back-and-forth
            GameObject wp1 = new GameObject("Waypoint1");
            wp1.transform.position = transform.position;
            wp1.transform.SetParent(waypointsParent.transform);
            waypoints[0] = wp1.transform;

            GameObject wp2 = new GameObject("Waypoint2");
            wp2.transform.position = transform.position + transform.forward * 10f;
            wp2.transform.SetParent(waypointsParent.transform);
            waypoints[1] = wp2.transform;
        }

        /// <summary>
        /// Override alert to stop patrolling
        /// </summary>
        protected override void TriggerAlert()
        {
            base.TriggerAlert();
            
            // Stop patrolling
            if (animator != null)
            {
                animator.SetBool("IsWalking", false);
            }
        }

        protected override void OnDrawGizmosSelected()
        {
            base.OnDrawGizmosSelected();

            // Draw patrol path
            if (waypoints != null && waypoints.Length > 1)
            {
                Gizmos.color = Color.yellow;
                for (int i = 0; i < waypoints.Length; i++)
                {
                    if (waypoints[i] != null)
                    {
                        Gizmos.DrawSphere(waypoints[i].position, 0.3f);
                        
                        int nextIndex = (i + 1) % waypoints.Length;
                        if (waypoints[nextIndex] != null)
                        {
                            Gizmos.DrawLine(waypoints[i].position, waypoints[nextIndex].position);
                        }
                    }
                }
            }
        }
    }
}
