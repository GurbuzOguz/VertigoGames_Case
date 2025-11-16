using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

public class UI_ButtonHandler : MonoBehaviour, IPointerClickHandler
{
    public Action OnClicked;
    public bool interactable = true;

    [SerializeField] private Button unityButton;

    private void OnValidate()
    {
        if (unityButton == null)
            unityButton = GetComponent<Button>();
    }

    public void SetInteractable(bool state)
    {
        interactable = state;

        if (unityButton != null)
            unityButton.interactable = state;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!interactable) return;

        OnClicked?.Invoke();
    }
}