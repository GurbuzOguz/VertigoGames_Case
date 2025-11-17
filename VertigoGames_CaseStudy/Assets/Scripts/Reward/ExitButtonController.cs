using UnityEngine;
using UnityEngine.UI;

public class ExitButtonController : MonoBehaviour
{
    [SerializeField] private Button ui_exitButton;

    private void OnEnable()
    {
        WheelEvents.OnSpinRequest += DisableButtonDuringSpin;
        WheelEvents.OnLevelNumberChanged += UpdateButtonByLevel;
    }

    private void OnDisable()
    {
        WheelEvents.OnSpinRequest -= DisableButtonDuringSpin;
        WheelEvents.OnLevelNumberChanged -= UpdateButtonByLevel;
    }

    private void DisableButtonDuringSpin()
    {
        if (ui_exitButton != null)
            ui_exitButton.interactable = false;
    }

    private void UpdateButtonByLevel(int level)
    {
        if (ui_exitButton == null)
            return;

        if (level == 1 || level % 5 == 0)
            ui_exitButton.interactable = true;
        else
            ui_exitButton.interactable = false;
    }

    private void OnValidate()
    {
        if (ui_exitButton == null)
            ui_exitButton = transform.GetComponent<Button>();
    }
}