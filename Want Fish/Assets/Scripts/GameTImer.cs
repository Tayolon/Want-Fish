using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameTimer : MonoBehaviour
{
    public Button sleepBtn;
    public Button shopBtn;
    public GameObject endScreen;
    public Button endDayBtn;
    [Header("Day Timer")]
    public float timeRemaining = 300f;
    public int days = 1;
    public bool timerRunning;
    public TMP_Text timerText;
    public TMP_Text daysText;

    void Start()
    {
        sleepBtn.onClick.AddListener(SleepTimer);
        endDayBtn.onClick.AddListener(ResetTimer);
    }

    void Update()
    {
        UpdateTimerUI();
        UpdateDaysUI();
        if (!timerRunning)
            return;

        if (timeRemaining > 0f)
        {
            timeRemaining -= Time.deltaTime;
        }
        else
        {
            timeRemaining = 0f;
            timerRunning = false;
            OnTimerFinished();
        }
    }

    private void OnTimerFinished()
    {
        Debug.Log("day ended [GameTimer]");
        endScreen.SetActive(true);
        
        if ((days + 1) % 5 == 0)
        {
            InventoryManager.Instance.sellTime = true;
        }
        else
        {
            InventoryManager.Instance.sellTime = false;
        }

        if ((days + 1) % 10 == 0)
        {
            shopBtn.gameObject.SetActive(true);
            UpgradeManager.Instance.ShowRandomUpgrades();
        }
        else
        {
            shopBtn.gameObject.SetActive(false);
        }
    }

    void UpdateTimerUI()
    {
        int minutes = Mathf.FloorToInt(timeRemaining / 60f);
        int seconds = Mathf.FloorToInt(timeRemaining % 60f);

        timerText.text = $"{minutes:00}:{seconds:00}";
    }

    void UpdateDaysUI()
    {
        daysText.text = $"Hari: {days}";
    }

    void ResetTimer()
    {
        Debug.Log("new day started [GameTimer]");
        UpgradeManager.Instance.RefreshCost = 100;
        timeRemaining = 300;
        timerRunning = true;
        endScreen.SetActive(false);

        if (days % 5 == 0)
        {
            Debug.Log("sell day [GameTimer]");
            if (!CurrencyManager.Instance.TryPayQuota())
            {
                DeathManager.Instance.TriggerDeath();
            }
        }

        if (days % 10 == 0)
        {
            Debug.Log("shop appears [GameTimer]");
        }

        days++;
    }

    void SleepTimer()
    {
        Debug.Log("slept [GameTimer]");
        timeRemaining = 0;
        timerRunning = true;
    }
}
