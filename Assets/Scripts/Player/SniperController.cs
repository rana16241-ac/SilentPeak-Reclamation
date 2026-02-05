using UnityEngine;
using UnityEngine.EventSystems;

namespace SilentPeak.Player
{
    /// <summary>
    /// Controls sniper aiming, shooting, and scope mechanics
    /// Handles touch input for mobile devices
    /// </summary>
    public class SniperController : MonoBehaviour
    {
        [Header("Camera")]
        public Camera sniperCamera;
        public Transform cameraTarget;

        [Header("Aiming")]
        public float aimSensitivity = 1f;
        public float maxAimAngleX = 60f;
        public float maxAimAngleY = 45f;
        public LayerMask shootableLayer;

        [Header("Scope")]
        public float[] zoomLevels = { 2f, 4f, 8f, 12f };
        public int currentZoomIndex = 0;
        public float zoomSpeed = 5f;
        private float targetFOV;
        private float defaultFOV = 60f;

        [Header("Breath Control")]
        public float breathDuration = 5f;
        public float breathRecoveryRate = 0.5f;
        public float breathStabilization = 0.8f;
        private float currentBreath = 1f;
        private bool holdingBreath = false;

        [Header("Shooting")]
        public float fireRate = 1f;
        public GameObject bulletImpactEffect;
        public GameObject bloodEffect;
        private float nextFireTime = 0f;

        [Header("Weapon Sway")]
        public float swayAmount = 0.02f;
        public float swaySpeed = 2f;
        private Vector3 swayPosition;

        [Header("UI")]
        public GameObject scopeOverlay;
        public GameObject crosshair;

        private Vector2 aimRotation;
        private bool isScoped = false;
        private Vector2 touchStartPos;
        private bool isDragging = false;

        private void Start()
        {
            if (sniperCamera == null)
                sniperCamera = Camera.main;

            targetFOV = defaultFOV;
            currentBreath = 1f;
            
            // Apply weapon upgrades
            ApplyWeaponUpgrades();
        }

        private void Update()
        {
            HandleInput();
            UpdateAiming();
            UpdateBreath();
            UpdateWeaponSway();
            UpdateScope();
        }

        /// <summary>
        /// Apply weapon upgrades from player data
        /// </summary>
        private void ApplyWeaponUpgrades()
        {
            if (Core.DataManager.Instance?.playerData != null)
            {
                var data = Core.DataManager.Instance.playerData;
                
                // Scope upgrade affects zoom levels
                float scopeMultiplier = 1f + (data.scopeLevel * 0.2f);
                for (int i = 0; i < zoomLevels.Length; i++)
                {
                    zoomLevels[i] *= scopeMultiplier;
                }
                
                // Stability upgrade reduces sway
                swayAmount *= (1f - data.stabilityLevel * 0.15f);
                
                // Reload upgrade affects fire rate
                fireRate *= (1f + data.reloadLevel * 0.1f);
            }
        }

        /// <summary>
        /// Handle touch and mouse input
        /// </summary>
        private void HandleInput()
        {
            // Touch input for mobile
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                
                // Ignore UI touches
                if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                    return;

                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        touchStartPos = touch.position;
                        isDragging = true;
                        break;

                    case TouchPhase.Moved:
                        if (isDragging)
                        {
                            Vector2 delta = touch.deltaPosition;
                            aimRotation.x += delta.x * aimSensitivity * Time.deltaTime;
                            aimRotation.y -= delta.y * aimSensitivity * Time.deltaTime;
                        }
                        break;

                    case TouchPhase.Ended:
                        isDragging = false;
                        break;
                }
            }
            // Mouse input for testing in editor
            else if (Input.GetMouseButton(0))
            {
                if (!EventSystem.current.IsPointerOverGameObject())
                {
                    float mouseX = Input.GetAxis("Mouse X");
                    float mouseY = Input.GetAxis("Mouse Y");
                    
                    aimRotation.x += mouseX * aimSensitivity;
                    aimRotation.y -= mouseY * aimSensitivity;
                }
            }

