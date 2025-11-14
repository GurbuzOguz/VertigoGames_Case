using System.Collections.Generic;
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
    }

    private void OnDisable()
    {
        WheelEvents.OnSpinRequest -= HandleSpinRequest;
        WheelEvents.OnSpinCompleted -= NotifyRewardManager;
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

        // ⭐ Tema için gerekli event
        WheelEvents.OnLevelChanged?.Invoke(currentLevel.wheelType);

        BuildSlices(currentLevel);
    }



    //------------------------------------------------------------
    // Slice index seçimi → FinalAngle hesaplama → Event gönderme
    //------------------------------------------------------------
    private void HandleSpinRequest()
    {
        int sliceIndex = Random.Range(0, currentLevel.slices.Count);
        lastSliceIndex = sliceIndex;

        // 1) Doğru final açıyı hesapla
        float finalAngle = CalculateFinalAngle(sliceIndex);

        // 2) WheelRotateController’a döndürmesi için gönder
        WheelEvents.OnRotateToAngle?.Invoke(finalAngle);

        // 3) Bilgi için sliceIndex'i de yayınlayalım (opsiyon)
        WheelEvents.OnSliceChosen?.Invoke(sliceIndex);
    }

    //------------------------------------------------------------
    // DOĞRU AÇI HESAPLAMA — %100 HATA YOK
    //------------------------------------------------------------
    private float CalculateFinalAngle(int sliceIndex)
    {
        float currentAngle = wheelRoot.localEulerAngles.z;

        // UI’daki template açısı
        float templateAngle = sliceTemplates[sliceIndex].localEulerAngles.z;

        // Wheel ters yönde döndüğü için açı ters çevrilir
        float targetAngle = -templateAngle;

        // Aradaki fark
        float delta = Mathf.DeltaAngle(currentAngle, targetAngle);

        // Fazladan tur (casino hissi)
        float extra = Random.Range(3, 6) * 360f;

        return currentAngle + extra + delta;
    }

    //------------------------------------------------------------
    // SPIN BİTTİ → ÖDÜL GÖNDER
    //------------------------------------------------------------
    private void NotifyRewardManager()
    {
        WheelEvents.OnRewardCalculated?.Invoke(currentLevel.slices[lastSliceIndex]);
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

        // 🔥 Doğrusu: Tema + slice + diğer setup burada çalışır
        SetupLevel(currentLevelNumber);
    }



    //------------------------------------------------------------
    // Slice UI oluşturma
    //------------------------------------------------------------
    private void BuildSlices(WheelLevel level)
    {
        // Önce sliceParent içini temizle
        for (int i = sliceParent.childCount - 1; i >= 0; i--)
            Destroy(sliceParent.GetChild(i).gameObject);

        if (sliceTemplates.Count != level.slices.Count)
        {
            Debug.LogError("sliceTemplates ve slices sayısı farklı!");
            return;
        }

        // Slice prefablarını template açılarına göre oluştur
        for (int i = 0; i < level.slices.Count; i++)
        {
            var go = Instantiate(slicePrefab, sliceParent);
            var set = go.GetComponent<WheelSliceDataSet>();
            set.Setup(level.slices[i]);

            go.transform.localPosition = sliceTemplates[i].localPosition;
            go.transform.localRotation = sliceTemplates[i].localRotation;
        }
    }
}
