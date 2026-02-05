using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace SilentPeak.UI
{
    /// <summary>
    /// Controls the weapon shop UI and upgrade system
    /// Handles weapon stat upgrades and purchases
    /// </summary>
    public class WeaponShopController : MonoBehaviour
    {
        [Header("Upgrade Panels")]
        public UpgradePanel scopePanel;
        public UpgradePanel stabilityPanel;
        public UpgradePanel reloadPanel;
        public UpgradePanel damagePanel;
        public UpgradePanel silencerPanel;

        [Header("UI Elements")]
        public TextMeshProUGUI coinsText;
        public Button backButton;

        [Header("Upgrade Costs")]
        public int[] upgradeCosts = { 500, 1000, 2000, 4000, 8000 }; // Costs for levels 1-5

        private void Start()
        {
            InitializeShop();
            backButton?.onClick.AddListener(CloseShop);
        }

        private void OnEnable()
        {
            RefreshShop();
        }

        /// <summary>
        /// Initialize shop UI
        /// </summary>
        private void InitializeShop()
        {
            SetupUpgradePanel(scopePanel, Core.WeaponUpgradeType.Scope, "Scope", "Increases zoom levels");
            SetupUpgradePanel(stabilityPanel, Core.WeaponUpgradeType.Stability, "Stability", "Reduces weapon sway");
            SetupUpgradePanel(reloadPanel, Core.WeaponUpgradeType.Reload, "Reload Speed", "Faster fire rate");
            SetupUpgradePanel(damagePanel, Core.WeaponUpgradeType.Damage, "Damage", "Increases bullet damage");
            SetupUpgradePanel(silencerPanel, Core.WeaponUpgradeType.Silencer, "Silencer", "Better stealth");
        }

        /// <summary>
        /// Setup individual upgrade panel
        /// </summary>
        private void SetupUpgradePanel(UpgradePanel panel, Core.WeaponUpgradeType upgradeType, string title, string description)
        {
            if (panel == null) return;

            panel.titleText.text = title;
            panel.descriptionText.text = description;
            panel.upgradeButton.onClick.AddListener(() => PurchaseUpgrade(upgradeType, panel));
        }

        /// <summary>
        /// Refresh shop display
        /// </summary>
        private void RefreshShop()
        {
            if (Core.DataManager.Instance?.playerData == null) return;

            var playerData = Core.DataManager.Instance.playerData;

            // Update coins display
            if (coinsText != null)
                coinsText.text = playerData.coins.ToString();

            // Update each upgrade panel
            UpdateUpgradePanel(scopePanel, playerData.scopeLevel);
            UpdateUpgradePanel(stabilityPanel, playerData.stabilityLevel);
            UpdateUpgradePanel(reloadPanel, playerData.reloadLevel);
            UpdateUpgradePanel(damagePanel, playerData.damageLevel);
            UpdateUpgradePanel(silencerPanel, playerData.silencerLevel);
        }

        /// <summary>
        /// Update upgrade panel display
        /// </summary>
        private void UpdateUpgradePanel(UpgradePanel panel, int currentLevel)
        {
            if (panel == null) return;

            // Update level display
            panel.levelText.text = $"Level {currentLevel}/5";

            // Update level indicators
            for (int i = 0; i < panel.levelIndicators.Length; i++)
            {
                if (panel.levelIndicators[i] != null)
                {
                    panel.levelIndicators[i].enabled = i < currentLevel;
                }
            }

            // Check if max level
            if (currentLevel >= 5)
            {
                panel.upgradeButton.interactable = false;
                panel.costText.text = "MAX";
            }
            else
            {
                int cost = upgradeCosts[currentLevel]; // Cost for next level
                panel.costText.text = cost.ToString();
                
                // Check if player can afford
                bool canAfford = Core.DataManager.Instance.playerData.coins >= cost;
                panel.upgradeButton.interactable = canAfford;
            }
        }

        /// <summary>
        /// Purchase an upgrade
        /// </summary>
        private void PurchaseUpgrade(Core.WeaponUpgradeType upgradeType, UpgradePanel panel)
        {
            if (Core.DataManager.Instance?.playerData == null) return;

            int currentLevel = GetCurrentLevel(upgradeType);
            
            if (currentLevel >= 5)
            {
                ShowMessage("Already at maximum level!");
                return;
            }

            int cost = upgradeCosts[currentLevel];

            if (Core.DataManager.Instance.UpgradeWeapon(upgradeType, cost))
            {
                // Success
                Core.AudioManager.Instance?.PlaySFX(Core.AudioManager.Instance.purchaseSuccess);
                ShowMessage($"{upgradeType} upgraded to level {currentLevel + 1}!");
                RefreshShop();
            }
            else
            {
                // Not enough coins
                Core.AudioManager.Instance?.PlaySFX(Core.AudioManager.Instance.purchaseFailed);
                ShowMessage("Not enough coins!");
            }
        }

        /// <summary>
        /// Get current level for upgrade type
        /// </summary>
        private int GetCurrentLevel(Core.WeaponUpgradeType upgradeType)
        {
            var playerData = Core.DataManager.Instance.playerData;
            
            switch (upgradeType)
            {
                case Core.WeaponUpgradeType.Scope:
                    return playerData.scopeLevel;
                case Core.WeaponUpgradeType.Stability:
                    return playerData.stabilityLevel;
                case Core.WeaponUpgradeType.Reload:
                    return playerData.reloadLevel;
                case Core.WeaponUpgradeType.Damage:
                    return playerData.damageLevel;
                case Core.WeaponUpgradeType.Silencer:
                    return playerData.silencerLevel;
                default:
                    return 0;
            }
        }

        /// <summary>
        /// Show message to player
        /// </summary>
        private void ShowMessage(string message)
        {
            Debug.Log($"Shop Message: {message}");
            // TODO: Implement proper notification system
        }

        /// <summary>
        /// Close shop
        /// </summary>
        private void CloseShop()
        {
            Core.AudioManager.Instance?.PlayButtonClick();
            gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Upgrade panel UI structure
    /// </summary>
    [System.Serializable]
    public class UpgradePanel
    {
        public TextMeshProUGUI titleText;
        public TextMeshProUGUI descriptionText;
        public TextMeshProUGUI levelText;
        public TextMeshProUGUI costText;
        public Button upgradeButton;
        public Image[] levelIndicators; // 5 indicators for 5 levels
    }
}
