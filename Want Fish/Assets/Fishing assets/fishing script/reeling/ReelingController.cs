using UnityEngine;

public class ReelingController : MonoBehaviour
{
    private float baseTargetSpeed;

// base value (buat reset / fallback)
float baseMinMoveDuration;
float baseMaxMoveDuration;
float baseMinIdleDuration;
float baseMaxIdleDuration;
float baseMinSpeedMultiplier;
float baseMaxSpeedMultiplier;

float baseBurstChance;
float baseBurstSpeedMultiplier;
float baseMinBurstDuration;
float baseMaxBurstDuration;


    [Header("UI")]
    public GameObject reelingUI;
    public RectTransform greenZone;
    public RectTransform targetLine;
    public UnityEngine.UI.Slider progressBar;

    [Header("Green Zone Size")]
public float baseGreenHeight = 120f;   // ukuran normal
public float shrinkPerDifficulty = 6f; // tiap 1 difficulty menyusut berapa px


    [Header("Grace Time")]
    public float failDelay = 0.5f;

    private float failTimer;

    [Header("Target Random Movement")]
    public float minMoveDuration = 0.2f;
    public float maxMoveDuration = 0.8f;
    public float minIdleDuration = 0.1f;
    public float maxIdleDuration = 0.6f;
    public float minSpeedMultiplier = 0.6f;
    public float maxSpeedMultiplier = 1.4f;

    float moveTimer;
    bool isIdle;
    float currentSpeedMultiplier;

[Header("Target Burst Speed")]
public float burstChance = 0.2f; // 20% chance jadi burst
public float burstSpeedMultiplier = 2.2f;

public float minBurstDuration = 0.15f;
public float maxBurstDuration = 0.4f;

bool isBurst;


    [Header("Movement")]
    public float greenMoveSpeed = 400f;
    public float greenGravity = 600f;
    public float targetSpeed = 300f;

    [Header("SP")]
    public float spGainPerSecond = 1f;

    private float currentSP;
    private float requiredSP;
    private float spLossPerSecond;

    private float greenVelocity;
    private float targetDirection = 1;

    [Header("Bounds")]
public RectTransform blueBar;
float greenMinY;
float greenMaxY;
float targetMinY;
float targetMaxY;


public enum ReelingState
{
    None,
    Active,
    Success,
    Fail
}

    private ReelingState state;

    #region START

void Awake()
{
    baseTargetSpeed = targetSpeed;

    baseMinMoveDuration = minMoveDuration;
    baseMaxMoveDuration = maxMoveDuration;
    baseMinIdleDuration = minIdleDuration;
    baseMaxIdleDuration = maxIdleDuration;
    baseMinSpeedMultiplier = minSpeedMultiplier;
    baseMaxSpeedMultiplier = maxSpeedMultiplier;

    baseBurstChance = burstChance;
    baseBurstSpeedMultiplier = burstSpeedMultiplier;
    baseMinBurstDuration = minBurstDuration;
    baseMaxBurstDuration = maxBurstDuration;
}

void OnEnable()
{
    state = ReelingState.None;
    Debug.Log("Reeling Update jalan");

}

public bool IsReeling()
{
    return state == ReelingState.Active;
}

//start-----------------------------------------sefoksofhsfkdshvishdliugeldknvkfvkdhfkeyginibukangptygnuliscommentnyakuranglebihygdibawahsiniygpenting
   public void StartReeling(FishRarityData rarityData, int reelDifficulty)

{

        PickNewTargetBehavior();

// ==========================
// APPLY RARITY MODIFIER
// ==========================

// movement
minMoveDuration = rarityData.minMoveDuration;
maxMoveDuration = rarityData.maxMoveDuration;

minIdleDuration = rarityData.minIdleDuration;
maxIdleDuration = rarityData.maxIdleDuration;

minSpeedMultiplier = rarityData.minSpeedMultiplier;
maxSpeedMultiplier = rarityData.maxSpeedMultiplier;

// target speed
targetSpeed =
    baseTargetSpeed *
    rarityData.targetSpeedMultiplier *
    (1f + rarityData.spLossPerSecond * 0.2f);

// burst
burstChance = rarityData.burstChance;
burstSpeedMultiplier = rarityData.burstSpeedMultiplier;
minBurstDuration = rarityData.minBurstDuration;
maxBurstDuration = rarityData.maxBurstDuration;


        // =====================
        // SET GREEN ZONE SIZE
        // =====================
        float shrinkAmount = reelDifficulty * shrinkPerDifficulty;
float finalHeight = Mathf.Max(
    baseGreenHeight - shrinkAmount,
    40f // batas minimum biar masih playable
);

greenZone.SetSizeWithCurrentAnchors(
    RectTransform.Axis.Vertical,
    finalHeight
);


        float barHalf = blueBar.rect.height / 2f;
    float greenHalf = greenZone.rect.height / 2f;
    float targetHalf = targetLine.rect.height / 2f;

    // batas hijau (biar ujungnya gak keluar)
    greenMinY = -barHalf + greenHalf;
    greenMaxY = barHalf - greenHalf;

    // batas target line
targetMinY = -barHalf + targetHalf;
targetMaxY = barHalf - targetHalf;



failTimer = failDelay;
float offset = Random.Range(-50f, 50f);
targetLine.anchoredPosition =
    greenZone.anchoredPosition + Vector2.up * offset;


    greenVelocity = 0;
    targetDirection = Random.value > 0.5f ? 1 : -1;

        reelingUI.SetActive(true);
//good luck mahamin code nya
        requiredSP = Random.Range(
            rarityData.minSP,
            rarityData.maxSP
        );

        spLossPerSecond = rarityData.spLossPerSecond;

        currentSP = 0;
        progressBar.value = 0;
        progressBar.maxValue = requiredSP;

targetSpeed = baseTargetSpeed * (1f + rarityData.spLossPerSecond * 0.2f);


        state = ReelingState.Active;
    }
    #endregion

