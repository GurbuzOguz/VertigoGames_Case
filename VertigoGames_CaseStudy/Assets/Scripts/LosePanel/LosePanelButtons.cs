using UnityEngine;
using UnityEngine.EventSystems;

public class LosePanelButtons : MonoBehaviour
{
    [Header("Auto References (OnValidate)")]
    [SerializeField] private UIButtonHandler ui_button_giveup;
    [SerializeField] private UIButtonHandler ui_button_revive;

    private void OnEnable()
    {
        ui_button_giveup.onClick += OnGiveUpPressed;
        ui_button_revive.onClick += OnRevivePressed;
    }

    private void OnDisable()
    {
        ui_button_giveup.onClick -= OnGiveUpPressed;
        ui_button_revive.onClick -= OnRevivePressed;
    }

    // ------------------------------------------------------------
    // OnValidate → Auto-Bind Buttons
    // ------------------------------------------------------------
    private void OnValidate()
    {
        if (ui_button_giveup == null)
            ui_button_giveup = transform.Find("ui_button_giveup")?.GetComponent<UIButtonHandler>();

        if (ui_button_revive == null)
            ui_button_revive = transform.Find("ui_button_revive")?.GetComponent<UIButtonHandler>();
    }

    // ------------------------------------------------------------
    // Button actions → Event based
    // ------------------------------------------------------------
    private void OnGiveUpPressed()
    {
        WheelEvents.OnGiveUpChosen?.Invoke();
    }

    private void OnRevivePressed()
    {
        WheelEvents.OnReviveChosen?.Invoke();
    }
}