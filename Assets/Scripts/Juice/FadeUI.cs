using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class FadeUI : JuiceEffect
{
    [SerializeField] private Image Target;
    [SerializeField] private bool PlayOnStart;
    [SerializeField] private float Delay;
    [SerializeField] private bool DestroyAfterFade;
    [SerializeField] private float Duration;
    [SerializeField] private Color TargetColor;

    private float _timer;
    private float _currentDelay;
    
    // Start is called before the first frame update
    void Start()
    {
        if (PlayOnStart)
            StartCoroutine(FadeCoroutine());
    }

    public override void Play()
    {
        _currentDelay = Delay;
        StartCoroutine(FadeCoroutine());
    }

    public void SkipDelay(bool skip)
    {
        _currentDelay = skip ? 0.0f : Delay;
    }
    
    private IEnumerator FadeCoroutine()
    {
        if (_currentDelay > 0.0f)
            yield return new WaitForSecondsRealtime(_currentDelay);
        
        _timer = 0.0f;
        Color startColor = Target.color;

        while (_timer < Duration)
        {
            Target.color = Color.Lerp(startColor, TargetColor, _timer / Duration);
            _timer += Time.unscaledDeltaTime;
            yield return null;
        }

        Target.color = TargetColor;
        yield return null;
        
        if (DestroyAfterFade)
            Destroy(gameObject);
    }
}
