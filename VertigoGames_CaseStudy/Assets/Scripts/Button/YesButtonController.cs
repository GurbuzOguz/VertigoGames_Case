using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YesButtonController : MonoBehaviour
{
    [SerializeField] private UI_ButtonHandler handler;

    private void OnEnable()
    {
        handler.OnClicked += Click;
    }

    private void OnDisable()
    {
        handler.OnClicked -= Click;
    }

    private void Click()
    {
        WheelEvents.OnConfirmCollectRequested?.Invoke();
        WheelEvents.OnLevelReset?.Invoke();
    }
    
    private void OnValidate()
    {
        if (handler == null)
            handler = GetComponentInChildren<UI_ButtonHandler>(true);
    }
}


