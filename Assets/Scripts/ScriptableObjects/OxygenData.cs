using System;
using UnityEngine;

[CreateAssetMenu]
public class OxygenData : ScriptableObject
{
    public VoidEvent OnLowOxygenLevel;
    public VoidEvent OnNoMoreOxygen;
    public VoidEvent OnOxygenLevelChange;
    public float maxOxygenLevel;
    public float currentOxygenLevel;
    public float lowOxygenLevel;

    public void ChangeOxygenLevelBy(float n)
    {
        currentOxygenLevel += n;
        currentOxygenLevel = Mathf.Clamp(currentOxygenLevel, 0f, maxOxygenLevel);
        if (currentOxygenLevel == 0f)
        {
            if (OnNoMoreOxygen != null)
                OnNoMoreOxygen?.Raise();
            else
                Debug.LogWarning("No more oxygen but OnNoMoreOxygen is not attached"); 
        }
        if (currentOxygenLevel <= lowOxygenLevel)
        {
            if (OnLowOxygenLevel != null)
                OnLowOxygenLevel?.Raise();
            else
                Debug.LogWarning("Oxygen level low but OnLowOxygenLevel gameEvent is not attached"); 
        }
        if (OnOxygenLevelChange != null)
            OnOxygenLevelChange?.Raise();
        else
            Debug.LogWarning("Oxygen level changed but OnOxygenLevelChange is not attached"); 
    }

    public void ResetOxygenLevelToMax()
    {
        currentOxygenLevel = maxOxygenLevel; 
        OnOxygenLevelChange?.Raise(); 
    }
}
