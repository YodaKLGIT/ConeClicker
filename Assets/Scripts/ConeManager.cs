using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using Assets.Scripts;
using System;

public class ConeManager : MonoBehaviour
{
    public static ConeManager instance;

    public GameObject MainGameCanvas;
    [SerializeField] private GameObject _upgradeCanvas;
    [SerializeField] private TextMeshProUGUI _coneCountText;
    [SerializeField] private TextMeshProUGUI _conePerSecondText;
    [SerializeField] private TextMeshProUGUI _fragmentCountText;
    [SerializeField] public GameObject _coneObj;

    [Space]
    public ConeUpgrade[] coneUpgrades;
    [SerializeField] private GameObject _UpgradeUIToSpawn;
    [SerializeField] private Transform _UpgradeUIParent;
    public GameObject ConesPerSecondObjToSpawn;

    // Objects
    public double CurrentConeCount { get; set; }
    public double CurrentFragmentCount { get; set; }
    public double CurrentConePerSecond { get; set; }

    // Upgrades
    public double ConesPerClickUpgrade { get; set; }

    private InitializeUpgrades _InitializeUpgrades;
    private ConeDisplay _coneDisplay;
    private ConePerSecondTimer _conePerSecondTimer;

    [SerializeField] private float spinChance = 25f; // 25% chance to spin
    public GameObject GetConeObj()
    {
        return _coneObj;
    }

    private void Awake()
    {
        if (instance == null) instance = this;

        _coneDisplay = GetComponent<ConeDisplay>();
        _conePerSecondTimer = FindObjectOfType<ConePerSecondTimer>(); // Locate the timer script

        UpdateConeUI();
        UpdateFragmentsUI();
        UpdateConesPerSecondUI();

        _upgradeCanvas.SetActive(false);
        MainGameCanvas.SetActive(true);

        _InitializeUpgrades = GetComponent<InitializeUpgrades>();
        _InitializeUpgrades.Initialize(coneUpgrades, _UpgradeUIToSpawn, _UpgradeUIParent);
    }

    private void Start()
    {
        LoadGame();  // Load the game and its data
        SyncConePerSecondTimer(); // Sync timer after loading game
    }

    #region UI Updates
    private void UpdateConeUI()
    {
        _coneDisplay.UpdateConeText(CurrentConeCount, _coneCountText, " cones");
    }

    private void UpdateConesPerSecondUI()
    {
        _coneDisplay.UpdateConeText(CurrentConePerSecond, _conePerSecondText, " p/s");
    }

    private void UpdateFragmentsUI()
    {
        _coneDisplay.UpdateConeText(CurrentFragmentCount, _fragmentCountText, " fragments");
    }
    #endregion

    #region On Cone Clicked
    public void OnConeClicked()
    {
        IncreaseCone();

        _coneObj.transform.DOBlendableScaleBy(new Vector3(7.05f, 7.05f, 7.05f), 0.05f)
            .OnComplete(ConeScaleBack);

        if (UnityEngine.Random.Range(0f, 100f) <= spinChance)
        {
            _coneObj.transform.DORotate(new Vector3(0, 360, 0), 0.5f, RotateMode.FastBeyond360);
        }

        TryGainFragment();
    }

    private void TryGainFragment()
    {
        if (UnityEngine.Random.Range(0f, 100f) <= 0.5f) // 0.5% chance
        {
            CurrentFragmentCount++;
            UpdateFragmentsUI();
        }
    }

    private void ConeScaleBack()
    {
        _coneObj.transform.DOBlendableScaleBy(new Vector3(-7.05f, -7.05f, -7.05f), 0.05f);
    }

    public void IncreaseCone()
    {
        CurrentConeCount += 1 + ConesPerClickUpgrade;
        UpdateConeUI();
    }

