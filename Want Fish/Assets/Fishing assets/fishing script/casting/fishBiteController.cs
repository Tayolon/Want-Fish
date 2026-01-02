using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FishBiteController : MonoBehaviour
{
    [Header("Root")]
    public GameObject root;
    public CanvasGroup canvasGroup;

    [Header("Image")]
    public Image biteImage;

    [Header("Sprites per Rarity")]
    public Sprite common;
    public Sprite uncommon;
    public Sprite rare;
    public Sprite epic;
    public Sprite legendary;
    public Sprite mythical;
    public Sprite exotic;
    public Sprite special;
    public Sprite ultimate;
    public Sprite secret;

    [Header("Timing")]
    public float fadeInTime = 0.2f;
    public float stayTime = 0.6f;
    public float fadeOutTime = 0.2f;

    void Awake()
    {
        root.SetActive(false);
    }

    public void Show(FishRarity rarity)
    {
        biteImage.sprite = GetSprite(rarity);
        StopAllCoroutines();
        StartCoroutine(PlaySequence());
    }

    IEnumerator PlaySequence()
    {
        root.SetActive(true);
        canvasGroup.alpha = 0;

        // Fade In
        float t = 0;
        while (t < fadeInTime)
        {
            t += Time.deltaTime;
            canvasGroup.alpha = t / fadeInTime;
            yield return null;
        }

        canvasGroup.alpha = 1;
        yield return new WaitForSeconds(stayTime);

        // Fade Out
        t = 0;
        while (t < fadeOutTime)
        {
            t += Time.deltaTime;
            canvasGroup.alpha = 1 - (t / fadeOutTime);
            yield return null;
        }

        canvasGroup.alpha = 0;
        root.SetActive(false);
    }

    Sprite GetSprite(FishRarity rarity)
    {
        return rarity switch
        {
            FishRarity.Common => common,
            FishRarity.Uncommon => uncommon,
            FishRarity.Rare => rare,
            FishRarity.Epic => epic,
            FishRarity.Legendary => legendary,
            FishRarity.Mythical => mythical,
            FishRarity.Exotic => exotic,
            FishRarity.Special => special,
            FishRarity.Ultimate => ultimate,
            FishRarity.Secret => secret,
            _ => common
        };
    }
}
