using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance;

    public int perak;
    public int quota;
    public int fishCoin;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadFishCoin();
            ResetRunData();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Updaate()
    {
        UIManager.Instance?.RefreshCurrency();
    }

    public void Add(int amount)
    {
        perak += amount;
        UIManager.Instance?.RefreshCurrency();
    }

    public bool Spend(int amount)
    {
        if (perak < amount) return false;
        perak -= amount;
        UIManager.Instance?.RefreshCurrency();
        return true;
    }

    public bool TryPayQuota()
    {
        if (perak < quota) return false;

        perak -= quota;
        quota = Mathf.CeilToInt(quota * 1.2f);
        UIManager.Instance?.RefreshCurrency();
        return true;
    }

    public void AddFishCoin(int amount)
    {
        fishCoin += amount;
        SaveFishCoin();
        UIManager.Instance?.RefreshCurrency();
    }

    void ResetRunData()
    {
        perak = 0;
        quota = 1000;
    }

    void SaveFishCoin()
    {
        PlayerPrefs.SetInt("FishCoin", fishCoin);
        PlayerPrefs.Save();
    }

    void LoadFishCoin()
    {
        fishCoin = PlayerPrefs.GetInt("FishCoin", 0);
    }
}
