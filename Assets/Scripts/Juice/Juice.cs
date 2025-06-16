using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Juice : MonoBehaviour
{
    [Tooltip("The game object containing all the juice effects")] [SerializeField]
    private GameObject JuiceContainer;

    [Tooltip("If true, this juice will play immediately on starting")] [SerializeField]
    private bool PlayOnStart;

    [Tooltip("If true, the juice will repeat every RepeatDelay seconds")] [SerializeField]
    private bool Loop;

    [Tooltip("If Loop is true, will wait this many seconds before playing the juice again")] [SerializeField]
    private float LoopInterval;
    
    // The list of juice effects that will be triggered when hit
    private JuiceEffect[] _juice;
    // Reference to the coroutine so it can be stopped at some point
    private Coroutine _playCoroutine;
    // Will continue the loop until this is false
    private bool _continueLooping;

    IEnumerator Start()
    {
        _juice = JuiceContainer.GetComponents<JuiceEffect>();

        if (PlayOnStart)
        {
            yield return null;
            Play();
        }
    }

    public void Stop()
    {
        _continueLooping = false;
    }
    
    public void Play()
    {
        if (_playCoroutine is null)
            _playCoroutine = StartCoroutine(PlayCoroutine());
    }

    private IEnumerator PlayCoroutine()
    {
        // Play once
        if (!Loop)
        {
            foreach (JuiceEffect effect in _juice)
                effect.Play();
            _playCoroutine = null;
            yield break;
        }

        // Play looping
        _continueLooping = true;
        while (_continueLooping)
        {
            foreach (JuiceEffect effect in _juice)
                effect.Play();
            
            if (_continueLooping)
                yield return new WaitForSeconds(LoopInterval);
        }

        _playCoroutine = null;
    }
    
}
