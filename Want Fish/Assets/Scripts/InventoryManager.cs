using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryManager : MonoBehaviour
{
    //semua logic selling di ToolTipUi
    //woopsie!!
    public static InventoryManager Instance;
    public GameTimer gameTimer;

    public AudioClip sellSound;
    public float sellVolume = 0.6f;

    private AudioSource audioSource;

    public bool sellTime;
    public int sellMult = 100;
    public List<CaughtFish> fishes = new();
    public GameObject itemSlotPrefab;
    public Transform inventoryParent;
    public GameObject inventoryCanvas;
    public Button inventoryOpen;
    public Button inventoryClose;

    [Header("Inventory")]
    public int maxInventory = 20;
    public int currentInventory;
    public TMP_Text amountText;
    public Button sellAllBtn;

    void Awake()
    {
        UpdateAmountText();

        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.playOnAwake = false;
    }

    void Start()
    {
        inventoryOpen.onClick.AddListener(OpenInventory);
        inventoryClose.onClick.AddListener(CloseInventory);
        sellAllBtn.onClick.AddListener(SellAll);
    }

    public void AddFish(CaughtFish fish)
    {
        fishes.Add(fish);
        CreateUISlot(fish);
        currentInventory++;
        UpdateAmountText();

        Debug.Log(
            $"Stored {fish.data.fishName} " +
            $"({fish.weight:0.00}kg, {fish.value}$)"
        );
    }

    public void RemoveFish(CaughtFish fish)
    {
        Debug.Log($"killed the {fish.data.fishName}🥀 [InventoryManager]");
        if (!fishes.Contains(fish)) return;

        fishes.Remove(fish);

        foreach (Transform child in inventoryParent)
        {
            ItemSlotUI slot = child.GetComponent<ItemSlotUI>();
            if (slot != null && slot.StoredFish == fish)
            {
                Destroy(child.gameObject);
                break;
            }
        }
        currentInventory--;
        UpdateAmountText();
    }

    void CreateUISlot(CaughtFish fish)
    {
        GameObject slotGO =
            Instantiate(itemSlotPrefab, inventoryParent);

        ItemSlotUI slot =
            slotGO.GetComponent<ItemSlotUI>();

        slot.SetFish(fish);
    }

    void OpenInventory()
    {
        inventoryCanvas.SetActive(true);
        gameTimer.timerRunning = false;
        sellAllBtn.gameObject.SetActive(sellTime);
        UpdateAmountText();
    }

    void CloseInventory()
    {
        inventoryCanvas.SetActive(false);
        gameTimer.timerRunning = true;
    }

    public bool IsFull()
    {
        return currentInventory >= maxInventory;
    }

    void UpdateAmountText()
    {
        amountText.text = $"{currentInventory}/{maxInventory}";
    }

    public void SellAll()
    {
        if (sellSound != null)
            audioSource.PlayOneShot(sellSound, sellVolume);

        if (!sellTime) return;
        if (fishes.Count == 0) return;

        List<CaughtFish> fishesCopy = new List<CaughtFish>(fishes);

        int totalValue = 0;

        foreach (CaughtFish fish in fishesCopy)
        {
            totalValue += fish.value;
            RemoveFish(fish);
        }

        totalValue = totalValue * sellMult / 100;

        CurrencyManager.Instance.Add(totalValue);
        RunManager.Instance.AddFishCoin(totalValue / 100);
        Debug.Log($"Sold all fish for {totalValue}$ [InventoryManager]");
    }
}