    void Update()
    {
        if (state != ReelingState.Active) return;

        HandleGreenZone();
        HandleTarget();
        HandleSP();
    }

    #region MOVEMENT
void HandleGreenZone()
{
    if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.W))
        greenVelocity = greenMoveSpeed;
    else
        greenVelocity -= greenGravity * Time.deltaTime;

    greenZone.anchoredPosition +=
        Vector2.up * greenVelocity * Time.deltaTime;

    float clampedY = Mathf.Clamp(
        greenZone.anchoredPosition.y,
        greenMinY,
        greenMaxY
    );

    greenZone.anchoredPosition =
        new Vector2(greenZone.anchoredPosition.x, clampedY);
}




void HandleTarget()
{
    moveTimer -= Time.deltaTime;

    if (moveTimer <= 0)
        PickNewTargetBehavior();

    if (isIdle)
        return;

    // gerak target
    targetLine.anchoredPosition +=
        Vector2.up *
        targetSpeed *
        currentSpeedMultiplier *
        targetDirection *
        Time.deltaTime;

    // cek batas
    if (targetLine.anchoredPosition.y >= targetMaxY)
    {
        targetLine.anchoredPosition =
            new Vector2(targetLine.anchoredPosition.x, targetMaxY);

        targetDirection = -1;          // ⬇️ BALIK ARAH
        // PickNewTargetBehavior();       // optional
        return;
    }

    if (targetLine.anchoredPosition.y <= targetMinY)
    {
        targetLine.anchoredPosition =
            new Vector2(targetLine.anchoredPosition.x, targetMinY);

        targetDirection = 1;           // ⬆️ BALIK ARAH
        // PickNewTargetBehavior();       // optional
        return;
    }
}



void PickNewTargetBehavior()
{
    isIdle = Random.value < 0.25f; // 25% diam
    isBurst = false;

    if (isIdle)
    {
        moveTimer = Random.Range(minIdleDuration, maxIdleDuration);
        currentSpeedMultiplier = 0f;
        return;
    }

    // tentukan arah
    targetDirection = Random.value > 0.5f ? 1 : -1;

    // cek burst
    if (Random.value < burstChance)
    {
        isBurst = true;
        moveTimer = Random.Range(minBurstDuration, maxBurstDuration);
        currentSpeedMultiplier = burstSpeedMultiplier;
    }
    else
    {
        moveTimer = Random.Range(minMoveDuration, maxMoveDuration);
        currentSpeedMultiplier =
            Random.Range(minSpeedMultiplier, maxSpeedMultiplier);
    }
}



    #endregion

    #region SP LOGIC
    void HandleSP()
{
    bool inside =
        targetLine.anchoredPosition.y >=
        greenZone.anchoredPosition.y - greenZone.rect.height / 2 &&
        targetLine.anchoredPosition.y <=
        greenZone.anchoredPosition.y + greenZone.rect.height / 2;

    if (failTimer > 0)
    {
        // grace time aktif → TIDAK ADA SP LOSS
        if (inside)
            currentSP += spGainPerSecond * Time.deltaTime;

        failTimer -= Time.deltaTime;
    }
    else
    {
        if (inside)
            currentSP += spGainPerSecond * Time.deltaTime;
        else
            currentSP -= spLossPerSecond * Time.deltaTime;
    }

    currentSP = Mathf.Clamp(currentSP, 0, requiredSP);
    progressBar.value = currentSP;

    if (currentSP >= requiredSP)
    {
        Success();
        return;
    }

    if (currentSP <= 0 && failTimer <= 0)
        Fail();
}
   #endregion

void Success()
{
    state = ReelingState.Success;
    reelingUI.SetActive(false);

    FishData fish = FishDatabase.Instance.currentFish;
    FishRevealController reveal =
        FindObjectOfType<FishRevealController>();

    reveal.ShowFish(fish);
}


    void Fail()
    {
        state = ReelingState.Fail;
        Debug.Log("IKAN KABUR!");
        reelingUI.SetActive(false);
    }
}
