using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class Shrink : JuiceEffect
{

    [Tooltip("The transform to apply the scale changes to")] [SerializeField]
    private Transform Target;
    
    [Tooltip("The factor to shrink by when played")] [SerializeField]
    public float ShrinkFactor;

    [Tooltip("If true, the Shrink Factor will simply be used as the final local scale of the object")] [SerializeField]
    private bool UseShrinkFactorAsEndScale;

    [SerializeField] public float Duration;
    [SerializeField] private float Delay;

    private float _timer;

    public override void Play()
    {
        StartCoroutine(ShrinkCoroutine());
    }

    private IEnumerator ShrinkCoroutine()
    {
        yield return new WaitForSecondsRealtime(Delay);

        _timer = 0.0f;
        Vector2 startScale = Target.transform.localScale;
        Vector2 endScale;
        if (!UseShrinkFactorAsEndScale)
            endScale = (1 - ShrinkFactor) * startScale;
        else
            endScale = Vector2.one * ShrinkFactor;

        while (_timer < Duration)
        {
            Target.transform.localScale = Vector2.Lerp(startScale, endScale, _timer / Duration);
            _timer += Time.unscaledDeltaTime;
            yield return null;
        }

        // Make sure the scale matches the target at the end
        Target.transform.localScale = endScale;
    }
}
