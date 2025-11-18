using UnityEngine;
using UnityEngine.UI;

public class ExitButtonController : MonoBehaviour
{
    [SerializeField] private Button ui_exitButton;
    [SerializeField] private UI_ButtonHandler handler;

    private void OnEnable()
    {
        WheelEvents.OnSpinRequest += DisableButtonDuringSpin;
        WheelEvents.OnLevelNumberChanged += UpdateButtonByLevel;

        handler.OnClicked += OnExitClick;
    }

    private void OnDisable()
    {
        WheelEvents.OnSpinRequest -= DisableButtonDuringSpin;
        WheelEvents.OnLevelNumberChanged -= UpdateButtonByLevel;

        handler.OnClicked -= OnExitClick;
    }

    private void DisableButtonDuringSpin()
    {
        ui_exitButton.interactable = false;
    }

    private void UpdateButtonByLevel(int level)
    {
        ui_exitButton.interactable = (level == 1 || level % 5 == 0);
    }

    private void OnExitClick()
    {
        if (!ui_exitButton.interactable) return;

        WheelEvents.OnExitRequested?.Invoke();
    }

    private void OnValidate()
    {
        if (ui_exitButton == null) ui_exitButton = GetComponent<Button>();
        if (handler == null) handler = GetComponent<UI_ButtonHandler>();
    }
}