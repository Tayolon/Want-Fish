using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.UI;

public class ItemTooltipUI : MonoBehaviour
{
    private AudioSource audioSource;
    public static ItemTooltipUI Instance;
    public AudioClip sellSound;
    public float sellVolume = 0.6f;
    CaughtFish currentFish;

    public GameObject root;
    public TMP_Text nameText;
    public TMP_Text weightText;
    public TMP_Text valueText;
    public TMP_Text rarityText;
    public Button discardBtn;
    public Button sellBtn;
    public TMP_Text sellText;
    

    public int a;
    public int e;

    bool isPinned;


    void Awake()
    {
        Instance = this;
        root.SetActive(false);

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.playOnAwake = false;

        discardBtn.onClick.AddListener(DiscardCurrentFish);
        sellBtn.onClick.AddListener(SellCurrentFish);
    }

    bool ClickedOnAllowedUI()
    {
        PointerEventData eventData =
            new PointerEventData(EventSystem.current)
            {
                position = Input.mousePosition
            };

        List<RaycastResult> results = new();
        EventSystem.current.RaycastAll(eventData, results);

        foreach (var result in results)
        {
            if (result.gameObject.transform.IsChildOf(root.transform))
                return true;
        }

        return false;
    }

    public void Show(CaughtFish fish)
    {
        if (isPinned) return;

        currentFish = fish;
        SetText(fish);
        UpdateSellButton();
        root.SetActive(true);
    }

    public void Pin(CaughtFish fish)
    {
        isPinned = true;
        currentFish = fish;
        SetText(fish);
        UpdateSellButton();
        root.SetActive(true);
    }

    public void Hide()
    {
        if (isPinned) return;
        root.SetActive(false);
    }

    public void Unpin()
    {
        isPinned = false;
        root.SetActive(false);
    }

    void SetText(CaughtFish fish)
    {
        nameText.text = fish.data.fishName;
        weightText.text = $"Weight: {fish.weight:0.00} kg";
        valueText.text = $"Value: ${fish.value}";
        rarityText.text = fish.data.rarity.ToString();
        sellText.text = $"Sell for {fish.value}$";
    }

    void DiscardCurrentFish()
    {
        if (currentFish == null) return;

        InventoryManager.Instance.RemoveFish(currentFish);

        currentFish = null;
        Unpin();
    }

    void SellCurrentFish()
    {
        if (sellSound != null && audioSource != null)
            audioSource.PlayOneShot(sellSound, sellVolume);

        if (currentFish == null) return;

        int value = currentFish.value;

        InventoryManager.Instance.RemoveFish(currentFish);

        value = value * InventoryManager.Instance.sellMult / 100;

        CurrencyManager.Instance.Add(value);
        RunManager.Instance.AddFishCoin(value / 100);

        Debug.Log($"Sold {currentFish.data.fishName} for {value}$ [InventoryManager]");

        currentFish = null;
        Unpin();
    }

    void UpdateSellButton()
    {
        bool canSell = InventoryManager.Instance.sellTime;
        sellBtn.gameObject.SetActive(canSell);
    }

    void Update()
    {
        if (!root.activeSelf) return;

        // Follow mouse ONLY if not pinned
        if (!isPinned)
        {  
            transform.position = Input.mousePosition + new Vector3(a, e, 0);
        }

        // Click anywhere else to unpin
        if (isPinned && Input.GetMouseButtonDown(0))
        {
            if (!ClickedOnAllowedUI())
            {
                Unpin();
            }
        }
    }
}
