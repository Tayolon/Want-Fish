using UnityEngine;

public class RunManager : MonoBehaviour
{
    public static RunManager Instance;

    public int runFishCoin;

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

    public void AddFishCoin(int amount)
    {
        runFishCoin += amount;
    }

    public void EndRun()
    {
        CurrencyManager.Instance.fishCoin += runFishCoin;
        runFishCoin = 0;
    }

    public void ResetRun()
    {
        runFishCoin = 0;
    }
}
