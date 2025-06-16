using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ParticleJuice : JuiceEffect
{
    [SerializeField] private ParticleSystem Particles;
    [SerializeField] private bool MatchParentRotation2D;
    
    public override void Play()
    {
        ParticleSystem particles = Instantiate(Particles);
        particles.transform.position = transform.position;
        
        if (MatchParentRotation2D)
        {
            Vector3 parentRight = transform.parent.right;
            parentRight.z = 0;
            particles.transform.right = parentRight;
        }
    }
}
