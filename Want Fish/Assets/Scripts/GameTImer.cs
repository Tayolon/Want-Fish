    using UnityEngine;
    using TMPro;
    using UnityEngine.UI;
    using UnityEngine.SceneManagement;

    public class GameTimer : MonoBehaviour
    {
        public static GameTimer Instance;

        public Button sleepBtn;
        public Button shopBtn;
        public GameObject endScreen;
        public GameObject winScreen;
        public Button winBtn;
        public Button endDayBtn;
        public bool dayEnded;
        public bool HasDayEnded() => dayEnded;

        [Header("Day Timer")]
        public float timeRemaining = 300f;
        public int days = 1;
        public bool timerRunning;
        public TMP_Text timerText;
        public TMP_Text daysText;

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
                return;
            }
        }

        void Start()
        {
            sleepBtn.onClick.AddListener(SleepTimer);
            endDayBtn.onClick.AddListener(ResetTimer);
            winBtn.onClick.AddListener(winnerwinner);
        }

        void Update()
        {
            if (CastingController.Instance.IsBusy())
            {
                sleepBtn.interactable = false;
            }
            else
            {
                sleepBtn.interactable = true;
            }
            
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
            dayEnded = true;
            
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
            dayEnded = false;
            UpgradeManager.Instance.RefreshCost = 100;
            timeRemaining = 90;
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

            if (days == 50)
            {
                Debug.Log("ya won [GameTimer]");
                MusicBg.Instance.WinSound();
                winScreen.SetActive(true);
                Time.timeScale = 0f;
            }

            days++;
        }

        void SleepTimer()
        {
            Debug.Log("slept [GameTimer]");
            timeRemaining = 0;
            timerRunning = true;
        }

        void winnerwinner()
        {
            CurrencyManager.Instance.AddFishCoin(6767);
            Time.timeScale = 1f;
            SceneManager.LoadScene("FishingHub");
        }
    }
