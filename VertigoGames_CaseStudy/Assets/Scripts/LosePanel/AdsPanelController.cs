using UnityEngine;
using DG.Tweening;

public class AdsPanelController : MonoBehaviour
{
    [SerializeField] private CanvasGroup panel;

    private void OnEnable()
    {
        WheelEvents.OnAdsRequested += ShowPanel;
        WheelEvents.OnAdsFinished += HidePanel;
    }

    private void OnDisable()
    {
        WheelEvents.OnAdsRequested -= ShowPanel;
        WheelEvents.OnAdsFinished -= HidePanel;
    }

    public void ShowPanel()
    {
        // GameObject aktif kalıyor → sadece görünürlük kontrolü

        panel.interactable = true;
        panel.blocksRaycasts = true;

        panel.DOKill(); // varsa eski animasyonu sil
        panel.DOFade(1f, 0.25f).SetEase(Ease.OutCubic);
    }

    public void HidePanel()
    {
        panel.interactable = false;
        panel.blocksRaycasts = false;

        panel.DOKill();
        panel.DOFade(0f, 0.2f).SetEase(Ease.OutQuad);
    }
}