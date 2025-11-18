using UnityEngine;
using DG.Tweening;

public class EndGamePanelController : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private UI_ButtonHandler restartButton;

    [Header("Animation Settings")]
    [SerializeField] private float fadeDuration = 0.35f;
    [SerializeField] private float startScale = 0.85f;

    private bool isOpen = false;

    private void Start()
    {
        HideInstant();
    }

    private void OnEnable()
    {
        WheelEvents.OnAllLevelsFinished += OpenPanel;

        if (restartButton != null)
            restartButton.OnClicked += RestartLevels;
    }

    private void OnDisable()
    {
        WheelEvents.OnAllLevelsFinished -= OpenPanel;

        if (restartButton != null)
            restartButton.OnClicked -= RestartLevels;
    }

    private void OpenPanel()
    {
        if (canvasGroup == null)
        {
            Debug.LogError("[EndGamePanelController] CanvasGroup referansı atanmadı!");
            return;
        }

        isOpen = true;

        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        canvasGroup.transform.localScale = Vector3.one * startScale;

        Sequence seq = DOTween.Sequence();
        seq.Append(canvasGroup.DOFade(1f, fadeDuration))
            .Join(canvasGroup.transform.DOScale(1f, fadeDuration).SetEase(Ease.OutBack))
            .OnComplete(() =>
            {
                canvasGroup.interactable = true;
                canvasGroup.blocksRaycasts = true;
            });
    }

    private void ClosePanel()
    {
        if (!isOpen) return;

        isOpen = false;

        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        canvasGroup.DOFade(0f, fadeDuration);

    }

    private void HideInstant()
    {
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

  
    private void RestartLevels()
    {
        ClosePanel();

        WheelEvents.OnLevelReset?.Invoke();
    }
}
