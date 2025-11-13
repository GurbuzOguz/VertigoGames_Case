using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SpinButtonController : MonoBehaviour
{
    [SerializeField] private Button spinButton;
    private bool isDisabled = false;

    private Vector3 defaultScale;

    private void OnValidate()
    {
        if (spinButton == null)
            spinButton = GetComponentInChildren<Button>(true);
    }

    private void Awake()
    {
        defaultScale = spinButton.transform.localScale;

        spinButton.onClick.AddListener(() =>
        {
            if (isDisabled) return;

            PlayClickAnimation();  
            WheelEvents.OnSpinRequest?.Invoke();
        });
    }

    private void OnEnable()
    {
        WheelEvents.OnSpinStarted += DisableButton;
        WheelEvents.OnSpinCompleted += EnableButton;
    }

    private void OnDisable()
    {
        WheelEvents.OnSpinStarted -= DisableButton;
        WheelEvents.OnSpinCompleted -= EnableButton;
    }

    private void PlayClickAnimation()
    {
        spinButton.transform.DOKill();
        spinButton.transform.localScale = defaultScale;

        spinButton.transform
            .DOScale(defaultScale * 1.15f, 0.1f)
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                spinButton.transform
                    .DOScale(defaultScale, 0.1f)
                    .SetEase(Ease.InQuad);
            });
    }

    private void DisableButton()
    {
        isDisabled = true;
        spinButton.interactable = false;
    }

    private void EnableButton()
    {
        isDisabled = false;
        spinButton.interactable = true;
    }
}