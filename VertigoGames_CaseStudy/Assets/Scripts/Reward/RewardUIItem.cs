using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class RewardUIItem : MonoBehaviour
{
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI amountText_value;
    public Transform IconTransform => iconImage.transform;

    private int currentAmount = 0;

    public void Setup(Sprite icon, int amount)
    {
        iconImage.sprite = icon;
        currentAmount = amount;
        amountText_value.text = "x" + amount;
    }


    public void AddAmount(int amount)
    {
        int start = currentAmount;
        int end = currentAmount + amount;

        currentAmount = end;

        // Text animasyonlu artış
        DOTween.To(() => start, x =>
        {
            amountText_value.text = "x" + x;
        }, end, 0.4f).SetEase(Ease.OutCubic);
    }
}