using UnityEngine;
using UnityEngine.EventSystems;

public class LosePanelButtons : MonoBehaviour
{
    [Header("Auto References (OnValidate)")]
    [SerializeField] private UI_ButtonHandler ui_button_giveup;
    [SerializeField] private UI_ButtonHandler ui_button_revive;

    private void OnEnable()
    {
        ui_button_giveup.OnClicked += OnGiveUpPressed;
        ui_button_revive.OnClicked += OnRevivePressed;
    }

    private void OnDisable()
    {
        ui_button_giveup.OnClicked -= OnGiveUpPressed;
        ui_button_revive.OnClicked -= OnRevivePressed;
    }


    private void OnValidate()
    {
        if (ui_button_giveup == null)
            ui_button_giveup = transform.Find("ui_button_giveup")?.GetComponent<UI_ButtonHandler>();

        if (ui_button_revive == null)
            ui_button_revive = transform.Find("ui_button_revive")?.GetComponent<UI_ButtonHandler>();
    }

    private void OnGiveUpPressed()
    {
        WheelEvents.OnGiveUpChosen?.Invoke();
    }

    private void OnRevivePressed()
    {
        WheelEvents.OnReviveChosen?.Invoke();
    }
}