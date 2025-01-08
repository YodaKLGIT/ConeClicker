using UnityEngine;

public abstract class ConeUpgrade : ScriptableObject
{
    public float UpgradeAmount = 1f;

    // The original cost and current cost
    public double OriginalUpgradeCost = 100;
    public double CurrentUpgradeCost = 100;

    // Multiplier for cost increase per purchase
    public double CostIncreaseMultiplierPerPurchase = 1.05f; // 5% increase

    // UI-related information
    public string UpgradeButtonText;
    [TextArea(3, 10)]
    public string UpgradeButtonDescription;

    // Abstract method to be implemented for applying the upgrade
    public abstract void ApplyUpgrade();

    // Method to apply cost increase after each purchase
    public void IncreaseCost()
    {
        // Correct the multiplier and ensure cost increases
        CurrentUpgradeCost = Mathf.CeilToInt((float)(CurrentUpgradeCost * (1 + CostIncreaseMultiplierPerPurchase)));
    }

    // Optional: Method to reset to the original cost
    public void ResetCost()
    {
        CurrentUpgradeCost = OriginalUpgradeCost;
    }
}
