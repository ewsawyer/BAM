using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class Explosion : MonoBehaviour
{
    [Tooltip("The amount of damage dealt by a direct hit")] [SerializeField]
    private float MaxDamage;

    [Tooltip("The amount of damage dealt by a hit at the outermost edge of the explosion")] [SerializeField]
    private float MinDamage;

    [Tooltip("The maximum distance from the explosion's center that is still considered a direct hit")] [SerializeField]
    private float DirectHitRadius;

    [Tooltip("The radius of the blast")] [SerializeField]
    private float BlastRadius;

    [Tooltip("The juice that plays when the explosion happens")] [SerializeField]
    private Juice ExplosionJuice;

    [FormerlySerializedAs("ExplosionGrowthDuration")] [Tooltip("The amount of time the explosion grows for")] [SerializeField]
    private float GrowthDuration;

    [Tooltip("The amount of time the explosion will hold at its maximum size before destroying itself")] [SerializeField]
    private float MaxSizeHoldDuration;

    private Collider2D _collider;
    
    // Start is called before the first frame update
    private void Start()
    {
        _collider = GetComponent<Collider2D>();
        
        // Set initial scale to size of direct hit
        transform.localScale = DirectHitRadius * Vector2.one;
        
        // Set up juice that scales the explosion up
        Shrink shrink = ExplosionJuice.GetComponent<Shrink>(); 
        shrink.ShrinkFactor = -BlastRadius * 2.0f;
        shrink.Duration = GrowthDuration;
        
        // Set a timer to destroy the explosion when it's done
        Destroy(gameObject, GrowthDuration + MaxSizeHoldDuration);
    }

    // Update is called once per frame
    private void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if we hit the player
        PlayerHealth ph = other.gameObject.GetComponent<PlayerHealth>();
        if (ph)
        {
            // Figure out how far the collision point is from the center of the explosion
            float distance = Vector2.Distance(other.ClosestPoint(transform.position), transform.position);

            // Calculate amount of damage to do
            float damage = 0.0f;
            if (distance <= DirectHitRadius)
                damage = MaxDamage;
            else
            {
                float t = 1 - Mathf.InverseLerp(DirectHitRadius, BlastRadius, distance);
                print(distance + ", " + BlastRadius + ", " + DirectHitRadius);
                damage = Mathf.Lerp(MinDamage, MaxDamage, t);
            }
            
            // Make player take damage
            print("dealt " + damage + " damage");
            ph.TakeHit(damage);
        }

        IExplodable[] explodables = other.attachedRigidbody.GetComponents<MonoBehaviour>().OfType<IExplodable>().ToArray();
        foreach (IExplodable e in explodables)
            e.OnExplode();
    }
}
