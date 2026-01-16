using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class FishRevealController : MonoBehaviour
{
    public GameTimer gameTimer;
    [Header("UI")]
    public GameObject fishRevealUI;
    public GameObject darkOverlay;
    public GameObject rarityPanel;
    public GameObject fishPanel;

    public Image rarityBackground;
    public Image fishImage;

    public TMP_Text youGotText;
    public TMP_Text fishNameText;

    [Header("Timing")]
    public float darkDelay = 0.4f;
    public float rarityScaleDuration = 0.5f;
    public float fishDelay = 0.4f;

    [Header("Rotation")]
    public float rarityRotateSpeed = 15f; // derajat per detik

    [Header("Rarity Backgrounds")]
    public Sprite commonBG;
    public Sprite uncommonBG;
    public Sprite rareBG;
    public Sprite epicBG;
    public Sprite legendaryBG;
    public Sprite mythicalBG;
    public Sprite exoticBG;
    public Sprite specialBG;
    public Sprite ultimateBG;
    public Sprite secretBG;

    Coroutine rotateRoutine;

bool isRevealing;


public void ShowFish(FishData fish)
{
    fishRevealUI.SetActive(true);
    StopAllCoroutines();
    isRevealing = true;
    StartCoroutine(RevealSequence(fish));
}

public bool IsShowing()
{
    return isRevealing;
}

    IEnumerator RevealSequence(FishData fish)
    {
        // RESET
        darkOverlay.SetActive(false);
        rarityPanel.SetActive(false);
        fishPanel.SetActive(false);

        rarityPanel.transform.localScale = Vector3.zero;
        fishPanel.transform.localScale = Vector3.zero;

        // 1️⃣ Dark overlay
        darkOverlay.SetActive(true);
        yield return new WaitForSeconds(darkDelay);

        // 2️⃣ Rarity popup
        rarityBackground.sprite = GetRarityBG(fish.rarity);
        rarityPanel.SetActive(true);

        yield return StartCoroutine(ScaleIn(rarityPanel.transform, rarityScaleDuration));

        rotateRoutine = StartCoroutine(RotateRarity());

        // 3️⃣ Fish popup
        yield return new WaitForSeconds(fishDelay);

        fishImage.sprite = fish.fishSprite;
        youGotText.text = "YOU GOT";
        fishNameText.text = fish.fishName;

        fishPanel.SetActive(true);
        yield return StartCoroutine(ScaleIn(fishPanel.transform, 0.35f));
    }

    IEnumerator ScaleIn(Transform target, float duration)
    {
        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            float s = Mathf.SmoothStep(0, 1, t / duration);
            target.localScale = Vector3.one * s;
            yield return null;
        }
        target.localScale = Vector3.one;
    }

    IEnumerator RotateRarity()
    {
        while (true)
        {
            rarityPanel.transform.Rotate(0, 0, -rarityRotateSpeed * Time.deltaTime);
            yield return null;
        }
    }

   public void Hide()
{
    isRevealing = false;

    StopAllCoroutines();

    if (rotateRoutine != null)
        StopCoroutine(rotateRoutine);

    darkOverlay.SetActive(false);
    rarityPanel.SetActive(false);
    fishPanel.SetActive(false);
    fishRevealUI.SetActive(false);
}


    Sprite GetRarityBG(FishRarity rarity)
    {
        return rarity switch
        {
            FishRarity.Common => commonBG,
            FishRarity.Uncommon => uncommonBG,
            FishRarity.Rare => rareBG,
            FishRarity.Epic => epicBG,
            FishRarity.Legendary => legendaryBG,
            FishRarity.Mythical => mythicalBG,
            FishRarity.Exotic => exoticBG,
            FishRarity.Special => specialBG,
            FishRarity.Ultimate => ultimateBG,
            FishRarity.Secret => secretBG,
            _ => commonBG
        };
    }

    void Update()
{
    if (!isRevealing) return;

    // Klik kiri / tap layar
    if (Input.GetMouseButtonDown(0) || Input.touchCount > 0)
    {
        gameTimer.timerRunning = true;
        Hide();
    }
}

}
