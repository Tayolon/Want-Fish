using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Currency UI")]
    public TMP_Text perakText;
    public TMP_Text quotaText;
    public TMP_Text fishCoinText;

    [Header("Death UI")]
    public GameObject deathUI;
    public Button deathButton;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        RefreshAll();

        if (deathButton != null)
            deathButton.onClick.AddListener(OnDeathButton);
    }

    public void RefreshAll()
    {
        RefreshCurrency();
    }

    public void RefreshCurrency()
    {
        if (CurrencyManager.Instance == null) return;

        if (perakText != null)
            perakText.text = CurrencyManager.Instance.perak.ToString();

        if (quotaText != null)
            quotaText.text = $"Kuota: {CurrencyManager.Instance.quota}";

        if (fishCoinText != null)
            fishCoinText.text = CurrencyManager.Instance.fishCoin.ToString();
    }

    public void ShowDeath()
    {
        if (deathUI == null) return;

        deathUI.SetActive(true);
    }

    void OnDeathButton()
    {
        deathUI.SetActive(false);
        DeathManager.Instance.GoToHub();
    }
}
