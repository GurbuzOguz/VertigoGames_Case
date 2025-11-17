using TMPro;
using UnityEngine;

public class ReviveManager : MonoBehaviour
{
    [Header("Revive Settings")] [SerializeField]
    private int reviveCost = 50;

    [SerializeField] private int startCoinValue = 250;
    [SerializeField] private float costMultiplier = 1.2f;

    [Header("UI References")] [SerializeField]
    private TMP_Text ui_reviveCost_value;

    [SerializeField] private TMP_Text ui_playerCoins_value;

    private int Coins
    {
        get => PlayerPrefs.GetInt("COINS", 0);
        set
        {
            PlayerPrefs.SetInt("COINS", value);
            ui_playerCoins_value.text = value.ToString();
        }
    }

    private void Start()
    {
        Coins = startCoinValue;

        ui_reviveCost_value.text = reviveCost.ToString();
        ui_playerCoins_value.text = "Coins : " + Coins;
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

            reviveCost = Mathf.RoundToInt(reviveCost * costMultiplier);
            
            ui_reviveCost_value.text = reviveCost.ToString();
            ui_playerCoins_value.text = "Coins : " + Coins;


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

        if (ui_playerCoins_value == null)
            ui_playerCoins_value = transform.Find("ui_playerCoins_value")?.GetComponent<TMP_Text>();
    }
}