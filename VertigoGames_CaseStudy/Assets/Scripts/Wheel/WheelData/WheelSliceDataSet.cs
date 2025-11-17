using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WheelSliceDataSet : MonoBehaviour
{
    [Header("Slice Data")]
    [SerializeField] private WheelSliceData wheelSliceData;

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI rewardText_value;
    [SerializeField] private Image iconImage;

    public void Setup(WheelSliceData data, float multiplier)
    {
        wheelSliceData = data;

        if (data == null)
        {
            Debug.LogError("WheelSliceDataSet.Setup -> data is NULL");
            return;
        }

        iconImage.sprite = data.iconSprite;

        int finalReward = Mathf.RoundToInt(data.rewardValue * multiplier);

        rewardText_value.text = finalReward > 0 ? "x" + finalReward : "";
    }
}