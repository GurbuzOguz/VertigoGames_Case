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
        WheelEvents.OnReviveSuccess += HandleRevive;
        WheelEvents.OnAdsFinished += HandleAdsFinished;

    }

    private void OnDisable()
    {
        WheelEvents.OnBombSelected -= OpenPanel;
        WheelEvents.OnGiveUpChosen -= HandleGiveUp;
        WheelEvents.OnReviveFailed -= HandleReviveFailed;
        WheelEvents.OnReviveSuccess -= HandleRevive;
        WheelEvents.OnAdsFinished -= HandleAdsFinished;
    }

    private void OpenPanel()
    {
        panel.alpha = 0;
        panel.interactable = true;
        panel.blocksRaycasts = true;

        panel.DOFade(1f, 0.25f);
    }

    private void ClosePanel()
    {
        panel.interactable = false;
        panel.blocksRaycasts = false;

        panel.DOFade(0f, 0.2f);
    }

    private void HandleGiveUp()
    {
        ClosePanel();
        WheelEvents.OnLevelReset?.Invoke();
    }

    private void HandleRevive()
    {
        ClosePanel();
    }

    private void HandleReviveFailed()
    {
        Debug.Log("Revive başarısız → coin yok.");
    }
    
    private void HandleAdsFinished()
    {
        ClosePanel();
    }
}