using System;
using UnityEngine;

public static class WheelEvents
{
    public static Action OnSpinRequest;              // Spin butonu tarafından tetiklenir
    public static Action<int> OnSliceChosen;         // WheelController slice seçtiğinde tetikler
    public static Action OnSpinStarted;              // WheelRotate başlarken
    public static Action<float> OnRotateToAngle;     // Wheel döndürülmesi gereken açı
    public static Action OnSpinCompleted;            // WheelRotate bittiğinde
    public static Action OnSliceTick;
    public static Action<WheelSliceData> OnRewardCalculated;
    public static Action<WheelType> OnLevelChanged;
    public static Action<int> OnLevelNumberChanged;
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