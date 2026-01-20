using UnityEngine;
using UnityEngine.SceneManagement;

public class ReelingController : MonoBehaviour
{
    public static ReelingController Instance;
    public GameTimer gameTimer;
    private float baseTargetSpeed;

    private Animator animator;  


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

    public AudioClip reelingLoopSound;
    public float reelingVolume = 0.5f;
    private AudioSource reelingAudio;

    public AudioClip fishCaughtSoundA;
    public AudioClip fishCaughtSoundB;
    public float fishCaughtVolume = 0.7f;


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
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        animator = GetComponent<Animator>();
        baseTargetSpeed = targetSpeed;

        reelingAudio = GetComponent<AudioSource>();
        if (reelingAudio == null)
            reelingAudio = gameObject.AddComponent<AudioSource>();

        reelingAudio.playOnAwake = false;
        reelingAudio.loop = true;

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
        SceneManager.sceneLoaded += OnSceneLoaded;
        state = ReelingState.None;
        Debug.Log("Reeling Update jalan");
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ApplyPermanentUpgrades();
    }

    void ApplyPermanentUpgrades()
    {
        baseGreenHeight = 120f;
        spGainPerSecond = 1f;

        int hookLevel =
            PermanentUpgradeManager.Instance.GetLevel("hook");

        baseGreenHeight += hookLevel * 15f;

        int lineLevel =
            PermanentUpgradeManager.Instance.GetLevel("line");

        spGainPerSecond += lineLevel * 1f;
    }



    public bool IsReeling()
    {
        return state == ReelingState.Active;
    }

    //start-----------------------------------------sefoksofhsfkdshvishdliugeldknvkfvkdhfkeyginibukangptygnuliscommentnyakuranglebihygdibawahsiniygpenting
    public void StartReeling(FishRarityData rarityData, int reelDifficulty)
    {
        PlayerFishingAnimation.Instance.StartReeling();

        if (reelingLoopSound != null && !reelingAudio.isPlaying)
        {
            reelingAudio.clip = reelingLoopSound;
            reelingAudio.volume = reelingVolume;
            reelingAudio.Play();
        }

        gameTimer.timerRunning = false;
        PickNewGreenBehavior();


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
        //WOW MAKASIH
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

        if (animator != null)
            animator.SetBool("IsReeling", true);
    }
    #endregion

    void Update()
    {
        if (state != ReelingState.Active) return;

        HandleTargetInput();       // PLAYER
        HandleGreenZoneRandom();   // MUSUH
        HandleSP();
    }


    #region MOVEMENT
    void HandleTargetInput()
    {
        if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.W))
            greenVelocity = greenMoveSpeed;
        else
            greenVelocity -= greenGravity * Time.deltaTime;

        targetLine.anchoredPosition +=
            Vector2.up * greenVelocity * Time.deltaTime;

        float clampedY = Mathf.Clamp(
            targetLine.anchoredPosition.y,
            targetMinY,
            targetMaxY
        );

        targetLine.anchoredPosition =
            new Vector2(targetLine.anchoredPosition.x, clampedY);
    }




    void HandleGreenZoneRandom()
    {
        moveTimer -= Time.deltaTime;

        if (moveTimer <= 0)
            PickNewGreenBehavior();

        if (isIdle)
            return;

        greenZone.anchoredPosition +=
            Vector2.up *
            targetSpeed *
            currentSpeedMultiplier *
            targetDirection *
            Time.deltaTime;

        if (greenZone.anchoredPosition.y >= greenMaxY)
        {
            greenZone.anchoredPosition =
                new Vector2(greenZone.anchoredPosition.x, greenMaxY);
            targetDirection = -1;
        }

        if (greenZone.anchoredPosition.y <= greenMinY)
        {
            greenZone.anchoredPosition =
                new Vector2(greenZone.anchoredPosition.x, greenMinY);
            targetDirection = 1;
        }
    }




    void PickNewGreenBehavior()
    {
        isIdle = Random.value < 0.25f;
        isBurst = false;

        if (isIdle)
        {
            moveTimer = Random.Range(minIdleDuration, maxIdleDuration);
            currentSpeedMultiplier = 0f;
            return;
        }

        targetDirection = Random.value > 0.5f ? 1 : -1;

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
        PlayerFishingAnimation.Instance.StopReeling();

        state = ReelingState.Success;
        reelingUI.SetActive(false);

        if (reelingAudio.isPlaying)
            reelingAudio.Stop();

        AudioClip chosenSound = null;

        if (fishCaughtSoundA != null && fishCaughtSoundB != null)
            chosenSound = Random.value < 0.5f ? fishCaughtSoundA : fishCaughtSoundB;
        else if (fishCaughtSoundA != null)
            chosenSound = fishCaughtSoundA;
        else if (fishCaughtSoundB != null)
            chosenSound = fishCaughtSoundB;

        if (chosenSound != null)
            reelingAudio.PlayOneShot(chosenSound, fishCaughtVolume);

            
        FishData fish = FishDatabase.Instance.currentFish;

        CaughtFish caught = CreateCaughtFish(fish);

        FindObjectOfType<FishRevealController>()
            .ShowFish(fish);

        Debug.Log("success");
        InventoryManager.Instance.AddFish(caught);
    }

    CaughtFish CreateCaughtFish(FishData fish)
    {
        float weight = Random.Range(
            fish.minWeight,
            fish.maxWeight
        );

        int value = Mathf.RoundToInt(
            fish.baseValue * weight
        );

        return new CaughtFish
        {
            data = fish,
            weight = weight,
            value = value
        };
    }



    void Fail()
    {
        PlayerFishingAnimation.Instance.StopReeling();

        state = ReelingState.Fail;
        reelingUI.SetActive(false);

        if (reelingAudio.isPlaying)
            reelingAudio.Stop();


        Debug.Log("IKAN KABUR!");
        CastingController.Instance.isFishing = false;
        CastingController.Instance.castingLocked = false;
        GameTimer.Instance.timerRunning = true;
    }
}