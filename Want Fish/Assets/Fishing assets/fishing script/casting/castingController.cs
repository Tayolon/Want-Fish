using UnityEngine;
using System.Collections;


public class CastingController : MonoBehaviour
{
    public RectTransform bar;
    public RectTransform indicator;
    public float speed = 600f;

    private int direction = 1;
    private bool isCasting = false;

    public int reelDifficulty;

    public GameObject castingBarUI;


    float redLimit;
    float orangeLimit;
    float greenLimit;
    float yellowLimit;

    [Header("Color Layers")]
public RectTransform redLayer;
public RectTransform orangeLayer;
public RectTransform greenLayer;
public RectTransform yellowLayer;

    [Header("Casting Stop Delay")]
    public float stopDelay = 0.4f;

[Header("Casting Result UI")]
public CastingResultController resultUI;


    [Header("Difficulty Wait Penalty")]
public float minWaitPenaltyPerDifficulty = 3f;
public float maxWaitPenaltyPerDifficulty = 5f;


    [Header("Waiting Fish Settings")]
public float minBiteTime = 1.5f;
public float maxBiteTime = 4f;

[Header("Fish Bite UI")]
public FishBiteController fishBiteUI;

    private bool isWaitingFish = false;


public ReelingController reelingController;
public FishDatabase fishDatabase;


    void Start()
    {
        redLimit = bar.rect.width / 2f;
        orangeLimit = redLimit * 0.7f;
        greenLimit = redLimit * 0.4f;
        yellowLimit = redLimit * 0.05f;
    }

    void Update()
    {
        if (!isCasting) return;

        indicator.anchoredPosition +=
            Vector2.right * speed * direction * Time.deltaTime;

        if (Mathf.Abs(indicator.anchoredPosition.x) >= redLimit)
            direction *= -1;
    }

public void StartCasting()
{
    if (isWaitingFish) return;

    castingBarUI.SetActive(true);
    isCasting = true;
    direction = 1;
    indicator.anchoredPosition =
        new Vector2(-redLimit, indicator.anchoredPosition.y);

    reelDifficulty = 0;
    RandomizeLayerPositions();
}


bool IsInside(RectTransform layer, float indicatorX)
{
    float half = layer.rect.width / 2f;
    float center = layer.anchoredPosition.x;

    return indicatorX >= center - half &&
           indicatorX <= center + half;
}

IEnumerator StopCastingDelay()
{
    // tunggu sebentar biar player bisa lihat hasil
    yield return new WaitForSeconds(stopDelay);

    castingBarUI.SetActive(false);

    EvaluateZone();
    StartWaitingFish();
}


public void StopCasting()
{
    if (!isCasting) return;

    isCasting = false; // hentikan gerakan indicator
    StartCoroutine(StopCastingDelay());
}


void RandomizeLayerPositions()
{
    float barHalf = redLimit;

    RandomizeLayer(orangeLayer, barHalf);
    RandomizeLayer(greenLayer, barHalf);
    RandomizeLayer(yellowLayer, barHalf);
}

void RandomizeLayer(RectTransform layer, float barHalf)
{
    float layerHalf = layer.rect.width / 2f;

    float minX = -barHalf + layerHalf;
    float maxX = barHalf - layerHalf;

    float randomX = Random.Range(minX, maxX);

    layer.anchoredPosition =
        new Vector2(randomX, layer.anchoredPosition.y);
}


    public bool IsCasting()
    {
        return isCasting;
    }

   void EvaluateZone()
{
    float x = indicator.anchoredPosition.x;

    if (IsInside(yellowLayer, x))
    {
        reelDifficulty += 0;
        resultUI.ShowYellow();
    }
    else if (IsInside(greenLayer, x))
    {
        reelDifficulty += 1;
        resultUI.ShowGreen();
    }
    else if (IsInside(orangeLayer, x))
    {
        reelDifficulty += 3;
        resultUI.ShowOrange();
    }
    else
    {
        reelDifficulty += 5;
        resultUI.ShowRed();
    }

    Debug.Log("Total Difficulty: " + reelDifficulty);
}


void StartWaitingFish()
{
    if (!isWaitingFish)
        StartCoroutine(WaitFishBite());
}

    IEnumerator WaitFishBite()
    {
        isWaitingFish = true;

        float baseWait = Random.Range(minBiteTime, maxBiteTime);

        float penalty = 0f;
        for (int i = 0; i < reelDifficulty; i++)
        {
            penalty += Random.Range(
                minWaitPenaltyPerDifficulty,
                maxWaitPenaltyPerDifficulty
            );
        }

        float waitTime = baseWait + penalty;

        Debug.Log("Menunggu ikan: " + waitTime + " detik");

        yield return new WaitForSeconds(waitTime);

        Debug.Log("Ikan menggigit!");
        isWaitingFish = false;

        FishData fish = fishDatabase.GetRandomFish();
        fishBiteUI.Show(fish.rarity);

        StartCoroutine(StartReelingDelay(fish));

    }

IEnumerator StartReelingDelay(FishData fish)
{
    yield return new WaitForSeconds(0.8f); // tunggu popup selesai

    reelingController.StartReeling(
        fish.rarityData,
        reelDifficulty
    );
}

    public bool IsBusy()
    {
        if (isWaitingFish)
            return true;

        if (reelingController != null && reelingController.IsReeling())
            return true;

        FishRevealController reveal =
            FindObjectOfType<FishRevealController>();

        if (reveal != null && reveal.IsShowing())
            return true;

        return false;
    }




void StartReeling()
{
    FishData fish = fishDatabase.GetRandomFish();

    if (fish == null)
    {
        Debug.LogError("Fish NULL!");
        return;
    }

    Debug.Log("Dapat ikan: " + fish.fishName);

    reelingController.StartReeling(
        fish.rarityData,
        reelDifficulty
    );
}



}
