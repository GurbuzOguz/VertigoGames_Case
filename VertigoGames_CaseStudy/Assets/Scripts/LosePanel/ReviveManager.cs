using System;
using TMPro;
using UnityEngine;

public class ReviveManager : MonoBehaviour
{
    [SerializeField] private int currentCoins;
    [SerializeField] private int reviveCost = 50;
    [SerializeField] private TextMeshProUGUI reviveCostText;

    private void Start()
    {
        reviveCostText.text = reviveCost.ToString();
        currentCoins = PlayerPrefs.GetInt("COINS", reviveCost);
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
        if (currentCoins >= reviveCost)
        {
            currentCoins -= reviveCost;
            PlayerPrefs.SetInt("COINS", currentCoins);

            Debug.Log("Revive başarılı!");
            WheelEvents.OnReviveSuccess?.Invoke();  // 🔥 panel kapanacak
        }
        else
        {
            Debug.Log("Revive başarısız (coin yok)");
            WheelEvents.OnReviveFailed?.Invoke();  // 🔥 panel KAPANMAYACAK
        }
    }

}