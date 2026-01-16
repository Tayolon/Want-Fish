using UnityEngine;

[CreateAssetMenu(fileName = "UpgradeCards", menuName = "Upgrades/Upgrade")]
public class UpgradeCards : ScriptableObject
{
    public string UpgradeName;
    public Sprite UpgradeSprite;
    public string UpgradeDescriptions;
    public bool SingleTimeUpgrade;
    public int Cost;
}
