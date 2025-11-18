using UnityEngine;
using TMPro;
using DG.Tweening;

public class AdsFunAnimationController : MonoBehaviour
{
    [Header("Auto References")]
    [SerializeField] private TMP_Text ui_timerText;
    [SerializeField] private TMP_Text ui_centerMovingText;
    [SerializeField] private TMP_Text ui_oguzSpecialText;

    [Header("Settings")]
    [SerializeField] private float centerMoveRange = 80f;
    [SerializeField] private float centerMoveDuration = 1.2f;
    [SerializeField] private float colorChangeDuration = 1f;
    [SerializeField] private float oguzScaleDuration = 0.8f;

    private int countdownValue = 5;

    private void OnEnable()
    {
        WheelEvents.OnAdsRequested += AdsStart;
        WheelEvents.OnAdsFinished += AdsFinish;

    }

    private void OnDisable()
    {
        WheelEvents.OnAdsRequested -= AdsStart;
        WheelEvents.OnAdsFinished -= AdsFinish;
    }

    public void AdsStart()
    {
        StartCountdown();
        PlayCenterMovingTextAnim();
        PlayOguzAnim();
    }

    public void AdsFinish()
    {
        DOTween.Kill(ui_centerMovingText);
        DOTween.Kill(ui_oguzSpecialText);
        DOTween.Kill(ui_timerText);
    }

    private void StartCountdown()
    {
        countdownValue = 3;
        ui_timerText.text = countdownValue.ToString();

        DOTween.To(() => countdownValue, x =>
            {
                countdownValue = x;
                ui_timerText.text = countdownValue.ToString();

            }, 0, 10f)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                ui_timerText.text = "0";
                WheelEvents.OnAdsFinished?.Invoke();
            });
    }

    private void PlayCenterMovingTextAnim()
    {
        RectTransform rt = ui_centerMovingText.rectTransform;
        Vector2 startPos = rt.anchoredPosition;

        rt.DOAnchorPosX(startPos.x + centerMoveRange, centerMoveDuration)
            .SetEase(Ease.InOutQuad)
            .SetLoops(-1, LoopType.Yoyo);

        ui_centerMovingText.DOColor(RandomColor(), colorChangeDuration)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.Linear);
    }
    
    private void PlayOguzAnim()
    {
        ui_oguzSpecialText.rectTransform
            .DOScale(1.2f, oguzScaleDuration)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo);

        ui_oguzSpecialText
            .DOColor(RandomColor(), colorChangeDuration)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.Linear);
    }

    private Color RandomColor()
    {
        return new Color(Random.value, Random.value, Random.value);
    }
    
    private void OnValidate()
    {
        if (ui_timerText == null)
            ui_timerText = transform.Find("ui_timerText")?.GetComponent<TMP_Text>();

        if (ui_centerMovingText == null)
            ui_centerMovingText = transform.Find("ui_centerMovingText")?.GetComponent<TMP_Text>();

        if (ui_oguzSpecialText == null)
            ui_oguzSpecialText = transform.Find("ui_oguzSpecialText")?.GetComponent<TMP_Text>();
    }
}
