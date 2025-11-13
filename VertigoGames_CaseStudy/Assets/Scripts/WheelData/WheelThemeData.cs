using UnityEngine;

[CreateAssetMenu(fileName = "WheelThemeData", menuName = "Wheel/Wheel Theme Data")]
public class WheelThemeData : ScriptableObject
{
    public WheelType wheelType;

    [Header("Görseller")]
    public Sprite spinBackground;
    public Sprite indicatorSprite;

    [Header("Renkler")]
    public Color titleColor = Color.white;
    public Color rewardTextColor = Color.white;
}