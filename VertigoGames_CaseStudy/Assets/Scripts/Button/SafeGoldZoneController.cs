using UnityEngine;
using TMPro;

public class SafeGoldZoneController : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TMP_Text ui_safezoneText;
    [SerializeField] private TMP_Text ui_goldzoneText;

    private void OnEnable()
    {
        WheelEvents.OnLevelNumberChanged += UpdateZoneUI;
    }

    private void OnDisable()
    {
        WheelEvents.OnLevelNumberChanged -= UpdateZoneUI;
    }

    private void UpdateZoneUI(int currentLevel)
    {
        int nextSafe = GetNextSafeZone(currentLevel);
        int nextGold = GetNextGoldZone(currentLevel);

        ui_safezoneText.text = "Safe Zone " + nextSafe;
        ui_goldzoneText.text = "Gold Zone " + nextGold;
    }

    private int GetNextSafeZone(int level)
    {
        if (level % 5 == 0)
            return level + 5;

        return level + (5 - (level % 5));
    }

    private int GetNextGoldZone(int level)
    {
        if (level % 30 == 0)
            return level + 30;

        return level + (30 - (level % 30));
    }
}