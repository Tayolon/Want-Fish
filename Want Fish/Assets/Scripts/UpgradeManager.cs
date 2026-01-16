    using System.Collections.Generic;
    using UnityEngine.UI;
    using TMPro;
    using UnityEngine;

    public class UpgradeManager : MonoBehaviour
    {
        public static UpgradeManager Instance;

        public GameObject ShopScreen;
        public Button CloseButton;
        public Button ShopButton;
        public Button RefreshButton;
        public int RefreshCost = 100;
        public TMP_Text refreshText;
        [Header("Upgrade Pool")]
        public List<UpgradeCards> allUpgrades = new();

        [Header("UI")]
        public GameObject upgradeCardPrefab;
        public Transform upgradeParent;

        void Awake()
        {
            RefreshButton.onClick.AddListener(refreshUpgrades);
            CloseButton.onClick.AddListener(closeShop);
            ShopButton.onClick.AddListener(openShop);

            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
        }

        void closeShop()
        {
            ShopScreen.SetActive(false);
        }

        void openShop()
        {
            ShopScreen.SetActive(true);
            updateRefreshText();
            RefreshButton.interactable = CurrencyManager.Instance.perak >= RefreshCost;
        }

        void updateRefreshText()
        {
            refreshText.text = $"Refresh for ${RefreshCost}";
        }

        void refreshUpgrades()
        {
            if (!CurrencyManager.Instance.Spend(RefreshCost))
            {
                Debug.Log("Not enough money to refresh");
                return;
            }
            RefreshCost = Mathf.CeilToInt(RefreshCost * 1.75f);
            updateRefreshText();
            ShowRandomUpgrades();
            RefreshButton.interactable = CurrencyManager.Instance.perak >= RefreshCost;

        }   

        public void ShowRandomUpgrades()
        {
            foreach (Transform child in upgradeParent)
                Destroy(child.gameObject);

            List<UpgradeCards> pool = new(allUpgrades);

            for (int i = 0; i < 3; i++)
            {
                if (pool.Count == 0) break;

                int index = Random.Range(0, pool.Count);
                UpgradeCards picked = pool[index];
                pool.RemoveAt(index);

                GameObject cardGO =
                    Instantiate(upgradeCardPrefab, upgradeParent);

                UpgradeCardUI card =
                    cardGO.GetComponent<UpgradeCardUI>();

                card.SetUpgrade(picked);
            }
        }

        public void ApplyUpgrade(UpgradeCards upgrade)
        {
            Debug.Log($"Applied upgrade: {upgrade.UpgradeName} [Upgrade Manager]");

            // EXAMPLES:
            // if (upgrade.UpgradeName == "Bigger Dih")
            //     DihManager.Instance.dihBigBoingBoing += 100%;

            //Pembaruan Kapasitas
            if (upgrade.UpgradeName == "Pembaruan Kapasitas")
            {
            InventoryManager.Instance.maxInventory += 3;
            }

            //Umpan Berkualitas
            if (upgrade.UpgradeName == "Umpan Berkualitas")
            {
                CastingController.Instance.minBiteTime *= 0.95f;
                CastingController.Instance.maxBiteTime *= 0.95f;

                CastingController.Instance.minWaitPenaltyPerDifficulty *= 0.95f;
                CastingController.Instance.maxWaitPenaltyPerDifficulty *= 0.95f;
            }

            //Jimat Keberuntungan
            if (upgrade.UpgradeName == "Jimat Keberuntungan")
            {
            FishDatabase.Instance.highRarityBonusPercent += 0.025f;
            }

            //Reeling Power +
            if (upgrade.UpgradeName == "Reeling Power +")
            {
            ReelingController.Instance.spGainPerSecond += 1f;
            //lowk gak bisa diganti detik detiknya sooo >.>
            }

            //Reeling Speed +
            if (upgrade.UpgradeName == "Reeling Speed +")
            {
            ReelingController.Instance.spGainPerSecond += 1f;
            //hehe ctrl c, ctrl v
            }
                

            //Pancingan Stabil
            if (upgrade.UpgradeName == "Pancingan Stabil")
            {
            ReelingController.Instance.maxSpeedMultiplier -= 0.05f;
            ReelingController.Instance.minSpeedMultiplier -= 0.05f;
            }
                
            //Handle yang baik
            if (upgrade.UpgradeName == "Handle yang baik")
            {
            ReelingController.Instance.greenGravity -= 1f;
            }
                
            //Kominukatif
            if (upgrade.UpgradeName == "Kominukatif")
            {
            InventoryManager.Instance.sellMult += 5;
            }
                
            //Pelajar Cepat
            if (upgrade.UpgradeName == "Pelajar Cepat")
            {
                // TODO: blehhh
            }   

            //Panen Berlimpah
            if (upgrade.UpgradeName == "Panen Berlimpah")
            {
                // TODO: blehhh
            }



            if (upgrade.SingleTimeUpgrade)
            {
                allUpgrades.Remove(upgrade);
            }
        }
    }
