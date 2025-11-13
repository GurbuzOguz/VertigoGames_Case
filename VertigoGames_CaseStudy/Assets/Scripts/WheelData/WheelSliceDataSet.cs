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

    public void Setup(WheelSliceData data)
    {
        wheelSliceData = data;

        if (data == null)
        {
            Debug.LogError("WheelSliceDataSet.Setup -> data is NULL");
            return;
        }

        iconImage.sprite = data.iconSprite;

        if (data.rewardValue > 0)
            rewardText_value.text = "x" + data.rewardValue.ToString();
        else
            rewardText_value.text = "";

    }
}