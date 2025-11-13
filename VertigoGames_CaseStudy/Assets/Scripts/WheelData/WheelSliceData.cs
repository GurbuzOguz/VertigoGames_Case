using UnityEngine;

public enum SliceType
{
    None,
    Cash,
    Grenade,
    Coin,
    BronzeChest,
    GoldChest,
    Armor,
    KnifePoints,
    Bomb 
}

[CreateAssetMenu(menuName = "Wheel/Slice Data", fileName = "SliceData_")]
public class WheelSliceData : ScriptableObject
{
    public SliceType sliceType;
    public Sprite iconSprite;
    public int rewardValue;
    public string description;
}