using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PermanentUpgradeRowUI : MonoBehaviour
{
    public PermanentUpgradeData upgrade;

    [Header("UI")]
    public TMP_Text nameText;
    public TMP_Text levelText;
    public TMP_Text costText;
    public Image fillBar;
    public Button upgradeButton;

    void OnEnable()
    {
        nameText.text = upgrade.displayName;
        upgradeButton.onClick.RemoveAllListeners();
        upgradeButton.onClick.AddListener(OnUpgradeClicked);
        Refresh();
    }

    void OnUpgradeClicked()
    {
        PermanentUpgradeManager.Instance.Upgrade(upgrade);
        Refresh();
    }

    void Refresh()
    {
        int level =
            PermanentUpgradeManager.Instance.GetLevel(upgrade.id);

        fillBar.fillAmount = (float)level / upgrade.maxLevel;
        levelText.text = $"{level}/{upgrade.maxLevel}";

        if (level >= upgrade.maxLevel)
        {
            costText.text = "MAX";
            upgradeButton.interactable = false;
        }
        else
        {
            int cost =
                PermanentUpgradeManager.Instance.GetCost(upgrade);

            costText.text = $"{cost} FC";
            upgradeButton.interactable =
                CurrencyManager.Instance.fishCoin >= cost;
        }
    }
}
