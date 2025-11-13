using System;
using System.Collections.Generic;
using UnityEngine;

public enum WheelType
{
    Bronze,
    Silver,
    Gold
}

[Serializable]
public class WheelLevel
{
    public int levelNumber;
    public WheelType wheelType;
    public List<WheelSliceData> slices = new List<WheelSliceData>(8);
}

[CreateAssetMenu(fileName = "WheelLevelDatabase", menuName = "Wheel/WheelLevelDatabase")]
public class WheelLevelDataBase: ScriptableObject
{
    [Header("All Levels List")]
    public List<WheelLevel> levels = new List<WheelLevel>();

    [Header("Slice Scriptable Objects List")]
    public List<WheelSliceData> allSlicePool = new List<WheelSliceData>();

#if UNITY_EDITOR
    
    [ContextMenu("Add New Level")]
    private void AddNewLevel()
    {
        int newIndex = levels.Count + 1;
        var newLevel = new WheelLevel();
        newLevel.levelNumber = newIndex;
        newLevel.wheelType = GetWheelTypeForLevel(newIndex);

        AutoFillSlices(newLevel);

        levels.Add(newLevel);
    }

    private WheelType GetWheelTypeForLevel(int levelNumber)
    {
        if (levelNumber % 30 == 0)
            return WheelType.Gold;
        if (levelNumber % 5 == 0)
            return WheelType.Silver;
        return WheelType.Bronze;
    }

    private void AutoFillSlices(WheelLevel level)
    {
        level.slices.Clear();

        if (allSlicePool == null || allSlicePool.Count == 0)
        {
            Debug.LogWarning("WheelLevelDatabase: allSlicePool boş, slice doldurulamıyor.");
            return;
        }

        // 8 adet slice doldur
        int safety = 0;
        while (level.slices.Count < 8 && safety < 1000)
        {
            safety++;
            var candidate = allSlicePool[UnityEngine.Random.Range(0, allSlicePool.Count)];

            // Silver & Gold level'larda bomba yasak
            if (level.wheelType != WheelType.Bronze && candidate.sliceType == SliceType.Bomb)
                continue;

            level.slices.Add(candidate);
        }
    }

    // Inspector'da elle + ile level eklesen bile kuralları düzeltmek için
    private void OnValidate()
    {
        for (int i = 0; i < levels.Count; i++)
        {
            var level = levels[i];

            // level numarasını liste sırasına göre sabitle
            level.levelNumber = i + 1;

            // level tipini kuraldan üret (5: silver, 30: gold)
            level.wheelType = GetWheelTypeForLevel(level.levelNumber);

            // slice sayısı 8 olsun
            if (level.slices == null)
                level.slices = new List<WheelSliceData>(8);

            // Silver & Gold'da BOMBs varsa temizle
            if (level.wheelType != WheelType.Bronze)
            {
                for (int s = level.slices.Count - 1; s >= 0; s--)
                {
                    var data = level.slices[s];
                    if (data != null && data.sliceType == SliceType.Bomb)
                        level.slices.RemoveAt(s);
                }
            }

            // Eksikse otomatik doldur
            if (level.slices.Count < 8)
                AutoFillSlices(level);
        }
    }
#endif
}