    public void OnUpgradeButtonClick(ConeUpgrade upgrade, UpgradeButtonReferences buttonRef)
    {
        // Check if the player has enough cones for the upgrade
        if (CurrentConeCount >= upgrade.CurrentUpgradeCost)
        {
            // Apply the upgrade
            upgrade.ApplyUpgrade();

            // Deduct the cost
            CurrentConeCount -= upgrade.CurrentUpgradeCost;
            UpdateConeUI();

            // Update the upgrade cost
            upgrade.IncreaseCost();

            // Update the UI with the new cost
            buttonRef.UpgradeCostText.text = "Cost: " + upgrade.CurrentUpgradeCost.ToString("F0");

            // Log the update process for debugging
            Debug.Log($"Upgraded: {upgrade.name} | New Cost: {upgrade.CurrentUpgradeCost} | Button Updated: {buttonRef.UpgradeCostText.text}");
        }
    }


    #endregion

    #region Timer Syncing
    private void SyncConePerSecondTimer()
    {
        if (_conePerSecondTimer != null)
        {
            _conePerSecondTimer.ConePerSecond = CurrentConePerSecond;
        }
        else
        {
            Debug.LogWarning("ConePerSecondTimer not found in the scene.");
        }
    }
    #endregion

    #region Button Presses
    public void OnUpgradeButtonPress()
    {
        _upgradeCanvas.SetActive(!_upgradeCanvas.activeSelf);
    }

    public void OnResumeButtonPress()
    {
        _upgradeCanvas.SetActive(false);
    }
    #endregion

    #region Simple Increases
    public void SimpleConeIncrease(double amount)
    {
        CurrentConeCount += amount;
        UpdateConeUI();
    }

    public void SimpleConePerSecondIncrease(double amount)
    {
        CurrentConePerSecond += amount;
        UpdateConesPerSecondUI();
        SyncConePerSecondTimer(); // Sync the timer whenever CPS changes
    }
    #endregion

    #region Save/Load Data
    public void SaveGame()
    {
        GameData data = new GameData
        {
            CurrentConeCount = CurrentConeCount,
            CurrentFragmentCount = CurrentFragmentCount,
            CurrentConePerSecond = CurrentConePerSecond,
            ConesPerClickUpgrade = ConesPerClickUpgrade,
            UpgradeCosts = new List<double>()
        };

        foreach (var upgrade in coneUpgrades)
        {
            data.UpgradeCosts.Add(upgrade.CurrentUpgradeCost);
        }

        SaveSystem.Save(data);
        Debug.Log("Game saved successfully.");
    }
    private void LoadGame()
    {
        GameData data = SaveSystem.Load();

        if (data != null)
        {
            CurrentConeCount = data.CurrentConeCount;
            CurrentFragmentCount = data.CurrentFragmentCount;
            CurrentConePerSecond = data.CurrentConePerSecond;
            ConesPerClickUpgrade = data.ConesPerClickUpgrade;

            // Load each upgrade's current cost
            if (data.UpgradeCosts != null)
            {
                for (int i = 0; i < coneUpgrades.Length; i++)
                {
                    ConeUpgrade upgrade = coneUpgrades[i];
                    if (i < data.UpgradeCosts.Count)
                    {
                        upgrade.CurrentUpgradeCost = data.UpgradeCosts[i];

                        // Update the UI for this specific upgrade
                        Transform buttonTransform = _UpgradeUIParent.GetChild(i);
                        UpgradeButtonReferences buttonRef = buttonTransform.GetComponent<UpgradeButtonReferences>();

                        if (buttonRef != null)
                        {
                            // Set the price text just like when upgrading
                            buttonRef.UpgradeCostText.text = "Cost: " + upgrade.CurrentUpgradeCost.ToString("F0");
                        }
                    }
                }
            }

            Debug.Log("Game Loaded");
        }
        else
        {
            Debug.LogWarning("No saved data found. Starting fresh.");
            CurrentConeCount = 0;
            CurrentFragmentCount = 0;
            CurrentConePerSecond = 0;
            ConesPerClickUpgrade = 0;
        }

        UpdateConeUI();
        UpdateFragmentsUI();
        UpdateConesPerSecondUI();
        SyncConePerSecondTimer();
    }





    private void OnApplicationQuit()
    {
        SaveGame();
    }
    #endregion
}
