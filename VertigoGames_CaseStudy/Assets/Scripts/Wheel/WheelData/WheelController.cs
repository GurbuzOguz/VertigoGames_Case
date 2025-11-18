using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class WheelController : MonoBehaviour
{
    [Header("Level Data")] [SerializeField]
    private WheelLevelDataBase levelDatabase;

    [Header("References")] [SerializeField]
    private Transform wheelRoot;

    [SerializeField] private Transform sliceParent;
    [SerializeField] private GameObject slicePrefab;
    [SerializeField] private List<Transform> sliceTemplates;

    private WheelLevel currentLevel;
    private int lastSliceIndex;
    private int currentLevelNumber = 1;

    private void Start()
    {
        currentLevelNumber = PlayerPrefs.GetInt("CurrentLevelIndex", 0);
        currentLevelNumber += 1;
        

        SetupLevel(currentLevelNumber);
    }

    private void OnEnable()
    {
        WheelEvents.OnSpinRequest += HandleSpinRequest;
        WheelEvents.OnSpinCompleted += NotifyRewardManager;
        WheelEvents.OnLevelReset += ResetLevel;
    }

    private void OnDisable()
    {
        WheelEvents.OnSpinRequest -= HandleSpinRequest;
        WheelEvents.OnSpinCompleted -= NotifyRewardManager;
        WheelEvents.OnLevelReset -= ResetLevel;
    }

    public void SetupLevel(int levelNum)
    {
        currentLevelNumber = levelNum;
        currentLevel = levelDatabase.levels.Find(l => l.levelNumber == levelNum);

        if (currentLevel == null)
        {
            Debug.LogError("Level bulunamadı: " + levelNum);
            return;
        }

        WheelEvents.OnLevelChanged?.Invoke(currentLevel.wheelType);
        WheelEvents.OnLevelNumberChanged?.Invoke(currentLevelNumber);

        BuildSlices(currentLevel);
    }

    private void HandleSpinRequest()
    {
        int sliceIndex = 0;

        if (currentLevel.selectionMode == SliceSelectionMode.Random)
        {
            if (currentLevel.useWeightedRandom)
                sliceIndex = GetWeightedRandomIndex(currentLevel.slices);
            else
                sliceIndex = Random.Range(0, currentLevel.slices.Count);
        }


        else if (currentLevel.selectionMode == SliceSelectionMode.Fixed)
            sliceIndex = Mathf.Clamp(currentLevel.fixedSliceIndex, 0, currentLevel.slices.Count - 1);

        lastSliceIndex = sliceIndex;

        float finalAngle = CalculateFinalAngle(sliceIndex);

        WheelEvents.OnRotateToAngle?.Invoke(finalAngle);
        WheelEvents.OnSliceChosen?.Invoke(sliceIndex);
    }
    
    private int GetWeightedRandomIndex(List<WheelSliceData> slices)
    {
        float totalWeight = 0f;

        for (int i = 0; i < slices.Count; i++)
            totalWeight += Mathf.Max(0.001f, slices[i].weight);

        float randomValue = Random.value * totalWeight;

        float cumulative = 0f;

        for (int i = 0; i < slices.Count; i++)
        {
            cumulative += Mathf.Max(0.001f, slices[i].weight);
            if (randomValue <= cumulative)
                return i;
        }

        return slices.Count - 1;
    }

    private float CalculateFinalAngle(int sliceIndex)
    {
        float currentAngle = wheelRoot.localEulerAngles.z;
        float templateAngle = sliceTemplates[sliceIndex].localEulerAngles.z;
        float targetAngle = -templateAngle;

        float delta = Mathf.DeltaAngle(currentAngle, targetAngle);
        float extra = Random.Range(3, 6) * 360f;

        return currentAngle + extra + delta;
    }

    private void NotifyRewardManager()
    {
        WheelSliceData slice = currentLevel.slices[lastSliceIndex];

        if (slice.sliceType == SliceType.Bomb)
        {
            Debug.Log("BOMBA seçildi → Lose!");
            WheelEvents.OnBombSelected?.Invoke();
            return;
        }

        WheelEvents.OnRewardCalculated?.Invoke(slice);
        GoToNextLevel();
    }


    private void GoToNextLevel()
    {
        currentLevelNumber++;

        WheelLevel nextLevel = levelDatabase.levels.Find(l => l.levelNumber == currentLevelNumber);

        if (nextLevel == null)
        {
            WheelEvents.OnAllLevelsFinished?.Invoke();
            return;
        }

        SetupLevel(currentLevelNumber);
    }



    private void BuildSlices(WheelLevel level)
    {
        for (int i = sliceParent.childCount - 1; i >= 0; i--)
            Destroy(sliceParent.GetChild(i).gameObject);

        if (sliceTemplates.Count != level.slices.Count)
        {
            Debug.LogError("sliceTemplates ve slices sayısı farklı!");
            return;
        }

        for (int i = 0; i < level.slices.Count; i++)
        {
            var go = Instantiate(slicePrefab, sliceParent);
            var set = go.GetComponent<WheelSliceDataSet>();

            set.Setup(level.slices[i], level.rewardMultiplier);

            go.transform.localPosition = sliceTemplates[i].localPosition;
            go.transform.localRotation = sliceTemplates[i].localRotation;

         
            go.transform.localScale = Vector3.zero;
            go.transform.DOScale(1f, 0.35f)
                .SetEase(Ease.OutBack)
                .SetDelay(i * 0.03f); 
        }
    }



    private void ResetLevel()
    {
        currentLevelNumber = 1;
        SetupLevel(currentLevelNumber);
    }
}