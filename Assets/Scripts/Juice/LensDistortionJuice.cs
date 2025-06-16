using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class LensDistortionJuice : JuiceEffect
{
    [SerializeField] private float Intensity;
    [SerializeField] private float Duration;
    
    private Volume _volume;
    private LensDistortion _distortion;
    private float _timer;

    public void Start()
    {
        _volume = GameObject.FindGameObjectWithTag("Global Volume").GetComponent<Volume>();
        LensDistortion temp;
        if(_volume.profile.TryGet(out temp))
            _distortion = temp;
    }

    public override void Play()
    {
        StartCoroutine(LensDistortionCoroutine());
    }

    private IEnumerator LensDistortionCoroutine()
    {
        float original = _distortion.intensity.value;
        
        // Ramp up
        _timer = 0.0f;
        while (_timer < Duration / 2.0f)
        {
            _distortion.intensity.value = Mathf.Lerp(original, Intensity, _timer / (Duration / 2.0f));
            _timer += Time.unscaledDeltaTime;
            yield return null;
        }

        // Ramp down
        _timer = 0.0f;
        while (_timer < Duration / 2.0f)
        {
            _distortion.intensity.value = Mathf.Lerp(Intensity, original, _timer / (Duration / 2.0f));
            _timer += Time.unscaledDeltaTime;
            yield return null;
        }

        _distortion.intensity.value = original;
    }
}
