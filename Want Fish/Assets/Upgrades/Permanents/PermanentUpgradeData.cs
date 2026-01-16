using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/Permanent Upgrade")]
public class PermanentUpgradeData : ScriptableObject
{
    public string id;
    public string displayName;
    public int maxLevel;
    public int baseCost;
}
