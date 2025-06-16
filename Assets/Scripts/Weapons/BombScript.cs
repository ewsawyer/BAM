using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class BombScript : MonoBehaviour
{
    // references
    public Aim aim;
    private Rigidbody2D _rb;
    [SerializeField] private GameObject explosionPrefab;
    
    // initial velocity
    public float launchSpeed = 4f;
    
    // detonation timers - may not have to use
    // only reason im using these is so it 
    public float detonateTimer = 2f;
    public float shakeTiming = .5f;
    
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.velocity = aim.Direction * launchSpeed;
    }
    
    void Update()
    {
        detonateTimer -= Time.deltaTime;
        if (detonateTimer <= 0.5)
        {
            _rb.velocity = Vector2.zero;
            StartCoroutine(ShakeCoroutine(shakeTiming));
        }
        if (detonateTimer <= 0)
        {
            Explode();
        }
    }

    private void Explode()
    {
        // destroy bomb, explosion animation?
        // damage based on distance from center point
        Destroy(gameObject);
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        
    }

    private IEnumerator ShakeCoroutine(float duration)
    {
        var startPosition = transform.position;
        var elapsedTime = 0f;

        while (elapsedTime < duration) {
            elapsedTime += Time.deltaTime;
            transform.position = startPosition + (Random.insideUnitSphere * 0.1f);
            yield return null;
        }
        transform.position = startPosition;
    }
}
