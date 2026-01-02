using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CastingResultController : MonoBehaviour
{
    [Header("UI")]
    public GameObject root;
    public RectTransform imageTransform;
    public Image image;
    CanvasGroup canvasGroup;

    [Header("Sprites")]
    public Sprite yellowSprite;
    public Sprite greenSprite;
    public Sprite orangeSprite;
    public Sprite redSprite;

    [Header("Rotation (degree)")]
    public float minRotation = -8f;
    public float maxRotation = 8f;

    [Header("Timing")]
    public float scaleInDuration = 0.25f;
    public float showDuration = 0.8f;
    public float fadeOutDuration = 0.25f;

    void Awake()
    {
        canvasGroup = root.GetComponent<CanvasGroup>();
        root.SetActive(false);
    }

    public void ShowYellow() => Show(yellowSprite);
    public void ShowGreen()  => Show(greenSprite);
    public void ShowOrange() => Show(orangeSprite);
    public void ShowRed()    => Show(redSprite);

    void Show(Sprite sprite)
    {
        if (sprite == null) return;

        gameObject.SetActive(true); // 🔥 WAJIB
        StartCoroutine(ShowRoutine(sprite));
    }

    IEnumerator ShowRoutine(Sprite sprite)
    {
        root.SetActive(true);
        image.sprite = sprite;

        float rot = Random.Range(minRotation, maxRotation);
        imageTransform.localRotation =
            Quaternion.Euler(0, 0, rot);

        imageTransform.localScale = Vector3.one * 0.6f;
        canvasGroup.alpha = 0f;

        // ===== IN =====
        float t = 0;
        while (t < scaleInDuration)
        {
            t += Time.deltaTime;
            float p = t / scaleInDuration;

            imageTransform.localScale =
                Vector3.Lerp(Vector3.one * 0.6f, Vector3.one, p);
            canvasGroup.alpha = p;

            yield return null;
        }

        imageTransform.localScale = Vector3.one;
        canvasGroup.alpha = 1f;

        yield return new WaitForSeconds(showDuration);

        // ===== OUT =====
        t = 0;
        while (t < fadeOutDuration)
        {
            t += Time.deltaTime;
            canvasGroup.alpha =
                Mathf.Lerp(1f, 0f, t / fadeOutDuration);
            yield return null;
        }

        root.SetActive(false);
    }
}
