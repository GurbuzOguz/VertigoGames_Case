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
    
    [Header("Only gold wheel slice pool")]
    public List<WheelSliceData> goldSlicePool = new List<WheelSliceData>();

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

        switch (level.wheelType)
        {
            // -------------------------------------------------------
            // ⭐ BRONZE LEVEL → Tam 1 adet bomba olacak
            // -------------------------------------------------------
            case WheelType.Bronze:
            {
                AddBombSlice(level);
                FillWithNonBomb(level, allSlicePool, 7); // geri kalan 7 dilim
                break;
            }

            // -------------------------------------------------------
            // ⭐ SILVER LEVEL → Bomba yok
            // -------------------------------------------------------
            case WheelType.Silver:
            {
                FillWithNonBomb(level, allSlicePool, 8);
                break;
            }

            // -------------------------------------------------------
            // ⭐ GOLD LEVEL → Bomba yok, özel gold havuzdan doldur
            // -------------------------------------------------------
            case WheelType.Gold:
            {
                FillWithNonBomb(level, goldSlicePool, 8);
                break;
            }
        }
        
        ShuffleList(level.slices);
    }
    
    private void ShuffleList<T>(List<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int swapIndex = UnityEngine.Random.Range(0, i + 1);
            (list[i], list[swapIndex]) = (list[swapIndex], list[i]); // tuple swap
        }
    }
    
    private void AddBombSlice(WheelLevel level)
    {
        var bomb = allSlicePool.Find(s => s.sliceType == SliceType.Bomb);
    
        if (bomb == null)
        {
            Debug.LogError("Bronze level için Bomb slice bulunamadı!");
            return;
        }

        level.slices.Add(bomb);
    }
    
    private void FillWithNonBomb(WheelLevel level, List<WheelSliceData> pool, int countToAdd)
    {
        if (pool == null || pool.Count == 0)
        {
            Debug.LogError("Slice pool boş, slice doldurulamadı.");
            return;
        }

        int added = 0;
        int safety = 0;

        while (added < countToAdd && safety < 1000)
        {
            safety++;

            var candidate = pool[UnityEngine.Random.Range(0, pool.Count)];

            if (candidate == null) 
                continue;

            // Bomb istemiyorsak atla
            if (candidate.sliceType == SliceType.Bomb)
                continue;

            level.slices.Add(candidate);
            added++;
        }

        if (added < countToAdd)
        {
            Debug.LogWarning($"FillWithNonBomb: Hedef {countToAdd}, eklenen {added}. Pool yetersiz olabilir.");
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