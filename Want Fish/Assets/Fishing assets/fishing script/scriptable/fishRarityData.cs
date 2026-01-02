using UnityEngine;

[CreateAssetMenu(menuName = "Fishing/Rarity Data")]
public class FishRarityData : ScriptableObject
{
    public FishRarity rarity;

    [Header("Bite Chance (%)")]
    public float biteChance;

    [Header("SP Required")]
    public int minSP;
    public int maxSP;

    [Header("SP Loss Per Second")]
    public float spLossPerSecond;

    // =========================
    // TARGET RANDOM MOVEMENT
    // =========================
    [Header("Target Movement")]
    public float minMoveDuration = 0.2f;
    public float maxMoveDuration = 0.8f;

    public float minIdleDuration = 0.1f;
    public float maxIdleDuration = 0.6f;

    public float minSpeedMultiplier = 0.6f;
    public float maxSpeedMultiplier = 1.4f;

    [Tooltip("Modifier ke targetSpeed dasar")]
    public float targetSpeedMultiplier = 1f;

    // =========================
    // TARGET BURST
    // =========================
    [Header("Burst Behavior")]
    [Range(0f, 1f)]
    public float burstChance = 0.2f;

    public float burstSpeedMultiplier = 2.2f;

    public float minBurstDuration = 0.15f;
    public float maxBurstDuration = 0.4f;
}
