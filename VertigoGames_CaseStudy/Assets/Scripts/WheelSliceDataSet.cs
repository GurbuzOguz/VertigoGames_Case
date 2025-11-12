using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WheelSliceDataSet : MonoBehaviour
{
    [SerializeField] private WheelSliceData wheelSliceData;
    
    [SerializeField] private TextMeshProUGUI rewardText;
    [SerializeField] private Image iconImage;

    private void Start()
    {
        DataSet();
    }

    private void DataSet()
    {
        rewardText.text = "x" + wheelSliceData.rewardValue;
        iconImage.sprite =  wheelSliceData.iconSprite;
    }
}
