using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WheelController : MonoBehaviour
{
    [Header("Data")] [SerializeField] private WheelLevelDataBase levelDatabase;

    [Header("WheelTheme Data")] [SerializeField]
    private WheelThemeData bronzeTheme;

    [SerializeField] private WheelThemeData silverTheme;
    [SerializeField] private WheelThemeData goldTheme;

    [Header("UI References")] [SerializeField]
    private Image spinBackground;

    [SerializeField] private Image indicatorImage;
    [SerializeField] private TextMeshProUGUI titleText_value;
    [SerializeField] private TextMeshProUGUI rewardInfoText_value;
    [SerializeField] private Transform sliceParent;
    [SerializeField] private GameObject slicePrefab;
    [SerializeField] private Transform wheelRoot;
    [SerializeField] private int currentLevelNumber = 1;

    public List<GameObject> sliceTemplates;

    private WheelLevel currentLevel;
    private int lastSliceIndex;


    private void Start()
    {
        SetupLevel(currentLevelNumber);
    }

    private void OnEnable()
    {
        WheelEvents.OnSpinRequest += HandleSpinRequest;
        WheelEvents.OnSpinCompleted += NotifyRewardManager;
    }

    private void OnDisable()
    {
        WheelEvents.OnSpinRequest -= HandleSpinRequest;
        WheelEvents.OnSpinCompleted -= NotifyRewardManager;
    }

    public void SetupLevel(int levelNumber)
    {
        currentLevel = levelDatabase.levels.Find(l => l.levelNumber == levelNumber);

        ApplyTheme(currentLevel.wheelType);
        BuildSlices(currentLevel);
        UpdateTexts(currentLevel);
    }

    private void HandleSpinRequest()
    {
        int sliceIndex = UnityEngine.Random.Range(0, currentLevel.slices.Count);

        WheelEvents.OnSliceChosen?.Invoke(sliceIndex);
        SaveSliceIndex(sliceIndex);
    }


    private void ApplyTheme(WheelType type)
    {
        WheelThemeData theme = bronzeTheme;

        switch (type)
        {
            case WheelType.Silver:
                theme = silverTheme;
                break;
            case WheelType.Gold:
                theme = goldTheme;
                break;
        }

        if (theme == null)
        {
            Debug.LogWarning($"Theme for {type} bulunamadı.");
            return;
        }

        if (spinBackground != null)
            spinBackground.sprite = theme.spinBackground;

        if (indicatorImage != null)
            indicatorImage.sprite = theme.indicatorSprite;

        if (titleText_value != null)
            titleText_value.color = theme.titleColor;

        if (rewardInfoText_value != null)
            rewardInfoText_value.color = theme.rewardTextColor;
    }
    
    private void ClearSlices()
    {
        for (int i = sliceParent.childCount - 1; i >= 0; i--)
        {
            Destroy(sliceParent.GetChild(i).gameObject);
        }
    }

    private void BuildSlices(WheelLevel level)
    {
        ClearSlices();
            
        if (sliceTemplates.Count != level.slices.Count)
        {
            Debug.LogError("sliceTemplates ve slices sayısı eşleşmiyor!");
            return;
        }

        for (int i = 0; i < level.slices.Count; i++)
        {
            var sliceData = level.slices[i];

            var go = Instantiate(slicePrefab, sliceParent);
            var sliceSet = go.GetComponent<WheelSliceDataSet>();
            sliceSet.Setup(sliceData);

            go.transform.localPosition = sliceTemplates[i].transform.localPosition;
            go.transform.localRotation = sliceTemplates[i].transform.localRotation;

            Debug.Log($"Created Slice {i}: {sliceData.sliceType}");
        }
    }


    private void UpdateTexts(WheelLevel level)
    {
        if (titleText_value != null)
        {
            switch (level.wheelType)
            {
                case WheelType.Bronze:
                    titleText_value.text = "BRONZE SPIN";
                    break;
                case WheelType.Silver:
                    titleText_value.text = "SILVER SPIN";
                    break;
                case WheelType.Gold:
                    titleText_value.text = "GOLD SPIN";
                    break;
            }
        }

        if (rewardInfoText_value != null)
        {
            // Örnek: "Up to x10 Rewards" gibi
            rewardInfoText_value.text = "Up To x10 Rewards";
        }
    }

    private void SaveSliceIndex(int index)
    {
        lastSliceIndex = index;
    }

    private void NotifyRewardManager()
    {
        var sliceData = currentLevel.slices[lastSliceIndex];
        WheelEvents.OnRewardCalculated?.Invoke(sliceData);
        GoToNextLevel();
        
        Debug.Log("SHOULD BE SLICE INDEX = " + lastSliceIndex);
    }

    private void GoToNextLevel()
    {
        currentLevelNumber++;
        SetupLevel(currentLevelNumber);
    }

#if UNITY_EDITOR
    [ContextMenu("Test Level 1")]
    private void TestLevel1()
    {
        SetupLevel(1);
    }

    [ContextMenu("Test Level 5")]
    private void TestLevel5()
    {
        SetupLevel(5);
    }

    [ContextMenu("Test Level 30")]
    private void TestLevel30()
    {
        SetupLevel(30);
    }
#endif
}