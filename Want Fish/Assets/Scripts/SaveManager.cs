using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SaveAll()
    {
        CurrencyManager.Instance.SaveFishCoin();
        PermanentUpgradeManager.Instance.SaveAll();
        PlayerPrefs.Save();

        Debug.Log("Game Saved");
    }

    public void LoadAll()
    {
        CurrencyManager.Instance.LoadFishCoin();
        PermanentUpgradeManager.Instance.LoadAll();

        Debug.Log("Game Loaded");
    }

    public void FullReset()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        Debug.Log("ALL SAVE DATA DELETED");
    }
}
