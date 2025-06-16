using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitFlash : JuiceEffect
{
    [Tooltip("The sprite renderer to change the color of")] [SerializeField]
    private SpriteRenderer Target;
    
    [Tooltip("Color to flash to when hit")] [SerializeField]
    private Color FlashColor;

    [Tooltip("Number of times to flash the desired color")] [SerializeField]
    private int NumFlashes;
    
    [Tooltip("Total duration of the effect")]
    [SerializeField] private float Duration;
    
    private Color _originalColor;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    public override void Play()
    {
        _originalColor = Target.color;
        StartCoroutine(FlashCoroutine());
    }

    private IEnumerator FlashCoroutine()
    {
        for (int i = 0; i < NumFlashes; i++)
        {
            Target.color = FlashColor;
            yield return new WaitForSecondsRealtime(Duration / NumFlashes / 2.0f);
            
            Target.color = _originalColor;
            yield return new WaitForSecondsRealtime(Duration / NumFlashes / 2.0f);
        }

        Target.color = _originalColor;
    }
}
