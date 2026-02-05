using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace SilentPeak.UI
{
    /// <summary>
    /// Controls the coin store UI for in-app purchases
    /// Handles coin package purchases
    /// </summary>
    public class CoinStoreController : MonoBehaviour
    {
        [Header("Coin Packages")]
        public CoinPackage[] coinPackages;

        [Header("UI")]
        public Transform packageContainer;
        public GameObject packageButtonPrefab;
        public Button backButton;
        public TextMeshProUGUI currentCoinsText;

        private void Start()
        {
            InitializeStore();
            backButton?.onClick.AddListener(CloseStore);
        }

        private void OnEnable()
        {
            RefreshStore();
        }

        /// <summary>
        /// Initialize coin store
        /// </summary>
        private void InitializeStore()
        {
            // Define coin packages
            coinPackages = new CoinPackage[]
            {
                new CoinPackage { price = "$2", coins = 3000, productId = "com.silentpeak.coins.3000" },
                new CoinPackage { price = "$5", coins = 7000, productId = "com.silentpeak.coins.7000" },
                new CoinPackage { price = "$10", coins = 15000, productId = "com.silentpeak.coins.15000" },
                new CoinPackage { price = "$15", coins = 20000, productId = "com.silentpeak.coins.20000" },
                new CoinPackage { price = "$20", coins = 30000, productId = "com.silentpeak.coins.30000" },
                new CoinPackage { price = "$25", coins = 40000, productId = "com.silentpeak.coins.40000" },
                new CoinPackage { price = "$30", coins = 50000, productId = "com.silentpeak.coins.50000" }
            };

            CreatePackageButtons();
        }

        /// <summary>
        /// Create UI buttons for each package
        /// </summary>
        private void CreatePackageButtons()
        {
            if (packageContainer == null) return;

            foreach (var package in coinPackages)
            {
                GameObject buttonObj = Instantiate(packageButtonPrefab, packageContainer);
                
                // Setup button
                Button button = buttonObj.GetComponent<Button>();
                button?.onClick.AddListener(() => PurchasePackage(package));

                // Setup text elements
                TextMeshProUGUI[] texts = buttonObj.GetComponentsInChildren<TextMeshProUGUI>();
                if (texts.Length >= 2)
                {
                    texts[0].text = $"{package.coins:N0} Coins";
                    texts[1].text = package.price;
                }
            }
        }

        /// <summary>
        /// Refresh store display
        /// </summary>
        private void RefreshStore()
        {
            if (currentCoinsText != null && Core.DataManager.Instance?.playerData != null)
            {
                currentCoinsText.text = $"Current: {Core.DataManager.Instance.playerData.coins:N0}";
            }
        }

        /// <summary>
        /// Purchase a coin package
        /// </summary>
        private void PurchasePackage(CoinPackage package)
        {
            Core.AudioManager.Instance?.PlayButtonClick();

            // In a real implementation, this would trigger IAP
            // For now, we'll simulate the purchase
            Debug.Log($"Attempting to purchase: {package.coins} coins for {package.price}");

            // TODO: Implement actual IAP using Unity IAP
            // For testing purposes, we'll just add the coins
            #if UNITY_EDITOR
                SimulatePurchase(package);
            #else
                InitiateIAPPurchase(package);
            #endif
        }

        /// <summary>
        /// Simulate purchase for testing
        /// </summary>
        private void SimulatePurchase(CoinPackage package)
        {
            Core.DataManager.Instance.PurchaseCoins(package.coins);
            Core.AudioManager.Instance?.PlaySFX(Core.AudioManager.Instance.purchaseSuccess);
            ShowPurchaseSuccess(package.coins);
            RefreshStore();
        }

        /// <summary>
        /// Initiate real IAP purchase
        /// </summary>
        private void InitiateIAPPurchase(CoinPackage package)
        {
            // TODO: Implement Unity IAP
            Debug.Log($"IAP Purchase: {package.productId}");
            
            // Placeholder for IAP implementation
            // This would normally call Unity IAP service
            // IAPManager.Instance.BuyProduct(package.productId);
        }

        /// <summary>
        /// Called when IAP purchase succeeds
        /// </summary>
        public void OnPurchaseSuccess(string productId, int coins)
        {
            Core.DataManager.Instance.PurchaseCoins(coins);
            Core.AudioManager.Instance?.PlaySFX(Core.AudioManager.Instance.purchaseSuccess);
            ShowPurchaseSuccess(coins);
            RefreshStore();
        }

        /// <summary>
        /// Called when IAP purchase fails
        /// </summary>
        public void OnPurchaseFailed(string reason)
        {
            Core.AudioManager.Instance?.PlaySFX(Core.AudioManager.Instance.purchaseFailed);
            ShowPurchaseError(reason);
        }

        /// <summary>
        /// Show purchase success message
        /// </summary>
        private void ShowPurchaseSuccess(int coins)
        {
            Debug.Log($"Purchase successful! +{coins} coins");
            // TODO: Show success popup
        }

        /// <summary>
        /// Show purchase error message
        /// </summary>
        private void ShowPurchaseError(string reason)
        {
            Debug.Log($"Purchase failed: {reason}");
            // TODO: Show error popup
        }

        /// <summary>
        /// Close store
        /// </summary>
        private void CloseStore()
        {
            Core.AudioManager.Instance?.PlayButtonClick();
            gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Coin package data structure
    /// </summary>
    [System.Serializable]
    public class CoinPackage
    {
        public string price;
        public int coins;
        public string productId;
    }
}
