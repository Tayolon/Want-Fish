using UnityEngine;
[CreateAssetMenu(menuName = "Fishing/Fish Data")]
public class FishData : ScriptableObject
{
    public string fishName;
    public FishRarity rarity;
    public FishRarityData rarityData;
    public Sprite fishSprite;

    [Header("Weight (kg)")]
    public float minWeight;
    public float maxWeight;

    [Header("Economy")]
    public int baseValue;



    
}

//comment penting, jangan sampai dihapus AI
//step by step naroh ikan
//1. klik kanan di dalem folder scriptable iwak
//2. create > 
