using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Mine : MonoBehaviour, IExplodable
{
    [Tooltip("Initial speed to throw the mine at")] [SerializeField]
    private float LaunchSpeed;
    
    [Tooltip("The mine will explode after a player spends this long in its proximity")] [SerializeField]
    private float ProximityTime;

    [Tooltip("The mine will explode on its own after this long")] [SerializeField]
    private float DetonateAfter;

    [Tooltip("How long to perform warning flashes for before automatic detonation")] [SerializeField]
    private float AutoDetonateWarningDuration;

    [Tooltip(
        "The proximity ring won't count down the explosion timer during for this many seconds after instantiation")]
    [SerializeField]
    private float GracePeriod;

    [Tooltip("The explosion to instantiate when the mine goes off")] [SerializeField]
    private Explosion MineExplosion;
    
    [Header("Juice")]
    [Tooltip("Juice to play each time the mine flashes")] [SerializeField]
    private Juice JuiceFlash;

    [Tooltip("Juice to play when the mine is placed")] [SerializeField]
    private Juice JuicePlaceMine;

    [Tooltip("Juice to play when the mine is warning that it's going to automatically detonate")] [SerializeField]
    private Juice JuiceAutoDetonate;
    
    private float _proximityTimer;
    private float _gracePeriodTimer;
    private Rigidbody2D _rigidbody;
    private float _detonationTimer;
    
    // Start is called before the first frame update
    void Start()
    {
        _gracePeriodTimer = GracePeriod;
        _detonationTimer = DetonateAfter;
        _rigidbody = GetComponent<Rigidbody2D>();
        _rigidbody.velocity = LaunchSpeed * transform.up;

        StartCoroutine(AutoDetonate());
    }

    // Update is called once per frame
    void Update()
    {
        if (_gracePeriodTimer > 0.0f)
            _gracePeriodTimer -= Time.deltaTime;
    }

    public void Explode()
    {
        Destroy(gameObject);
        Instantiate(MineExplosion, transform.position, Quaternion.identity);
    }

    public void OnExplode()
    {
        Explode();
    }

    private IEnumerator AutoDetonate()
    {
        yield return new WaitForSeconds(DetonateAfter - AutoDetonateWarningDuration);
        JuiceAutoDetonate.Play();
        yield return new WaitForSeconds(AutoDetonateWarningDuration);
        JuiceAutoDetonate.Stop();
        StartCoroutine(FlashAndExplode());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.GetComponent<PlayerHealth>() || _gracePeriodTimer > 0.0f)
            return;

        StartCoroutine(FlashAndExplode());
    }

    private IEnumerator FlashAndExplode()
    {
        JuiceFlash.Play();
        yield return new WaitForSeconds(ProximityTime);
        Explode();
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (_gracePeriodTimer > 0.0f)
            return;
        
        Explode();
    }
}
