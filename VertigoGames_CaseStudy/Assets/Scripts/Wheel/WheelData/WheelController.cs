using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class WheelController : MonoBehaviour
{
    [Header("Level Data")]
    [SerializeField] private WheelLevelDataBase levelDatabase;

    [Header("References")]
    [SerializeField] private Transform wheelRoot;          // WheelRotateController’ın döndürdüğü objedir
    [SerializeField] private Transform sliceParent;        // Instantiate edilen slice'ların parent'ı
    [SerializeField] private GameObject slicePrefab;       // Slice görsel prefabı
    [SerializeField] private List<Transform> sliceTemplates; // UI template referansları (her biri 0–7)

    private WheelLevel currentLevel;
    private int lastSliceIndex;
    private int currentLevelNumber = 1;

    private void Start()
    {
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
        int sliceIndex = Random.Range(0, currentLevel.slices.Count);
        lastSliceIndex = sliceIndex;

        float finalAngle = CalculateFinalAngle(sliceIndex);

        WheelEvents.OnRotateToAngle?.Invoke(finalAngle);
        WheelEvents.OnSliceChosen?.Invoke(sliceIndex);
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
            Debug.LogWarning("GoToNextLevel → Yeni level bulunamadı. En yüksek levele ulaşıldı.");
            return;
        }

        Debug.Log("Yeni level yüklendi → Level " + currentLevelNumber);
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
            set.Setup(level.slices[i]);

            go.transform.localPosition = sliceTemplates[i].localPosition;
            go.transform.localRotation = sliceTemplates[i].localRotation;
            go.transform.DOScale(Vector3.zero, .5f).SetEase(Ease.OutBack).From();
        }
    }
    
    private void ResetLevel()
    {
        currentLevelNumber = 1;
        SetupLevel(currentLevelNumber);
    }
}
