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
    }

    private void OnDisable()
    {
        WheelEvents.OnRewardCalculated -= HandleReward;
    }

    private void HandleReward(WheelSliceData data)
    {
        if (data.sliceType == SliceType.Bomb)
        {
            ResetRewards(); 
            return;
        }

        AddOrUpdateReward(data);
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