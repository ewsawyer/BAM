using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePosition : JuiceEffect
{
    public enum Style
    {
        Offset,
        Absolute
    }

    [SerializeField] private float Delay;
    [SerializeField] private float Duration;
    [SerializeField] private Vector3 MoveTo;
    [SerializeField] private MovePosition.Style MoveStyle;
    [SerializeField] private AnimationCurve Curve;

    private float _timer;
    
    public override void Play()
    {
        StartCoroutine(MoveCoroutine());
    }

    public void PlayReversed()
    {
        StartCoroutine(MoveCoroutine(true));
    }

    public float GetDelay()
    {
        return Delay;
    }

    public float GetDuration()
    {
        return Duration;
    }

    public IEnumerator MoveCoroutine(bool reversed=false)
    {
        if (Delay > 0.0f)
            yield return new WaitForSeconds(Delay);

        _timer = 0.0f;
        
        Vector3 startPos = transform.position;
        Vector3 endPos = reversed ? -MoveTo : MoveTo;
        if (MoveStyle == Style.Offset)
            endPos = startPos + endPos;
        
        while (_timer < Duration)
        {
            Vector3 pos = Vector3.Lerp(startPos, endPos, Curve.Evaluate(_timer / Duration));
            transform.position = pos;
            _timer += Time.unscaledDeltaTime;
            yield return null;
        }

        transform.position = endPos;
    }
}
