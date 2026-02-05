using UnityEngine;
using System.Collections.Generic;

namespace SilentPeak.Core
{
    /// <summary>
    /// Manages all player data, save/load operations, and progression
    /// Handles local storage using PlayerPrefs
    /// </summary>
    public class DataManager : MonoBehaviour
    {
        public static DataManager Instance { get; private set; }

        [Header("Player Data")]
        public PlayerData playerData;

        private const string SAVE_KEY = "SilentPeakSaveData";
        private const string FIRST_LAUNCH_KEY = "FirstLaunch";

        private void Awake()
        {
            // Singleton pattern
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                LoadGame();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        /// <summary>
        /// Check if this is the first time launching the game
        /// </summary>
        public bool IsFirstLaunch()
        {
            return !PlayerPrefs.HasKey(FIRST_LAUNCH_KEY);
        }

        /// <summary>
        /// Mark first launch as complete
        /// </summary>
        public void CompleteFirstLaunch()
        {
            PlayerPrefs.SetInt(FIRST_LAUNCH_KEY, 1);
            PlayerPrefs.Save();
        }

        /// <summary>
        /// Create new player profile
        /// </summary>
        public void CreateNewPlayer(string playerName, int age)
        {
            playerData = new PlayerData
            {
                playerName = playerName,
                age = age,
                coins = 1000, // Starting coins
                unlockedLevels = new List<LevelProgress>(),
                
                // Default weapon stats
                scopeLevel = 1,
                stabilityLevel = 1,
                reloadLevel = 1,
                damageLevel = 1,
                silencerLevel = 1,
                
                // Default customization
                selectedHairStyle = 0,
                selectedHairColor = 0,
                selectedUniform = 0,
                selectedHelmet = 0,
                selectedGloves = 0,
                selectedShoes = 0,
                
                // Stats
                totalMissionsCompleted = 0,
                totalHeadshots = 0,
                totalKills = 0,
                bestMissionTime = 0
            };

            // Unlock first level
            UnlockLevel(1, 1);
            CompleteFirstLaunch();
            SaveGame();
        }

        /// <summary>
        /// Add coins to player account
        /// </summary>
        public void AddCoins(int amount)
        {
            if (playerData != null)
            {
                playerData.coins += amount;
                SaveGame();
                
                // Update UI if available
                if (UIManager.Instance != null)
                {
                    UIManager.Instance.UpdateCoinsDisplay();
                }
            }
        }

        /// <summary>
        /// Spend coins from player account
        /// </summary>
        public bool SpendCoins(int amount)
        {
            if (playerData != null && playerData.coins >= amount)
            {
                playerData.coins -= amount;
                SaveGame();
                
                // Update UI if available
                if (UIManager.Instance != null)
                {
                    UIManager.Instance.UpdateCoinsDisplay();
                }
                
                return true;
            }
            return false;
        }

        /// <summary>
        /// Unlock a specific level
        /// </summary>
        public void UnlockLevel(int baseNumber, int subLevel)
        {
            if (playerData == null) return;

            LevelProgress level = playerData.unlockedLevels.Find(l => 
                l.baseNumber == baseNumber && l.subLevel == subLevel);

            if (level == null)
            {
                playerData.unlockedLevels.Add(new LevelProgress
                {
                    baseNumber = baseNumber,
                    subLevel = subLevel,
                    unlocked = true,
                    stars = 0,
                    bestTime = 0,
                    headshots = 0
                });
                SaveGame();
            }
        }

        /// <summary>
        /// Update level progress after completion
        /// </summary>
        public void UpdateLevelProgress(int baseNumber, int subLevel, int stars, int time, int headshots)
        {
            if (playerData == null) return;

            LevelProgress level = playerData.unlockedLevels.Find(l => 
                l.baseNumber == baseNumber && l.subLevel == subLevel);

            if (level != null)
            {
                // Update if new record
                if (stars > level.stars)
                {
                    level.stars = stars;
                }
                
                if (level.bestTime == 0 || time < level.bestTime)
                {
                    level.bestTime = time;
                }
                
                if (headshots > level.headshots)
                {
                    level.headshots = headshots;
                }
            }
            else
            {
                // Create new progress entry
                playerData.unlockedLevels.Add(new LevelProgress
                {
                    baseNumber = baseNumber,
                    subLevel = subLevel,
                    unlocked = true,
                    stars = stars,
                    bestTime = time,
                    headshots = headshots
                });
            }

            // Update global stats
            playerData.totalMissionsCompleted++;
            playerData.totalHeadshots += headshots;
            
            SaveGame();
        }

        /// <summary>
        /// Check if a level is unlocked
        /// </summary>
        public bool IsLevelUnlocked(int baseNumber, int subLevel)
        {
            if (playerData == null) return false;

            LevelProgress level = playerData.unlockedLevels.Find(l => 
                l.baseNumber == baseNumber && l.subLevel == subLevel);
            
            return level != null && level.unlocked;
        }

        /// <summary>
        /// Get level progress data
        /// </summary>
        public LevelProgress GetLevelProgress(int baseNumber, int subLevel)
        {
            if (playerData == null) return null;

            return playerData.unlockedLevels.Find(l => 
                l.baseNumber == baseNumber && l.subLevel == subLevel);
        }

        /// <summary>
        /// Upgrade weapon stat
        /// </summary>
        public bool UpgradeWeapon(WeaponUpgradeType upgradeType, int cost)
        {
            if (playerData == null || !SpendCoins(cost)) return false;

            switch (upgradeType)
            {
                case WeaponUpgradeType.Scope:
                    playerData.scopeLevel++;
                    break;
                case WeaponUpgradeType.Stability:
                    playerData.stabilityLevel++;
                    break;
                case WeaponUpgradeType.Reload:
                    playerData.reloadLevel++;
                    break;
                case WeaponUpgradeType.Damage:
                    playerData.damageLevel++;
                    break;
                case WeaponUpgradeType.Silencer:
                    playerData.silencerLevel++;
                    break;
            }

            SaveGame();
            return true;
        }

        /// <summary>
        /// Watch ad reward
        /// </summary>
        public void WatchAd()
        {
            AddCoins(100);
            Debug.Log("Ad watched! +100 coins");
        }

        /// <summary>
        /// Purchase coins with real money
        /// </summary>
        public void PurchaseCoins(int amount)
        {
            AddCoins(amount);
        }

        /// <summary>
        /// Save game data to PlayerPrefs
        /// </summary>
        public void SaveGame()
        {
            if (playerData != null)
            {
                string json = JsonUtility.ToJson(playerData);
                PlayerPrefs.SetString(SAVE_KEY, json);
                PlayerPrefs.Save();
                Debug.Log("Game saved successfully");
            }
        }

        /// <summary>
        /// Load game data from PlayerPrefs
        /// </summary>
        public void LoadGame()
        {
            if (PlayerPrefs.HasKey(SAVE_KEY))
            {
                string json = PlayerPrefs.GetString(SAVE_KEY);
                playerData = JsonUtility.FromJson<PlayerData>(json);
                Debug.Log("Game loaded successfully");
            }
            else
            {
                playerData = null;
                Debug.Log("No save data found");
            }
        }

        /// <summary>
        /// Delete all save data (for testing)
        /// </summary>
        public void DeleteSaveData()
        {
            PlayerPrefs.DeleteAll();
            playerData = null;
            Debug.Log("Save data deleted");
        }
    }

