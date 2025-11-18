using UnityEngine;
using DG.Tweening;

public class RewardConfirmPanelController : MonoBehaviour
{
    [SerializeField] private CanvasGroup panel;

    private void OnEnable()
    {
        WheelEvents.OnCancelCollectRequested += HidePanel;
        WheelEvents.OnConfirmCollectRequested += HidePanel;
    }

    private void OnDisable()
    {
        WheelEvents.OnCancelCollectRequested -= HidePanel;
        WheelEvents.OnConfirmCollectRequested -= HidePanel;
    }

    public void Show()
    {
        panel.gameObject.SetActive(true);
        panel.alpha = 0;
        panel.interactable = true;
        panel.blocksRaycasts = true;

        panel.DOFade(1f, 0.25f);
    }

    private void HidePanel()
    {
        panel.interactable = false;
        panel.blocksRaycasts = false;

        panel.DOFade(0f, 0.25f)
            .OnComplete(() => panel.gameObject.SetActive(false));
    }
}