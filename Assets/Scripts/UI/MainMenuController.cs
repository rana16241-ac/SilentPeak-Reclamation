using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

namespace SilentPeak.UI
{
    /// <summary>
    /// Controls the main menu UI and navigation
    /// Handles player setup, shop access, and level selection
    /// </summary>
    public class MainMenuController : MonoBehaviour
    {
        [Header("UI Panels")]
        public GameObject mainMenuPanel;
        public GameObject playerSetupPanel;
        public GameObject weaponShopPanel;
        public GameObject customizationPanel;
        public GameObject premiumPanel;
        public GameObject coinStorePanel;
        public GameObject levelSelectPanel;
        public GameObject settingsPanel;

        [Header("Player Setup")]
        public TMP_InputField nameInput;
        public TMP_InputField ageInput;
        public Button confirmSetupButton;
        public TextMeshProUGUI setupErrorText;

        [Header("Top Bar")]
        public TextMeshProUGUI playerNameText;
        public TextMeshProUGUI coinsText;
        public Button profileButton;
        public Button levelMapButton;
        public Button watchAdButton;

        [Header("Main Buttons")]
        public Button weaponShopButton;
        public Button customizationButton;
        public Button playButton;
        public Button premiumButton;
        public Button coinStoreButton;
        public Button settingsButton;
        public Button quitButton;

        [Header("Animation")]
        public Animator menuAnimator;

        private void Start()
        {
            // Check if player data exists
            if (Core.DataManager.Instance.playerData == null)
            {
                ShowPlayerSetup();
            }
            else
            {
                ShowMainMenu();
            }

            SetupButtons();
            
            // Play menu music
            if (Core.AudioManager.Instance != null)
            {
                Core.AudioManager.Instance.PlayMusic(Core.AudioManager.Instance.menuMusic, true);
            }
        }

        /// <summary>
        /// Setup all button listeners
        /// </summary>
        private void SetupButtons()
        {
            weaponShopButton?.onClick.AddListener(() => OpenPanel(weaponShopPanel));
            customizationButton?.onClick.AddListener(() => OpenPanel(customizationPanel));
            playButton?.onClick.AddListener(OpenLevelSelect);
            premiumButton?.onClick.AddListener(() => OpenPanel(premiumPanel));
            coinStoreButton?.onClick.AddListener(() => OpenPanel(coinStorePanel));
            settingsButton?.onClick.AddListener(() => OpenPanel(settingsPanel));
            levelMapButton?.onClick.AddListener(OpenLevelSelect);
            watchAdButton?.onClick.AddListener(WatchAd);
            confirmSetupButton?.onClick.AddListener(ConfirmPlayerSetup);
            quitButton?.onClick.AddListener(QuitGame);
            profileButton?.onClick.AddListener(ShowPlayerProfile);
        }

        /// <summary>
        /// Show player setup screen for first-time users
        /// </summary>
        private void ShowPlayerSetup()
        {
            mainMenuPanel?.SetActive(false);
            playerSetupPanel?.SetActive(true);
            
            if (setupErrorText != null)
                setupErrorText.text = "";
        }

        /// <summary>
        /// Show main menu
        /// </summary>
        private void ShowMainMenu()
        {
            playerSetupPanel?.SetActive(false);
            mainMenuPanel?.SetActive(true);
            UpdateTopBar();
        }

        /// <summary>
        /// Confirm player setup and create profile
        /// </summary>
        private void ConfirmPlayerSetup()
        {
            string playerName = nameInput?.text.Trim();
            string ageText = ageInput?.text.Trim();

            // Validation
            if (string.IsNullOrEmpty(playerName))
            {
                ShowSetupError("Please enter your name");
                return;
            }

            if (playerName.Length < 3)
            {
                ShowSetupError("Name must be at least 3 characters");
                return;
            }

            if (!int.TryParse(ageText, out int age) || age < 10 || age > 100)
            {
                ShowSetupError("Please enter a valid age (10-100)");
                return;
            }

            // Create player profile
            Core.DataManager.Instance.CreateNewPlayer(playerName, age);
            ShowMainMenu();
            
            Core.AudioManager.Instance?.PlayButtonClick();
        }

        /// <summary>
        /// Show setup error message
        /// </summary>
        private void ShowSetupError(string message)
        {
            if (setupErrorText != null)
            {
                setupErrorText.text = message;
                setupErrorText.color = Color.red;
            }
        }

        /// <summary>
        /// Update top bar with player info
        /// </summary>
        private void UpdateTopBar()
        {
            if (Core.DataManager.Instance.playerData != null)
            {
                if (playerNameText != null)
                    playerNameText.text = Core.DataManager.Instance.playerData.playerName;
                
                if (coinsText != null)
                    coinsText.text = Core.DataManager.Instance.playerData.coins.ToString();
            }
        }

        /// <summary>
        /// Open a specific panel
        /// </summary>
        private void OpenPanel(GameObject panel)
        {
            if (panel == null) return;

            CloseAllPanels();
            panel.SetActive(true);
            Core.AudioManager.Instance?.PlayButtonClick();
        }

        /// <summary>
        /// Close all panels
        /// </summary>
        private void CloseAllPanels()
        {
            weaponShopPanel?.SetActive(false);
            customizationPanel?.SetActive(false);
            premiumPanel?.SetActive(false);
            coinStorePanel?.SetActive(false);
            levelSelectPanel?.SetActive(false);
            settingsPanel?.SetActive(false);
        }

        /// <summary>
        /// Open level selection
        /// </summary>
        private void OpenLevelSelect()
        {
            OpenPanel(levelSelectPanel);
        }

        /// <summary>
        /// Watch ad for coins
        /// </summary>
        private void WatchAd()
        {
            Core.DataManager.Instance.WatchAd();
            UpdateTopBar();
            Core.AudioManager.Instance?.PlaySFX(Core.AudioManager.Instance.coinCollect);
            
            // Show notification
            ShowNotification("+100 Coins!");
        }

        /// <summary>
        /// Show player profile
        /// </summary>
        private void ShowPlayerProfile()
        {
            // TODO: Implement profile panel
            Core.AudioManager.Instance?.PlayButtonClick();
            Debug.Log("Profile clicked");
        }

        /// <summary>
        /// Back to main menu from any panel
        /// </summary>
        public void BackToMainMenu()
        {
            CloseAllPanels();
            mainMenuPanel?.SetActive(true);
            Core.AudioManager.Instance?.PlayButtonClick();
        }

        /// <summary>
        /// Show notification message
        /// </summary>
        private void ShowNotification(string message)
        {
            // TODO: Implement notification system
            Debug.Log($"Notification: {message}");
        }

        /// <summary>
        /// Quit game
        /// </summary>
        private void QuitGame()
        {
            Core.AudioManager.Instance?.PlayButtonClick();
            
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #else
                Application.Quit();
            #endif
        }

        /// <summary>
        /// Update coins display (called from other scripts)
        /// </summary>
        public void RefreshCoinsDisplay()
        {
            UpdateTopBar();
        }
    }
}