    /// <summary>
    /// Player data structure
    /// </summary>
    [System.Serializable]
    public class PlayerData
    {
        // Profile
        public string playerName;
        public int age;
        public int coins;
        
        // Progression
        public List<LevelProgress> unlockedLevels;
        
        // Weapon upgrades (1-5 levels each)
        public int scopeLevel;
        public int stabilityLevel;
        public int reloadLevel;
        public int damageLevel;
        public int silencerLevel;
        
        // Customization
        public int selectedHairStyle;
        public int selectedHairColor;
        public int selectedUniform;
        public int selectedHelmet;
        public int selectedGloves;
        public int selectedShoes;
        
        // Statistics
        public int totalMissionsCompleted;
        public int totalHeadshots;
        public int totalKills;
        public int bestMissionTime;
    }

    /// <summary>
    /// Level progress data
    /// </summary>
    [System.Serializable]
    public class LevelProgress
    {
        public int baseNumber;
        public int subLevel;
        public bool unlocked;
        public int stars; // 0-3 stars
        public int bestTime; // In seconds
        public int headshots;
    }

    /// <summary>
    /// Weapon upgrade types
    /// </summary>
    public enum WeaponUpgradeType
    {
        Scope,
        Stability,
        Reload,
        Damage,
        Silencer
    }
}
