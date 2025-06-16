using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour, IExplodable
{
    [Tooltip("The speed of the arrow")] [SerializeField]
    private float Speed;

    [Tooltip("The amount of damage the arrow will do if it hits a player")] [SerializeField]
    private float Damage;

    [Tooltip("The maximum number of times the arrow can ricochet")] [SerializeField]
    private int NumRicochets;
    
    [Header("Juice")]
    [Tooltip("The juice to play when ricocheting off a wall")] [SerializeField]
    private Juice JuiceRicochet;

    [Tooltip("The juice that plays when the arrow hits a player")] [SerializeField]
    private Juice JuicePlayerHit;

    private Rigidbody2D _rigidbody;
    private int _ricochets;
    
    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _rigidbody.velocity = Speed * transform.up;

        _ricochets = NumRicochets;
    }

    // Update is called once per frame
    void Update()
    {
        transform.up = _rigidbody.velocity;
    }

    public void OnExplode()
    {
        Destroy(gameObject);
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerHealth ph = collision.gameObject.GetComponent<PlayerHealth>();
        if (ph)
        {
            ph.TakeHit(Damage);
            Destroy(gameObject);
            JuicePlayerHit.Play();
        }

        // If we get here, we're ricocheting
        _ricochets--;
        
        JuiceRicochet.Play();
        
        if(_ricochets < 0)
            Destroy(gameObject);
    }
}
