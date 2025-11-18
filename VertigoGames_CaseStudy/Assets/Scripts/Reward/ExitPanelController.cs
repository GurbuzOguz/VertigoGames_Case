using UnityEngine;
using DG.Tweening;

public class ExitPanelController : MonoBehaviour
{
    [SerializeField] private CanvasGroup panel;

    private void OnEnable()
    {
        WheelEvents.OnExitRequested += Show;
        WheelEvents.OnConfirmCollectRequested += Hide;
        WheelEvents.OnCancelCollectRequested += Hide;
    }

    private void OnDisable()
    {
        WheelEvents.OnExitRequested -= Show;
        WheelEvents.OnConfirmCollectRequested -= Hide;
        WheelEvents.OnCancelCollectRequested -= Hide;
    }

    private void Show()
    {
        panel.gameObject.SetActive(true);
        panel.alpha = 0;
        panel.interactable = true;
        panel.blocksRaycasts = true;
        panel.DOFade(1f, 0.2f);
    }

    private void Hide()
    {
        panel.interactable = false;
        panel.blocksRaycasts = false;
        panel.DOFade(0f, 0.2f).OnComplete(() =>
        {
            panel.gameObject.SetActive(false);
        });
    }
}