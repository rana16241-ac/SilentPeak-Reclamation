using UnityEngine;

namespace SilentPeak.Enemy
{
    /// <summary>
    /// Base class for all enemy types
    /// Handles health, death, and alert mechanics
    /// </summary>
    public class EnemyBase : MonoBehaviour
    {
        [Header("Enemy Settings")]
        public EnemyType enemyType;
        public int health = 100;
        public float alertRadius = 20f;
        public float alertDelay = 0.5f;

        [Header("Visual")]
        public Renderer enemyRenderer;
        public GameObject deathEffect;
        public GameObject alertIndicator;

        [Header("Animation")]
        public Animator animator;

        protected bool isDead = false;
        protected bool isAlerted = false;

        protected virtual void Start()
        {
            if (enemyRenderer == null)
                enemyRenderer = GetComponentInChildren<Renderer>();

            if (animator == null)
                animator = GetComponent<Animator>();

            if (alertIndicator != null)
                alertIndicator.SetActive(false);
        }

        /// <summary>
        /// Take damage from sniper shot
        /// </summary>
        public virtual void TakeDamage(bool isHeadshot)
        {
            if (isDead) return;

            // Apply damage multiplier from player upgrades
            int damage = 100;
            if (Core.DataManager.Instance?.playerData != null)
            {
                damage += Core.DataManager.Instance.playerData.damageLevel * 10;
            }

            // Headshot is instant kill
            if (isHeadshot)
            {
                damage = health;
            }

            health -= damage;

            if (health <= 0)
            {
                Die(isHeadshot);
            }
            else
            {
                // Enemy survived - trigger alert
                TriggerAlert();
            }
        }

        /// <summary>
        /// Handle enemy death
        /// </summary>
        protected virtual void Die(bool wasHeadshot)
        {
            if (isDead) return;
            isDead = true;

            // Play death animation
            if (animator != null)
            {
                animator.SetTrigger("Die");
            }

            // Notify game manager
            if (Core.GameManager.Instance != null)
            {
                Core.GameManager.Instance.EnemyKilled(wasHeadshot);
            }

            // Play sound
            if (Core.AudioManager.Instance != null)
            {
                Core.AudioManager.Instance.PlayEnemyHit(wasHeadshot);
            }

            // Spawn death effect
            if (deathEffect != null)
            {
                Instantiate(deathEffect, transform.position, Quaternion.identity);
            }

            // Destroy after delay
            Destroy(gameObject, 2f);
        }

        /// <summary>
        /// Trigger alert system
        /// </summary>
        protected virtual void TriggerAlert()
        {
            if (isAlerted || isDead) return;
            isAlerted = true;

            // Show alert indicator
            if (alertIndicator != null)
            {
                alertIndicator.SetActive(true);
            }

            // Play alert animation
            if (animator != null)
            {
                animator.SetTrigger("Alert");
            }

            // Trigger game alarm after delay
            Invoke(nameof(ActivateAlarm), alertDelay);
        }

        /// <summary>
        /// Activate the base alarm
        /// </summary>
        private void ActivateAlarm()
        {
            if (Core.GameManager.Instance != null && !isDead)
            {
                Core.GameManager.Instance.TriggerAlarm();
            }
        }

        /// <summary>
        /// Get enemy type specific behavior
        /// </summary>
        public virtual string GetEnemyTypeName()
        {
            return enemyType.ToString();
        }

        protected virtual void OnDrawGizmosSelected()
        {
            // Draw alert radius
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, alertRadius);
        }
    }

    /// <summary>
    /// Enemy type enumeration
    /// </summary>
    public enum EnemyType
    {
        GuardSoldier,       // Static guard
        PatrolSoldier,      // Moving patrol
        HeavySoldier,       // Slower, tougher
        SniperGuard,        // Long-range enemy
        Commander,          // Alerts base faster
        EliteRebel          // Late-game only
    }
}
