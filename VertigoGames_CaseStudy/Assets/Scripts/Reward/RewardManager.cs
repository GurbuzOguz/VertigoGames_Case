using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;

public class RewardManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Transform rewardListParent;
    [SerializeField] private GameObject rewardItemPrefab;

    private Dictionary<SliceType, RewardUIItem> activeRewards = new();

    private void OnEnable()
    {
        WheelEvents.OnRewardCalculated += HandleReward;
        WheelEvents.OnLevelReset += ResetRewards;
    }

    private void OnDisable()
    {
        WheelEvents.OnRewardCalculated -= HandleReward;
        WheelEvents.OnLevelReset -= ResetRewards;
    }

    private void HandleReward(WheelSliceData data)
    {
        if (data.sliceType == SliceType.Bomb)
        {
            ResetRewards();
            return;
        }

        RewardUIItem uiItem;

        if (!activeRewards.TryGetValue(data.sliceType, out uiItem))
        {
            // 🟦 1) ÖNCE UI ITEM OLUŞTUR (Ama sayıyı artırma)
            var go = Instantiate(rewardItemPrefab, rewardListParent);
            uiItem = go.GetComponent<RewardUIItem>();
            uiItem.Setup(data.iconSprite, 0); // amount = 0 başla

            activeRewards.Add(data.sliceType, uiItem);
        }

        // 🟦 2) Pop efekt oynat
        RewardPopEffectController.Instance.PlayPopEffect(data, uiItem.IconTransform, () =>
        {
            // 🟦 3) Efekt bitince sayıyı artır
            uiItem.AddAmount(data.rewardValue);
        });
    }


    
    private void ResetRewards()
    {
        foreach (var item in activeRewards.Values)
            Destroy(item.gameObject);

        activeRewards.Clear();
    }
    
    private void AddOrUpdateReward(WheelSliceData data)
    {
        if (activeRewards.ContainsKey(data.sliceType))
        {
            // SAME REWARD → Sayıyı arttır
            activeRewards[data.sliceType].AddAmount(data.rewardValue);
        }
        else
        {
            // YENİ BİR REWARD → UI item oluştur
            var go = Instantiate(rewardItemPrefab, rewardListParent);
            var rewardUI = go.GetComponent<RewardUIItem>();

            rewardUI.Setup(data.iconSprite, data.rewardValue);

            activeRewards.Add(data.sliceType, rewardUI);
        }
    }
}