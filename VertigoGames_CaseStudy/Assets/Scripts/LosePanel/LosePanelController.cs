using UnityEngine;
using DG.Tweening;

public class LosePanelController : MonoBehaviour
{
    [SerializeField] private CanvasGroup panel;

    private void OnEnable()
    {
        WheelEvents.OnBombSelected += OpenPanel;

        WheelEvents.OnGiveUpChosen += HandleGiveUp;
        WheelEvents.OnReviveFailed += HandleReviveFailed;
        WheelEvents.OnReviveSuccess += HandleReviveSuccess;

        WheelEvents.OnAdsRequested += HandleAdsRequested;
        WheelEvents.OnAdsFinished += HandleAdsFinished;
    }

    private void OnDisable()
    {
        WheelEvents.OnBombSelected -= OpenPanel;

        WheelEvents.OnGiveUpChosen -= HandleGiveUp;
        WheelEvents.OnReviveFailed -= HandleReviveFailed;
        WheelEvents.OnReviveSuccess -= HandleReviveSuccess;

        WheelEvents.OnAdsRequested -= HandleAdsRequested;
        WheelEvents.OnAdsFinished -= HandleAdsFinished;
    }

    private void OpenPanel()
    {
        panel.alpha = 0;
        panel.interactable = true;
        panel.blocksRaycasts = true;

        panel.DOKill();
        panel.DOFade(1f, 0.25f);
    }

    private void ClosePanel()
    {
        panel.interactable = false;
        panel.blocksRaycasts = false;

        panel.DOKill();
        panel.DOFade(0f, 0.2f);
    }

    private void HandleGiveUp()
    {
        ClosePanel();
        WheelEvents.OnLevelReset?.Invoke();
    }

    private void HandleReviveSuccess()
    {
        ClosePanel();
    }

    private void HandleReviveFailed()
    {
        Debug.Log("Revive başarısız → coin yok.");
    }

    private void HandleAdsRequested()
    {
        ClosePanel();   
    }

    private void HandleAdsFinished()
    {
        ClosePanel();  
    }
}