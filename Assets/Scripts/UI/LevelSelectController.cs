using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

namespace SilentPeak.UI
{
    /// <summary>
    /// Controls level selection UI
    /// Displays available bases and sub-levels
    /// </summary>
    public class LevelSelectController : MonoBehaviour
    {
        [Header("UI Elements")]
        public Transform baseContainer;
        public GameObject baseButtonPrefab;
        public GameObject levelDetailPanel;
        public Button backButton;
        public Button playButton;

        [Header("Level Detail")]
        public TextMeshProUGUI levelTitleText;
        public TextMeshProUGUI levelDescriptionText;
        public TextMeshProUGUI enemyCountText;
        public TextMeshProUGUI bestTimeText;
        public TextMeshProUGUI headshotsText;
        public Image[] starsImages;

        [Header("Sub-Level Buttons")]
        public Transform subLevelContainer;
        public GameObject subLevelButtonPrefab;

        private int selectedBase = 1;
        private int selectedSubLevel = 1;
        private List<GameObject> baseButtons = new List<GameObject>();
        private List<GameObject> subLevelButtons = new List<GameObject>();

        private void Start()
        {
            InitializeLevelSelect();
            backButton?.onClick.AddListener(CloseLevelSelect);
            playButton?.onClick.AddListener(StartSelectedLevel);
        }

        private void OnEnable()
        {
            RefreshLevelSelect();
        }

        /// <summary>
        /// Initialize level selection UI
        /// </summary>
        private void InitializeLevelSelect()
        {
            CreateBaseButtons();
            
            if (levelDetailPanel != null)
                levelDetailPanel.SetActive(false);
        }

        /// <summary>
        /// Create buttons for all bases
        /// </summary>
        private void CreateBaseButtons()
        {
            if (baseContainer == null || baseButtonPrefab == null) return;

            // Clear existing buttons
            foreach (var btn in baseButtons)
            {
                if (btn != null) Destroy(btn);
            }
            baseButtons.Clear();

            // Create buttons for 20 bases
            for (int i = 1; i <= 20; i++)
            {
                int baseNumber = i;
                GameObject buttonObj = Instantiate(baseButtonPrefab, baseContainer);
                baseButtons.Add(buttonObj);

                // Setup button
                Button button = buttonObj.GetComponent<Button>();
                button?.onClick.AddListener(() => SelectBase(baseNumber));

                // Setup text
                TextMeshProUGUI buttonText = buttonObj.GetComponentInChildren<TextMeshProUGUI>();
                if (buttonText != null)
                {
                    buttonText.text = $"Base {baseNumber}";
                }

                // Check if unlocked
                bool isUnlocked = Core.DataManager.Instance.IsLevelUnlocked(baseNumber, 1);
                button.interactable = isUnlocked;

                // Visual feedback for locked bases
                Image buttonImage = buttonObj.GetComponent<Image>();
                if (buttonImage != null)
                {
                    buttonImage.color = isUnlocked ? Color.white : new Color(0.5f, 0.5f, 0.5f, 0.5f);
                }
            }
        }

        /// <summary>
        /// Select a base and show sub-levels
        /// </summary>
        private void SelectBase(int baseNumber)
        {
            selectedBase = baseNumber;
            Core.AudioManager.Instance?.PlayButtonClick();
            
            CreateSubLevelButtons();
            
            // Select first unlocked sub-level
            for (int i = 1; i <= 8; i++)
            {
                if (Core.DataManager.Instance.IsLevelUnlocked(baseNumber, i))
                {
                    SelectSubLevel(i);
                    break;
                }
            }
        }

        /// <summary>
        /// Create buttons for sub-levels
        /// </summary>
        private void CreateSubLevelButtons()
        {
            if (subLevelContainer == null || subLevelButtonPrefab == null) return;

            // Clear existing buttons
            foreach (var btn in subLevelButtons)
            {
                if (btn != null) Destroy(btn);
            }
            subLevelButtons.Clear();

            // Create 8 sub-level buttons
            int[] enemyCounts = { 3, 7, 10, 15, 20, 24, 27, 30 };
            
            for (int i = 1; i <= 8; i++)
            {
                int subLevel = i;
                GameObject buttonObj = Instantiate(subLevelButtonPrefab, subLevelContainer);
                subLevelButtons.Add(buttonObj);

                // Setup button
                Button button = buttonObj.GetComponent<Button>();
                button?.onClick.AddListener(() => SelectSubLevel(subLevel));

                // Setup text
                TextMeshProUGUI[] texts = buttonObj.GetComponentsInChildren<TextMeshProUGUI>();
                if (texts.Length > 0)
                {
                    texts[0].text = $"Level {subLevel}";
                }
                if (texts.Length > 1)
                {
                    texts[1].text = $"{enemyCounts[i - 1]} Enemies";
                }

                // Check if unlocked
                bool isUnlocked = Core.DataManager.Instance.IsLevelUnlocked(selectedBase, subLevel);
                button.interactable = isUnlocked;

                // Show stars if completed
                var progress = Core.DataManager.Instance.GetLevelProgress(selectedBase, subLevel);
                if (progress != null && progress.stars > 0)
                {
                    // TODO: Display stars on button
                }
            }
        }

