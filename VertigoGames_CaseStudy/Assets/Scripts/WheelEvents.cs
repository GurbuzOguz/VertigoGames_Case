using System;
using UnityEngine;

public static class WheelEvents
{
    public static Action OnSpinRequest;              // Spin butonu tarafından tetiklenir
    public static Action<int> OnSliceChosen;         // WheelController slice seçtiğinde tetikler
    public static Action OnSpinStarted;              // WheelRotate başlarken
    public static Action OnSpinCompleted;            // WheelRotate bittiğinde
    public static Action OnSliceTick;
}