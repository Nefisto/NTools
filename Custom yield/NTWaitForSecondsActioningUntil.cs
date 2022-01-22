using System;
using UnityEngine;

/// <summary>
/// Will act similar to the WaitForSeconds BUT will pass the amount of time passed until started
/// </summary>
public class NTWaitForSecondsActioningUntil : CustomYieldInstruction
{
    private float timer = 0f;
    private float counter = 0f;
    
    private Action<float> action;
    
    public NTWaitForSecondsActioningUntil(float seconds, Action<float> action)
    {
        timer = seconds;
        this.action = action;
    }

    public override bool keepWaiting
    {
        get
        {
            counter += Time.deltaTime;

            action?.Invoke(counter);
            
            return !(counter >= timer);
        }
    }
}