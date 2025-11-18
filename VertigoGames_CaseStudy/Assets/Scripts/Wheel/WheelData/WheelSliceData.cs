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
    GoldSpinKnife,
    GoldSpinRiffle,
    GoldSpinShotgun,
    Bomb 
}

[CreateAssetMenu(menuName = "Wheel/Slice Data", fileName = "SliceData_")]
public class WheelSliceData : ScriptableObject
{
    public SliceType sliceType;
    public Sprite iconSprite;
    public int rewardValue;
    public string description;
    
    [Header("Weight Settings (Random Mode)")]
    public float weight = 1f;
}