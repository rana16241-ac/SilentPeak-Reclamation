using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace SilentPeak.Core
{
    /// <summary>
    /// Manages all UI elements during gameplay
    /// Handles HUD updates, mission panels, and UI interactions
    /// </summary>
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance { get; private set; }

        [Header("HUD Elements")]
        public TextMeshProUGUI enemiesRemainingText;
        public TextMeshProUGUI coinsText;
        public TextMeshProUGUI headshotsText;
        public TextMeshProUGUI missionTimeText;
        public GameObject crosshair;
        public Image scopeOverlay;

        [Header("Mission Panels")]
        public GameObject missionCompletePanel;
        public GameObject missionFailedPanel;
        public GameObject pausePanel;

        [Header("Mission Complete UI")]
        public TextMeshProUGUI rewardCoinsText;
        public TextMeshProUGUI missionTimeCompleteText;
        public Image[] starsImages;
        public TextMeshProUGUI headshotsCompleteText;

        [Header("Buttons")]
        public Button pauseButton;
        public Button resumeButton;
        public Button restartButton;
        public Button mainMenuButton;
        public Button nextLevelButton;

        [Header("Scope UI")]
        public GameObject scopeUI;
        public TextMeshProUGUI zoomLevelText;
        public Image breathMeter;

        private float missionStartTime;
        private bool timerRunning = false;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            InitializeUI();
            SetupButtons();
            
            if (GameManager.Instance != null && GameManager.Instance.missionActive)
            {
                StartMissionTimer();
            }
        }

        private void Update()
        {
            if (timerRunning)
            {
                UpdateMissionTimer();
            }
        }

        /// <summary>
        /// Initialize UI elements
        /// </summary>
        private void InitializeUI()
        {
            // Hide panels initially
            if (missionCompletePanel != null)
                missionCompletePanel.SetActive(false);
            
            if (missionFailedPanel != null)
                missionFailedPanel.SetActive(false);
            
            if (pausePanel != null)
                pausePanel.SetActive(false);
            
            if (scopeUI != null)
                scopeUI.SetActive(false);

            UpdateCoinsDisplay();
            
            if (GameManager.Instance != null)
            {
                UpdateEnemiesRemaining(GameManager.Instance.enemiesRemaining);
            }
        }

        /// <summary>
        /// Setup button listeners
        /// </summary>
        private void SetupButtons()
        {
            if (pauseButton != null)
                pauseButton.onClick.AddListener(PauseGame);
            
            if (resumeButton != null)
                resumeButton.onClick.AddListener(ResumeGame);
            
            if (restartButton != null)
                restartButton.onClick.AddListener(RestartMission);
            
            if (mainMenuButton != null)
                mainMenuButton.onClick.AddListener(ReturnToMainMenu);
            
            if (nextLevelButton != null)
                nextLevelButton.onClick.AddListener(LoadNextLevel);
        }

        /// <summary>
        /// Update enemies remaining display
        /// </summary>
        public void UpdateEnemiesRemaining(int count)
        {
            if (enemiesRemainingText != null)
            {
                enemiesRemainingText.text = $"Enemies: {count}/{GameManager.Instance.totalEnemies}";
            }
        }

        /// <summary>
        /// Update coins display
        /// </summary>
        public void UpdateCoinsDisplay()
        {
            if (coinsText != null && DataManager.Instance != null && DataManager.Instance.playerData != null)
            {
                coinsText.text = DataManager.Instance.playerData.coins.ToString();
            }
        }

        /// <summary>
        /// Update headshots display
        /// </summary>
        public void UpdateHeadshotsDisplay(int headshots)
        {
            if (headshotsText != null)
            {
                headshotsText.text = $"Headshots: {headshots}";
            }
        }

        /// <summary>
        /// Start mission timer
        /// </summary>
        public void StartMissionTimer()
        {
            missionStartTime = Time.time;
            timerRunning = true;
        }

        /// <summary>
        /// Update mission timer display
        /// </summary>
        private void UpdateMissionTimer()
        {
            if (missionTimeText != null)
            {
                float elapsed = Time.time - missionStartTime;
                int minutes = Mathf.FloorToInt(elapsed / 60f);
                int seconds = Mathf.FloorToInt(elapsed % 60f);
                missionTimeText.text = $"{minutes:00}:{seconds:00}";
            }
        }

        /// <summary>
        /// Show mission complete panel
        /// </summary>
        public void ShowMissionComplete(int coinsEarned, int stars, int completionTime)
        {
            timerRunning = false;
            
            if (missionCompletePanel != null)
            {
                missionCompletePanel.SetActive(true);
                
                if (rewardCoinsText != null)
                    rewardCoinsText.text = $"+{coinsEarned}";
                
                if (missionTimeCompleteText != null)
                {
                    int minutes = completionTime / 60;
                    int seconds = completionTime % 60;
                    missionTimeCompleteText.text = $"{minutes:00}:{seconds:00}";
                }
                
                if (headshotsCompleteText != null && GameManager.Instance != null)
                    headshotsCompleteText.text = $"{GameManager.Instance.headshots}/{GameManager.Instance.totalEnemies}";
                
                // Display stars
                DisplayStars(stars);
            }
        }

        /// <summary>
        /// Display star rating
        /// </summary>
        private void DisplayStars(int stars)
        {
            if (starsImages == null || starsImages.Length == 0) return;

            for (int i = 0; i < starsImages.Length; i++)
            {
                if (starsImages[i] != null)
                {
                    starsImages[i].enabled = i < stars;
                }
            }
        }

        /// <summary>
        /// Show mission failed panel
        /// </summary>
        public void ShowMissionFailed()
        {
            timerRunning = false;
            
            if (missionFailedPanel != null)
            {
                missionFailedPanel.SetActive(true);
            }
        }

        /// <summary>
        /// Show/hide scope UI
        /// </summary>
        public void ToggleScopeUI(bool show)
        {
            if (scopeUI != null)
            {
                scopeUI.SetActive(show);
            }
            
            if (crosshair != null)
            {
                crosshair.SetActive(!show);
            }
        }

        /// <summary>
        /// Update zoom level display
        /// </summary>
        public void UpdateZoomLevel(float zoom)
        {
            if (zoomLevelText != null)
            {
                zoomLevelText.text = $"{zoom:F1}x";
            }
        }

        /// <summary>
        /// Update breath meter
        /// </summary>
        public void UpdateBreathMeter(float value)
        {
            if (breathMeter != null)
            {
                breathMeter.fillAmount = value;
            }
        }

        /// <summary>
        /// Pause game
        /// </summary>
        private void PauseGame()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.PauseGame();
            }
            
            if (pausePanel != null)
            {
                pausePanel.SetActive(true);
            }
            
            AudioManager.Instance?.PlayButtonClick();
        }

        /// <summary>
        /// Resume game
        /// </summary>
        private void ResumeGame()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.ResumeGame();
            }
            
            if (pausePanel != null)
            {
                pausePanel.SetActive(false);
            }
            
            AudioManager.Instance?.PlayButtonClick();
        }

        /// <summary>
        /// Restart mission
        /// </summary>
        private void RestartMission()
        {
            AudioManager.Instance?.PlayButtonClick();
            
            if (GameManager.Instance != null)
            {
                GameManager.Instance.RestartMission();
            }
        }

        /// <summary>
        /// Return to main menu
        /// </summary>
        private void ReturnToMainMenu()
        {
            AudioManager.Instance?.PlayButtonClick();
            
            if (GameManager.Instance != null)
            {
                GameManager.Instance.ReturnToMainMenu();
            }
        }

        /// <summary>
        /// Load next level
        /// </summary>
        private void LoadNextLevel()
        {
            AudioManager.Instance?.PlayButtonClick();
            
            if (GameManager.Instance != null)
            {
                int nextSubLevel = GameManager.Instance.currentSubLevel + 1;
                int nextBase = GameManager.Instance.currentBase;
                
                if (nextSubLevel > 8)
                {
                    nextSubLevel = 1;
                    nextBase++;
                }
                
                GameManager.Instance.StartMission(nextBase, nextSubLevel);
            }
        }

        /// <summary>
        /// Show notification message
        /// </summary>
        public void ShowNotification(string message, float duration = 2f)
        {
            // TODO: Implement notification system
            Debug.Log($"Notification: {message}");
        }
    }
}
