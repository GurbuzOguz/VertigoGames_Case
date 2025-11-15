using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class UIButtonHandler : MonoBehaviour, IPointerClickHandler
{
    public Action onClick;

    public void OnPointerClick(PointerEventData eventData)
    {
        onClick?.Invoke();
    }
}