            // Clamp rotation
            aimRotation.x = Mathf.Clamp(aimRotation.x, -maxAimAngleX, maxAimAngleX);
            aimRotation.y = Mathf.Clamp(aimRotation.y, -maxAimAngleY, maxAimAngleY);
        }

        /// <summary>
        /// Update camera aiming
        /// </summary>
        private void UpdateAiming()
        {
            if (cameraTarget != null)
            {
                Quaternion targetRotation = Quaternion.Euler(aimRotation.y, aimRotation.x, 0);
                cameraTarget.localRotation = Quaternion.Lerp(
                    cameraTarget.localRotation, 
                    targetRotation, 
                    Time.deltaTime * 10f
                );
            }
        }

        /// <summary>
        /// Update breath control system
        /// </summary>
        private void UpdateBreath()
        {
            if (holdingBreath && currentBreath > 0)
            {
                currentBreath -= Time.deltaTime / breathDuration;
                currentBreath = Mathf.Max(0, currentBreath);
            }
            else if (!holdingBreath && currentBreath < 1f)
            {
                currentBreath += Time.deltaTime * breathRecoveryRate;
                currentBreath = Mathf.Min(1f, currentBreath);
            }

            // Update UI
            if (Core.UIManager.Instance != null)
            {
                Core.UIManager.Instance.UpdateBreathMeter(currentBreath);
            }
        }

        /// <summary>
        /// Update weapon sway effect
        /// </summary>
        private void UpdateWeaponSway()
        {
            if (!holdingBreath || currentBreath <= 0)
            {
                float swayX = Mathf.Sin(Time.time * swaySpeed) * swayAmount;
                float swayY = Mathf.Cos(Time.time * swaySpeed * 0.8f) * swayAmount;
                swayPosition = new Vector3(swayX, swayY, 0);
            }
            else
            {
                swayPosition = Vector3.Lerp(swayPosition, Vector3.zero, Time.deltaTime * 5f);
            }

            if (sniperCamera != null)
            {
                sniperCamera.transform.localPosition = swayPosition;
            }
        }

        /// <summary>
        /// Update scope zoom
        /// </summary>
        private void UpdateScope()
        {
            if (isScoped)
            {
                targetFOV = defaultFOV / zoomLevels[currentZoomIndex];
            }
            else
            {
                targetFOV = defaultFOV;
            }

            if (sniperCamera != null)
            {
                sniperCamera.fieldOfView = Mathf.Lerp(
                    sniperCamera.fieldOfView, 
                    targetFOV, 
                    Time.deltaTime * zoomSpeed
                );
            }
        }

        /// <summary>
        /// Fire the sniper rifle
        /// </summary>
        public void Fire()
        {
            if (Time.time < nextFireTime) return;
            if (!Core.GameManager.Instance.missionActive) return;

            nextFireTime = Time.time + 1f / fireRate;

            // Play shot sound
            Core.AudioManager.Instance?.PlaySniperShot();

            // Perform raycast
            Ray ray = sniperCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 1000f, shootableLayer))
            {
                // Check if hit an enemy
                Enemy.EnemyBase enemy = hit.collider.GetComponent<Enemy.EnemyBase>();
                
                if (enemy != null)
                {
                    bool isHeadshot = hit.collider.CompareTag("EnemyHead");
                    enemy.TakeDamage(isHeadshot);
                    
                    // Spawn blood effect
                    if (bloodEffect != null)
                    {
                        Instantiate(bloodEffect, hit.point, Quaternion.LookRotation(hit.normal));
                    }
                }
                else
                {
                    // Missed shot - trigger alarm
                    Core.GameManager.Instance.ShotMissed();
                    
                    // Spawn impact effect
                    if (bulletImpactEffect != null)
                    {
                        Instantiate(bulletImpactEffect, hit.point, Quaternion.LookRotation(hit.normal));
                    }
                }
            }
            else
            {
                // Missed completely
                Core.GameManager.Instance.ShotMissed();
            }

            // Add recoil
            AddRecoil();
        }

        /// <summary>
        /// Add recoil effect
        /// </summary>
        private void AddRecoil()
        {
            float recoilAmount = 2f;
            aimRotation.y += Random.Range(-recoilAmount, recoilAmount * 0.5f);
            aimRotation.x += Random.Range(-recoilAmount * 0.5f, recoilAmount * 0.5f);
        }

        /// <summary>
        /// Toggle scope view
        /// </summary>
        public void ToggleScope()
        {
            isScoped = !isScoped;
            
            if (scopeOverlay != null)
                scopeOverlay.SetActive(isScoped);
            
            if (crosshair != null)
                crosshair.SetActive(!isScoped);
            
            Core.AudioManager.Instance?.PlaySFX(Core.AudioManager.Instance.scopeZoom);
            
            if (Core.UIManager.Instance != null)
            {
                Core.UIManager.Instance.ToggleScopeUI(isScoped);
                Core.UIManager.Instance.UpdateZoomLevel(zoomLevels[currentZoomIndex]);
            }
        }

        /// <summary>
        /// Cycle through zoom levels
        /// </summary>
        public void CycleZoom()
        {
            if (!isScoped) return;

            currentZoomIndex = (currentZoomIndex + 1) % zoomLevels.Length;
            Core.AudioManager.Instance?.PlaySFX(Core.AudioManager.Instance.scopeZoom, 0.5f);
            
            if (Core.UIManager.Instance != null)
            {
                Core.UIManager.Instance.UpdateZoomLevel(zoomLevels[currentZoomIndex]);
            }
        }

        /// <summary>
        /// Hold breath for stability
        /// </summary>
        public void HoldBreath(bool hold)
        {
            holdingBreath = hold && currentBreath > 0;
            
            if (hold && currentBreath > 0)
            {
                Core.AudioManager.Instance?.PlaySFX(Core.AudioManager.Instance.breathHold, 0.3f);
            }
        }
    }
}
