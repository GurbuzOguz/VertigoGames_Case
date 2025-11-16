using System;
using UnityEngine;

public static class WheelEvents
{
    //Spin Events
    public static Action OnSpinRequest;              
    public static Action<int> OnSliceChosen;        
    public static Action OnSpinStarted;             
    public static Action<float> OnRotateToAngle;     
    public static Action OnSpinCompleted;            
    
    //Sound Event
    public static Action OnSliceTick;
    
    //Reward Event
    public static Action<WheelSliceData> OnRewardCalculated;
    
    //Level Events
    public static Action<WheelType> OnLevelChanged;
    public static Action<int> OnLevelNumberChanged;
    
    //Bomb Selected Events
    public static Action OnBombSelected;
    public static Action OnLevelReset;
    
    //Lose Panel Events
    public static Action OnGiveUpChosen;
    public static Action OnContinueChosen;
    public static Action OnReviveChosen;
    public static Action OnReviveFailed;
    public static Action OnReviveSuccess;
    
    //Ads Panel Events
    public static Action OnAdsFinished;
    public static Action OnAdsRequested;
}