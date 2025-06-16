using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShaker : JuiceEffect
{
    [Tooltip("The amplitude of the camera shake")] [SerializeField]
    public float Amplitude;
    
    public override void Play()
    {
        PositionShaker shaker = Camera.main.GetComponent<PositionShaker>();
        shaker.Amplitude = Amplitude;
        if (shaker.IsShaking())
            shaker.ReplenishShakeTime();
        else
            shaker.Play();
    }
}
