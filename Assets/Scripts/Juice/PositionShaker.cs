using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PositionShaker : JuiceEffect
{

    // Uses radians
    public struct SineWave 
    {
        private float _amplitude;
        private float _wavelength;
        private float _phase;  // Positive phase shifts the wave to the right

        public SineWave(float a, float w, float p)
        {
            _amplitude = a;
            _wavelength = w;
            _phase = p;
        }
	
        public float Evaluate(float x)
        {
            return _amplitude * Mathf.Sin((x - _phase) / _wavelength * 2 * Mathf.PI);
        }
    }

    public struct Curve
    {
        private List<SineWave> _waves;
        
        public void Add(SineWave w)
        {
            if (_waves is null)
                _waves = new List<SineWave>();
            
            _waves.Add(w);
        }
        
        public float Evaluate(float x)
        {
            float y = 0.0f;
            foreach (SineWave w in _waves)
                y += w.Evaluate(x);
            return y;
        }
    }

    [Tooltip("The amount of time the camera shake should last")] [SerializeField]
    private float Duration;

    [Tooltip("The amplitude of the camera shake")] [SerializeField]
    public float Amplitude;

    [Tooltip("The frequency of the camera vibration")] [SerializeField]
    private float Frequency;

    [Tooltip("The number of waves to add to the final curve")] [SerializeField]
    private int NumberOfWaves;

    [Tooltip("The factor by which to reduce the amplitude and wavelength of each wave in the curve")] [SerializeField]
    private float WaveShrinkingFactor;

    [Tooltip("The object to shake")] [SerializeField]
    private Transform Target;
    
    private Curve _curve;
    private float _timer;
    private float _currentShakeDuration;
    private bool _isShaking;
    
    // Start is called before the first frame update
    void Start()
    {
        _curve = new Curve();
        float amp = 1.0f;
        float wavelength = 2 * Mathf.PI;
        for (int i = 0; i < NumberOfWaves; i++)
        {
            SineWave wave = new SineWave(amp, wavelength, Random.Range(0, Mathf.PI));
            _curve.Add(wave);
            amp /= WaveShrinkingFactor;
            wavelength /= WaveShrinkingFactor;
        }
    }

    public void ReplenishShakeTime()
    {
        _currentShakeDuration = _timer + Duration;
    }

    public bool IsShaking()
    {
        return _isShaking;
    }
    
    public override void Play()
    {
        StartCoroutine(ShakeCoroutine());
    }

    private IEnumerator ShakeCoroutine()
    {
        Vector3 originalPosition = transform.localPosition;
        _isShaking = true;
        
        _timer = 0.0f;
        _currentShakeDuration = Duration;
        Vector3 currentOffset = Vector2.zero;
		
        while (_timer < _currentShakeDuration)
        {
            Vector3 pos = Target.transform.localPosition;
            pos -= currentOffset;
            float x = _timer * Frequency;
            currentOffset.x = Amplitude * _curve.Evaluate((x / Duration) * 2 * Mathf.PI);
            currentOffset.y = Amplitude * _curve.Evaluate((x / Duration) * 2 * Mathf.PI + Mathf.PI / 4.0f);
            pos += currentOffset;
            Target.transform.localPosition = pos;
            _timer += Time.unscaledDeltaTime;
            yield return null;
        }

        _isShaking = false;
        transform.localPosition = originalPosition;
    }
}
