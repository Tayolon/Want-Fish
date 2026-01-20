using UnityEngine;
using System.Collections.Generic;

public class PermanentUpgradeManager : MonoBehaviour
{
    public static PermanentUpgradeManager Instance;

    public List<PermanentUpgradeData> upgrades = new();
    private Dictionary<string, int> levels = new();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadAll();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public int GetLevel(string id)
    {
        return levels.TryGetValue(id, out int lvl) ? lvl : 0;
    }

    public int GetCost(PermanentUpgradeData u)
    {
        int level = GetLevel(u.id);
        return Mathf.CeilToInt(u.baseCost * (1f + level * 0.25f));
    }

    public bool CanUpgrade(PermanentUpgradeData u)
    {
        return GetLevel(u.id) < u.maxLevel &&
               CurrencyManager.Instance.fishCoin >= GetCost(u);
    }

    public void Upgrade(PermanentUpgradeData u)
    {
        if (!CanUpgrade(u)) return;

        CurrencyManager.Instance.AddFishCoin(-GetCost(u));
        levels[u.id] = GetLevel(u.id) + 1;

        SaveManager.Instance.SaveAll();
    }
    
    public void SaveAll()
    {
        foreach (var pair in levels)
        {
            PlayerPrefs.SetInt($"Upgrade_{pair.Key}", pair.Value);
        }
    }

    public void LoadAll()
    {
        foreach (var u in upgrades)
        {
            levels[u.id] =
                PlayerPrefs.GetInt($"Upgrade_{u.id}", 0);
        }
    }

    public void FullReset()
    {
        levels.Clear();

        foreach (var u in upgrades)
        {
            levels[u.id] = 0;
        }

        SaveAll();
        PlayerPrefs.Save();
    }

}
