using UnityEngine;
using UnityEngine.SceneManagement;

namespace SilentPeak.Core
{
    /// <summary>
    /// Main game manager handling game state, mission flow, and core game logic
    /// Singleton pattern ensures only one instance exists throughout the game
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        [Header("Game State")]
        public GameState currentState;
        public int currentBase = 1;
        public int currentSubLevel = 1;

        [Header("Mission Data")]
        public bool missionActive = false;
        public int enemiesRemaining = 0;
        public int totalEnemies = 0;
        public bool alarmTriggered = false;
        public int shotsHit = 0;
        public int shotsMissed = 0;
        public int headshots = 0;
        public float missionStartTime;

        [Header("Mission Settings")]
        private readonly int[] enemyCountPerLevel = { 3, 7, 10, 15, 20, 24, 27, 30 };

        private void Awake()
        {
            // Singleton pattern implementation
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
        }

        private void Start()
        {
            // Set target frame rate for consistent performance
            Application.targetFrameRate = 60;
            
            // Force landscape orientation
            Screen.orientation = ScreenOrientation.LandscapeLeft;
            Screen.autorotateToLandscapeRight = true;
            Screen.autorotateToLandscapeLeft = true;
            Screen.autorotateToPortrait = false;
            Screen.autorotateToPortraitUpsideDown = false;
        }

        /// <summary>
        /// Initialize and start a new mission
        /// </summary>
        public void StartMission(int baseNumber, int subLevel)
        {
            currentBase = baseNumber;
            currentSubLevel = subLevel;
            missionActive = true;
            alarmTriggered = false;
            shotsHit = 0;
            shotsMissed = 0;
            headshots = 0;
            missionStartTime = Time.time;
            
            // Calculate enemy count based on sub-level
            totalEnemies = GetEnemyCountForLevel(subLevel);
            enemiesRemaining = totalEnemies;
            
            currentState = GameState.InGame;
            
            // Load gameplay scene
            SceneManager.LoadScene("GamePlay");
        }

        /// <summary>
        /// Called when an enemy is successfully eliminated
        /// </summary>
        public void EnemyKilled(bool wasHeadshot = false)
        {
            shotsHit++;
            enemiesRemaining--;
            
            if (wasHeadshot)
            {
                headshots++;
            }
            
            // Update UI
            if (UIManager.Instance != null)
            {
                UIManager.Instance.UpdateEnemiesRemaining(enemiesRemaining);
            }
            
            // Check if mission is complete
            if (enemiesRemaining <= 0)
            {
                MissionComplete();
            }
        }

        /// <summary>
        /// Called when player misses a shot
        /// </summary>
        public void ShotMissed()
        {
            shotsMissed++;
            TriggerAlarm();
        }

        /// <summary>
        /// Trigger the alarm system - mission fails
        /// </summary>
        public void TriggerAlarm()
        {
            if (!alarmTriggered && missionActive)
            {
                alarmTriggered = true;
                MissionFailed();
            }
        }

        /// <summary>
        /// Handle mission completion
        /// </summary>
        private void MissionComplete()
        {
            missionActive = false;
            currentState = GameState.MissionComplete;
            
            float missionTime = Time.time - missionStartTime;
            int coinsEarned = CalculateReward(missionTime);
            int stars = CalculateStars(missionTime);
            
            // Add coins to player account
            DataManager.Instance.AddCoins(coinsEarned);
            
            // Update level progress
            DataManager.Instance.UpdateLevelProgress(currentBase, currentSubLevel, stars, (int)missionTime, headshots);
            
            // Unlock next level
            if (currentSubLevel < 8)
            {
                DataManager.Instance.UnlockLevel(currentBase, currentSubLevel + 1);
            }
            else if (currentBase < 20)
            {
                DataManager.Instance.UnlockLevel(currentBase + 1, 1);
            }
            
            // Show victory UI
            if (UIManager.Instance != null)
            {
                UIManager.Instance.ShowMissionComplete(coinsEarned, stars, (int)missionTime);
            }
            
            // Play success sound
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlaySFX(AudioManager.Instance.missionComplete);
            }
        }

        /// <summary>
        /// Handle mission failure
        /// </summary>
        private void MissionFailed()
        {
            missionActive = false;
            currentState = GameState.MissionFailed;
            
            // Play alarm siren
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlayAlarmSiren();
            }
            
            // Show failure UI
            if (UIManager.Instance != null)
            {
                UIManager.Instance.ShowMissionFailed();
            }
        }

        /// <summary>
        /// Get enemy count for specific sub-level
        /// </summary>
        private int GetEnemyCountForLevel(int subLevel)
        {
            int index = Mathf.Clamp(subLevel - 1, 0, enemyCountPerLevel.Length - 1);
            return enemyCountPerLevel[index];
        }

        /// <summary>
        /// Calculate mission reward based on performance
        /// </summary>
        private int CalculateReward(float completionTime)
        {
            int baseReward = 100 * currentSubLevel;
            
            // Bonus for all headshots
            if (headshots == totalEnemies)
            {
                baseReward += 200;
            }
            
            // Bonus for speed (under 2 minutes)
            if (completionTime < 120f)
            {
                baseReward += 150;
            }
            
            // Bonus for perfect accuracy
            if (shotsMissed == 0)
            {
                baseReward += 100;
            }
            
            return baseReward;
        }

        /// <summary>
        /// Calculate star rating (1-3 stars)
        /// </summary>
        private int CalculateStars(float completionTime)
        {
            int stars = 1; // Base star for completion
            
            // Second star for good time
            if (completionTime < 180f) // Under 3 minutes
            {
                stars++;
            }
            
            // Third star for excellent performance
            if (headshots >= totalEnemies * 0.8f && shotsMissed == 0)
            {
                stars++;
            }
            
            return stars;
        }

        /// <summary>
        /// Restart current mission
        /// </summary>
        public void RestartMission()
        {
            StartMission(currentBase, currentSubLevel);
        }

        /// <summary>
        /// Return to main menu
        /// </summary>
        public void ReturnToMainMenu()
        {
            currentState = GameState.MainMenu;
            SceneManager.LoadScene("MainMenu");
        }

        /// <summary>
        /// Pause the game
        /// </summary>
        public void PauseGame()
        {
            Time.timeScale = 0f;
            currentState = GameState.Paused;
        }

        /// <summary>
        /// Resume the game
        /// </summary>
        public void ResumeGame()
        {
            Time.timeScale = 1f;
            currentState = GameState.InGame;
        }

        /// <summary>
        /// Quit the application
        /// </summary>
        public void QuitGame()
        {
            Application.Quit();
        }
    }

    /// <summary>
    /// Game state enumeration
    /// </summary>
    public enum GameState
    {
        MainMenu,
        InGame,
        Paused,
        MissionComplete,
        MissionFailed
    }
}
