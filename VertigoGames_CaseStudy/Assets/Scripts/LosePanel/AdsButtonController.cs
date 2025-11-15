using UnityEngine;

public class AdsButtonController : MonoBehaviour
{
    [SerializeField] private UIButtonHandler ui_button_ads;

    private void OnEnable()
    {
        ui_button_ads.onClick += HandleAdsButton;
    }

    private void OnDisable()
    {
        ui_button_ads.onClick -= HandleAdsButton;
    }

    private void HandleAdsButton()
    {
        WheelEvents.OnAdsRequested?.Invoke();
    }

    private void OnValidate()
    {
        if (ui_button_ads == null)
        {
            ui_button_ads = transform.Find("ui_button_ads")?.GetComponent<UIButtonHandler>();
        }
    }
}