using System;
using TMPro;
using UnityEngine;

public class ReviveManager : MonoBehaviour
{
    [SerializeField] private int reviveCost = 50;
    [SerializeField] private TMP_Text ui_reviveCost_value;

    private int Coins
    {
        get => PlayerPrefs.GetInt("COINS", 0);
        set => PlayerPrefs.SetInt("COINS", value);
    }

    private void Start()
    {
        ui_reviveCost_value.text = reviveCost.ToString();
    }

    private void OnEnable()
    {
        WheelEvents.OnReviveChosen += TryRevive;
    }

    private void OnDisable()
    {
        WheelEvents.OnReviveChosen -= TryRevive;
    }

    private void TryRevive()
    {
        if (Coins >= reviveCost)
        {
            Coins -= reviveCost;
            WheelEvents.OnReviveSuccess?.Invoke();
        }
        else
        {
            WheelEvents.OnReviveFailed?.Invoke();
        }
    }

    private void OnValidate()
    {
        if (ui_reviveCost_value == null)
            ui_reviveCost_value = transform.Find("ui_reviveCost_value")?.GetComponent<TMP_Text>();
    }
}