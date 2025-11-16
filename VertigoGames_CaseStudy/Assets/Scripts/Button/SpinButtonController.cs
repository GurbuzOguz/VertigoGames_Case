using UnityEngine;
using DG.Tweening;

public class SpinButtonController : MonoBehaviour
{
    [Header("References (Auto)")]
    [SerializeField] private UI_ButtonHandler buttonHandler;
    [SerializeField] private Transform animationTarget;

    private bool isDisabled = false;
    private Vector3 defaultScale;

    private void OnValidate()
    {
        if (buttonHandler == null)
            buttonHandler = GetComponentInChildren<UI_ButtonHandler>(true);

        if (animationTarget == null)
            animationTarget = transform.Find("anim_target");
    }

    private void Awake()
    {
        defaultScale = animationTarget.localScale;
    }

    private void OnEnable()
    {
        WheelEvents.OnSpinStarted += DisableButton;
        WheelEvents.OnSpinCompleted += EnableButton;
        buttonHandler.OnClicked += OnButtonClick;
    }

    private void OnDisable()
    {
        WheelEvents.OnSpinStarted -= DisableButton;
        WheelEvents.OnSpinCompleted -= EnableButton;
        buttonHandler.OnClicked -= OnButtonClick;
    }

    private void OnButtonClick()
    {
        if (isDisabled) return;

        PlayClickAnimation();
        WheelEvents.OnSpinRequest?.Invoke();
    }

    private void PlayClickAnimation()
    {
        animationTarget.DOKill();
        animationTarget.localScale = defaultScale;

        animationTarget
            .DOScale(defaultScale * 1.15f, 0.1f)
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                animationTarget
                    .DOScale(defaultScale, 0.1f)
                    .SetEase(Ease.InQuad);
            });
    }

    private void DisableButton()
    {
        isDisabled = true;
        buttonHandler.SetInteractable(false);
    }

    private void EnableButton()
    {
        isDisabled = false;
        buttonHandler.SetInteractable(true);
    }
}