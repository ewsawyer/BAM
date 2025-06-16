using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    [Tooltip("Speed that the rocket will move")] [SerializeField]
    private float LaunchSpeed;

    [Tooltip("Explosion to cause when the rocket collides with something")] [SerializeField]
    private Explosion ExplosionPrefab;

    private Rigidbody2D _rigidbody;
    
    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        _rigidbody.velocity = LaunchSpeed * transform.up;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Destroy(gameObject);
        Explosion e = Instantiate(ExplosionPrefab, transform.position, Quaternion.identity);
    }
}
