using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ChromaticAberrationJuice : JuiceEffect
{
    [SerializeField] private float Intensity;
    [SerializeField] private float Duration;
    
    private Volume _volume;
    private ChromaticAberration _aberration;
    private float _timer;

    public void Start()
    {
        _volume = GameObject.FindGameObjectWithTag("Global Volume").GetComponent<Volume>();
        ChromaticAberration temp;
        if(_volume.profile.TryGet(out temp))
            _aberration = temp;
    }

    public override void Play()
    {
        StartCoroutine(ChromaticAberrationCoroutine());
    }

    private IEnumerator ChromaticAberrationCoroutine()
    {
        float original = _aberration.intensity.value;
        
        // Ramp up
        _timer = 0.0f;
        while (_timer < Duration / 2.0f)
        {
            _aberration.intensity.value = Mathf.Lerp(original, Intensity, _timer / (Duration / 2.0f));
            _timer += Time.unscaledDeltaTime;
            yield return null;
        }

        // Ramp down
        _timer = 0.0f;
        while (_timer < Duration / 2.0f)
        {
            _aberration.intensity.value = Mathf.Lerp(Intensity, original, _timer / (Duration / 2.0f));
            _timer += Time.unscaledDeltaTime;
            yield return null;
        }

        _aberration.intensity.value = original;
    }
}
