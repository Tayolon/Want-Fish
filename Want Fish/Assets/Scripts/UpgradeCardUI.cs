using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UpgradeCardUI : MonoBehaviour
{
    public TMP_Text nameText;
    public TMP_Text descriptionText;
    public Image iconImage;
    public TMP_Text costText;
    public Button buyButton;

    UpgradeCards data;

    void Start()
    {
        buyButton.onClick.AddListener(BuyUpgrade);
    }

    public void SetUpgrade(UpgradeCards upgrade)
    {
        data = upgrade;
        int costUi=upgrade.Cost;
        nameText.text = upgrade.UpgradeName;
        descriptionText.text = upgrade.UpgradeDescriptions;
        iconImage.sprite = upgrade.UpgradeSprite;
        costText.text = $"${costUi}";
    }

    
    void BuyUpgrade()
    {
        // Try spend money
        if (!CurrencyManager.Instance.Spend(data.Cost))
            return;

        // Apply upgrade effect
        UpgradeManager.Instance.ApplyUpgrade(data);
        Destroy(gameObject);
    }
}
