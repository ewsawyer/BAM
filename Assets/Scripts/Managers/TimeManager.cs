using System.Collections;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance;
    
    [Tooltip("The timescale to return to after a timescale change")] [SerializeField]
    private float OriginalScale;

    public float Scale;
    public float Duration;

    private float _timer;
    private bool _isTimeScaleAltered;

    void Awake()
    {
        if (Instance is null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }

    public void ChangeTimeScale(float scale, float duration)
    {
        if (_isTimeScaleAltered && duration > Duration - _timer)
        {
            Duration = _timer + duration;
            this.Scale = scale;
        }
        else if (!_isTimeScaleAltered)
        {
            Duration = duration;
            Scale = scale;
            StartCoroutine(Slowdown());
        }
    }
    
    private IEnumerator Slowdown()
    {
        _isTimeScaleAltered = true;
        _timer = 0.0f;

        while (_timer < Duration)
        {
            Time.timeScale = Scale;
            Time.fixedDeltaTime = 0.02f * Scale;
            _timer += Time.unscaledDeltaTime;
            yield return null;
        }

        Time.timeScale = 1.0f;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
        _isTimeScaleAltered = false;
    }

}
