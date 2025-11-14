using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class WheelThemeManager : MonoBehaviour
{
    [Header("Theme Data List")]
    [SerializeField] private List<WheelThemeData> themes;

    [Header("UI References")]
    [SerializeField] private Image spinBackgroundImage;
    [SerializeField] private Image indicatorImage;
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private Color defaultRewardTextColor;

    private void OnEnable()
    {
        WheelEvents.OnLevelChanged += ApplyTheme;
    }

    private void OnDisable()
    {
        WheelEvents.OnLevelChanged -= ApplyTheme;
    }

    private void ApplyTheme(WheelType wheelType)
    {
        WheelThemeData theme = themes.Find(t => t.wheelType == wheelType);

        if (theme == null)
        {
            Debug.LogWarning("Theme bulunamadı: " + wheelType);
            return;
        }
        
        titleText.text = theme.displayName;

        spinBackgroundImage.sprite = theme.spinBackground;
        indicatorImage.sprite = theme.indicatorSprite;

        titleText.color = theme.titleColor;
    }
}