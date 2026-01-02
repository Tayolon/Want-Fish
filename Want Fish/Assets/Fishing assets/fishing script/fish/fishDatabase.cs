using System.Collections.Generic;
using UnityEngine;
//aku gk tau harus set beratnya gimana biar ngaruh ke harga
//takutnya,  berat jadi beda-beda, jadi bakal kerasa jauh banget, kayak ikan gede dari rare bisa aja ngalahin ikan kecil dari exotic
// ide dari gpt "weight affect XP bukan uang"

public class FishDatabase : MonoBehaviour
{
    public static FishDatabase Instance;

    public List<FishData> allFish;
    public FishData currentFish;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public FishData GetRandomFish()
    {
        FishRarityData rolledRarity = RollRarityData();
        if (rolledRarity == null) return null;

        List<FishData> pool =
            allFish.FindAll(f => f.rarityData == rolledRarity);

        if (pool.Count == 0) return null;

        currentFish = pool[Random.Range(0, pool.Count)];
        return currentFish;
    }


    FishRarityData RollRarityData()
    {
        // Ambil rarity unik & urutkan berdasarkan enum
        List<FishRarityData> rarityList = new List<FishRarityData>();

        foreach (var fish in allFish)
        {
            if (fish.rarityData != null &&
                !rarityList.Contains(fish.rarityData))
            {
                rarityList.Add(fish.rarityData);
            }
        }

        // Urutkan berdasarkan enum rarity
        rarityList.Sort((a, b) =>
            a.rarity.CompareTo(b.rarity)
        );

        // Tier-based roll
        foreach (var rarity in rarityList)
        {
            float roll = Random.Range(0f, 100f);

            if (roll <= rarity.biteChance)
            {
                Debug.Log("ROLL SUCCESS: " + rarity.rarity);
                return rarity;
            }
        }

        // fallback → Common
        Debug.Log("ROLL FALLBACK → COMMON");
        return rarityList[0];
    }

}