        /// <summary>
        /// Select a sub-level and show details
        /// </summary>
        private void SelectSubLevel(int subLevel)
        {
            selectedSubLevel = subLevel;
            Core.AudioManager.Instance?.PlayButtonClick();
            
            ShowLevelDetails();
        }

        /// <summary>
        /// Show level details panel
        /// </summary>
        private void ShowLevelDetails()
        {
            if (levelDetailPanel == null) return;

            levelDetailPanel.SetActive(true);

            // Get level progress
            var progress = Core.DataManager.Instance.GetLevelProgress(selectedBase, selectedSubLevel);

            // Update title
            if (levelTitleText != null)
            {
                levelTitleText.text = $"Base {selectedBase} - Level {selectedSubLevel}";
            }

            // Update description
            if (levelDescriptionText != null)
            {
                levelDescriptionText.text = GetLevelDescription(selectedBase, selectedSubLevel);
            }

            // Update enemy count
            if (enemyCountText != null)
            {
                int[] enemyCounts = { 3, 7, 10, 15, 20, 24, 27, 30 };
                int enemies = enemyCounts[selectedSubLevel - 1];
                enemyCountText.text = $"Enemies: {enemies}";
            }

            // Update best time
            if (bestTimeText != null && progress != null && progress.bestTime > 0)
            {
                int minutes = progress.bestTime / 60;
                int seconds = progress.bestTime % 60;
                bestTimeText.text = $"Best Time: {minutes:00}:{seconds:00}";
            }
            else if (bestTimeText != null)
            {
                bestTimeText.text = "Best Time: --:--";
            }

            // Update headshots
            if (headshotsText != null && progress != null)
            {
                headshotsText.text = $"Best Headshots: {progress.headshots}";
            }
            else if (headshotsText != null)
            {
                headshotsText.text = "Best Headshots: 0";
            }

            // Update stars
            if (starsImages != null && progress != null)
            {
                for (int i = 0; i < starsImages.Length; i++)
                {
                    if (starsImages[i] != null)
                    {
                        starsImages[i].enabled = i < progress.stars;
                    }
                }
            }

            // Enable play button
            if (playButton != null)
            {
                playButton.interactable = Core.DataManager.Instance.IsLevelUnlocked(selectedBase, selectedSubLevel);
            }
        }

        /// <summary>
        /// Get level description
        /// </summary>
        private string GetLevelDescription(int baseNumber, int subLevel)
        {
            string[] baseNames = {
                "Mountain Outpost", "Desert Fortress", "Coastal Base", "Forest Camp",
                "Urban Compound", "Arctic Station", "Jungle Hideout", "Canyon Base",
                "Island Facility", "Valley Stronghold", "Ridge Outpost", "Plains Base",
                "Swamp Camp", "Highland Fort", "Riverside Base", "Cliff Station",
                "Plateau Compound", "Gorge Outpost", "Lakeside Base", "Summit Fortress"
            };

            string baseName = baseNames[Mathf.Clamp(baseNumber - 1, 0, baseNames.Length - 1)];
            return $"Eliminate all enemies at {baseName} without being detected. One missed shot triggers the alarm.";
        }

        /// <summary>
        /// Start selected level
        /// </summary>
        private void StartSelectedLevel()
        {
            Core.AudioManager.Instance?.PlayButtonClick();
            
            if (Core.GameManager.Instance != null)
            {
                Core.GameManager.Instance.StartMission(selectedBase, selectedSubLevel);
            }
        }

        /// <summary>
        /// Refresh level selection
        /// </summary>
        private void RefreshLevelSelect()
        {
            CreateBaseButtons();
        }

        /// <summary>
        /// Close level selection
        /// </summary>
        private void CloseLevelSelect()
        {
            Core.AudioManager.Instance?.PlayButtonClick();
            gameObject.SetActive(false);
        }
    }
}
