using UnityEngine;

public class MutableWaitForSeconds : CustomYieldInstruction
{
    private float endTime = 0f;

    public float EndTime => endTime;

    public override bool keepWaiting => Time.time < endTime;

    public void SetSeconds(float seconds)
    {
        if (seconds < 0f) return;
        
        endTime = Time.time + seconds;
    }
}